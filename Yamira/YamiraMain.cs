// ======================================================================================================
// Yamira - USB Drive Protection Software
// © Copyright 2024-2025, Eray Türkay.
// Project Type: Open Source
// License: MIT License
// Website: https://www.turkaysoftware.com/yamira
// GitHub: https://github.com/turkaysoftware/yamira
// ======================================================================================================

using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
// TS MODULES
using static Yamira.TSModules;

namespace Yamira{
    public partial class YamiraMain : Form{
        public YamiraMain(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            DataMainTable.RowTemplate.Height = (int)(26 * this.DeviceDpi / 96f);
            for (int i = 0; i < 6; i++){ DataMainTable.Columns.Add("y_data", "Data"); }
            DataMainTable.Columns[0].Width = (int)(75 * this.DeviceDpi / 96f);
            DataMainTable.Columns[1].Width = (int)(200 * this.DeviceDpi / 96f);
            DataMainTable.Columns[2].Width = (int)(90 * this.DeviceDpi / 96f);
            DataMainTable.Columns[3].Width = (int)(90 * this.DeviceDpi / 96f);
            DataMainTable.Columns[4].Width = (int)(90 * this.DeviceDpi / 96f);
            DataMainTable.Columns[5].Width = (int)(150 * this.DeviceDpi / 96f);
            foreach (DataGridViewColumn columnPadding in DataMainTable.Columns){
                int scaledPadding = (int)(3 * this.DeviceDpi / 96f);
                columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
            }
            //
            foreach (DataGridViewColumn DataTable in DataMainTable.Columns){
                DataTable.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // LANGUAGE SET MODES
            englishToolStripMenuItem.Tag = "en";
            polishToolStripMenuItem.Tag = "pl";
            turkishToolStripMenuItem.Tag = "tr";
            // LANGUAGE SET EVENTS
            englishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            polishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            turkishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            //
            TSThemeModeHelper.InitializeGlobalTheme();
            SystemEvents.UserPreferenceChanged += (s, e) => TSUseSystemTheme();
            //
            StartTSUSBIndicator();
        }
        // GLOBAL VARIABLES
        // ======================================================================================================
        public static string lang, lang_path, default_create_folder = "TSProtectionSystem";
        public static int theme, themeSystem, startup_status;
        // LOCAL VARIABLES
        // ======================================================================================================
        readonly string ts_wizard_name = "TS Wizard";
        readonly bool virtualization_mode = false;
        // USB STATUS INDICATOR
        // ======================================================================================================
        private ManagementEventWatcher TSUSBIndicator = null;
        private volatile bool isTSUSBIndicatorDispose = false;
        // UI COLORS
        // ======================================================================================================
        static readonly List<Color> header_colors = new List<Color>() { Color.Transparent, Color.Transparent, Color.Transparent };
        // HEADER SETTINGS
        // ======================================================================================================
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e){
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float dpiScale = g.DpiX / 96f;
                // TICK BG
                // using (SolidBrush bgBrush = new SolidBrush(header_colors[0])){ RectangleF bgRect = new RectangleF( e.ImageRectangle.Left, e.ImageRectangle.Top, e.ImageRectangle.Width,e.ImageRectangle.Height); g.FillRectangle(bgBrush, bgRect); }
                using (Pen anti_alias_pen = new Pen(header_colors[2], 3 * dpiScale)){
                    Rectangle rect = e.ImageRectangle;
                    Point p1 = new Point((int)(rect.Left + 3 * dpiScale), (int)(rect.Top + rect.Height / 2));
                    Point p2 = new Point((int)(rect.Left + 7 * dpiScale), (int)(rect.Bottom - 4 * dpiScale));
                    Point p3 = new Point((int)(rect.Right - 2 * dpiScale), (int)(rect.Top + 3 * dpiScale));
                    g.DrawLines(anti_alias_pen, new Point[] { p1, p2, p3 });
                }
            }
        }
        private class HeaderColors : ProfessionalColorTable{
            public override Color MenuItemSelected => header_colors[0];
            public override Color ToolStripDropDownBackground => header_colors[0];
            public override Color ImageMarginGradientBegin => header_colors[0];
            public override Color ImageMarginGradientEnd => header_colors[0];
            public override Color ImageMarginGradientMiddle => header_colors[0];
            public override Color MenuItemSelectedGradientBegin => header_colors[0];
            public override Color MenuItemSelectedGradientEnd => header_colors[0];
            public override Color MenuItemPressedGradientBegin => header_colors[0];
            public override Color MenuItemPressedGradientMiddle => header_colors[0];
            public override Color MenuItemPressedGradientEnd => header_colors[0];
            public override Color MenuItemBorder => header_colors[0];
            public override Color CheckBackground => header_colors[0];
            public override Color ButtonSelectedBorder => header_colors[0];
            public override Color CheckSelectedBackground => header_colors[0];
            public override Color CheckPressedBackground => header_colors[0];
            public override Color MenuBorder => header_colors[0];
            public override Color SeparatorLight => header_colors[1];
            public override Color SeparatorDark => header_colors[1];
        }

        // LOAD SOFTWARE SETTINGS
        // ======================================================================================================
        private void RunSoftwareEngine(){
            // DOUBLE BUFFERS
            // ======================================================================================================
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DataMainTable, new object[] { true });
            // THEME - LANG - VIEW MODE PRELOADER
            // ======================================================================================================
            TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
            //
            int theme_mode = int.TryParse(software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus"), out int the_status) && (the_status == 0 || the_status == 1 || the_status == 2) ? the_status : 1;
            if (theme_mode == 2) { themeSystem = 2; Theme_engine(GetSystemTheme(2)); } else Theme_engine(theme_mode);
            darkThemeToolStripMenuItem.Checked = theme_mode == 0;
            lightThemeToolStripMenuItem.Checked = theme_mode == 1;
            systemThemeToolStripMenuItem.Checked = theme_mode == 2;
            //
            string lang_mode = software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus");
            var languageFiles = new Dictionary<string, (object langResource, ToolStripMenuItem menuItem, bool fileExists)>{
                { "en", (ts_lang_en, englishToolStripMenuItem, File.Exists(ts_lang_en)) },
                { "pl", (ts_lang_pl, polishToolStripMenuItem, File.Exists(ts_lang_pl)) },
                { "tr", (ts_lang_tr, turkishToolStripMenuItem, File.Exists(ts_lang_tr)) },
            };
            foreach (var langLoader in languageFiles) { langLoader.Value.menuItem.Enabled = langLoader.Value.fileExists; }
            var (langResource, selectedMenuItem, _) = languageFiles.ContainsKey(lang_mode) ? languageFiles[lang_mode] : languageFiles["en"];
            Lang_engine(Convert.ToString(langResource), lang_mode);
            selectedMenuItem.Checked = true;
            //
            string startup_mode = software_read_settings.TSReadSettings(ts_settings_container, "StartupStatus");
            startup_status = int.TryParse(startup_mode, out int str_status) && (str_status == 0 || str_status == 1) ? str_status : 0;
            WindowState = startup_status == 1 ? FormWindowState.Maximized : FormWindowState.Normal;
            windowedToolStripMenuItem.Checked = startup_status == 0;
            fullScreenToolStripMenuItem.Checked = startup_status == 1;
        }
        // LOAD USB DRIVES
        // ======================================================================================================
        private void LoadUSBDrives(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                DataMainTable.Rows.Clear();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                //
                string protect_on = software_lang.TSReadLangs("Yamira", "y_protect_status_active");
                string protect_off = software_lang.TSReadLangs("Yamira", "y_protect_status_disabled");
                //
                int detectedDeviceCount = allDrives.Count(drive => drive.DriveType == DriveType.Removable && drive.IsReady);
                foreach (DriveInfo drive in allDrives.Where(d => d.DriveType == DriveType.Removable && d.IsReady)){
                    string deviceName = string.IsNullOrEmpty(drive.VolumeLabel) ? software_lang.TSReadLangs("Yamira", "y_default_name") : drive.VolumeLabel;
                    string writePermissionStatus = HasWritePermission(drive.RootDirectory.FullName) ? protect_off : protect_on;
                    DataMainTable.Rows.Add(drive.Name, deviceName, drive.DriveFormat, TS_FormatSize(drive.TotalSize), TS_FormatSize(drive.AvailableFreeSpace), writePermissionStatus);
                }
                DataMainTable.ClearSelection();
                Label_NotUSB.Visible = detectedDeviceCount == 0;
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_default_error"), ex.Message));
            }
        }
        private bool HasWritePermission(string path){
            try{
                string tempFilePath = Path.Combine(path, Path.GetRandomFileName());
                using (FileStream fs = File.Create(tempFilePath, 1, FileOptions.DeleteOnClose)){
                    return true;
                }
            }catch{ return false; }
        }
        private void VirtualizationModeData(){
            DataMainTable.Rows.Add(@"F:\", "TS SanDisk Ultra Luxe", "FAT32", "29,7 GB", "14,2 GB", "On");
            DataMainTable.Rows.Add(@"G:\", "TS Samsung Bar Plus", "NTFS", "59,5 GB", "27,5 GB", "On");
            DataMainTable.Rows.Add(@"H:\", "TS Samsung Bar Plus", "NTFS", "119,1 GB", "58,6 GB", "Off");
            DataMainTable.ClearSelection();
        }
        // REFRESH
        // ======================================================================================================
        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e){
            LoadUSBDrives();
        }
        // YAMIRA LOAD
        // ======================================================================================================
        private void Yamira_Load(object sender, EventArgs e){
            Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
            HeaderMenu.Cursor = Cursors.Hand;
            //
            RunSoftwareEngine();
            if (!virtualization_mode){
                LoadUSBDrives();
            }else{
                VirtualizationModeData();
            }
            //
            Task softwareUpdateCheck = Task.Run(() => Software_update_check(0));
        }
        // BTN ACTIVE PROTECT
        // ======================================================================================================
        private void BtnActiveProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_process_start_info"));
                return;
            }
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = software_lang.TSReadLangs("Yamira", "y_default_name");
            }
            if (HasWritePermission(rootPath)){
                if (driveInfo.DriveFormat == "NTFS"){
                    string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                    DialogResult check_open_protect = TS_MessageBoxEngine.TS_MessageBox(this, 4, software_lang.TSReadLangs("Yamira", "y_protect_on_info"));
                    if (check_open_protect == DialogResult.Yes){
                        try{
                            if (Directory.Exists(rootPath)){
                                if (!Directory.Exists(firewallFolderPath)){
                                    Directory.CreateDirectory(firewallFolderPath);
                                }
                                // Root dizine yazma izni kaldırılıyor, firewall klasörüne yazma izni veriliyor
                                TS_USBProtect(rootPath, false); // Root directory write off (yazma kapalı)
                                TS_USBProtect(firewallFolderPath, true); // Firewall folder write on (yazma açık)
                                //
                                LoadUSBDrives();
                                //
                                DialogResult successAfterOpen = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("Yamira", "y_process_success"), $"{rootPath} ({deviceName})", "\n\n", firewallFolderPath, "\n\n"));
                                if (successAfterOpen == DialogResult.Yes){
                                    Process.Start(firewallFolderPath);
                                }
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Yamira", "y_root_directory_not_null"));
                            }
                        }catch (Exception ex){
                            LoadUSBDrives();
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_default_error"), ex.Message));
                        }
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system"));
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("Yamira", "y_protect_on"));
            }
        }
        // BTN DISABLED PROTECT
        // ======================================================================================================
        private void BtnDisabledProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_process_start_info"));
                return;
            }
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = software_lang.TSReadLangs("Yamira", "y_default_name");
            }
            if (!HasWritePermission(rootPath)){
                string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                DialogResult check_disabled_protect = TS_MessageBoxEngine.TS_MessageBox(this, 4, software_lang.TSReadLangs("Yamira", "y_protect_off_info"));
                if (check_disabled_protect == DialogResult.Yes){
                    try{
                        if (Directory.Exists(rootPath)){
                            // Root dizine yazma izni veriliyor, firewall klasörüne de izin veriliyor
                            TS_USBProtect(rootPath, true);  // Root directory write on (yazma açık)
                            TS_USBProtect(firewallFolderPath, true); // Firewall folder write on (yazma açık)
                            //
                            LoadUSBDrives();
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("Yamira", "y_process_disabled_success"), rootPath, deviceName));
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Yamira", "y_root_directory_not_null"));
                        }
                    }catch (Exception ex){
                        LoadUSBDrives();
                        TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_default_error"), ex.Message));
                    }
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("Yamira", "y_protect_off"));
            }
        }
        // TS USB PROTECT ALGORITHM
        // ======================================================================================================
        void TS_USBProtect(string folderPath, bool allowWriteAccess){
            try{
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();
                string allUsers = "Everyone"; // 'Everyone' group for all users
                // Önce tüm Write izin engelleme kurallarını kaldır (varsa)
                var denyRules = dirSecurity.GetAccessRules(true, true, typeof(NTAccount)).OfType<FileSystemAccessRule>().Where(r => r.IdentityReference.Value == allUsers && r.FileSystemRights.HasFlag(FileSystemRights.Write) && r.AccessControlType == AccessControlType.Deny).ToList();
                foreach (var rule in denyRules){
                    dirSecurity.RemoveAccessRule(rule);
                }
                if (allowWriteAccess){
                    // Write erişim izni ver
                    dirSecurity.AddAccessRule(new FileSystemAccessRule(allUsers, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                }else{
                    // Write erişim engelle
                    dirSecurity.AddAccessRule(new FileSystemAccessRule(allUsers, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny));
                }
                dirInfo.SetAccessControl(dirSecurity);
            }catch (Exception ex){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_default_error"), ex.Message));
            }
        }
        // NTFS FORMAT BTN
        // ======================================================================================================
        private void BtnFormatNTFS_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_process_start_info"));
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            //
            if (Directory.Exists(rootPath)){
                DriveInfo driveInfo = new DriveInfo(rootPath);
                // Check file system
                if (!string.Equals(driveInfo.DriveFormat, "NTFS", StringComparison.OrdinalIgnoreCase)){
                    DialogResult result = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("Yamira", "y_ntfs_warning"), "\n\n", "\n\n"));
                    if (result == DialogResult.Yes){
                        try{
                            string userInput = Interaction.InputBox(
                                software_lang.TSReadLangs("Yamira", "y_ntfs_info"),
                                software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name"),
                                software_lang.TSReadLangs("Yamira", "y_default_name"),
                                -1,
                                -1
                            );
                            if (!string.IsNullOrEmpty(userInput)){
                                string pattern = @"[^\w\s]";
                                bool containsSpecialCharacters = Regex.IsMatch(userInput, pattern);
                                //
                                if (!containsSpecialCharacters) {
                                    // Formatting process
                                    Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode) + " - " + software_lang.TSReadLangs("Yamira", "y_ntfs_format_title");
                                    Task formatNtfs = Task.Run(() => FormatDrive(driveInfo.RootDirectory.ToString(), TSFormatSafeLabel(userInput)));
                                }
                                else{
                                    TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_ntfs_info"));
                                }
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name_null"));
                            }
                        }catch (Exception){ }
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system_not"));
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Yamira", "y_root_directory_not_null"));
            }
        }
        // NTFS FORMAT DRIVE ALGORITHM
        // ======================================================================================================
        private void FormatDrive(string driveLetter, string driveName){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string scriptFilePath = null;
            try{
                char letter = char.ToUpperInvariant(driveLetter.Trim()[0]);
                string diskPartScript = $"select volume {letter}:\r\nformat fs=ntfs quick label=\"{driveName}\"";
                //
                scriptFilePath = Path.GetTempFileName();
                File.WriteAllText(scriptFilePath, diskPartScript);
                //
                ProcessStartInfo processInfo = new ProcessStartInfo{
                    FileName = "diskpart.exe",
                    Arguments = $"/s \"{scriptFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas"
                };
                using (Process process = Process.Start(processInfo)){
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
                    LoadUSBDrives();
                    if (process.ExitCode == 0){
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("Yamira", "y_ntfs_format_success"));
                    }else{
                        TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_ntfs_format_error"), error));
                    }
                }
            }catch (Exception ex){
                Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
                LoadUSBDrives();
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_default_error"), ex.Message));
            }
            finally{
                if (!string.IsNullOrEmpty(scriptFilePath) && File.Exists(scriptFilePath)){
                    try{
                        File.Delete(scriptFilePath);
                    }catch { }
                }
            }
        }
        public string TSFormatSafeLabel(string input){
            if (string.IsNullOrWhiteSpace(input))
                return "DEFAULT_LABEL";
            var replacements = new (string, string)[]{
                ("ç", "c"), ("Ç", "C"),
                ("ı", "i"), ("I", "I"),
                ("ğ", "g"), ("Ğ", "G"),
                ("ö", "o"), ("Ö", "O"),
                ("ş", "s"), ("Ş", "S"),
                ("ü", "u"), ("Ü", "U")
            };
            var sb = new StringBuilder(input.Trim());
            foreach (var (oldStr, newStr) in replacements){
                sb.Replace(oldStr, newStr);
            }
            string result = sb.ToString();
            //
            char[] invalidChars = Path.GetInvalidFileNameChars();
            var cleanSb = new StringBuilder();
            foreach (char c in result){
                if (!invalidChars.Contains(c)){
                    cleanSb.Append(c);
                }else{
                    cleanSb.Append('_');
                }
            }
            result = cleanSb.ToString();
            result = result.Replace(' ', '_');
            if (result.Length > 32)
                result = result.Substring(0, 32);
            if (string.IsNullOrWhiteSpace(result))
                result = "DEFAULT_LABEL";
            return result;
        }
        // ======================================================================================================
        // USB STATUS INDICATOR
        private void StartTSUSBIndicator(){
            Task.Run(() =>{
                try{
                    if (TSUSBIndicator == null){
                        // USB takılma (EventType=2) veya çıkarılma (EventType=3) olaylarını dinle
                        var query_usb_indicator = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 OR EventType = 3");
                        TSUSBIndicator = new ManagementEventWatcher(query_usb_indicator);
                        TSUSBIndicator.EventArrived += USBIndicator_EventArrived;
                        TSUSBIndicator.Start();
                    }
                }catch (Exception ex){
                    if (this.IsHandleCreated && !this.IsDisposed){
                        this.BeginInvoke((MethodInvoker)(() =>{
                            TSGetLangs software_lang = new TSGetLangs(lang_path);
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Yamira", "y_usb_indicator_error"), ex.Message));
                        }));
                    }
                }
            });
        }
        private void USBIndicator_EventArrived(object sender, EventArrivedEventArgs e){
            if (isTSUSBIndicatorDispose) return;
            if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing){
                try{
                    this.BeginInvoke((MethodInvoker)(() =>{
                        if (isTSUSBIndicatorDispose) return;
                        LoadUSBDrives();
                    }));
                }catch{ }
            }
        }
        private void DisposeUSBIndicator(){
            if (TSUSBIndicator != null){
                try{
                    isTSUSBIndicatorDispose = true;
                    TSUSBIndicator.EventArrived -= USBIndicator_EventArrived;
                    TSUSBIndicator.Stop();
                    TSUSBIndicator.Dispose();
                    TSUSBIndicator = null;
                }catch{ }
                finally{
                    isTSUSBIndicatorDispose = false;
                }
            }
        }
        // ======================================================================================================
        // THEME SETTINGS
        private ToolStripMenuItem selected_theme = null;
        private void Select_theme_active(object target_theme){
            if (target_theme == null)
                return;
            ToolStripMenuItem clicked_theme = (ToolStripMenuItem)target_theme;
            if (selected_theme == clicked_theme)
                return;
            Select_theme_deactive();
            selected_theme = clicked_theme;
            selected_theme.Checked = true;
        }
        private void Select_theme_deactive(){
            foreach (ToolStripMenuItem theme in themeToolStripMenuItem.DropDownItems){
                theme.Checked = false;
            }
        }
        // THEME SWAP
        // ======================================================================================================
        private void SystemThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 2; Theme_engine(GetSystemTheme(2)); SaveTheme(2); Select_theme_active(sender);
        }
        private void LightThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 0; Theme_engine(1); SaveTheme(1); Select_theme_active(sender);
        }
        private void DarkThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 0; Theme_engine(0); SaveTheme(0); Select_theme_active(sender);
        }
        private void TSUseSystemTheme(){ if (themeSystem == 2) Theme_engine(GetSystemTheme(2)); }
        private void SaveTheme(int ts){
            // SAVE CURRENT THEME
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "ThemeStatus", Convert.ToString(ts));
            }catch (Exception){ }
        }
        private void Theme_engine(int ts){
            try{
                theme = ts;
                //
                TSThemeModeHelper.SetThemeMode(ts == 0);
                //
                if (theme == 1){
                    TSImageRenderer(settingsToolStripMenuItem, Properties.Resources.tm_settings_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(themeToolStripMenuItem, Properties.Resources.tm_theme_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(languageToolStripMenuItem, Properties.Resources.tm_language_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(startupToolStripMenuItem, Properties.Resources.tm_startup_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(checkforUpdatesToolStripMenuItem, Properties.Resources.tm_update_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(refreshToolStripMenuItem, Properties.Resources.tm_refresh_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(tSWizardToolStripMenuItem, Properties.Resources.tm_ts_wizard_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(bmacToolStripMenuItem, Properties.Resources.tm_bmac_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(aboutToolStripMenuItem, Properties.Resources.tm_about_light, 0, ContentAlignment.MiddleRight);
                    // UI
                    TSImageRenderer(BtnActiveProtect, Properties.Resources.ct_secure_on_light, 15, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BtnDisabledProtect, Properties.Resources.ct_secure_off_light, 15, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BtnFormatNTFS, Properties.Resources.ct_ntfs_format_light, 15, ContentAlignment.MiddleLeft);
                }else if (theme == 0){
                    TSImageRenderer(settingsToolStripMenuItem, Properties.Resources.tm_settings_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(themeToolStripMenuItem, Properties.Resources.tm_theme_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(languageToolStripMenuItem, Properties.Resources.tm_language_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(startupToolStripMenuItem, Properties.Resources.tm_startup_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(checkforUpdatesToolStripMenuItem, Properties.Resources.tm_update_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(refreshToolStripMenuItem, Properties.Resources.tm_refresh_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(tSWizardToolStripMenuItem, Properties.Resources.tm_ts_wizard_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(bmacToolStripMenuItem, Properties.Resources.tm_bmac_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(aboutToolStripMenuItem, Properties.Resources.tm_about_dark, 0, ContentAlignment.MiddleRight);
                    // UI
                    TSImageRenderer(BtnActiveProtect, Properties.Resources.ct_secure_on_dark, 15, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BtnDisabledProtect, Properties.Resources.ct_secure_off_dark, 15, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BtnFormatNTFS, Properties.Resources.ct_ntfs_format_dark, 15, ContentAlignment.MiddleLeft);
                }
                //
                header_colors[0] = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                header_colors[1] = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                header_colors[2] = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                HeaderMenu.Renderer = new HeaderMenuColors();
                // TOOLTIP
                MainToolTip.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                MainToolTip.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // HEADER MENU
                var bg = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                var fg = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                HeaderMenu.ForeColor = fg;
                HeaderMenu.BackColor = bg;
                SetMenuStripColors(HeaderMenu, bg, fg);
                // CONTENT BG
                BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGColor");
                Panel_Right.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(theme), "PageContainerBGColor");
                // ALL BUTTON
                foreach (Control control in FLP_Container.Controls){
                    if (control is Button button){
                        button.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBGColor");
                        button.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                        button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                        button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                        button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                    }
                }
                // NOT USB DEVICE LABEL
                Label_NotUSB.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                Label_NotUSB.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // DATA TABLE
                DataMainTable.BackgroundColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                DataMainTable.GridColor = TS_ThemeEngine.ColorMode(theme, "DataGridGridColor");
                DataMainTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                DataMainTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridFEColor");
                DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridAlternatingColor");
                DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridHeaderBGColor");
                DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "DataGridHeaderBGColor");
                DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridHeaderFEColor");
                DataMainTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "DataGridHeaderBGColor");
                DataMainTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridHeaderFEColor");
                //
                Software_other_page_preloader();
            }catch (Exception){ }
        }
        private void SetMenuStripColors(MenuStrip menuStrip, Color bgColor, Color fgColor){
            if (menuStrip == null) return;
            foreach (ToolStripItem item in menuStrip.Items){
                if (item is ToolStripMenuItem menuItem){
                    SetMenuItemColors(menuItem, bgColor, fgColor);
                }
            }
        }
        private void SetMenuItemColors(ToolStripMenuItem menuItem, Color bgColor, Color fgColor){
            if (menuItem == null) return;
            menuItem.BackColor = bgColor;
            menuItem.ForeColor = fgColor;
            foreach (ToolStripItem item in menuItem.DropDownItems){
                if (item is ToolStripMenuItem subMenuItem){
                    SetMenuItemColors(subMenuItem, bgColor, fgColor);
                }
            }
        }
        private void SetContextMenuColors(ContextMenuStrip contextMenu, Color bgColor, Color fgColor){
            if (contextMenu == null) return;
            foreach (ToolStripItem item in contextMenu.Items){
                if (item is ToolStripMenuItem menuItem){
                    SetMenuItemColors(menuItem, bgColor, fgColor);
                }
            }
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private void Select_lang_active(object target_lang){
            ToolStripMenuItem selected_lang = null;
            Select_lang_deactive();
            if (target_lang != null){
                if (selected_lang != (ToolStripMenuItem)target_lang){
                    selected_lang = (ToolStripMenuItem)target_lang;
                    selected_lang.Checked = true;
                }
            }
        }
        private void Select_lang_deactive(){
            foreach (ToolStripMenuItem disabled_lang in languageToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        private void LanguageToolStripMenuItem_Click(object sender, EventArgs e){
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string langCode){
                if (lang != langCode && AllLanguageFiles.ContainsKey(langCode)){
                    Lang_preload(AllLanguageFiles[langCode], langCode);
                    Select_lang_active(sender);
                }
            }
        }
        private void Lang_preload(string lang_type, string lang_code){
            Lang_engine(lang_type, lang_code);
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_code);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult lang_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("LangChange", "lang_change_notification"), "\n\n", "\n\n"));
            if (lang_change_message == DialogResult.Yes){ DisposeUSBIndicator(); Application.Restart(); }
        }
        private void Lang_engine(string lang_type, string lang_code){
            try{
                lang_path = lang_type;
                lang = lang_code;
                // GLOBAL ENGINE
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                // SETTINGS
                settingsToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_settings");
                // THEMES
                themeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_theme");
                lightThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_light");
                darkThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_dark");
                systemThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_system");
                // LANGS
                languageToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_language");
                englishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_en");
                polishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_pl");
                turkishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_tr");
                // INITIAL VIEW
                startupToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_start");
                windowedToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_windowed");
                fullScreenToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_full_screen");
                // UPDATE CHECK
                checkforUpdatesToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_update");
                // REFRESH
                refreshToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_refresh");
                // TS WIZARD
                tSWizardToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_ts_wizard");
                // BMAC
                bmacToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_bmac");
                // ABOUT
                aboutToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_about");
                //
                DataMainTable.Columns[0].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_root_directory");
                DataMainTable.Columns[1].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_device_name");
                DataMainTable.Columns[2].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_file_system");
                DataMainTable.Columns[3].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_size");
                DataMainTable.Columns[4].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_freespace");
                DataMainTable.Columns[5].HeaderText = software_lang.TSReadLangs("Yamira", "y_dgv_protect_mode");
                //
                BtnActiveProtect.Text = " " + software_lang.TSReadLangs("Yamira", "y_btn_protect_active");
                BtnDisabledProtect.Text = " " + software_lang.TSReadLangs("Yamira", "y_btn_protect_disabled");
                BtnFormatNTFS.Text = " " + software_lang.TSReadLangs("Yamira", "y_btn_ntfs_format");
                //
                Label_NotUSB.Text = software_lang.TSReadLangs("Yamira", "y_not_usb_storage_device");
                //
                Software_other_page_preloader();
            }catch (Exception){ }
        }
        private void Software_other_page_preloader(){
            // SOFTWARE ABOUT
            try{
                YamiraAbout software_about = new YamiraAbout();
                string software_about_name = "yamira_about";
                software_about.Name = software_about_name;
                if (Application.OpenForms[software_about_name] != null){
                    software_about = (YamiraAbout)Application.OpenForms[software_about_name];
                    software_about.About_preloader();
                }
            }catch (Exception){ }
        }
        // STARTUP SETINGS
        // ======================================================================================================
        private void Select_startup_mode_active(object target_startup_mode){
            ToolStripMenuItem selected_startup_mode = null;
            Select_startup_mode_deactive();
            if (target_startup_mode != null){
                if (selected_startup_mode != (ToolStripMenuItem)target_startup_mode){
                    selected_startup_mode = (ToolStripMenuItem)target_startup_mode;
                    selected_startup_mode.Checked = true;
                }
            }
        }
        private void Select_startup_mode_deactive(){
            foreach (ToolStripMenuItem disabled_startup in startupToolStripMenuItem.DropDownItems){
                disabled_startup.Checked = false;
            }
        }
        private void WindowedToolStripMenuItem_Click(object sender, EventArgs e){
            if (startup_status != 0){ startup_status = 0; Startup_mode_settings("0"); Select_startup_mode_active(sender); }
        }
        private void FullScreenToolStripMenuItem_Click(object sender, EventArgs e){
            if (startup_status != 1){ startup_status = 1; Startup_mode_settings("1"); Select_startup_mode_active(sender); }
        }
        private void Startup_mode_settings(string get_startup_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "StartupStatus", get_startup_value);
            }catch (Exception){ }
        }
        // SOFTWARE OPERATION CONTROLLER MODULE
        // ======================================================================================================
        private static bool Software_operation_controller(string __target_software_path){
            var exeFiles = Directory.GetFiles(__target_software_path, "*.exe");
            var runned_process = Process.GetProcesses();
            foreach (var exe_path in exeFiles){
                string exe_name = Path.GetFileNameWithoutExtension(exe_path);
                if (runned_process.Any(p => {
                    try{
                        return string.Equals(p.ProcessName, exe_name, StringComparison.OrdinalIgnoreCase);
                    }catch{
                        return false;
                    }
                })){
                    return true;
                }
            }
            return false;
        }
        // TS WIZARD STARTER MODE
        // ======================================================================================================
        private string[] Ts_wizard_starter_mode(){
            string[] ts_wizard_exe_files = { "TSWizard_arm64.exe", "TSWizard_x64.exe", "TSWizard.exe" };
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64){
                return new[] { ts_wizard_exe_files[0], ts_wizard_exe_files[1], ts_wizard_exe_files[2] }; // arm64 > x64 > default
            }else if (Environment.Is64BitOperatingSystem){
                return new[] { ts_wizard_exe_files[1], ts_wizard_exe_files[0], ts_wizard_exe_files[2] }; // x64 > arm64 > default
            }else{
                return new[] { ts_wizard_exe_files[2], ts_wizard_exe_files[1], ts_wizard_exe_files[0] }; // default > x64 > arm64
            }
        }
        // UPDATE CHECK ENGINE
        // ======================================================================================================
        private void CheckforUpdatesToolStripMenuItem_Click(object sender, EventArgs e){
            Software_update_check(1);
        }
        public void Software_update_check(int _check_update_ui){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (!IsNetworkCheck()){
                    if (_check_update_ui == 1){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_not_connection"), "\n\n"), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                    }
                    return;
                }
                using (WebClient webClient = new WebClient()){
                    string client_version = TS_VersionEngine.TS_SofwareVersion(2, Program.ts_version_mode).Trim();
                    int client_num_version = Convert.ToInt32(client_version.Replace(".", string.Empty));
                    //
                    string[] version_content = webClient.DownloadString(TS_LinkSystem.github_link_lv).Split('=');
                    string last_version = version_content[1].Trim();
                    int last_num_version = Convert.ToInt32(last_version.Replace(".", string.Empty));
                    //
                    if (client_num_version < last_num_version){
                        try{
                            string baseDir = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName);
                            string ts_wizard_path = Ts_wizard_starter_mode().Select(name => Path.Combine(baseDir, name)).FirstOrDefault(File.Exists);
                            //
                            if (ts_wizard_path != null){
                                if (!Software_operation_controller(Path.GetDirectoryName(ts_wizard_path))){
                                    // Update available
                                    DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_available_ts_wizard"), Application.ProductName, "\n\n", client_version, "\n", last_version, "\n\n", ts_wizard_name), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                                    if (info_update == DialogResult.Yes){
                                        Process.Start(new ProcessStartInfo { FileName = ts_wizard_path, WorkingDirectory = Path.GetDirectoryName(ts_wizard_path) });
                                    }
                                }else{
                                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification"), ts_wizard_name));
                                }
                            }else{
                                // Update available
                                DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_available"), Application.ProductName, "\n\n", client_version, "\n", last_version, "\n\n"), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                                if (info_update == DialogResult.Yes){
                                    Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link_lr) { UseShellExecute = true });
                                }
                            }
                        }catch (Exception){ }
                    }else if (_check_update_ui == 1 && client_num_version == last_num_version){
                        // No update available
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_not_available"), Application.ProductName, "\n", client_version), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                    }else if (_check_update_ui == 1 && client_num_version > last_num_version){
                        // Access before public use
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_newer"), "\n\n", string.Format("v{0}", client_version)), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                    }
                }
            }catch (Exception ex){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_error"), "\n\n", ex.Message), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
            }
        }
        // BUY ME A COFFEE LINK
        // ======================================================================================================
        private void BmacToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_bmac) { UseShellExecute = true });
            }catch (Exception){ }
        }
        // TS WIZARD
        private void TSWizardToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                string baseDir = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName);
                string ts_wizard_path = Ts_wizard_starter_mode().Select(name => Path.Combine(baseDir, name)).FirstOrDefault(File.Exists);
                //
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                //
                if (ts_wizard_path != null){
                    if (!Software_operation_controller(Path.GetDirectoryName(ts_wizard_path))){
                        Process.Start(new ProcessStartInfo { FileName = ts_wizard_path, WorkingDirectory = Path.GetDirectoryName(ts_wizard_path) });
                    }else{
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification"), ts_wizard_name));
                    }
                }else{
                    DialogResult ts_wizard_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("TSWizard", "tsw_content"), software_lang.TSReadLangs("HeaderMenu", "header_menu_ts_wizard"), Application.CompanyName, "\n\n", Application.ProductName, Application.CompanyName, "\n\n"), string.Format("{0} - {1}", Application.ProductName, ts_wizard_name));
                    if (ts_wizard_query == DialogResult.Yes){
                        Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_wizard) { UseShellExecute = true });
                    }
                }
            }catch (Exception){ }
        }
        // TS TOOL LAUNCHER MODULE
        // ======================================================================================================
        private void TSToolLauncher<T>(string formName, string langKey) where T : Form, new(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                T tool = new T { Name = formName };
                if (Application.OpenForms[formName] == null){
                    tool.Show();
                }else{
                    if (Application.OpenForms[formName].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[formName].WindowState = FormWindowState.Normal;
                    }
                    string public_message = string.Format(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification"), software_lang.TSReadLangs("HeaderMenu", langKey));
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, public_message);
                    Application.OpenForms[formName].Activate();
                }
            }catch (Exception){ }
        }
        // YAMIRA ABOUT
        // ======================================================================================================
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e){
            TSToolLauncher<YamiraAbout>("yamira_about", "header_menu_about");
        }
        // EXIT
        // ======================================================================================================
        private void Yamira_FormClosing(object sender, FormClosingEventArgs e){
            DisposeUSBIndicator();
            Application.Exit();
        }
    }
}