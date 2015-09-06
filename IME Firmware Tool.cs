#pragma warning disable 1998
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using IME_Firmware_Tool.Properties;
using System.IO.Compression;
using System.Threading;
using System.Security;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace IME_Firmware_Tool
{
    public partial class IMEFirmwareTool : Form
    {
        #region Vars
        string InstalledMEFirmwareVersion, ServerFileExtension, ServerVersion;
        string[] Args;
        Uri UpdateDownloadLink, MainLink = new Uri("http://www.win-raid.com/t596f39-Intel-Management-Engine-Drivers-Firmware-amp-System-Tools.html");
        Settings Settings = new Settings();
        string OneDotFiveMB = "1.5MB";
        string FiveMB = "5MB";
        WebClient DownloadUpdateClient = new WebClient();
        bool FlashingUpdateThatWasDownloaded = false, OneDotFiveMBFound = false, FiveMBFound = false;
        WebClient DebugOrReleaseProgramUpdate = new WebClient();

        #region Var Propertys
        private static string VersionPrivateWithDots = VersionUntouchedWithDots;
        private static long VersionPrivateWithoutDots = VersionUntouchedWithoutDots;
        public static int Major { get { return Assembly.GetEntryAssembly().GetName().Version.Major; } }
        public static int Minor { get { return Assembly.GetEntryAssembly().GetName().Version.Minor; } }
        public static int Build { get { return Assembly.GetEntryAssembly().GetName().Version.Build; } }
        public static int Revision { get { return Assembly.GetEntryAssembly().GetName().Version.Revision; } }
        public static string VersionUntouchedWithDots
        {
            get
            {
                if (Revision == 0 && Build == 0)
                    return string.Format("{0}.{1}", Major, Minor);
                else if (Revision == 0)
                    return string.Format("{0}.{1}.{2}", Major, Minor, Build);
                else
                    return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
            }
        }
        public static long VersionUntouchedWithoutDots { get { return long.Parse(string.Format("{0}{1}{2}{3}", Major, Minor, Build, Revision)); } }
        public static string VersionWithDots { get { return VersionPrivateWithDots; } set { VersionPrivateWithDots = value; } }
        public static long VersionWithoutDots { get { return VersionPrivateWithoutDots; } set { VersionPrivateWithoutDots = value; } }
        public static long ServerVersionWithoutDots { get; set; }
        public static string ServerVersionWithDots { get; set; }
        public static string ChangeLog { get; set; }
        public static bool ProgramUpdateChangeLog { get; set; }
        private static string BuildDatePrivate { get; set; }
        public static string BuildDate { get { return BuildDatePrivate; } set { BuildDatePrivate = value; } }
        public static string AssemblyName { get { return Assembly.GetEntryAssembly().GetName().Name; } }
        public static string ExecutablesFullPath { get { return Assembly.GetEntryAssembly().Location; } }
        public static string ExecutablesNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(ExecutablesFullPath); } }
        public static string ExecutablesNameWithExtension { get { return Path.GetFileName(ExecutablesFullPath); } }
        public static string InstalledDirectory { get { return Application.StartupPath; } }
        public static string CurrentActivity { get; set; }
        public static string ServerTemp { get; set; }
        public static string BuildDateOrReleaseDateFormatted { get; set; }
        public static Uri DebugEditionDownloadLink { get { return new Uri("https://dl.dropboxusercontent.com/s/176bxaxn4fekrh6/IME%20Firmware%20Tool.exe"); } }
        public static Uri ReleaseEditionDownloadLink { get { return new Uri("https://dl.dropboxusercontent.com/s/vfk3dfrjm0009b7/IME%20Firmware%20Tool.exe"); } }
        public static string ExactLocationOfProgram { get { return string.Format("{0}{1}{2}", InstalledDirectory, Expressions.BackSlash, ExecutablesNameWithExtension); } }
        public static string ProgramOldLocation { get { return string.Format("{0}{1}{2}", InstalledDirectory, Expressions.BeginsWith.Old, ExecutablesNameWithExtension); } }
        public static string ProgramTempFolderLocation { get { return string.Format("{0}{1}{2}", Path.GetTempPath(), Expressions.BackSlash, ExecutablesNameWithExtension); } }
        #endregion
        #endregion

        #region Main
        public IMEFirmwareTool(string[] Argss)
        {
            InitializeComponent();
            Args = Argss;

            if (Settings.NeedsSettingsUpgrade)
            {
                Settings.Upgrade();
                Settings.NeedsSettingsUpgrade = false;
            }

            InstalledMEFirmwareVersionLabel.Text = "Installed ME Firmware Version: Calculating";
            FirmwareType.Text = "Firmware Type: Calculating";
            if (File.Exists(string.Format(@"{0}\{1}", Application.StartupPath, "error.log")))
                File.Delete(string.Format(@"{0}\{1}", Application.StartupPath, "error.log"));
            Settings.PropertyChanged += Settings_PropertyChanged;
            ForceResetCheckBox.Checked = Settings.ForceReset;
            AllowFlashingOfSameFirmwareVersionCheckBox.Checked = Settings.AllowSameVersion;

            BuildDate = ProgramInformation.GetBuildDateTime(ExecutablesFullPath);
            ProgramVersionLabel.Text = string.Format("Version: {0}", VersionUntouchedWithDots);
            BuildDateLabel.Text = string.Format("Build Date: {0}", BuildDate);

            CopyrightLabel.Text = string.Format("Copyright: {0}", ProgramInformation.AssemblyCopyright);

            try
            {
                if (!Debugging.IsDebugging)
                {
                    if (File.Exists(string.Format(@"{0}\{1}.dll", Application.StartupPath, "SevenZipSharp")))
                        File.Delete(string.Format(@"{0}\{1}.dll", Application.StartupPath, "SevenZipSharp"));

                    File.WriteAllBytes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "SevenZipSharp"), Resources.SevenZipSharp);
                    File.SetAttributes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "SevenZipSharp"),
                            File.GetAttributes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "SevenZipSharp")) | FileAttributes.Hidden);
                }

                if (File.Exists(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z")))
                    File.Delete(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z"));

                File.WriteAllBytes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z"), Resources._7z);
                File.SetAttributes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z"),
                        File.GetAttributes(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z")) | FileAttributes.Hidden);
            }
            catch { }

            #region Program Shortcut
            if (!Directory.Exists(string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Programs), Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location))))
                Directory.CreateDirectory(string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Programs), Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)));
            IWshRuntimeLibrary.WshShell Shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut Shortcut = (IWshRuntimeLibrary.IWshShortcut)Shell.CreateShortcut(Path.Combine(string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)), string.Format("{0}.lnk", Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location))));
            Shortcut.Description = string.Format("{0} - {1}", Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location), ProgramInformation.AssemblyDescription);
            Shortcut.TargetPath = string.Format(@"{0}\{1}", Application.StartupPath, Path.GetFileName(Assembly.GetEntryAssembly().Location));
            Shortcut.Save();
            #endregion

            if (File.Exists(ProgramOldLocation))
                File.Delete(ProgramOldLocation);
        }
        void MEUpdateChecker_Load(object sender, EventArgs e)
        {
            bool StopThread = false;
            var AnimationThread = new Thread(new ThreadStart(async delegate
            {
                try
                {
                    while (true)
                    {
                        if (StopThread)
                            break;
                        else
                        {
                            await Task.Delay(110);
                            Image IMG = StartupLoadingPictureBox.Image;
                            IMG.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            Invoke(new Action(delegate { StartupLoadingPictureBox.Image = IMG; }));
                        }
                    }
                    Invoke(new Action(delegate { StartupLoadingPictureBox.Image = Resources.Update; }));
                }
                catch { }
                Application.ExitThread();
            }));
            AnimationThread.IsBackground = true;
            AnimationThread.Start();

            Task.Run(async delegate
            {
                #region Installed ME Version
                string ZipFileWereOn = "MEInfo-7x.zip";
            TryNextMEInfoVersion:
                try
                {
                    if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                        File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                }
                catch { }

                File.WriteAllBytes(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), ZipFileWereOn == "MEInfo-7x.zip" ? Resources.MEInfo_7x : ZipFileWereOn == "MEInfo-8x.zip" ? Resources.MEInfo_8x :
                                   ZipFileWereOn == "MEInfo-90x.zip" ? Resources.MEInfo_90x : ZipFileWereOn == "MEInfo-91x.zip" ? Resources.MEInfo_91x : ZipFileWereOn == "MEInfo-95x.zip" ? Resources.MEInfo_95x :
                                   ZipFileWereOn == "MEInfo-96x.zip" ? Resources.MEInfo_96x : Resources.MEInfo_10x);

                try
                {
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEInfo")))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEInfo"), true);
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
                }
                catch { }

                ZipFile.ExtractToDirectory(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), Path.GetTempPath());
                await Task.Delay(3);
                Directory.Move(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), string.Format(@"{0}{1}", Path.GetTempPath(), "MEInfo"));
                Process MEInfo = new Process();
                MEInfo.StartInfo.FileName = string.Format(@"{0}{1}\{2}", Path.GetTempPath(), "MEInfo", "MEInfoWin.exe");
                MEInfo.StartInfo.RedirectStandardInput = true;
                MEInfo.StartInfo.RedirectStandardOutput = true;
                MEInfo.StartInfo.RedirectStandardError = true;
                MEInfo.StartInfo.UseShellExecute = false;
                MEInfo.StartInfo.CreateNoWindow = true;
                MEInfo.Start();
                MEInfo.WaitForExit();

                try
                {
                    if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                        File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                }
                catch { }

                try
                {
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEInfo")))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEInfo"), true);
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
                }
                catch { }

                StringReader MEInfoData = new StringReader(MEInfo.StandardOutput.ReadToEnd());
                if (File.Exists(string.Format(@"{0}\{1}", Application.StartupPath, "error.log")))
                    File.Delete(string.Format(@"{0}\{1}", Application.StartupPath, "error.log"));
                string Data = null;
                bool UnsupportedMEInfo = false;
                while ((Data = MEInfoData.ReadLine()) != null)
                {
                    if (Data.ToLower().Contains("Unknown or unsupported hardware platform".ToLower()))
                    {
                        UnsupportedMEInfo = true;
                        break;
                    }
                    else if (Data.ToLower().Contains("FW Version".ToLower()))
                    {
                        InstalledMEFirmwareVersion = Regex.Replace(Data, "[^0-9.]", string.Empty).Trim();
                        try
                        {
                            MEInfo.Kill();
                            MEInfo.Dispose();
                        }
                        catch { }
                        Invoke(new Action(delegate { InstalledMEFirmwareVersionLabel.Text = string.Format("Installed IME Firmware Version: {0}", InstalledMEFirmwareVersion); }));
                    }
                }
                if (UnsupportedMEInfo || InstalledMEFirmwareVersion == null)
                {
                    try { MEInfo.Dispose(); }
                    catch { }

                    try
                    {
                        if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                            File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                    }
                    catch { }

                    if (ZipFileWereOn == "MEInfo-7x.zip")
                        ZipFileWereOn = "MEInfo-8x.zip";
                    else if (ZipFileWereOn == "MEInfo-8x.zip")
                        ZipFileWereOn = "MEInfo-90x.zip";
                    else if (ZipFileWereOn == "MEInfo-90x.zip")
                        ZipFileWereOn = "MEInfo-91x.zip";
                    else if (ZipFileWereOn == "MEInfo-91x.zip")
                        ZipFileWereOn = "MEInfo-95x.zip";
                    else if (ZipFileWereOn == "MEInfo-95x.zip")
                        ZipFileWereOn = "MEInfo-96x.zip";
                    else if (ZipFileWereOn == "MEInfo-96x.zip")
                        ZipFileWereOn = "MEInfo-10x.zip";
                    else
                    {
                        Invoke(new Action(delegate
                        {
                            MessageBox.Show("Your ME Firmware Version isn't compatible with any of the following versions 7x, 8x, 9x, or 10x, program cannot continue, will now close.", "Unsupported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }));
                    }
                    goto TryNextMEInfoVersion;
                }
                #endregion

                #region ME Type
                if (InstalledMEFirmwareVersion.StartsWith("7"))
                    ZipFileWereOn = "MEFlash-7x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("8"))
                    ZipFileWereOn = "MEFlash-8x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("9.0"))
                    ZipFileWereOn = "MEFlash-90x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("9.1"))
                    ZipFileWereOn = "MEFlash-91x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("9.5"))
                    ZipFileWereOn = "MEFlash-95x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("9.6"))
                    ZipFileWereOn = "MEFlash-96x.zip";
                else if (InstalledMEFirmwareVersion.StartsWith("10"))
                    ZipFileWereOn = "MEFlash-10x.zip";

                try
                {
                    if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                        File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                }
                catch { }

                File.WriteAllBytes(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), ZipFileWereOn == "MEFlash-7x.zip" ? Resources.MEFlash_7x : ZipFileWereOn == "MEFlash-8x.zip" ? Resources.MEFlash_8x :
                                   ZipFileWereOn == "MEFlash-90x.zip" ? Resources.MEFlash_90x : ZipFileWereOn == "MEFlash-91x.zip" ? Resources.MEFlash_91x : ZipFileWereOn == "MEFlash-95x.zip" ? Resources.MEFlash_95x :
                                   ZipFileWereOn == "MEFlash-96x.zip" ? Resources.MEFlash_96x : Resources.MEFlash_10x);

                try
                {
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"), true);
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
                }
                catch { }

                ZipFile.ExtractToDirectory(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), Path.GetTempPath());
                await Task.Delay(3);
                Directory.Move(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"));

                Process GetFileVersion = new Process();
                GetFileVersion.StartInfo.FileName = "cmd.exe";
                GetFileVersion.StartInfo.RedirectStandardInput = true;
                GetFileVersion.StartInfo.RedirectStandardOutput = true;
                GetFileVersion.StartInfo.RedirectStandardError = true;
                GetFileVersion.StartInfo.UseShellExecute = false;
                GetFileVersion.StartInfo.CreateNoWindow = true;
                GetFileVersion.Start();
                GetFileVersion.StandardInput.WriteLine("C:");
                GetFileVersion.StandardInput.WriteLine(string.Format("cd {0}", string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")));
                GetFileVersion.StandardInput.WriteLine("FWUpdLcl.exe -SAVE old.bin");
                GetFileVersion.StandardInput.WriteLine("exit");
                GetFileVersion.WaitForExit();

                var MESize = new FileInfo(string.Format(@"{0}{1}\old.bin", Path.GetTempPath(), "MEFlash")).Length;
                if (MESize <= 1572864)
                {
                    OneDotFiveMBFound = true;
                    Invoke(new Action(delegate { FirmwareType.Text = string.Format("Firmware Type: {0}", OneDotFiveMB); }));
                }
                else
                {
                    FiveMBFound = true;
                    Invoke(new Action(delegate { FirmwareType.Text = string.Format("Firmware Type: {0}", FiveMB); }));
                }

                try
                {
                    if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                        File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                }
                catch { }

                try
                {
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"), true);
                    if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                        Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
                }
                catch { }

                StopThread = true;
                Invoke(new Action(delegate
                {
                    StartupLoadingPictureBox.Visible = false;
                    CheckServerButton.Enabled = true;
                }));
                #endregion
            });
            try
            {
                if (Args[0] == "Update")
                {
                    Tabs.SelectedTab = AboutTab;
                    ProgramUpdatePictureBox_Click(this, new EventArgs());
                }
            }
            catch { }
        }
        void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Settings.Save();
        }
        async void ProgramUpdatePictureBox_Click(object sender, EventArgs e)
        {
            ProgramUpdatePictureBox.Enabled = false;
            ProgramVersionLabel.ForeColor = Color.Blue;
            BuildDateLabel.ForeColor = Color.Blue;
            CreatedByLabel.ForeColor = Color.Blue;
            EmailLabel.ForeColor = Color.Blue;
            CopyrightLabel.ForeColor = Color.Blue;

            bool StopThread = false;
            var AnimationThread = new Thread(new ThreadStart(async delegate
            {
                while (true)
                {
                    if (StopThread)
                        break;
                    else
                    {
                        await Task.Delay(110);
                        Image IMG = ProgramUpdatePictureBox.Image;
                        IMG.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        Invoke(new Action(delegate { ProgramUpdatePictureBox.Image = IMG; }));
                    }
                }
                Invoke(new Action(delegate
                {
                    ProgramUpdatePictureBox.Image = Resources.Update;
                    ProgramUpdatePictureBox.Enabled = true;
                }));
                Application.ExitThread();
            }));
            AnimationThread.IsBackground = true;
            AnimationThread.Start();

            await CheckForUpdates();
            StopThread = true;
        }
        async Task CheckForUpdates()
        {
            DebugAndReleaseUpdate Result = await CheckForDebugOrReleaseUpdates();
            if (Result == DebugAndReleaseUpdate.UpdateToDate)
            {
                File.Delete(ProgramTempFolderLocation);
                ProgramVersionLabel.ForeColor = Color.DarkGreen;
                BuildDateLabel.ForeColor = Color.DarkGreen;
                CreatedByLabel.ForeColor = Color.DarkGreen;
                EmailLabel.ForeColor = Color.DarkGreen;
                CopyrightLabel.ForeColor = Color.DarkGreen;
            }
            else
            {
                try
                {
                    ProgramVersionLabel.ForeColor = Color.Red;
                    BuildDateLabel.ForeColor = Color.Red;
                    CreatedByLabel.ForeColor = Color.Red;
                    EmailLabel.ForeColor = Color.Red;
                    CopyrightLabel.ForeColor = Color.Red;
                    File.Move(ExactLocationOfProgram, ProgramOldLocation);
                    File.SetAttributes(ProgramOldLocation, File.GetAttributes(ProgramOldLocation) | FileAttributes.Hidden);
                    File.Move(ProgramTempFolderLocation, ExactLocationOfProgram);
                    Process.Start(ExactLocationOfProgram, "Update");
                    this.Close();
                }
                catch { }
            }
        }
        async Task<DebugAndReleaseUpdate> CheckForDebugOrReleaseUpdates()
        {
            dynamic MD5HashLocalFileResult = null;
            dynamic MD5HashServerFileResult = null;
            await Task.Run(delegate
            {
#if DEBUG
                DebugOrReleaseProgramUpdate.DownloadFile(DebugEditionDownloadLink, ProgramTempFolderLocation);
#else
                DebugOrReleaseProgramUpdate.DownloadFile(ReleaseEditionDownloadLink, ProgramTempFolderLocation);
#endif
                using (var MD5HashLocalFile = MD5.Create())
                {
                    using (var stream = File.OpenRead(ExactLocationOfProgram))
                        MD5HashLocalFileResult = BitConverter.ToString(MD5HashLocalFile.ComputeHash(stream)).Replace("-", null);
                }
                using (var MD5HashServerFile = MD5.Create())
                {
                    using (var stream = File.OpenRead(ProgramTempFolderLocation))
                        MD5HashServerFileResult = BitConverter.ToString(MD5HashServerFile.ComputeHash(stream)).Replace("-", null);
                }
            });
            BuildDateOrReleaseDateFormatted = ProgramInformation.GetBuildDateTime(ProgramTempFolderLocation);
#if DEBUG
            if (MD5HashLocalFileResult != MD5HashServerFileResult)
                return DebugAndReleaseUpdate.UpdateFound;
#else
            if (MD5HashLocalFileResult != MD5HashServerFileResult)
            {
                var ServerVersion = FileVersionInfo.GetVersionInfo(ProgramTempFolderLocation);
                if (int.Parse(ServerVersion.FileVersion.Replace(".", null)) == VersionUntouchedWithoutDots)
                    return DebugAndReleaseUpdate.UpdateFound;
                else
                    return DebugAndReleaseUpdate.Failed;
            }
#endif
            else
                return DebugAndReleaseUpdate.UpdateToDate;
        }
        #endregion

        #region Main Procedure
        async void CheckServerButton_Click(object sender, EventArgs e)
        {
            CheckServerButton.Enabled = false;
            DownloadUpdateButton.EnabledChanged -= DownloadUpdateButton_EnabledChanged;
            DownloadUpdateButton.Enabled = false;
            DownloadUpdateButton.EnabledChanged += DownloadUpdateButton_EnabledChanged;
            FlashUpdateButton.Enabled = false;
            ServerVersionLabel.Visible = false;
            InstalledMEFirmwareVersionLabel.ForeColor = Color.Blue;
            FirmwareType.ForeColor = Color.Blue;
            StatusLabel.ForeColor = Color.Blue;
            StatusLabel.Text = "Checking server for new updates...";
            StatusLabel.Visible = true;
            await WinRAIDUpdateCheckMethod();
            CheckingComplete();
        }
        void CheckingComplete()
        {
            //InstalledMEFirmwareVersion = "0";
            if (int.Parse(InstalledMEFirmwareVersion.Replace(".", null)) == int.Parse(ServerVersion.Replace(".", null)))
            {
                Invoke(new Action(delegate
                {
                    StatusLabel.ForeColor = Color.DarkGreen;
                    StatusLabel.Text = "You're up to date! ✔";
                    InstalledMEFirmwareVersionLabel.ForeColor = Color.DarkGreen;
                    FirmwareType.ForeColor = Color.DarkGreen;
                }));
            }
            else if (int.Parse(InstalledMEFirmwareVersion.Replace(".", null)) < int.Parse(ServerVersion.Replace(".", null)))
            {
                Invoke(new Action(delegate
                {
                    ServerVersionLabel.Text = string.Format("Server Version: {0}", ServerVersion);
                    ServerVersionLabel.Visible = true;
                    StatusLabel.ForeColor = Color.DarkGreen;
                    StatusLabel.Text = "An available update has been found! ✔";
                    InstalledMEFirmwareVersionLabel.ForeColor = Color.Red;
                    FirmwareType.ForeColor = Color.Red;
                }));
            }
            else if (int.Parse(InstalledMEFirmwareVersion.Replace(".", null)) > int.Parse(ServerVersion.Replace(".", null)))
            {
                Invoke(new Action(delegate
                {
                    StatusLabel.ForeColor = Color.Purple;
                    StatusLabel.Text = "You're more up to date than server! ✔";
                    InstalledMEFirmwareVersionLabel.ForeColor = Color.Purple;
                    FirmwareType.ForeColor = Color.Purple;
                }));
            }
            Invoke(new Action(delegate
            {
                CheckServerButton.Enabled = true;
                DownloadUpdateButton.Enabled = true;
            }));
        }
        void CalculateMainLink()
        {
            if (InstalledMEFirmwareVersion.StartsWith("6"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-6.x/orderby,4/");
            else if (InstalledMEFirmwareVersion.StartsWith("7"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-7.x/orderby,4/");
            else if (InstalledMEFirmwareVersion.StartsWith("8"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-8.x/orderby,4/");
            else if (!InstalledMEFirmwareVersion.StartsWith("9.5"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-9.x/Serie-9.0.x-9.1.x/orderby,4/");
            else if (InstalledMEFirmwareVersion.StartsWith("9.5"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-9.x/Serie-9.5.x/orderby,4/");
            else if (InstalledMEFirmwareVersion.StartsWith("10"))
                MainLink = new Uri("http://www.station-drivers.com/index.php/downloads/Drivers/Intel/Management-Engine-Interface-(MEI)/Firmwares/Serie-10.x/orderby,4/");
        }
        async void DownloadUpdateButton_Click(object sender, EventArgs e)
        {
            DownloadUpdateButton.Enabled = false;
            CheckServerButton.Enabled = false;
            FlashUpdateButton.Enabled = false;
            FirmwareUpdateCheckingLoadingPictureBox.Visible = false;

            StatusLabel.Text = "Retrieving server filename extension for download...";
            StatusLabel.ForeColor = Color.DarkGreen;

            if (!Directory.Exists(string.Format(@"{0}\IME Firmware Update", Environment.GetFolderPath(Environment.SpecialFolder.Desktop))))
                Directory.CreateDirectory(string.Format(@"{0}\IME Firmware Update", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            if (!Directory.Exists(string.Format(@"{0}\IME Firmware Update\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion)))
                Directory.CreateDirectory(string.Format(@"{0}\IME Firmware Update\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion));
            try
            {
                if (File.Exists(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension)))
                    File.Delete(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension));
            }
            catch { }

            await Task.Run(async delegate
            {
                using (var Client = new WebClient())
                {
                    Client.OpenRead(UpdateDownloadLink);

                    string Header_ContentDisposition = Client.ResponseHeaders["content-disposition"];
                    try { ServerFileExtension = Path.GetExtension(new ContentDisposition(Header_ContentDisposition).FileName); }
                    catch { ServerFileExtension = Path.GetExtension(UpdateDownloadLink.AbsoluteUri); }

                    #region Progress Changed
                    Client.DownloadProgressChanged += async delegate(object DownloadProgressSender, DownloadProgressChangedEventArgs Event)
                    {
                        try
                        {
                            int Percent = Convert.ToInt32(Math.Truncate((Convert.ToDouble(Event.BytesReceived / 100) / Convert.ToDouble(Event.TotalBytesToReceive / 100)) * 100));
                            Invoke(new Action(delegate
                            {
                                if (StatusLabel.ForeColor != Color.DarkGreen)
                                    StatusLabel.ForeColor = Color.DarkGreen;
                                StatusLabel.Text = string.Format("Downloading update {0}%...", Percent);
                            }));
                        }
                        catch { }
                    };
                    #endregion

                    #region Download Complete
                    Client.DownloadFileCompleted += async delegate(object DownloadFileSender, AsyncCompletedEventArgs Event)
                    {
                        Invoke(new Action(delegate
                        {
                            try
                            {
                                if (Event.Error == null)
                                {
                                    StatusLabel.Text = "Download Complete! ✔";
                                    StatusLabel.ForeColor = Color.DarkGreen;
                                    if (ServerFileExtension != ".zip")
                                    {
                                        StatusLabel.Text = string.Format("Extracting {0}...", ServerFileExtension);
                                        if (Environment.Is64BitProcess)
                                            SevenZip.SevenZipExtractor.SetLibraryPath(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z64"));
                                        else
                                            SevenZip.SevenZipExtractor.SetLibraryPath(string.Format(@"{0}\{1}.dll", Application.StartupPath, "7z"));
                                        SevenZip.SevenZipExtractor TempFile = new SevenZip.SevenZipExtractor(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension));
                                        TempFile.BeginExtractArchive(Path.GetDirectoryName(string.Format(@"{0}\IME Firmware Update\{1}\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion)));
                                        TempFile.ExtractionFinished += TempFile_ExtractionFinished;
                                        return;
                                    }
                                    else
                                    {
                                        StatusLabel.Text = "Extracting .zip...";
                                        ZipFile.ExtractToDirectory(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension),
                                                                   string.Format(@"{0}\IME Firmware Update\{1}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion));
                                        StatusLabel.Text = "Extracting .zip Complete! ✔";
                                        FlashUpdateButton.Enabled = true;
                                        CheckServerButton.Enabled = true;
                                        DownloadUpdateButton.Enabled = true;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (File.Exists(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension)))
                                            File.Delete(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension));
                                    }
                                    catch { }
                                    CheckServerButton.Enabled = true;
                                    DownloadUpdateButton.Enabled = true;
                                    FlashUpdateButton.Enabled = false;
                                    MessageBox.Show(this, Event.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    if (File.Exists(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension)))
                                        File.Delete(string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension));
                                }
                                catch { }
                                CheckServerButton.Enabled = true;
                                DownloadUpdateButton.Enabled = true;
                                FlashUpdateButton.Enabled = false;
                                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }));
                        Application.ExitThread();
                    };
                    #endregion

                    await Client.DownloadFileTaskAsync(UpdateDownloadLink, string.Format(@"{0}\IME Firmware Update\{1}\Update{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ServerVersion, ServerFileExtension));
                }
            });
        }
        void FlashUpdateButton_Click(object sender, EventArgs e)
        {
            FlashUpdateButton.Enabled = false;
            FlashingUpdateThatWasDownloaded = true;
            Tabs.SelectedTab = FirmwareFlashingTab;
            SelectFiletoFlashLinkLabel_LinkClicked(this, new LinkLabelLinkClickedEventArgs(null));
        }
        async void SelectFiletoFlashLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectFiletoFlashLinkLabel.Enabled = false;
            string ZipFileWereOn = null;
            OpenFileDialog FileBrowser = new OpenFileDialog();
            FileBrowser.Filter = "Firmware Files|*.bin";
            if (FlashingUpdateThatWasDownloaded)
            {
                FileBrowser.InitialDirectory = string.Format(@"{0}\IME Firmware Update\", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                FlashingUpdateThatWasDownloaded = false;
            }
            string FileMEVersion = null;
            if (FileBrowser.ShowDialog() == DialogResult.OK)
            {
                await Task.Run(async delegate
                {
                    if (InstalledMEFirmwareVersion.StartsWith("7"))
                        ZipFileWereOn = "MEFlash-7x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("8"))
                        ZipFileWereOn = "MEFlash-8x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("9.0"))
                        ZipFileWereOn = "MEFlash-90x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("9.1"))
                        ZipFileWereOn = "MEFlash-91x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("9.5"))
                        ZipFileWereOn = "MEFlash-95x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("9.6"))
                        ZipFileWereOn = "MEFlash-96x.zip";
                    else if (InstalledMEFirmwareVersion.StartsWith("10"))
                        ZipFileWereOn = "MEFlash-10x.zip";

                    try
                    {
                        if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                            File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
                    }
                    catch { }

                    File.WriteAllBytes(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), ZipFileWereOn == "MEFlash-7x.zip" ? Resources.MEFlash_7x : ZipFileWereOn == "MEFlash-8x.zip" ? Resources.MEFlash_8x :
                                       ZipFileWereOn == "MEFlash-90x.zip" ? Resources.MEFlash_90x : ZipFileWereOn == "MEFlash-91x.zip" ? Resources.MEFlash_91x : ZipFileWereOn == "MEFlash-95x.zip" ? Resources.MEFlash_95x :
                                       ZipFileWereOn == "MEFlash-96x.zip" ? Resources.MEFlash_96x : Resources.MEFlash_10x);

                    try
                    {
                        if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")))
                            Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"), true);
                        if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                            Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
                    }
                    catch { }

                    ZipFile.ExtractToDirectory(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn), Path.GetTempPath());
                    await Task.Delay(3);
                    Directory.Move(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"));
                    File.Copy(FileBrowser.FileName, string.Format(@"{0}{1}\{2}", Path.GetTempPath(), "MEFlash", FileBrowser.SafeFileName));

                    Process GetFileVersion = new Process();
                    GetFileVersion.StartInfo.FileName = "cmd.exe";
                    GetFileVersion.StartInfo.RedirectStandardInput = true;
                    GetFileVersion.StartInfo.RedirectStandardOutput = true;
                    GetFileVersion.StartInfo.RedirectStandardError = true;
                    GetFileVersion.StartInfo.UseShellExecute = false;
                    GetFileVersion.StartInfo.CreateNoWindow = true;
                    GetFileVersion.Start();
                    GetFileVersion.StandardInput.WriteLine("C:");
                    GetFileVersion.StandardInput.WriteLine(string.Format("cd {0}", string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")));
                    GetFileVersion.StandardInput.WriteLine(string.Format("FWUpdLcl.exe -FWVER \"{0}\"", FileBrowser.SafeFileName));
                    GetFileVersion.StandardInput.WriteLine("exit");
                    GetFileVersion.WaitForExit();
                    StringReader FileVersionData = new StringReader(GetFileVersion.StandardOutput.ReadToEnd());
                    if (File.Exists(string.Format(@"{0}\{1}", Application.StartupPath, "error.log")))
                        File.Delete(string.Format(@"{0}\{1}", Application.StartupPath, "error.log"));
                    string Data = null;
                    while ((Data = FileVersionData.ReadLine()) != null)
                    {
                        if (Data.ToLower().Contains("FW Version".ToLower()))
                        {
                            FileMEVersion = Regex.Replace(Data, "[^0-9.]", string.Empty).Trim();
                            break;
                        }
                    }
                });

                bool ProperFile = false;
                string[] VersionNumbers = FileMEVersion.Split('.');
                if ((VersionNumbers[0].Length == 1 || VersionNumbers[0].Length == 2) && (VersionNumbers[1].Length == 1) && (VersionNumbers[2].Length == 2 || VersionNumbers[2].Length == 1) && (VersionNumbers[3].Length == 4))
                    ProperFile = true;

                if (!ProperFile)
                    MessageBox.Show(this, string.Format("The file '{0}' you have selected is not a proper IME firmware, flashing has been denied.", FileBrowser.SafeFileName), "Firmware Flasher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (string.IsNullOrEmpty(FileMEVersion.Replace(".", null)))
                    MessageBox.Show(this, string.Format("The firmware version of the file '{0}' could not be determined, flashing has been denied.", FileBrowser.SafeFileName), "Firmware Flasher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    if (int.Parse(InstalledMEFirmwareVersion.Replace(".", null)) == int.Parse(FileMEVersion.Replace(".", null)) && !Settings.AllowSameVersion)
                    {
                        MessageBox.Show(this, "You have selected the same firmware version to flash, to continue with this request you must navigate to Preferences > 'Allow Flashing of Same Firmware Version' and enable this setting.", "Firmware Flasher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Tabs.SelectedTab = PreferencesTab;
                    }
                    else if ((FileMEVersion.StartsWith("10") && !InstalledMEFirmwareVersion.StartsWith("10")) || (int.Parse(InstalledMEFirmwareVersion.Remove(1, InstalledMEFirmwareVersion.Length - 1)) < int.Parse(FileMEVersion.Remove(1, FileMEVersion.Length - 1)))) /*|| (InstalledMEFirmwareVersion.StartsWith("7") && !FileMEVersion.StartsWith("8") && !FileMEVersion.StartsWith("7")))*/
                        MessageBox.Show(this, "You cannot upgrade to anything above your series, flashing has been denied.", "Firmware Flasher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if ((int.Parse(InstalledMEFirmwareVersion.Remove(1, InstalledMEFirmwareVersion.Length - 1)) > int.Parse(FileMEVersion.Remove(1, FileMEVersion.Length - 1)))) /*|| (InstalledMEFirmwareVersion.StartsWith("7") && !FileMEVersion.StartsWith("8") && !FileMEVersion.StartsWith("7")))*/
                        MessageBox.Show(this, "You cannot downgrade to anything below your series, flashing has been denied.", "Firmware Flasher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        string Message = string.Format("Current IME Firmware Version: {0}\nFile IME Firmware Version: {1}\n\nAre you sure you want to update your IME version?", InstalledMEFirmwareVersion, FileMEVersion);
                        if (MessageBox.Show(this, Message, "Flash Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var Writer = new StringWriter();
                            Writer.WriteLine("@echo off");
                            Writer.WriteLine("pushd %~dp0");
                            Writer.Write("FWUpdLcl ");

                            if (Settings.AllowSameVersion)
                                Writer.Write("-ALLOWSV ");
                            if (Settings.ForceReset)
                                Writer.Write("-FORCERESET ");

                            Writer.WriteLine(string.Format("-F {0}", FileBrowser.SafeFileName));
                            Writer.Write("pause");
                            File.WriteAllText(string.Format(@"{0}{1}\{2}", Path.GetTempPath(), "MEFlash", "Flash.bat"), Writer.ToString().Trim());

                            Process MEFlasher = new Process();
                            MEFlasher.StartInfo.FileName = string.Format(@"{0}{1}\{2}", Path.GetTempPath(), "MEFlash", "Flash.bat");
                            MEFlasher.Start();
                            MEFlasher.WaitForExit();
                        }
                    }
                }
            }

            try
            {
                if (File.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn)))
                    File.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn));
            }
            catch { }

            try
            {
                if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash")))
                    Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), "MEFlash"), true);
                if (Directory.Exists(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null))))
                    Directory.Delete(string.Format(@"{0}{1}", Path.GetTempPath(), ZipFileWereOn.Replace(".zip", null)), true);
            }
            catch { }

            SelectFiletoFlashLinkLabel.Enabled = true;
        }
        void CheckServerButton_EnabledChanged(object sender, EventArgs e)
        {
            if (CheckServerButton.Enabled)
                FirmwareUpdateCheckingLoadingPictureBox.Visible = false;
            else
                FirmwareUpdateCheckingLoadingPictureBox.Visible = true;
        }
        void SelectFiletoFlashLinkLabel_EnabledChanged(object sender, EventArgs e)
        {
            if (!SelectFiletoFlashLinkLabel.Enabled)
                FlashingPictureBox.Visible = true;
            else
                FlashingPictureBox.Visible = false;
        }
        void DownloadUpdateButton_EnabledChanged(object sender, EventArgs e)
        {
            if (!DownloadUpdateButton.Enabled)
                DownloadUpdatePictureBox.Visible = true;
            else
                DownloadUpdatePictureBox.Visible = false;
        }
        void TempFile_ExtractionFinished(object sender, EventArgs e)
        {
            StatusLabel.Text = string.Format("Extracting {0} Complete! ✔", ServerFileExtension);
            FlashUpdateButton.Enabled = true;
            CheckServerButton.Enabled = true;
            DownloadUpdateButton.Enabled = true;
        }
        async Task WinRAIDUpdateCheckMethod()
        {
            string ServerVersionTemp = null;
            Uri MediaFireLink = null;
            var BackupMethodThread = new Thread(new ThreadStart(delegate
            {
                using (WebBrowser Browser = new WebBrowser())
                {
                    Browser.ScriptErrorsSuppressed = true;

                    #region DocumentCompleted
                    Browser.DocumentCompleted += async delegate(object DocumentCompletedSender, WebBrowserDocumentCompletedEventArgs Event)
                    {
                        if (Event.Url.Equals(MainLink))
                        {
                            while (Browser.IsBusy)
                                await Task.Delay(3);

                            foreach (HtmlElement Element in Browser.Document.Links)
                            {
                                if (OneDotFiveMBFound)
                                {
                                    try { ServerVersionTemp = Regex.Replace(Element.InnerText.Substring(Element.InnerText.ToLower().IndexOf("Firmware".ToLower()), Element.InnerText.Length - Element.InnerText.ToLower().IndexOf("Firmware".ToLower())), "[^0-9.]", string.Empty).Trim(); }
                                    catch { }
                                    if (!InstalledMEFirmwareVersion.StartsWith("9"))
                                    {
                                        if (Element.GetAttribute("href").StartsWith("http://www.mediafire.com/download/") && Element.GetAttribute("href").ToLower().Contains("(1.5MB)".ToLower()) && ServerVersionTemp.StartsWith(InstalledMEFirmwareVersion.Remove(1, InstalledMEFirmwareVersion.Length - 1)))
                                        {
                                            MediaFireLink = new Uri(Element.GetAttribute("href"));
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (Element.GetAttribute("href").StartsWith("http://www.mediafire.com/download/") && Element.GetAttribute("href").ToLower().Contains("(1.5MB)".ToLower()) && ServerVersionTemp.StartsWith(InstalledMEFirmwareVersion.Remove(3, InstalledMEFirmwareVersion.Length - 3)))
                                        {
                                            MediaFireLink = new Uri(Element.GetAttribute("href"));
                                            break;
                                        }
                                    }
                                }
                                else if (FiveMBFound)
                                {
                                    try { ServerVersionTemp = Regex.Replace(Element.InnerText.Substring(Element.InnerText.ToLower().IndexOf("Firmware".ToLower()), Element.InnerText.Length - Element.InnerText.ToLower().IndexOf("Firmware".ToLower())), "[^0-9.]", string.Empty).Trim(); }
                                    catch { }
                                    if (!InstalledMEFirmwareVersion.StartsWith("9"))
                                    {
                                        if (Element.GetAttribute("href").StartsWith("http://www.mediafire.com/download/") && Element.GetAttribute("href").ToLower().Contains("(5MB)".ToLower()) && ServerVersionTemp.StartsWith(InstalledMEFirmwareVersion.Remove(1, InstalledMEFirmwareVersion.Length - 1)))
                                        {
                                            MediaFireLink = new Uri(Element.GetAttribute("href"));
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (Element.GetAttribute("href").StartsWith("http://www.mediafire.com/download/") && Element.GetAttribute("href").ToLower().Contains("(5MB)".ToLower()) && ServerVersionTemp.StartsWith(InstalledMEFirmwareVersion.Remove(3, InstalledMEFirmwareVersion.Length - 3)))
                                        {
                                            MediaFireLink = new Uri(Element.GetAttribute("href"));
                                            break;
                                        }
                                    }
                                }
                            }
                            ServerVersion = ServerVersionTemp;
                            Browser.Navigate(MediaFireLink);
                        }
                        else if (Event.Url.AbsoluteUri.StartsWith("http://www.mediafire.com/download/"))
                        {
                            foreach (HtmlElement Element in Browser.Document.Links)
                            {
                                if (Element.GetAttribute("href").StartsWith("http://download"))
                                {
                                    //Element.InvokeMember("Click");
                                    UpdateDownloadLink = new Uri(Element.GetAttribute("href"));
                                    Application.ExitThread();
                                }
                            }
                        }
                    };
                    #endregion

                    #region Navigating
                    //Browser.Navigating += async delegate(object NavigatingSender, WebBrowserNavigatingEventArgs Event)
                    //{
                    //    if (Event.Url.AbsoluteUri.StartsWith("http://download"))
                    //    {
                    //        Event.Cancel = true;

                    //        UpdateDownloadLink = Event.Url;

                    //        Application.ExitThread();
                    //    }
                    //};
                    #endregion

                    Browser.Parent = new Control();
                    Browser.Parent.Enabled = false;
                    Browser.Navigate(MainLink);
                    Application.Run();
                }
            }));

            #region Starting
            BackupMethodThread.SetApartmentState(ApartmentState.STA);
            BackupMethodThread.IsBackground = true;
            BackupMethodThread.Start();
            #endregion

            while (BackupMethodThread.IsAlive)
                await Task.Delay(3);
        }
        #endregion

        #region About
        void MyWebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.glitchyhack.x10.mx/");
        }
        #endregion

        #region Preferences
        void AllowFlashingOfSameFirmwareVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.AllowSameVersion = AllowFlashingOfSameFirmwareVersionCheckBox.Checked;
        }
        void ForceResetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ForceReset = ForceResetCheckBox.Checked;
        }
        #endregion

        #region Structs
        public struct Expressions
        {
            public static string InternetConnectionTestPassed { get { return "Internet connection status is online! ✔"; } }
            public static string TestingInternetConnection { get { return "Testing internet connection..."; } }
            public static string InternetConnectionTestIPStatusFailed
            {
                get
                {
                    return string.Format("Your internet connection appears to be offline at the moment, all requests that involve internet connectivity will be rejected with this message until internet {0}",
                        "access has returned online.");
                }
            }
            public static string ProgramUpdateCheckingServer { get { return "Checking update server for new updates..."; } }
            public static string CheckingServerHasFailed { get { return "Checking Server has failed! ✕"; } }
            public static string YouAreUpToDate { get { return "You're up to date! ✔"; } }
            public static string AnUpdateHasBeenFound { get { return "An available update has been found! ✔"; } }
            public static string YourVersionIsHigherThanServer { get { return "You're more up to date than server! ✔"; } }
            public static string Sending { get { return "Sending..."; } }
            public static string SendingComplete { get { return "Sending Complete! ✔"; } }
            public static string SendingFailed { get { return "Sending has failed! ✕"; } }
            public static string InternetConnectionTestFailed { get { return "Internet connection status is offline! ✕"; } }
            public static string CouldNotCalculateDownloadSpeed { get { return "Couldn't calculate download speed! ∞"; } }
            public static string CouldNotCalculateTimeRemaining { get { return "Couldn't calculate time remaining! ∞"; } }
            public static string CouldNotCalculateFileSize { get { return "Couldn't calculate Current MB / Total MB! ∞"; } }
            public static string DownloadHasFailed { get { return "Download has failed! ✕"; } }
            public static string DownloadWasCancelled { get { return "Download was cancelled! ✕"; } }
            public static string DownloadHasCompleted { get { return "Download Complete! ✔"; } }
            public static string DownloadIsInitializing { get { return "Your download is initializing..."; } }
            public static string DownloadHasBeenInitialized { get { return "Your download has been initialized. ✔"; } }
            public static string Success { get { return "Success"; } }
            public static string Error { get { return "Error"; } }
            public static string Infinity { get { return "Infinity"; } }
            public static string InfinitySymbol { get { return "∞"; } }
            public static string NumberZero { get { return "0"; } }
            public static string DebugSettingsDownloadLinkTitle { get { return "[Debug Feature] Download Link"; } }
            public static string BackSlash { get { return @"\"; } }
            public static string ForwardSlash { get { return "/"; } }
            public static string Percent { get { return "%"; } }
            public static string ThreeDots { get { return "..."; } }
            public static string GBPerSecond { get { return "GB/s"; } }
            public static string MBPerSecond { get { return "MB/s"; } }
            public static string KBPerSecond { get { return "KB/s"; } }
            public static string BytesPerSecond { get { return "B/s"; } }
            public static string GB { get { return "GB"; } }
            public static string MB { get { return "MB"; } }
            public static string KB { get { return "KB"; } }
            public static string Bytes { get { return "B"; } }
            public static string Hours { get { return "h"; } }
            public static string Minutes { get { return "m"; } }
            public static string Seconds { get { return "s"; } }
            public static string Milliseconds { get { return "ms"; } }
            public static string DownloadLabelsFormatted { get { return "0.00"; } }
            public static string UnsupportedHardwareTitle { get { return "Unsupported Hardware"; } }
            public static string UnsupportedHardware { get { return "Unsupported hardware detected! ✕"; } }
            public static string HardwareCheckReturnedMotherboardManufacturerAsNull { get { return "Hardware check returned your motherboard manufacturer as null."; } }
            public static string HardwareCheckReturnedMotherboardAsNull { get { return "Hardware check returned your motherboard as null."; } }
            public static string RequestingMotherboardSentSuccess
            {
                get
                {
                    return string.Format("Hardware Report was sent successfully, thank you.{0}{1}", Environment.NewLine, "Check for updates in a few weeks and your company might be supported if it is possible.");
                }
            }
            public static string GatheringInformation { get { return "Gathering Information..."; } }
            public static string CouldNotRetrieveInformation { get { return "Couldn't retrieve information!"; } }
            public static string MHz { get { return "MHz"; } }
            public static string GHz { get { return "GHz"; } }
            public static string UEFIUpdateCheckingServer { get { return "Checking server for new updates..."; } }
            public static string MotherboardManufacturerRequestingMessage
            {
                get
                {
                    return string.Format("{0}\n\n{1}",
                        "We're sorry, but your motherboard manufacturer appears to be not supported at this time.",
                        "Would you like to automatically request your motherboard manufacturer to be supported by sending us a report with your current hardware information that you have installed?");
                }
            }
            public static string DEBUGON { get { return "DEBUG: ON"; } }
            public static string DEBUGOFF { get { return "DEBUG: OFF"; } }
            public static string DebugSettingsHasBeenEnabled { get { return "Debug Settings has been enabled."; } }
            public static string DebugSettingsHasBeenDisabled { get { return "Debug Settings has been disabled."; } }
            public static string DebugSettingsErrorTestMessage { get { return "Error testing message"; } }
            public static string DirectoryCreated { get { return "Directory Created Successfully!"; } }
            public static string StartingProgramActivity { get { return "Starting Program"; } }
            public static string BrowsingProgramActivity { get { return "Browsing Program"; } }
            public static string AMIFlashDownloadActivity { get { return "AMI UEFI Flasher Download"; } }
            public static string AiSuiteIIDownloadActivity { get { return "AiSuiteII Download"; } }
            public static string AiSuiteIIIDownloadActivity { get { return "AiSuiteIII Download"; } }
            public static string Default { get { return "Default"; } }
            public static string Updating { get { return "Updating..."; } }
            public static string UpdatingFailed { get { return "Updating has failed! ✕"; } }
            public static string ProgramGUIDCheckGlobal { get { return @"Global\"; } }
            public static string ExtractingFileComplete { get { return "Extracting File Complete! ✔"; } }
            public static string ExtractingFileFailed { get { return "Extracting File Failed! ✕"; } }
            public static string WindowsRegistryStartupLocation { get { return @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; } }
            public static string DebugSettingsTurnedON { get { return "ON"; } }
            public static string DebugSettingsTurnedOFF { get { return "OFF"; } }
            public static string UpdateFoundOnProgramStartupMessage { get { return "Do you want to apply this new update now?"; } }
            public static string AutoProgramUpdateOnStartupTitle { get { return "UEFI Download Tool Program Updater"; } }
            public static string YouAreNotSignedIn { get { return "You're not signed in ✕"; } }
            public static string YouAreSignedIn { get { return "You're signed in ✔"; } }
            public static string YouAreBeingSignedIn { get { return "Signing in..."; } }
            public static string YouAreBeingSignedOut { get { return "Signing out..."; } }
            public static string AccountIsBeingCreated { get { return "Account is being created..."; } }
            public static string ChangePassword
            {
                get
                {
                    return "We're going to take you to my website to change your password and we're also going to log you out of your account inside UEFI Download Tool, after you're done changing your password on the site login with the new info inside UEFI Download Tool.";
                }
            }
            public static string ChangePasswordTitle { get { return "Change Password"; } }
            public static string PasswordUpdated { get { return "Your password has been successfully updated."; } }
            public static string PasswordUpdateToolTip { get { return "This will allow you to update your password once you have entered your new preferred one."; } }
            public static string LoginButtonToolTip { get { return "This will log you into your account if it exists."; } }
            public static string UEFIDownloadToolDependenciesDeleter { get { return "UEFI Download Tool Dependencies Deleter"; } }
            public static string AccountLoginFailed { get { return "There was a problem while trying to sign you into your account.\nThis could mean that the website is down right now and a connection could not be established."; } }
            public static string AccountLogoutFailed { get { return "There was a problem while trying to sign you out of your account.\nThis could mean that the website is down right now and a connection could not be established."; } }
            public static string AccountLoginFailedErrorMessageNull { get { return "No error could be received, something has went wrong.\nThis could mean that the website is down right now and a connection could not be established."; } }
            public static string Update { get { return "Update"; } }
            public static string UEFIUpdateZip { get { return "UEFI Update.zip"; } }
            public static string Extracting { get { return "Extracting"; } }
            public static string Complete { get { return "Complete! ✔"; } }
            public static string FeedbackSentSuccess { get { return "Your feedback was sent successfully, thank you."; } }
            public static string ErrorReportSentSuccess { get { return "Your error report was sent successfully, thank you."; } }
            public static string Login { get { return "Login"; } }
            public static string Refreshing { get { return "Refreshing..."; } }
            public static string SigningIn { get { return "Signing in..."; } }
            public static string NVIDIA { get { return "NVIDIA"; } }
            public static string HREF { get { return "href"; } }
            public static string HREFNodes { get { return "//a[@href]"; } }
            public static string OnClick { get { return "onclick"; } }
            public static string OnClickNodes { get { return "//a[@onclick]"; } }
            public static string DIVClassNodes { get { return "//div[@class]"; } }
            public static string TRNodes { get { return "//tr"; } }
            public static string TDNodes { get { return "//td"; } }
            public static string THNodes { get { return "//th"; } }
            public static string TDClassNodes { get { return "//td[@class]"; } }
            public static string TDClassRemarkNodes { get { return "//td[@class='Remark']"; } }
            public static string TDClassDateNodes { get { return "//td[@class='Date']"; } }
            public static string DIVClassNoticeNodes { get { return "//div[@class='Notice']"; } }
            public static string Class { get { return "Class"; } }
            public static string AMD { get { return "AMD"; } }
            public static string DebugSettingsManualMotherboardOverrideError { get { return "You cannot leave these fields blank."; } }
            public static string OperationWasCancelled { get { return "Operation Was Cancelled ✕"; } }
            public static string MotherboardMismatch { get { return "Motherboard Mismatch"; } }
            public static string MotherboardDoesNotSupportUpdates { get { return "The selected motherboard on the server does not seem to support UEFI updates, sorry."; } }
            public static string UpdatesUnavailable { get { return "Updates Unavailable"; } }
            public static string LinkNotFound
            {
                get
                {
                    return string.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "The requested motherboard link was not found. [404]",
                        "This error usually happens when your motherboard is not found at all on the manufacturers website.");
                }
            }
            public static string ClassName { get { return "ClassName"; } }
            public static string MotherboardMatchNotFound { get { return "Your motherboard didn't match any of the motherboards that we found on the server, therefore no updates could be found, sorry."; } }
            public static string SyncingApplicationSettingsMessage { get { return "Syncing Application Settings..."; } }
            public static string InvokeClick { get { return "Click"; } }
            public static string NotPresent { get { return "Not Present"; } }
            public static string WindowsLogin { get { return "Windows Login"; } }
            public static string Debug { get { return "Debug"; } }
            public static string Release { get { return "Release"; } }
            public static string UEFIUpdateTitle { get { return "UEFI Update"; } }
            public static string UEFIUpdateMessage { get { return "An update has been found.\n\n"; } }
            public static string CheckingForANewerCopyOfCurrentVersionDolphinUpdater { get { return string.Format("Checking for a newer build of {0}", ProgramInformation.AssemblyTitle); } }
            public static string ServiceTag { get { return "Service Tag"; } }
            public static string BIOS { get { return "BIOS"; } }
            public static string Register { get { return "Register"; } }
            public static string Cancel { get { return "Cancel"; } }
            public static string Canceling { get { return "Canceling..."; } }
            public static string ThreadBackgroundAbortRequested { get { return "Background, AbortRequested"; } }
            public static string BuildDateReturnedNullMessage { get { return "UEFI Download Tool's Build Date has returned null."; } }
            public static string Unknown { get { return "Unknown"; } }
            public static string ErrorReportFile { get { return "Error Report"; } }
            public static string FeedbackFile { get { return "Feedback"; } }
            public static string RequestingMotherboardManufacturerFile { get { return "Requesting Motherboard Manufacturer"; } }
            public static string ProgramInformationTitle { get { return "--- Program Information ---"; } }
            public static string ImportantInformationTitle { get { return "--- Important Information ---"; } }
            public static string HardwareInformationTitle { get { return "--- Hardware Information ---"; } }
            public static string AdditionalInformationTitle { get { return "--- Additional Information ---"; } }
            public static string Send { get { return "Send"; } }
            public static string ErrorReportDidNotSendSuccessfullyMessage { get { return "Your error report did not send successfully."; } }
            public static string ErrorGettingHardwareInformationMessage { get { return "An error has occurred while getting your hardware information."; } }
            public static string FeedbackDidNotSendSuccessfullyMessage { get { return "Your feedback did not send successfully."; } }
            public static string RequestDidNotSendSuccessfullyMessage { get { return "Your request did not send successfully."; } }
            public struct BeginsWith
            {
                public static string LastChecked { get { return "Last Checked: "; } }
                public static string Version { get { return "Version: "; } }
                public static string ServerVersion { get { return "Server Version: "; } }
                public static string BuildDate { get { return "Build Date: "; } }
                public static string Old { get { return @"\Old "; } }
                public static string ReleaseDate { get { return "Release Date: "; } }
                public static string Program { get { return "Program: "; } }
                public static string Activity { get { return "Activity: "; } }
                public static string TheirThoughts { get { return "Their Thoughts: "; } }
                public static string UsersMessage { get { return "User's Message: "; } }
                public static string MotherboardManufacturer { get { return "Motherboard Manufacturer: "; } }
                public static string MotherboardModel { get { return "Motherboard Model: "; } }
                public static string UEFIBuildDate { get { return "UEFI Build Date: "; } }
                public static string UEFIVendor { get { return "UEFI Vendor: "; } }
                public static string UEFIVersion { get { return "UEFI Version: "; } }
                public static string ProcessorName { get { return "Processor Name: "; } }
                public static string CurrentProcessorFrequency { get { return "Current Processor Frequency: "; } }
                public static string MaximumProcessorFrequency { get { return "Maximum Processor Frequency: "; } }
                public static string ProcessorCores { get { return "Processor Cores: "; } }
                public static string ProcessorThreads { get { return "Processor Threads: "; } }
                public static string IntelVirtualizationTechnology { get { return "Intel® Virtualization Technology: "; } }
                public static string OperatingSystem { get { return "Operating System: "; } }
                public static string CurrentRAMFrequency { get { return "Current RAM Frequency: "; } }
                public static string GraphicsCard { get { return "Graphics Card: "; } }
                public static string RequestingMotherboard { get { return "Requesting Motherboard "; } }
                public static string Status { get { return "Status: "; } }
                public static string AutoDelay { get { return "Auto Delay: "; } }
                public static string DownloadingWithSpace { get { return "Downloading "; } }
                public static string DownloadingWithoutSpace { get { return "Downloading"; } }
                public static string ETA { get { return "ETA: "; } }
                public static string ElapsedTime { get { return "Elapsed Time: "; } }
                public static string File { get { return "File: "; } }
                public static string DayAndTime { get { return "Day & Time: "; } }
                public static string Account { get { return "Account: "; } }
                public static string ErrorReceived { get { return "Error Received: "; } }
                public static string MonitorResolutions { get { return "Monitor Resolutions: "; } }
                public static string PCUsername { get { return "PC Username: "; } }
                public static string SerialNumber { get { return "Serial Number: "; } }
                public static string AccountUsername { get { return "Account Username: "; } }
                public static string InstalledMotherboard { get { return "Installed Motherboard: "; } }
                public static string TargetMotherboard { get { return "Target Motherboard: "; } }
                public static string TargetProduct { get { return "Target Product: "; } }
                public static string UpdateStatus { get { return "Update Status: "; } }
                public static string ScheduledDateTime { get { return "Scheduled Date/Time: "; } }
                public static string FileName { get { return "File Name: "; } }
                public static string Edition { get { return "Edition: "; } }
                public static string Copyright { get { return "Copyright: "; } }
                public static string MethodName { get { return "Method Name: "; } }
                public static string LineNumber { get { return "Line Number: "; } }
                public static string AdditionalInformation { get { return "Additional Information: "; } }
            }
            public struct EndsWith
            {
                private static string UEFIUpdateDownloadFolderPrivate = @"\UEFI Update";
                public static string UEFIUpdateDownloadFolder { get { return UEFIUpdateDownloadFolderPrivate; } set { UEFIUpdateDownloadFolderPrivate = value; } }
                public static string AppLimitCloudComputingSharpBox { get { return @"\AppLimit.CloudComputing.SharpBox.dll"; } }
                public static string NewtonsoftJsonNet40 { get { return @"\Newtonsoft.Json.Net40.dll"; } }
                public static string HtmlAgilityPack { get { return @"\HtmlAgilityPack.dll"; } }
                public static string TextFile { get { return ".txt"; } }
                public static string AMIFlashZip { get { return @"\amiflash.zip"; } }
                public static string AiSuiteIIZip { get { return @"\AiSuiteII.zip"; } }
                public static string AiSuiteIIIZip { get { return @"\AiSuiteIII.zip"; } }
                public static string DotConfig { get { return ".config"; } }
                public static string DropBoxDownloadFile { get { return @"\DropBox DownloadFile.exe"; } }
                public static string DotZip { get { return ".zip"; } }
                public static string DotExe { get { return ".exe"; } }
                public static string UEFIDownloadTool { get { return string.Format(@"\{0}.exe", ProgramInformation.AssemblyTitle); } }
            }
            public struct ProgramUpdate
            {
                public static string CheckServer { get { return "[Program Update/Check Server]"; } }
                public static string ApplyUpdate { get { return "[Program Update/Apply Update]"; } }
            }
            public struct Extras
            {
                public static string Downloads { get { return "[Extras/Downloads]"; } }
                public static string DeleteFile { get { return "[Extras/Downloads/Deleting Broken File]"; } }
                public static string UnzipFailed { get { return "[Extras/Downloads/Unzip File]"; } }
                public static string PreferencesFeatureOperationsFailed { get { return "[Extras/Download/Preferences Features Failed]"; } }
            }
            public struct SystemAnalysis
            {
                public static string MotherboardManufacturer { get { return "[System Analysis/Motherboard Manufacturer]"; } }
                public static string MotherboardModel { get { return "[System Analysis/Motherboard Model]"; } }
                public static string UEFIBuildDate { get { return "[System Analysis/UEFI Build Date]"; } }
                public static string UEFIVendor { get { return "[System Analysis/UEFI Vendor]"; } }
                public static string UEFIVersion { get { return "[System Analysis/UEFI Version]"; } }
                public static string ProcessorName { get { return "[System Analysis/Processor Name]"; } }
                public static string CurrentProcessorFrequency { get { return "[System Analysis/Current Processor Frequency]"; } }
                public static string MaximumProcessorFrequency { get { return "[System Analysis/Maximum Processor Frequency]"; } }
                public static string ProcessorCores { get { return "[System Analysis/Processor Cores]"; } }
                public static string ProcessorThreads { get { return "[System Analysis/Processor Threads]"; } }
                public static string IntelVirtualizationTechnology { get { return "[System Analysis/Intel® Virtualization Technology]"; } }
                public static string OperatingSystem { get { return "[System Analysis/Operating System]"; } }
                public static string CurrentRAMFrequency { get { return "[System Analysis/Current RAM Frequency]"; } }
                public static string GraphicsCard { get { return "[System Analysis/Graphics Card]"; } }
                public static string MonitorResolutions { get { return "[System Analysis/Monitor Resolutions]"; } }
                public static string PCUsername { get { return "[System Analysis/PC Username]"; } }
                public static string SerialNumber { get { return "[System Analysis/Serial Number]"; } }
            }
            public struct UEFIUpdate
            {
                public static string CheckServer { get { return "[UEFI Update/Check Server]"; } }
                public static string DownloadUpdate { get { return "[UEFI Update/Download Update]"; } }
                public static string AutoRequestMotherboard { get { return "[UEFI Update/Check Server/Auto Request Motherboard Manufacturer]"; } }
            }
            public struct Shortcuts
            {
                public static string DownloadFolder { get { return "[Shortcuts/Download Folder]"; } }
            }
            public struct About
            {
                public struct Feedback
                {
                    public static string SendingFailed { get { return "[About/Feedback]"; } }
                    public static string DefaultMessage { get { return "Type your feedback message here. [Optional]"; } }
                    public static string Sending { get { return "[About/Feedback/Sending]"; } }
                }
            }
            public struct AccountManagement
            {
                public static string ChangePasswordFailed { get { return "[Account Management/Change Password]"; } }
                public static string LoginFailed { get { return "[Account Management/Login]"; } }
                public static string LogoutFailed { get { return "[Account Management/Logout]"; } }
            }
            public struct Testing
            {
                public static string TestingError { get { return "[Testing Stuff]"; } }
            }
            public struct ErrorReport
            {
                public static string DefaultMessage { get { return "Type your custom message here. [Optional]"; } }
            }
        }
        public struct ProgramInformation
        {
            public static string AssemblyTitle
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if (attributes.Length > 0)
                    {
                        AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                        if (titleAttribute.Title != string.Empty) return titleAttribute.Title;
                    }
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                }
            }
            public static string AssemblyFileVersion
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyFileVersionAttribute)attributes[0]).Version;
                }
            }
            public static string AssemblyDescription
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyDescriptionAttribute)attributes[0]).Description;
                }
            }
            public static string AssemblyProduct
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyProductAttribute)attributes[0]).Product;
                }
            }
            public static string AssemblyCopyright
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
                }
            }
            public static string AssemblyCompany
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyCompanyAttribute)attributes[0]).Company;
                }
            }
            public static string AssemblyTrademark
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((AssemblyTrademarkAttribute)attributes[0]).Trademark;
                }
            }
            public static string AssemblyGUID
            {
                get
                {
                    object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
                    if (attributes.Length == 0) return string.Empty;
                    return ((GuidAttribute)attributes[0]).Value;
                }
            }
            public static string GetBuildDateTime(string FileLocation)
            {
                if (File.Exists(FileLocation))
                {
                    var Buffer = new byte[Math.Max(Marshal.SizeOf(typeof(ImageFileHeader)), 4)];
                    using (var FileStream = new FileStream(FileLocation, FileMode.Open, FileAccess.Read))
                    {
                        FileStream.Position = 0x3C;
                        FileStream.Read(Buffer, 0, 4);
                        FileStream.Position = BitConverter.ToUInt32(Buffer, 0);
                        FileStream.Read(Buffer, 0, 4);
                        FileStream.Read(Buffer, 0, Buffer.Length);
                    }
                    var PinnedBuffer = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
                    try
                    {
                        var CoffHeader = (ImageFileHeader)Marshal.PtrToStructure(PinnedBuffer.AddrOfPinnedObject(), typeof(ImageFileHeader));
                        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(CoffHeader.TimeDateStamp * TimeSpan.TicksPerSecond)).ToString(DateFormats.LastChecked);
                    }
                    finally { PinnedBuffer.Free(); }
                }
                return null;
            }
        }
        public struct ImageFileHeader
        {
            public ushort Machine { get; set; }
            public ushort NumberOfSections { get; set; }
            public uint TimeDateStamp { get; set; }
            public uint PointerToSymbolTable { get; set; }
            public uint NumberOfSymbols { get; set; }
            public ushort SizeOfOptionalHeader { get; set; }
            public ushort Characteristics { get; set; }
        }
        public struct DateFormats
        {
            public static string LastChecked { get { return "MMMM d, yyyy h:mm:ss tt"; } }
            public static string UEFIBuildDateParseExtract { get { return "yyyyMMdd"; } }
            public static string LongDate { get { return "MMMM d, yyyy"; } }
            public static string HistoryLog { get { return "dddd h:mm:ss tt"; } }
        }
        public enum DebugAndReleaseUpdate : byte
        {
            UpdateToDate,
            UpdateFound,
            UpdateCancelled,
            Failed,
            Unknown
        }
        #endregion
    }
}