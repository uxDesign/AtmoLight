﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Linq;
using proto;

namespace AtmoLight.Targets
{
  class HyperionHandler : ITargets
  {
    #region Fields

    public Target Name { get { return Target.Hyperion; } }

    private static TcpClient Socket = new TcpClient();
    private Stream Stream;
    private Boolean Connected = false;

    private string hyperionIP = "";
    private int hyperionPort = 0;
    private int hyperionPriority = 0;
    private int hyperionPriorityStaticColor = 0;
    private int[] staticColor = { 0, 0, 0 };
    private Boolean hyperionReconnectOnError = false;

    #endregion
    #region Hyperion

    public void Initialise(bool force = false)
    {
      try
      {
        Connect();
        ClearPriority(hyperionPriority);
        ChangeEffect(Core.GetCurrentEffect());
      }
      catch (Exception e)
      {
        Log.Error("Hyperion: Error during initialise");
        Log.Error("Exception: {0}", e.Message);
      }
    }

    public void ReInitialise(bool force = false)
    {

    }

    public void Dispose()
    {
      if (Socket.Connected)
      {
        ClearPriority(hyperionPriority);
        ClearPriority(hyperionPriorityStaticColor);
        Socket.Close();
      }
    }
    public bool IsConnected()
    {
      //=== TEMP === remove when we add reconnect handling
      if (Connected == false)
      {
        Connect();
      }
      return Connected;
    }

    public void Connect()
    {
      //Use connection thread to prevent Mediaportal lag due to connect errors
      Thread t = new Thread(ConnectThread);
      t.Start();
    }
    private void ConnectThread()
    {
      try
      {
        Log.Debug("Hyperion: Trying to connect");

        //Close old socket and create new TCP client which allows it to reconnect when calling Connect()
        try
        {
          Socket.Close();
        }
        catch (Exception e)
        {
          Log.Error("Hyperion: Error while closing socket");
          Log.Error("Exception: {0}", e.Message);
        }
        Socket = new TcpClient();

        Socket.SendTimeout = 5000;
        Socket.ReceiveTimeout = 5000;
        Socket.Connect(hyperionIP, hyperionPort);
        Log.Debug("Hyperion: Connected");
        Stream = Socket.GetStream();
        Connected = Socket.Connected;
      }
      catch (Exception e)
      {
        Log.Error("Hyperion: Error while connecting");
        Log.Error("Exception: {0}", e.Message);
        Connected = false;
      }

    }
    public void ChangeColor(int red, int green, int blue)
    {
      ColorRequest colorRequest = ColorRequest.CreateBuilder()
        .SetRgbColor((red * 256 * 256) + (green * 256) + blue)
        .SetPriority(hyperionPriorityStaticColor)
        .SetDuration(-1)
        .Build();

      HyperionRequest request = HyperionRequest.CreateBuilder()
        .SetCommand(HyperionRequest.Types.Command.COLOR)
        .SetExtension(ColorRequest.ColorRequest_, colorRequest)
        .Build();

      SendRequest(request);
    }
    public void ClearPriority(int priority)
    {
      ClearRequest clearRequest = ClearRequest.CreateBuilder()
      .SetPriority(priority)
      .Build();

      HyperionRequest request = HyperionRequest.CreateBuilder()
      .SetCommand(HyperionRequest.Types.Command.CLEAR)
      .SetExtension(ClearRequest.ClearRequest_, clearRequest)
      .Build();

      SendRequest(request);
    }
    public void ClearAll()
    {
      HyperionRequest request = HyperionRequest.CreateBuilder()
      .SetCommand(HyperionRequest.Types.Command.CLEARALL)
      .Build();

      SendRequest(request);
    }
    public bool ChangeEffect(ContentEffect effect)
    {
      switch (effect)
      {
        case ContentEffect.LEDsDisabled:
        case ContentEffect.Undefined:
          ClearPriority(hyperionPriority);
          ClearPriority(hyperionPriorityStaticColor);
          break;
        case ContentEffect.StaticColor:
          ChangeColor(staticColor[0], staticColor[1], staticColor[2]);
          break;
      }
      return true;
    }
    public void ChangeProfile()
    {
      return;
    }
    public void ChangeImage(byte[] pixeldata, byte[] dummy)
    {
      // Hyperion expects the bytestring to be the size of 3*width*height.
      // So 3 bytes per pixel, as in RGB.
      // Given pixeldata however is 4 bytes per pixel, as in RGBA.
      // So we need to remove the last byte per pixel.
      byte[] newpixeldata = new byte[Core.GetCaptureHeight() * Core.GetCaptureWidth() * 3];
      int x = 0;
      int i = 0;
      while (i <= (newpixeldata.GetLength(0) - 2))
      {
        newpixeldata[i] = pixeldata[i + x + 2];
        newpixeldata[i + 1] = pixeldata[i + x + 1];
        newpixeldata[i + 2] = pixeldata[i + x];
        i += 3;
        x++;
      }

      ImageRequest imageRequest = ImageRequest.CreateBuilder()
        .SetImagedata(Google.ProtocolBuffers.ByteString.CopyFrom(newpixeldata))
        .SetImageheight(Core.GetCaptureHeight())
        .SetImagewidth(Core.GetCaptureWidth())
        .SetPriority(hyperionPriority)
        .SetDuration(-1)
        .Build();

      HyperionRequest request = HyperionRequest.CreateBuilder()
        .SetCommand(HyperionRequest.Types.Command.IMAGE)
        .SetExtension(ImageRequest.ImageRequest_, imageRequest)
        .Build();

      SendRequest(request);
    }

    private void SendRequest(HyperionRequest request)
    {
      if (Socket.Connected)
      {
        int size = request.SerializedSize;

        Byte[] header = new byte[4];
        header[0] = (byte)((size >> 24) & 0xFF);
        header[1] = (byte)((size >> 16) & 0xFF);
        header[2] = (byte)((size >> 8) & 0xFF);
        header[3] = (byte)((size) & 0xFF);

        int headerSize = header.Count();
        Stream.Write(header, 0, headerSize);
        request.WriteTo(Stream);
        Stream.Flush();
        HyperionReply reply = receiveReply();
      }
      else
      {
        Connected = false;
        Connect();
      }
    }

    private HyperionReply receiveReply()
    {
      Stream input = Socket.GetStream();
      byte[] header = new byte[4];
      input.Read(header, 0, 4);
      int size = (header[0] << 24) | (header[1] << 16) | (header[2] << 8) | (header[3]);
      byte[] data = new byte[size];
      input.Read(data, 0, size);
      HyperionReply reply = HyperionReply.ParseFrom(data);
      return reply;
    }
    #endregion

    #region settings
    public void setHyperionIP(string ip)
    {
      hyperionIP = ip;
    }
    public void setHyperionPort(int port)
    {
      hyperionPort = port;
    }
    public void SetStaticColor(int red, int green, int blue)
    {
      staticColor[0] = red;
      staticColor[1] = green;
      staticColor[2] = blue;
    }
    public void setHyperionPriority(int priority)
    {
      hyperionPriority = priority;
    }
    public void setHyperionPriorityStaticColor(int priority)
    {
      hyperionPriorityStaticColor = priority;
    }
    public void setReconnectOnError(Boolean reconnectOnError)
    {
      hyperionReconnectOnError = reconnectOnError;
    }
    #endregion
  }
}
