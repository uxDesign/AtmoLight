﻿using System;
using MediaPortal.Profile;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AtmoLight
{
  #region class Win32API
  public sealed class Win32API
  {
    private const int MAX_PATH = 260;

    public enum CSIDL
    {
      CSIDL_DESKTOP = 0x0000,          // <desktop>
      CSIDL_INTERNET = 0x0001,         // Internet Explorer (icon on desktop)
      CSIDL_PROGRAMS = 0x0002,         // Start Menu\Programs
      CSIDL_CONTROLS = 0x0003,         // My Computer\Control Panel
      CSIDL_PRINTERS = 0x0004,         // My Computer\Printers
      CSIDL_PERSONAL = 0x0005,         // My Documents
      CSIDL_FAVORITES = 0x0006,        // <user name>\Favorites
      CSIDL_STARTUP = 0x0007,          // Start Menu\Programs\Startup
      CSIDL_RECENT = 0x0008,           // <user name>\Recent
      CSIDL_SENDTO = 0x0009,           // <user name>\SendTo
      CSIDL_BITBUCKET = 0x000a,        // <desktop>\Recycle Bin
      CSIDL_STARTMENU = 0x000b,        // <user name>\Start Menu
      CSIDL_MYDOCUMENTS = 0x000c,      // logical "My Documents" desktop icon
      CSIDL_MYMUSIC = 0x000d,          // "My Music" folder
      CSIDL_MYVIDEO = 0x000e,          // "My Videos" folder
      CSIDL_DESKTOPDIRECTORY = 0x0010,        // <user name>\Desktop
      CSIDL_DRIVES = 0x0011,                  // My Computer
      CSIDL_NETWORK = 0x0012,                 // Network Neighborhood (My Network Places)
      CSIDL_NETHOOD = 0x0013,                 // <user name>\nethood
      CSIDL_FONTS = 0x0014,                   // windows\fonts
      CSIDL_TEMPLATES = 0x0015,
      CSIDL_COMMON_STARTMENU = 0x0016,        // All Users\Start Menu
      CSIDL_COMMON_PROGRAMS = 0X0017,         // All Users\Start Menu\Programs
      CSIDL_COMMON_STARTUP = 0x0018,          // All Users\Startup
      CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019, // All Users\Desktop
      CSIDL_APPDATA = 0x001a,                 // <user name>\Application Data
      CSIDL_PRINTHOOD = 0x001b,               // <user name>\PrintHood
      CSIDL_LOCAL_APPDATA = 0x001c,           // <user name>\Local Settings\Applicaiton Data (non roaming)
      CSIDL_ALTSTARTUP = 0x001d,              // non localized startup
      CSIDL_COMMON_ALTSTARTUP = 0x001e,       // non localized common startup
      CSIDL_COMMON_FAVORITES = 0x001f,
      CSIDL_INTERNET_CACHE = 0x0020,
      CSIDL_COOKIES = 0x0021,
      CSIDL_HISTORY = 0x0022,
      CSIDL_COMMON_APPDATA = 0x0023,          // All Users\Application Data
      CSIDL_WINDOWS = 0x0024,                 // GetWindowsDirectory()
      CSIDL_SYSTEM = 0x0025,                  // GetSystemDirectory()
      CSIDL_PROGRAM_FILES = 0x0026,           // C:\Program Files
      CSIDL_MYPICTURES = 0x0027,              // C:\Program Files\My Pictures
      CSIDL_PROFILE = 0x0028,                 // USERPROFILE
      CSIDL_SYSTEMX86 = 0x0029,               // x86 system directory on RISC
      CSIDL_PROGRAM_FILESX86 = 0x002a,        // x86 C:\Program Files on RISC
      CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common
      CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c, // x86 Program Files\Common on RISC
      CSIDL_COMMON_TEMPLATES = 0x002d,        // All Users\Templates
      CSIDL_COMMON_DOCUMENTS = 0x002e,        // All Users\Documents
      CSIDL_COMMON_ADMINTOOLS = 0x002f,       // All Users\Start Menu\Programs\Administrative Tools
      CSIDL_ADMINTOOLS = 0x0030,              // <user name>\Start Menu\Programs\Administrative Tools
      CSIDL_CONNECTIONS = 0x0031,             // Network and Dial-up Connections
      CSIDL_COMMON_MUSIC = 0x0035,            // All Users\My Music
      CSIDL_COMMON_PICTURES = 0x0036,         // All Users\My Pictures
      CSIDL_COMMON_VIDEO = 0x0037,            // All Users\My Video
      CSIDL_CDBURN_AREA = 0x003b              // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
    }

    [DllImport("shell32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

    [DllImport("shell32.dll", SetLastError = true)]
    static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

    public static string GetSpecialFolder(CSIDL nFolder)
    {
      IntPtr ptrAppData = IntPtr.Zero;
      SHGetSpecialFolderLocation(IntPtr.Zero, nFolder, ref ptrAppData);
      string dirAppData = "";
      StringBuilder sbAppData = new StringBuilder(MAX_PATH);

      if (true == SHGetPathFromIDListW(ptrAppData, sbAppData))
      {
        dirAppData = sbAppData.ToString();
      }

      sbAppData = null;
      Marshal.FreeCoTaskMem(ptrAppData);
      return dirAppData;
    }

  }
  #endregion
  public class Settings
  {
    #region Config variables
    // Generic
    public static ContentEffect effectVideo;
    public static ContentEffect effectMusic;
    public static ContentEffect effectRadio;
    public static ContentEffect effectMenu;
    public static ContentEffect effectMPExit;
    public static int killButton = 0;
    public static int profileButton = 0;
    public static int menuButton = 0;
    public static bool sbs3dOn = false;
    public static bool manualMode = false;
    public static bool lowCPU = false;
    public static int lowCPUTime = 0;
    public static bool delay = false;
    public static int delayReferenceTime = 0;
    public static int delayReferenceRefreshRate = 0;
    public static DateTime excludeTimeStart;
    public static DateTime excludeTimeEnd;
    public static int staticColorRed = 0;
    public static int staticColorGreen = 0;
    public static int staticColorBlue = 0;
    public static bool restartOnError = true;
    public static bool blackbarDetection = false;
    public static int blackbarDetectionTime = 0;
    public static int blackbarDetectionThreshold;
    public static string gifFile = "";
    public static int captureWidth = 0;
    public static int captureHeight = 0;
    public static int powerModeChangedDelay;
    public static int vuMeterMindB;
    public static double vuMeterMinHue;
    public static double vuMeterMaxHue;
    public static string currentLanguageFile;

    // Atmowin
    public static bool atmoWinTarget;
    public static string atmowinExe = "";
    public static bool startAtmoWin = true;
    public static bool exitAtmoWin = true;

    // Boblight
    public static bool boblightTarget;
    public static string boblightIP;
    public static int boblightPort;
    public static int boblightMaxFPS;
    public static int boblightMaxReconnectAttempts;
    public static int boblightReconnectDelay;
    public static int boblightSpeed;
    public static int boblightAutospeed;
    public static bool boblightInterpolation;
    public static int boblightSaturation;
    public static int boblightValue;
    public static int boblightThreshold;
    public static double boblightGamma;

    // Hyperion
    public static bool hyperionTarget;
    public static string hyperionIP = "";
    public static int hyperionPort = 0;
    public static int hyperionPriority = 0;
    public static int hyperionReconnectDelay = 0;
    public static int hyperionReconnectAttempts = 0;
    public static int hyperionPriorityStaticColor = 0;
    public static bool hyperionLiveReconnect;

    // Hue
    public static bool hueTarget;
    public static string hueExe = "";
    public static bool hueStart;
    public static bool hueIsRemoteMachine;
    public static string hueIP = "";
    public static int huePort = 0;
    public static int hueReconnectDelay = 0;
    public static int hueReconnectAttempts = 0;
    public static bool hueBridgeEnableOnResume;
    public static bool hueBridgeDisableOnSuspend;
    public static int hueMinDiversion;
    public static int hueThreshold;
    public static int hueBlackThreshold;
    public static double hueSaturation;
    public static bool hueUseOverallLightness;

    // AmbiBox
    public static bool ambiBoxTarget;
    public static string ambiBoxIP;
    public static int ambiBoxPort;
    public static int ambiBoxMaxReconnectAttempts;
    public static int ambiBoxReconnectDelay;
    public static string ambiBoxMediaPortalProfile;
    public static string ambiBoxExternalProfile;
    public static string ambiBoxPath;
    public static bool ambiBoxAutoStart;
    public static bool ambiBoxAutoStop;

    // AtmoOrb
    public static bool atmoOrbTarget;
    public static int atmoOrbBroadcastPort;
    public static int atmoOrbThreshold;
    public static int atmoOrbMinDiversion;
    public static double atmoOrbSaturation;
    public static double atmoOrbGamma;
    public static int atmoOrbBlackThreshold;
    public static bool atmoOrbUseOverallLightness;
    public static List<string> atmoOrbLamps = new List<string>();


    #endregion

    public static DateTime LoadTimeSetting(MediaPortal.Profile.Settings reader, string name, string defaultTime)
    {
      string s = reader.GetValueAsString("atmolight", name, defaultTime);
      DateTime dt;
      if (!DateTime.TryParse(s, out dt))
        dt = DateTime.Parse(defaultTime);
      return dt;
    }


    public static void LoadSettings()
    {
      using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        // Legacy support
        // The effect settings were integers in the past, but now are strings.
        // In order to avoid a lot of people loosing their effect settings during an update,
        // we convert the old settings to the new ones.
        int effectVideoInt;
        if (int.TryParse(reader.GetValueAsString("atmolight", "effectVideo", "MediaPortalLiveMode"), out effectVideoInt))
        {
          effectVideo = OldIntToNewContentEffect(effectVideoInt);
          SaveSpecificSetting("effectVideo", effectVideo.ToString());
        }
        else
        {
          effectVideo = (ContentEffect)Enum.Parse(typeof(ContentEffect), reader.GetValueAsString("atmolight", "effectVideo", "MediaPortalLiveMode"));
        }

        int effectMusicInt;
        if (int.TryParse(reader.GetValueAsString("atmolight", "effectMusic", "LEDsDisabled"), out effectMusicInt))
        {
          effectMusic = OldIntToNewContentEffect(effectMusicInt);
          SaveSpecificSetting("effectMusic", effectMusic.ToString());
        }
        else
        {
          effectMusic = (ContentEffect)Enum.Parse(typeof(ContentEffect), reader.GetValueAsString("atmolight", "effectMusic", "LEDsDisabled"));
        }

        int effecRadioInt;
        if (int.TryParse(reader.GetValueAsString("atmolight", "effectRadio", "LEDsDisabled"), out effecRadioInt))
        {
          effectRadio = OldIntToNewContentEffect(effecRadioInt);
          SaveSpecificSetting("effectRadio", effectRadio.ToString());
        }
        else
        {
          effectRadio = (ContentEffect)Enum.Parse(typeof(ContentEffect), reader.GetValueAsString("atmolight", "effectRadio", "LEDsDisabled"));
        }

        int effectMenuInt;
        if (int.TryParse(reader.GetValueAsString("atmolight", "effectMenu", "LEDsDisabled"), out effectMenuInt))
        {
          effectMenu = OldIntToNewContentEffect(effectMenuInt);
          SaveSpecificSetting("effectMenu", effectMenu.ToString());
        }
        else
        {
          effectMenu = (ContentEffect)Enum.Parse(typeof(ContentEffect), reader.GetValueAsString("atmolight", "effectMenu", "LEDsDisabled"));
        }

        int effectMPExitInt;
        if (int.TryParse(reader.GetValueAsString("atmolight", "effectMPExit", "LEDsDisabled"), out effectMPExitInt))
        {
          effectMPExit = OldIntToNewContentEffect(effectMPExitInt == 4 ? 5 : effectMPExitInt);
          SaveSpecificSetting("effectMPExit", effectMPExit.ToString());
        }
        else
        {
          effectMPExit = (ContentEffect)Enum.Parse(typeof(ContentEffect), reader.GetValueAsString("atmolight", "effectMPExit", "LEDsDisabled"));
        }
        
        // Normal settings loading
        currentLanguageFile = reader.GetValueAsString("atmolight", "CurrentLanguageFile", Win32API.GetSpecialFolder(Win32API.CSIDL.CSIDL_COMMON_APPDATA) + "\\Team MediaPortal\\MediaPortal\\language\\Atmolight\\en.xml");
        atmowinExe = reader.GetValueAsString("atmolight", "atmowinexe", "");
        killButton = reader.GetValueAsInt("atmolight", "killbutton", 4);
        profileButton = reader.GetValueAsInt("atmolight", "cmbutton", 4);
        menuButton = reader.GetValueAsInt("atmolight", "menubutton", 4);
        excludeTimeStart = LoadTimeSetting(reader, "excludeTimeStart", "00:00");
        excludeTimeEnd = LoadTimeSetting(reader, "excludeTimeEnd", "00:00");
        manualMode = reader.GetValueAsBool("atmolight", "OffOnStart", false);
        sbs3dOn = reader.GetValueAsBool("atmolight", "SBS_3D_ON", false);
        lowCPU = reader.GetValueAsBool("atmolight", "lowCPU", false);
        lowCPUTime = reader.GetValueAsInt("atmolight", "lowCPUTime", 0);
        delay = reader.GetValueAsBool("atmolight", "Delay", false);
        delayReferenceTime = reader.GetValueAsInt("atmolight", "DelayTime", 0);
        exitAtmoWin = reader.GetValueAsBool("atmolight", "ExitAtmoWin", true);
        startAtmoWin = reader.GetValueAsBool("atmolight", "StartAtmoWin", true);
        staticColorRed = reader.GetValueAsInt("atmolight", "StaticColorRed", 0);
        staticColorGreen = reader.GetValueAsInt("atmolight", "StaticColorGreen", 0);
        staticColorBlue = reader.GetValueAsInt("atmolight", "StaticColorBlue", 0);
        restartOnError = reader.GetValueAsBool("atmolight", "RestartOnError", true);
        delayReferenceRefreshRate = reader.GetValueAsInt("atmolight", "DelayRefreshRate", 50);
        blackbarDetection = reader.GetValueAsBool("atmolight", "BlackbarDetection", false);
        blackbarDetectionTime = reader.GetValueAsInt("atmolight", "BlackbarDetectionTime", 1000);
        gifFile = reader.GetValueAsString("atmolight", "GIFFile", "");
        captureWidth = reader.GetValueAsInt("atmolight", "captureWidth", 64);
        captureHeight = reader.GetValueAsInt("atmolight", "captureHeight", 64);
        hyperionIP = reader.GetValueAsString("atmolight", "hyperionIP", "127.0.0.1");
        hyperionPort = reader.GetValueAsInt("atmolight", "hyperionPort", 19445);
        hyperionReconnectDelay = reader.GetValueAsInt("atmolight", "hyperionReconnectDelay", 10000);
        hyperionReconnectAttempts = reader.GetValueAsInt("atmolight", "hyperionReconnectAttempts", 5);
        hyperionPriority = reader.GetValueAsInt("atmolight", "hyperionPriority", 1);
        hyperionPriorityStaticColor = reader.GetValueAsInt("atmolight", "hyperionStaticColorPriority", 1);
        hyperionLiveReconnect = reader.GetValueAsBool("atmolight", "hyperionLiveReconnect", false);
        hueExe = reader.GetValueAsString("atmolight", "hueExe", "");
        hueStart = reader.GetValueAsBool("atmolight", "hueStart", true);
        hueIsRemoteMachine = reader.GetValueAsBool("atmolight", "hueIsRemoteMachine", false);
        hueIP = reader.GetValueAsString("atmolight", "hueIP", "127.0.0.1");
        huePort = reader.GetValueAsInt("atmolight", "huePort", 20123);
        hueReconnectDelay = reader.GetValueAsInt("atmolight", "hueReconnectDelay", 10000);
        hueReconnectAttempts = reader.GetValueAsInt("atmolight", "hueReconnectAttempts", 5);
        hueBridgeEnableOnResume = reader.GetValueAsBool("atmolight", "hueBridgeEnableOnResume", false);
        hueBridgeDisableOnSuspend = reader.GetValueAsBool("atmolight", "hueBridgeDisableOnSuspend", false);
        boblightIP = reader.GetValueAsString("atmolight", "boblightIP", "127.0.0.1");
        boblightPort = reader.GetValueAsInt("atmolight", "boblightPort", 19333);
        boblightMaxFPS = reader.GetValueAsInt("atmolight", "boblightMaxFPS", 10);
        boblightMaxReconnectAttempts = reader.GetValueAsInt("atmolight", "boblightMaxReconnectAttempts", 5);
        boblightReconnectDelay = reader.GetValueAsInt("atmolight", "boblightReconnectDelay", 5000);
        boblightSpeed = reader.GetValueAsInt("atmolight", "boblightSpeed", 100);
        boblightAutospeed = reader.GetValueAsInt("atmolight", "boblightAutospeed", 0);
        boblightInterpolation = reader.GetValueAsBool("atmolight", "boblightInterpolation", true);
        boblightSaturation = reader.GetValueAsInt("atmolight", "boblightSaturation", 1);
        boblightValue = reader.GetValueAsInt("atmolight", "boblightValue", 1);
        boblightThreshold = reader.GetValueAsInt("atmolight", "boblightThreshold", 20);
        boblightGamma = Double.Parse(reader.GetValueAsString("atmolight", "boblightGamma", "2.2").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        atmoWinTarget = reader.GetValueAsBool("atmolight", "atmoWinTarget", true);
        boblightTarget = reader.GetValueAsBool("atmolight", "boblightTarget", false);
        hueTarget = reader.GetValueAsBool("atmolight", "hueTarget", false);
        hyperionTarget = reader.GetValueAsBool("atmolight", "hyperionTarget", false);
        blackbarDetectionThreshold = reader.GetValueAsInt("atmolight", "blackbarDetectionThreshold", 20);
        powerModeChangedDelay = reader.GetValueAsInt("atmolight", "powerModeChangedDelay", 5000);
        ambiBoxTarget = reader.GetValueAsBool("atmolight", "ambiBoxTarget", false);
        ambiBoxIP = reader.GetValueAsString("atmolight", "ambiBoxIP", "127.0.0.1");
        ambiBoxPort = reader.GetValueAsInt("atmolight", "ambiBoxPort", 3636);
        ambiBoxMaxReconnectAttempts = reader.GetValueAsInt("atmolight", "ambiBoxMaxReconnectAttempts", 5);
        ambiBoxReconnectDelay = reader.GetValueAsInt("atmolight", "ambiBoxReconnectDelay", 5000);
        ambiBoxMediaPortalProfile = reader.GetValueAsString("atmolight", "ambiBoxMediaPortalProfile", "MediaPortal");
        ambiBoxExternalProfile = reader.GetValueAsString("atmolight", "ambiBoxExternalProfile", "External");
        ambiBoxPath = reader.GetValueAsString("atmolight", "ambiBoxPath", "C:\\Program Files (x86)\\AmbiBox\\AmbiBox.exe");
        ambiBoxAutoStart = reader.GetValueAsBool("atmolight", "ambiBoxAutoStart", false);
        ambiBoxAutoStop = reader.GetValueAsBool("atmolight", "ambiBoxAutoStop", false);
        atmoOrbTarget = reader.GetValueAsBool("atmolight", "atmoOrbTarget", false);
        atmoOrbBlackThreshold = reader.GetValueAsInt("atmolight", "atmoOrbBlackThreshold", 16);
        atmoOrbBroadcastPort = reader.GetValueAsInt("atmolight", "atmoOrbBroadcastPort", 49692);
        atmoOrbGamma = Double.Parse(reader.GetValueAsString("atmolight", "atmoOrbGamma", "1").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        atmoOrbMinDiversion = reader.GetValueAsInt("atmolight", "atmoOrbMinDiversion", 16);
        atmoOrbSaturation = Double.Parse(reader.GetValueAsString("atmolight", "atmoOrbSaturation", "0.2").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        atmoOrbThreshold = reader.GetValueAsInt("atmolight", "atmoOrbThreshold", 0);
        atmoOrbUseOverallLightness = reader.GetValueAsBool("atmolight", "atmoOrbUseOverallLightness", true);
        string atmoOrbLampTemp = reader.GetValueAsString("atmolight", "atmoOrbLamps", "");
        string[] atmoOrbLampTempSplit = atmoOrbLampTemp.Split('|');
        for (int i = 0; i < atmoOrbLampTempSplit.Length; i++)
        {
          if (!string.IsNullOrEmpty(atmoOrbLampTempSplit[i]))
          {
            atmoOrbLamps.Add(atmoOrbLampTempSplit[i]);
          }
        }
        vuMeterMindB = reader.GetValueAsInt("atmolight", "vuMeterMindB", -24);
        vuMeterMinHue = Double.Parse(reader.GetValueAsString("atmolight", "vuMeterMinHue", "0,74999").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        vuMeterMaxHue = Double.Parse(reader.GetValueAsString("atmolight", "vuMeterMaxHue", "0,95833").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        hueThreshold = reader.GetValueAsInt("atmolight", "hueThreshold", 16);
        hueBlackThreshold = reader.GetValueAsInt("atmolight", "hueBlackThreshold", 16);
        hueMinDiversion = reader.GetValueAsInt("atmolight", "hueMinDiversion", 16);
        hueSaturation = Double.Parse(reader.GetValueAsString("atmolight", "hueSaturation", "0.2").Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
        hueUseOverallLightness = reader.GetValueAsBool("atmolight", "hueUseOverallLightness", true);

      }
    }
    public static void SaveSettings()
    {
      using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("atmolight", "atmowinexe", atmowinExe);
        reader.SetValue("atmolight", "effectVideo", effectVideo.ToString());
        reader.SetValue("atmolight", "effectMusic", effectMusic.ToString());
        reader.SetValue("atmolight", "effectRadio", effectRadio.ToString());
        reader.SetValue("atmolight", "effectMenu", effectMenu.ToString());
        reader.SetValue("atmolight", "effectMPExit", effectMPExit.ToString());
        reader.SetValue("atmolight", "killbutton", (int)killButton);
        reader.SetValue("atmolight", "cmbutton", (int)profileButton);
        reader.SetValue("atmolight", "menubutton", (int)menuButton);
        reader.SetValueAsBool("atmolight", "OffOnStart", manualMode);
        reader.SetValueAsBool("atmolight", "SBS_3D_ON", sbs3dOn);
        reader.SetValueAsBool("atmolight", "lowCPU", lowCPU);
        reader.SetValue("atmolight", "lowCPUTime", lowCPUTime);
        reader.SetValueAsBool("atmolight", "Delay", delay);
        reader.SetValue("atmolight", "DelayTime", delayReferenceTime);
        reader.SetValueAsBool("atmolight", "ExitAtmoWin", exitAtmoWin);
        reader.SetValueAsBool("atmolight", "StartAtmoWin", startAtmoWin);
        reader.SetValue("atmolight", "excludeTimeStart", excludeTimeStart.ToString("HH:mm"));
        reader.SetValue("atmolight", "excludeTimeEnd", excludeTimeEnd.ToString("HH:mm"));
        reader.SetValue("atmolight", "CurrentLanguageFile", currentLanguageFile);
        reader.SetValue("atmolight", "StaticColorRed", staticColorRed);
        reader.SetValue("atmolight", "StaticColorGreen", staticColorGreen);
        reader.SetValue("atmolight", "StaticColorBlue", staticColorBlue);
        reader.SetValueAsBool("atmolight", "RestartOnError", restartOnError);
        reader.SetValue("atmolight", "DelayRefreshRate", delayReferenceRefreshRate);
        reader.SetValueAsBool("atmolight", "BlackbarDetection", blackbarDetection);
        reader.SetValue("atmolight", "BlackbarDetectionTime", blackbarDetectionTime);
        reader.SetValue("atmolight", "GIFFile", gifFile);
        reader.SetValue("atmolight", "captureWidth", (int)captureWidth);
        reader.SetValue("atmolight", "captureHeight", (int)captureHeight);
        reader.SetValue("atmolight", "hyperionIP", hyperionIP);
        reader.SetValue("atmolight", "hyperionPort", (int)hyperionPort);
        reader.SetValue("atmolight", "hyperionPriority", (int)hyperionPriority);
        reader.SetValue("atmolight", "hyperionReconnectDelay", (int)hyperionReconnectDelay);
        reader.SetValue("atmolight", "hyperionReconnectAttempts", (int)hyperionReconnectAttempts);
        reader.SetValue("atmolight", "hyperionStaticColorPriority", (int)hyperionPriorityStaticColor);
        reader.SetValueAsBool("atmolight", "hyperionLiveReconnect", hyperionLiveReconnect);
        reader.SetValue("atmolight", "hueExe", hueExe);
        reader.SetValueAsBool("atmolight", "hueStart", hueStart);
        reader.SetValueAsBool("atmolight", "hueIsRemoteMachine", hueIsRemoteMachine);
        reader.SetValue("atmolight", "hueIP", hueIP);
        reader.SetValue("atmolight", "huePort", (int)huePort);
        reader.SetValue("atmolight", "hueReconnectDelay", (int)hueReconnectDelay);
        reader.SetValue("atmolight", "hueReconnectAttempts", (int)hueReconnectAttempts);
        reader.SetValueAsBool("atmolight", "hueBridgeEnableOnResume", hueBridgeEnableOnResume);
        reader.SetValueAsBool("atmolight", "hueBridgeDisableOnSuspend", hueBridgeDisableOnSuspend);
        reader.SetValue("atmolight", "boblightIP", boblightIP);
        reader.SetValue("atmolight", "boblightPort", boblightPort);
        reader.SetValue("atmolight", "boblightMaxFPS", boblightMaxFPS);
        reader.SetValue("atmolight", "boblightMaxReconnectAttempts", boblightMaxReconnectAttempts);
        reader.SetValue("atmolight", "boblightReconnectDelay", boblightReconnectDelay);
        reader.SetValue("atmolight", "boblightSpeed", boblightSpeed);
        reader.SetValue("atmolight", "boblightAutospeed", boblightAutospeed);
        reader.SetValue("atmolight", "boblightSaturation", boblightSaturation);
        reader.SetValue("atmolight", "boblightValue", boblightValue);
        reader.SetValue("atmolight", "boblightThreshold", boblightThreshold);
        reader.SetValueAsBool("atmolight", "boblightInterpolation", boblightInterpolation);
        reader.SetValue("atmolight", "boblightGamma", boblightGamma.ToString());
        reader.SetValue("atmolight", "blackbarDetectionThreshold", blackbarDetectionThreshold.ToString());
        reader.SetValue("atmolight", "powerModeChangedDelay", powerModeChangedDelay.ToString());
        reader.SetValue("atmolight", "ambiBoxIP", ambiBoxIP.ToString());
        reader.SetValue("atmolight", "ambiBoxPort", ambiBoxPort.ToString());
        reader.SetValue("atmolight", "ambiBoxMaxReconnectAttempts", ambiBoxMaxReconnectAttempts.ToString());
        reader.SetValue("atmolight", "ambiBoxReconnectDelay", ambiBoxReconnectDelay.ToString());
        reader.SetValue("atmolight", "ambiBoxMediaPortalProfile", ambiBoxMediaPortalProfile.ToString());
        reader.SetValue("atmolight", "ambiBoxExternalProfile", ambiBoxExternalProfile.ToString());
        reader.SetValue("atmolight", "ambiBoxPath", ambiBoxPath.ToString());
        reader.SetValueAsBool("atmolight", "ambiBoxAutoStart", ambiBoxAutoStart);
        reader.SetValueAsBool("atmolight", "ambiBoxAutoStop", ambiBoxAutoStop);
        reader.SetValue("atmolight", "vuMeterMindB", vuMeterMindB.ToString());
        reader.SetValue("atmolight", "vuMeterMinHue", vuMeterMinHue.ToString());
        reader.SetValue("atmolight", "vuMeterMaxHue", vuMeterMaxHue.ToString());
        reader.SetValueAsBool("atmolight", "atmoWinTarget", atmoWinTarget);
        reader.SetValueAsBool("atmolight", "boblightTarget", boblightTarget);
        reader.SetValueAsBool("atmolight", "hueTarget", hueTarget);
        reader.SetValueAsBool("atmolight", "hyperionTarget", hyperionTarget);
        reader.SetValueAsBool("atmolight", "ambiBoxTarget", ambiBoxTarget);
        reader.SetValueAsBool("atmolight", "atmoOrbTarget", atmoOrbTarget);
        reader.SetValue("atmolight", "atmoOrbBlackThreshold", atmoOrbBlackThreshold.ToString());
        reader.SetValue("atmolight", "atmoOrbBroadcastPort", atmoOrbBroadcastPort.ToString());
        reader.SetValue("atmolight", "atmoOrbGamma", atmoOrbGamma.ToString());
        reader.SetValue("atmolight", "atmoOrbMinDiversion", atmoOrbMinDiversion.ToString());
        reader.SetValue("atmolight", "atmoOrbSaturation", atmoOrbSaturation.ToString());
        reader.SetValue("atmolight", "atmoOrbThreshold", atmoOrbThreshold.ToString());
        reader.SetValueAsBool("atmolight", "atmoOrbUseOverallLightness", atmoOrbUseOverallLightness);
        string atmoOrbLampsTemp = "";
        for (int i = 0; i < atmoOrbLamps.Count; i++)
        {
          if (i > 0)
          {
            atmoOrbLampsTemp += "|";
          }
          atmoOrbLampsTemp += atmoOrbLamps[i];
        }
        reader.SetValue("atmolight", "atmoOrbLamps", atmoOrbLampsTemp);
        reader.SetValue("atmolight", "hueMinDiversion", hueMinDiversion.ToString());
        reader.SetValue("atmolight", "hueThreshold", hueThreshold.ToString());
        reader.SetValue("atmolight", "hueBlackThreshold", hueBlackThreshold.ToString());
        reader.SetValue("atmolight", "hueSaturation", hueSaturation.ToString());
        reader.SetValueAsBool("atmolight", "hueUseOverallLightness", hueUseOverallLightness);
      }
    }

    public static void SaveSpecificSetting(string Setting, String Value)
    {
      using (MediaPortal.Profile.Settings reader = new MediaPortal.Profile.Settings(MediaPortal.Configuration.Config.GetFile(MediaPortal.Configuration.Config.Dir.Config, "MediaPortal.xml")))
      {
        reader.SetValue("atmolight", Setting, Value);
      }
    }

    // Legacy Support
    /// <summary>
    /// Converts old ContentEffect integer (before rework) into new ContentEffect.
    /// This is done to not lose settings after updating AtmoLight.
    /// </summary>
    /// <param name="effectInt"></param>
    /// <returns></returns>
    private static ContentEffect OldIntToNewContentEffect(int effectInt)
    {
      switch (effectInt)
      {
        case 0:
          return ContentEffect.LEDsDisabled;
        case 1:
          return ContentEffect.ExternalLiveMode;
        case 2:
          return ContentEffect.AtmoWinColorchanger;
        case 3:
          return ContentEffect.AtmoWinColorchangerLR;
        case 4:
          return ContentEffect.MediaPortalLiveMode;
        case 5:
          return ContentEffect.StaticColor;
        case 6:
          return ContentEffect.GIFReader;
        case 7:
          return ContentEffect.VUMeter;
        case 8:
          return ContentEffect.VUMeterRainbow;
        default:
          return ContentEffect.LEDsDisabled;
      }
    }
  }
}
