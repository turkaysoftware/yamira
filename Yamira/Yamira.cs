using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
//
using static Yamira.TSModules;

namespace Yamira{
    public partial class Yamira : Form{
        public Yamira(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            DataMainTable.Columns.Add("x", "x");
            DataMainTable.Columns.Add("x", "x");
            DataMainTable.Columns.Add("x", "x");
            DataMainTable.Columns.Add("x", "x");
            DataMainTable.Columns.Add("x", "x");
            //
            foreach (DataGridViewColumn DataTable in DataMainTable.Columns){
                DataTable.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        // GLOBAL VARIABLES
        // ======================================================================================================
        public static string lang, lang_path, default_create_folder = "Firewall";
        public static int theme, ts_version_mode = 0, initial_status;
        // SOFTWARE VERSION - MEDIA LINK SYSTEM
        // ======================================================================================================
        static TS_VersionEngine TS_SoftwareVersion = new TS_VersionEngine();
        static TS_LinkSystem TS_LinkSystem = new TS_LinkSystem();
        // UI COLORS
        // ======================================================================================================
        static List<Color> header_colors = new List<Color>() { Color.Transparent, Color.Transparent };
        // HEADER SETTINGS
        // ======================================================================================================
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
        }
        private class HeaderColors : ProfessionalColorTable{
            public override Color MenuItemSelected { get { return header_colors[0]; } }
            public override Color ToolStripDropDownBackground { get { return header_colors[0]; } }
            public override Color ImageMarginGradientBegin { get { return header_colors[0]; } }
            public override Color ImageMarginGradientEnd { get { return header_colors[0]; } }
            public override Color ImageMarginGradientMiddle { get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientBegin { get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientEnd { get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientBegin { get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientMiddle { get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientEnd { get { return header_colors[0]; } }
            public override Color MenuItemBorder { get { return header_colors[0]; } }
            public override Color CheckBackground { get { return header_colors[0]; } }
            public override Color ButtonSelectedBorder { get { return header_colors[0]; } }
            public override Color CheckSelectedBackground { get { return header_colors[0]; } }
            public override Color CheckPressedBackground { get { return header_colors[0]; } }
            public override Color MenuBorder { get { return header_colors[0]; } }
            public override Color SeparatorLight { get { return header_colors[1]; } }
            public override Color SeparatorDark { get { return header_colors[1]; } }
        }
        // ======================================================================================================
        // SOFTWARE PRELOADER
        /*
            -- THEME --      |  -- LANGUAGE --    |   -- INITIAL MODE --
            0 = Dark Theme   |  Moved to          |   0 = Normal Windowed
            1 = Light Theme  |  TSModules.cs      |   1 = FullScreen Mode
        */
        private void software_preloader(){
            try{
                //
                bool alt_lang_available = false;
                bool en_lang_available = false;
                //
                // CHECK OS NAME
                string ui_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                // CHECK SOFTWARE LANG FOLDER
                if (Directory.Exists(yamira_lf)){
                    // CHECK LANG FILES
                    int get_langs_file = Directory.GetFiles(yamira_lf, "*.ini").Length;
                    if (get_langs_file > 0){
                        // CHECK SETTINGS
                        try{
                            // CHECK LANG FILES
                            if (!File.Exists(yamira_lang_en)){ englishToolStripMenuItem.Enabled = false; }else{ en_lang_available = true; }
                            if (!File.Exists(yamira_lang_tr)){ turkishToolStripMenuItem.Enabled = false; }else{ alt_lang_available = true; }
                            //
                            if (en_lang_available == true){
                                // CHECK TR LANG
                                if (File.Exists(ts_sf)){
                                    GetSoftwareSetting(alt_lang_available);
                                }else{
                                    // DETECT SYSTEM THEME
                                    TSSettingsSave software_settings_save = new TSSettingsSave(ts_sf);
                                    string get_system_theme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "").ToString().Trim();
                                    software_settings_save.TSWriteSettings(ts_settings_container, "ThemeStatus", get_system_theme);
                                    // DETECT SYSTEM LANG / BYPASS LANGUAGE
                                    if (alt_lang_available){
                                        if (ui_lang != "" && ui_lang != string.Empty){
                                            software_settings_save.TSWriteSettings(ts_settings_container, "LanguageStatus", ui_lang);
                                        }else{
                                            software_settings_save.TSWriteSettings(ts_settings_container, "LanguageStatus", "en");
                                        }
                                        GetSoftwareSetting(true);
                                    }else{
                                        software_settings_save.TSWriteSettings(ts_settings_container, "LanguageStatus", "en");
                                        GetSoftwareSetting(false);
                                    }
                                    // SET INITIAL MODE
                                    software_settings_save.TSWriteSettings(ts_settings_container, "InitialStatus", "0");
                                }
                            }else{
                                software_prelaoder_alert(0);
                            }
                        }catch (Exception){ }
                    }else{
                        software_prelaoder_alert(1);
                    }
                }else{
                    software_prelaoder_alert(2);
                }
            }catch (Exception){ }
        }
        // PRELOAD ALERT
        // ======================================================================================================
        private void software_prelaoder_alert(int pre_mode){
            DialogResult open_last_release = DialogResult.OK;
            if (pre_mode == 0){
                open_last_release = MessageBox.Show($"English language (English.ini) is a compulsory language. The English.ini file seems to be missing.\n\nWould you like to view and download the latest version of {Application.ProductName} again?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }else if (pre_mode == 1){
                open_last_release = MessageBox.Show($"No language file found.\nThere seems to be a problem with the language files.\n\nWould you like to view and download the latest version of {Application.ProductName} again?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }else if (pre_mode == 2){
                open_last_release = MessageBox.Show($"y_langs folder not found.\nThe folder seems to be missing.\n\nWould you like to view and download the latest version of {Application.ProductName} again?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
            if (open_last_release == DialogResult.Yes){
                Process.Start(TS_LinkSystem.github_link_lr);
            }
            software_exit();
        }
        // SOFTWARE LOAD LANGS SETTINGS
        // ======================================================================================================
        private void GetSoftwareSetting(bool _lang_wrapper){
            // THEME - LANG - VIEW MODE PRELOADER
            TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
            string theme_mode = software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus");
            switch (theme_mode){
                case "0":
                    theme_engine(0);
                    darkThemeToolStripMenuItem.Checked = true;
                    break;
                default:
                    theme_engine(1);
                    lightThemeToolStripMenuItem.Checked = true;
                    break;
            }
            string lang_mode = software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus");
            if (_lang_wrapper){
                switch (lang_mode){
                    case "en":
                        lang_engine(yamira_lang_en, "en");
                        englishToolStripMenuItem.Checked = true;
                        break;
                    case "tr":
                        lang_engine(yamira_lang_tr, "tr");
                        turkishToolStripMenuItem.Checked = true;
                        break;
                    default:
                        lang_engine(yamira_lang_en, "en");
                        englishToolStripMenuItem.Checked = true;
                        break;
                }
            }else{
                lang_engine(yamira_lang_en, "en");
                englishToolStripMenuItem.Checked = true;
            }
            string initial_mode = software_read_settings.TSReadSettings(ts_settings_container, "InitialStatus");
            switch (initial_mode){
                case "0":
                    initial_status = 0;
                    windowedToolStripMenuItem.Checked = true;
                    //WindowState = FormWindowState.Normal;
                    break;
                case "1":
                    initial_status = 1;
                    fullScreenToolStripMenuItem.Checked = true;
                    WindowState = FormWindowState.Maximized;
                    break;
                default:
                    initial_status = 0;
                    windowedToolStripMenuItem.Checked = true;
                    //WindowState = FormWindowState.Normal;
                    break;
            }
        }
        // LOAD USB DRIVES
        // ======================================================================================================
        private void LoadUSBDrives(){
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            DataMainTable.Rows.Clear();
            int detectedDeviceCount = 0;
            //
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string protect_on = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_status_active").Trim()));
            string protect_off = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_status_disabled").Trim()));
            //
            foreach (DriveInfo drive in allDrives){
                if (drive.DriveType == DriveType.Removable && drive.IsReady){
                    detectedDeviceCount++;
                    string deviceName = drive.VolumeLabel;
                    if (string.IsNullOrEmpty(deviceName)){
                        deviceName = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_name").Trim()));
                    }
                    //
                    bool hasWritePermission = HasWritePermission(drive.RootDirectory.FullName);
                    //
                    string writePermissionStatus = hasWritePermission ? protect_on : protect_off;
                    //
                    DataMainTable.Rows.Add(drive.Name, deviceName, drive.DriveFormat, TS_FormatSize(drive.TotalSize), writePermissionStatus.ToString());
                }
            }
            DataMainTable.ClearSelection();
            // Not device found
            if (detectedDeviceCount == 0){
                Label_NotUSB.Visible = true;
            }else{
                Label_NotUSB.Visible = false;
            }
        }
        private bool HasWritePermission(string path){
            try{
                // Try creating a temporary file
                string tempFilePath = Path.Combine(path, Path.GetRandomFileName());
                using (FileStream fs = File.Create(tempFilePath, 1, FileOptions.DeleteOnClose)){
                    return false;
                }
            }catch{
                return true;
            }
        }
        // REFRESH
        // ======================================================================================================
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e){
            LoadUSBDrives();
        }
        // YAMIRA LOAD
        // ======================================================================================================
        private void Yamira_Load(object sender, EventArgs e){
            Text = TS_SoftwareVersion.TS_SofwareVersion(0, ts_version_mode);
            HeaderMenu.Cursor = Cursors.Hand;
            //
            software_preloader();
            LoadUSBDrives();
            //
            Task softwareUpdateCheck = Task.Run(() => software_update_check(0));
        }
        // BTN ACTIVE PROTECT
        // ======================================================================================================
        private void BtnActiveProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_process_start_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string fileSystem = driveInfo.DriveFormat;
            //
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_name").Trim()));
            }
            //
            if (!HasWritePermission(rootPath)){
                if (driveInfo.DriveFormat == "NTFS"){
                    string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                    DialogResult check_open_protect = MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_on_info").Trim())), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (check_open_protect == DialogResult.Yes){
                        try{
                            if (Directory.Exists(rootPath)){
                                // Create Folder Check
                                if (!Directory.Exists(firewallFolderPath)){
                                    Directory.CreateDirectory(firewallFolderPath);
                                }
                                // Remove write permission from the root directory and give write permission back to the Firewall folder
                                TS_USBProtect(rootPath, false); // Root directory write off
                                TS_USBProtect(firewallFolderPath, true); // Firewall folder write on
                                LoadUSBDrives();
                                //
                                DialogResult successAfterOpen = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_process_success").Trim())), string.Format("{0} ({1})", rootPath, deviceName), "\n\n", firewallFolderPath, "\n\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (successAfterOpen == DialogResult.Yes){
                                    Process.Start(firewallFolderPath);
                                }
                            }else{
                                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }catch (Exception ex){
                            LoadUSBDrives();
                            MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }else{
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }else{
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_on").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }
        // BTN DISABLED PROTECT
        // ======================================================================================================
        private void BtnDisabledProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_process_start_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string fileSystem = driveInfo.DriveFormat;
            //
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_name").Trim()));
            }
            //
            if (HasWritePermission(rootPath)){
                string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                DialogResult check_disabled_protect = MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_off_info").Trim())), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (check_disabled_protect == DialogResult.Yes){
                    try{
                        if (Directory.Exists(rootPath)){
                            // Restore write permission to the root directory and restore write permission to the Firewall folder
                            TS_USBProtect(rootPath, true); // Revoke write permission to root directory
                            TS_USBProtect(firewallFolderPath, true); // Grant write permission to the Firewall folder
                            LoadUSBDrives();
                            MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_process_disabled_success").Trim())), rootPath, deviceName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }else{
                            MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }catch (Exception ex){
                        LoadUSBDrives();
                        MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }else{
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_protect_off").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }
        // TS USB PROTECT ALGORITHM
        // ======================================================================================================
        static void TS_USBProtect(string folderPath, bool allowWriteAccess){
            try{
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dirSecurity = dirInfo.GetAccessControl();
                string allUsers = "Everyone"; // 'Everyone' group for all users
                //
                if (allowWriteAccess){
                    // Return write permission
                    dirSecurity.RemoveAccessRule(new FileSystemAccessRule(allUsers, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny));
                    dirSecurity.AddAccessRule(new FileSystemAccessRule(allUsers, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                }else{
                    // Remove write permission
                    dirSecurity.AddAccessRule(new FileSystemAccessRule(allUsers, FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Deny));
                }
                //
                dirInfo.SetAccessControl(dirSecurity);
            }catch (Exception){ }
        }
        // NTFS FORMAT BTN
        // ======================================================================================================
        private void BtnFormatNTFS_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_process_start_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            //
            if (Directory.Exists(rootPath)){
                DriveInfo driveInfo = new DriveInfo(rootPath);
                // Check file system
                if (driveInfo.DriveFormat != "NTFS"){
                    DialogResult result = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_warning").Trim())), "\n\n", "\n\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes){
                        try{
                            string userInput = Interaction.InputBox(
                                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_info").Trim())),
                                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name").Trim())),
                                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_name").Trim())),
                                -1,
                                -1
                            );
                            if (!string.IsNullOrEmpty(userInput)){
                                string pattern = @"[^\w\s]";
                                bool containsSpecialCharacters = Regex.IsMatch(userInput, pattern);
                                //
                                if (!containsSpecialCharacters) {
                                    // Formatting process
                                    Text = TS_SoftwareVersion.TS_SofwareVersion(0, ts_version_mode) + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_format_title").Trim()));
                                    Task formatNtfs = Task.Run(() => FormatDrive(driveInfo.RootDirectory.ToString(), TSFormatTurkishLangType(userInput.Trim())));
                                }else{
                                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }else{
                                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name_null").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }catch (Exception){ }
                    }
                }else{
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system_not").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }else{
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // NTFS FORMAT DRIVE ALGORITHM
        // ======================================================================================================
        private void FormatDrive(string driveLetter, string driveName){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                // Create diskpart script
                // Diskpart komut dosyasını oluştur
                string diskPartScript = $@"select volume {driveLetter[0]}:
                format fs=ntfs quick label={driveName}";
                // Create a temporary file for diskpart operation
                string scriptFilePath = Path.GetTempFileName();
                File.WriteAllText(scriptFilePath, diskPartScript);
                //
                ProcessStartInfo processInfo = new ProcessStartInfo{
                    FileName = "diskpart.exe",
                    Arguments = $"/s \"{scriptFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas" // Run as administrator
                };
                //
                using (Process process = Process.Start(processInfo)){
                    using (StreamReader outputReader = process.StandardOutput)
                    using (StreamReader errorReader = process.StandardError){
                        string output = outputReader.ReadToEnd();
                        string error = errorReader.ReadToEnd();
                        process.WaitForExit();
                        //
                        Text = TS_SoftwareVersion.TS_SofwareVersion(0, ts_version_mode);
                        LoadUSBDrives();
                        //
                        if (process.ExitCode == 0){
                            MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_format_success").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }else{
                            MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_ntfs_format_error").Trim())), error), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                // Clear temporary file
                File.Delete(scriptFilePath);
            }catch (Exception ex){
                Text = TS_SoftwareVersion.TS_SofwareVersion(0, ts_version_mode);
                LoadUSBDrives();
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_default_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string TSFormatTurkishLangType(string input){
            // English equivalents of Turkish characters
            var replacements = new (string, string)[]{
                ("ç", "c"), ("Ç", "C"),
                ("ı", "i"), ("I", "I"),
                ("ğ", "g"), ("Ğ", "G"),
                ("ö", "o"), ("Ö", "O"),
                ("ş", "s"), ("Ş", "S"),
                ("ü", "u"), ("Ü", "U")
            };
            var sb = new StringBuilder(input);
            foreach (var (oldStr, newStr) in replacements){
                sb.Replace(oldStr, newStr);
            }
            // Convert spaces to underscore
            sb.Replace(" ", "_");
            return sb.ToString();
        }
        // ======================================================================================================
        // THEME SETTINGS
        private void select_theme_active(object target_theme){
            ToolStripMenuItem selected_theme = null;
            select_theme_deactive();
            if (target_theme != null){
                if (selected_theme != (ToolStripMenuItem)target_theme){
                    selected_theme = (ToolStripMenuItem)target_theme;
                    selected_theme.Checked = true;
                }
            }
        }
        private void select_theme_deactive(){
            foreach (ToolStripMenuItem disabled_theme in themeToolStripMenuItem.DropDownItems){
                disabled_theme.Checked = false;
            }
        }
        private void lightThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 1){ theme_engine(1); select_theme_active(sender); }
        }
        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 0){ theme_engine(0); select_theme_active(sender); }
        }
        private void theme_engine(int ts){
            try{
                theme = ts;
                if (theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                    //
                    settingsToolStripMenuItem.Image = Properties.Resources.header_settings_light;
                    themeToolStripMenuItem.Image = Properties.Resources.header_theme_light;
                    languageToolStripMenuItem.Image = Properties.Resources.header_language_light;
                    initialViewToolStripMenuItem.Image = Properties.Resources.header_initial_light;
                    checkforUpdatesToolStripMenuItem.Image = Properties.Resources.header_update_light;
                    refreshToolStripMenuItem.Image = Properties.Resources.header_refresh_light;
                    aboutToolStripMenuItem.Image = Properties.Resources.header_about_light;
                }else if (theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                    //
                    settingsToolStripMenuItem.Image = Properties.Resources.header_settings_dark;
                    themeToolStripMenuItem.Image = Properties.Resources.header_theme_dark;
                    languageToolStripMenuItem.Image = Properties.Resources.header_language_dark;
                    initialViewToolStripMenuItem.Image = Properties.Resources.header_initial_dark;
                    checkforUpdatesToolStripMenuItem.Image = Properties.Resources.header_update_dark;
                    refreshToolStripMenuItem.Image = Properties.Resources.header_refresh_dark;
                    aboutToolStripMenuItem.Image = Properties.Resources.header_about_dark;
                }
                //
                header_colors[0] = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                header_colors[1] = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                HeaderMenu.Renderer = new HeaderMenuColors();
                // HEADER MENU
                HeaderMenu.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                HeaderMenu.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                // TOOLTIP
                MainToolTip.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                MainToolTip.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                // SETTINGS
                settingsToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                settingsToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                // THEMES
                themeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                themeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                lightThemeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                lightThemeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                darkThemeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                darkThemeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // LANGS
                languageToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                languageToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                englishToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                englishToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                turkishToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                turkishToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // INITIAL VIEW
                initialViewToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                initialViewToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                windowedToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                windowedToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                fullScreenToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                fullScreenToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // UPDATE ENGINE
                checkforUpdatesToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                checkforUpdatesToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // REFRESH
                refreshToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                refreshToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // ABOUT
                aboutToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
                aboutToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor2");
                // CONTENT BG
                BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGColor");
                Panel_Right.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(theme), "PageContainerBGColor");
                // ALL BUTTON
                foreach (Control control in Panel_Right.Controls){
                    if (control is Button button){
                        button.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBGColor");
                        button.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightColor");
                        button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightColor");
                        button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightColor");
                    }
                }
                // NOT USB DEVICE LABEL
                Label_NotUSB.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor2");
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
                software_other_page_preloader();
                //
                try{
                    TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                    software_setting_save.TSWriteSettings(ts_settings_container, "ThemeStatus", Convert.ToString(ts));
                }catch (Exception){ }
            }catch (Exception){ }
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private void select_lang_active(object target_lang){
            ToolStripMenuItem selected_lang = null;
            select_lang_deactive();
            if (target_lang != null){
                if (selected_lang != (ToolStripMenuItem)target_lang){
                    selected_lang = (ToolStripMenuItem)target_lang;
                    selected_lang.Checked = true;
                }
            }
        }
        private void select_lang_deactive(){
            foreach (ToolStripMenuItem disabled_lang in languageToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "en"){ lang_preload(yamira_lang_en, "en"); select_lang_active(sender); }
        }

        private void turkishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "tr"){ lang_preload(yamira_lang_tr, "tr"); select_lang_active(sender); }
        }
        private void lang_preload(string lang_type, string lang_code){
            lang_engine(lang_type, lang_code);
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_code);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult lang_change_message = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("LangChange", "lang_change_notification").Trim())), "\n\n", "\n\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (lang_change_message == DialogResult.Yes) { Application.Restart(); }
        }
        private void lang_engine(string lang_type, string lang_code){
            try{
                lang_path = lang_type;
                lang = lang_code;
                // GLOBAL ENGINE
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                // SETTINGS
                settingsToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_settings").Trim()));
                // THEMES
                themeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_theme").Trim()));
                lightThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderThemes", "theme_light").Trim()));
                darkThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderThemes", "theme_dark").Trim()));
                // LANGS
                languageToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_language").Trim()));
                englishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderLangs", "lang_en").Trim()));
                turkishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderLangs", "lang_tr").Trim()));
                // INITIAL VIEW
                initialViewToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_start").Trim()));
                windowedToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_windowed").Trim()));
                fullScreenToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_full_screen").Trim()));
                // UPDATE CHECK
                checkforUpdatesToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_update").Trim()));
                // REFRESH
                refreshToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_refresh").Trim()));
                // ABOUT
                aboutToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_about").Trim()));
                //
                DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_dgv_root_directory").Trim()));
                DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_dgv_device_name").Trim()));
                DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_dgv_file_system").Trim()));
                DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_dgv_size").Trim()));
                DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_dgv_protect_mode").Trim()));
                //
                BtnActiveProtect.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_btn_protect_active").Trim()));
                BtnDisabledProtect.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_btn_protect_disabled").Trim()));
                BtnFormatNTFS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_btn_ntfs_format").Trim()));
                //
                Label_NotUSB.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("Yamira", "y_not_usb_storage_device").Trim()));
                //
                software_other_page_preloader();
            }catch (Exception){ }
        }
        private void software_other_page_preloader(){
            // SOFTWARE ABOUT
            try{
                YamiraAbout software_about = new YamiraAbout();
                string software_about_name = "yamira_about";
                software_about.Name = software_about_name;
                if (Application.OpenForms[software_about_name] != null){
                    software_about = (YamiraAbout)Application.OpenForms[software_about_name];
                    software_about.about_preloader();
                }
            }catch (Exception){ }
        }
        // INITIAL SETINGS
        // ======================================================================================================
        private void select_initial_mode_active(object target_initial_mode){
            ToolStripMenuItem selected_initial_mode = null;
            select_initial_mode_deactive();
            if (target_initial_mode != null){
                if (selected_initial_mode != (ToolStripMenuItem)target_initial_mode){
                    selected_initial_mode = (ToolStripMenuItem)target_initial_mode;
                    selected_initial_mode.Checked = true;
                }
            }
        }
        private void select_initial_mode_deactive(){
            foreach (ToolStripMenuItem disabled_initial in initialViewToolStripMenuItem.DropDownItems){
                disabled_initial.Checked = false;
            }
        }
        private void windowedToolStripMenuItem_Click(object sender, EventArgs e){
            if (initial_status != 0){ initial_status = 0; initial_mode_settings("0"); select_initial_mode_active(sender); }
        }
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e){
            if (initial_status != 1){ initial_status = 1; initial_mode_settings("1"); select_initial_mode_active(sender); }
        }
        private void initial_mode_settings(string get_inital_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "InitialStatus", get_inital_value);
            }catch (Exception){ }
        }
        // UPDATE CHECK ENGINE
        // ======================================================================================================
        private void checkforUpdatesToolStripMenuItem_Click(object sender, EventArgs e){
            software_update_check(1);
        }
        public bool IsNetworkCheck(){
            Ping ping = new Ping();
            try{
                PingReply reply = ping.Send("www.google.com");
                if (reply.Status == IPStatus.Success){
                    return true;
                }
            }catch (PingException){ }
            return false;
        }
        public void software_update_check(int _check_update_ui){
            string client_version = TS_SoftwareVersion.TS_SofwareVersion(2, ts_version_mode).Trim();
            int client_num_version = Convert.ToInt32(client_version.Replace(".", string.Empty));
            if (IsNetworkCheck() == true){
                string software_version_url = TS_LinkSystem.github_link_lt;
                WebClient webClient = new WebClient();
                try{
                    TSGetLangs software_lang = new TSGetLangs(lang_path);
                    string[] version_content = webClient.DownloadString(software_version_url).Split('=');
                    string last_version = version_content[1].Trim();
                    int last_num_version = Convert.ToInt32(version_content[1].Trim().Replace(".", string.Empty));
                    if (client_num_version < last_num_version){
                        DialogResult info_update = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareUpdate", "su_message").Trim())), Application.ProductName, "\n\n", client_version, "\n", last_version, "\n\n"), string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareUpdate", "su_title").Trim())), Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (info_update == DialogResult.Yes){
                            Process.Start(TS_LinkSystem.github_link_lr);
                        }
                    }else{
                        if (_check_update_ui == 1){
                            if (client_num_version == last_num_version){
                                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareUpdate", "su_no_update").Trim())), Application.ProductName, "\n", client_version), string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareUpdate", "su_title").Trim())), Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }catch (WebException){ }
            }else{
                checkforUpdatesToolStripMenuItem.Enabled = false;
            }
        }
        // YAMIRA ABOUT
        // ======================================================================================================
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                YamiraAbout yamira_about = new YamiraAbout();
                string yamira_about_name = "yamira_about";
                yamira_about.Name = yamira_about_name;
                if (Application.OpenForms[yamira_about_name] == null){
                    yamira_about.Show();
                }else{
                    if (Application.OpenForms[yamira_about_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[yamira_about_name].WindowState = FormWindowState.Normal;
                    }
                    Application.OpenForms[yamira_about_name].Activate();
                    MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("HeaderMenu", "header_menu_about").Trim()))), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch (Exception){ }
        }
        // EXIT
        // ======================================================================================================
        private void software_exit(){ Application.Exit(); }
        private void Yamira_FormClosing(object sender, FormClosingEventArgs e){ software_exit(); }
    }
}