using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
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
        public static string lang, lang_path, default_create_folder = "TSProtectionSystem";
        public static int theme, initial_status;
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
            int theme_mode = int.TryParse(TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus")), out int the_status) ? the_status : 1;
            theme_engine(theme_mode);
            darkThemeToolStripMenuItem.Checked = theme_mode == 0;
            lightThemeToolStripMenuItem.Checked = theme_mode == 1;
            //
            string lang_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus"));
            var languageFiles = new Dictionary<string, (object langResource, ToolStripMenuItem menuItem, bool fileExists)>{
                { "en", (ts_lang_en, englishToolStripMenuItem, File.Exists(ts_lang_en)) },
                { "tr", (ts_lang_tr, turkishToolStripMenuItem, File.Exists(ts_lang_tr)) },
            };
            foreach (var langLoader in languageFiles) { langLoader.Value.menuItem.Enabled = langLoader.Value.fileExists; }
            var (langResource, selectedMenuItem, _) = languageFiles.ContainsKey(lang_mode) ? languageFiles[lang_mode] : languageFiles["en"];
            lang_engine(Convert.ToString(langResource), lang_mode);
            selectedMenuItem.Checked = true;
            //
            string initial_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "InitialStatus"));
            initial_status = int.TryParse(initial_mode, out int ini_status) && (ini_status == 0 || ini_status == 1) ? ini_status : 0;
            WindowState = initial_status == 1 ? FormWindowState.Maximized : FormWindowState.Normal;
            windowedToolStripMenuItem.Checked = initial_status == 0;
            fullScreenToolStripMenuItem.Checked = initial_status == 1;
        }
        // LOAD USB DRIVES
        // ======================================================================================================
        private void LoadUSBDrives(){
            try{
                DataMainTable.Rows.Clear();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                //
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string protect_on = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_status_active"));
                string protect_off = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_status_disabled"));
                //
                int detectedDeviceCount = allDrives.Count(drive => drive.DriveType == DriveType.Removable && drive.IsReady);
                foreach (DriveInfo drive in allDrives){
                    if (drive.DriveType == DriveType.Removable && drive.IsReady){
                        string deviceName = string.IsNullOrEmpty(drive.VolumeLabel) ? TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_name")) : drive.VolumeLabel;
                        string writePermissionStatus = HasWritePermission(drive.RootDirectory.FullName) ? protect_on : protect_off;
                        //
                        DataMainTable.Rows.Add(drive.Name, deviceName, drive.DriveFormat, TS_FormatSize(drive.TotalSize), writePermissionStatus);
                    }
                }
                DataMainTable.ClearSelection();
                Label_NotUSB.Visible = detectedDeviceCount == 0;
            }catch (Exception){ }
        }
        private bool HasWritePermission(string path){
            try{
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
            Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
            HeaderMenu.Cursor = Cursors.Hand;
            //
            RunSoftwareEngine();
            LoadUSBDrives();
            //
            Task softwareUpdateCheck = Task.Run(() => software_update_check(0));
        }
        // BTN ACTIVE PROTECT
        // ======================================================================================================
        private void BtnActiveProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_process_start_info")));
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string fileSystem = driveInfo.DriveFormat;
            //
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_name"));
            }
            //
            if (!HasWritePermission(rootPath)){
                if (driveInfo.DriveFormat == "NTFS"){
                    string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                    DialogResult check_open_protect = TS_MessageBoxEngine.TS_MessageBox(this, 4, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_on_info")));
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
                                DialogResult successAfterOpen = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_process_success")), string.Format("{0} ({1})", rootPath, deviceName), "\n\n", firewallFolderPath, "\n\n"));
                                if (successAfterOpen == DialogResult.Yes){
                                    Process.Start(firewallFolderPath);
                                }
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null")));
                            }
                        }catch (Exception ex){
                            LoadUSBDrives();
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_error")), ex.Message));
                        }
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system")));
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_on")));
            } 
        }
        // BTN DISABLED PROTECT
        // ======================================================================================================
        private void BtnDisabledProtect_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            if (DataMainTable.SelectedRows.Count == 0){
                TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_process_start_info")));
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            var driveInfo = new DriveInfo(Path.GetPathRoot(rootPath));
            string fileSystem = driveInfo.DriveFormat;
            //
            string deviceName = driveInfo.VolumeLabel;
            if (string.IsNullOrEmpty(deviceName)){
                deviceName = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_name"));
            }
            //
            if (HasWritePermission(rootPath)){
                string firewallFolderPath = Path.Combine(rootPath, default_create_folder);
                DialogResult check_disabled_protect = TS_MessageBoxEngine.TS_MessageBox(this, 4, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_off_info")));
                if (check_disabled_protect == DialogResult.Yes){
                    try{
                        if (Directory.Exists(rootPath)){
                            // Restore write permission to the root directory and restore write permission to the Firewall folder
                            TS_USBProtect(rootPath, true); // Revoke write permission to root directory
                            TS_USBProtect(firewallFolderPath, true); // Grant write permission to the Firewall folder
                            LoadUSBDrives();
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_process_disabled_success")), rootPath, deviceName));
                        }
                        else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null")));
                        }
                    }catch (Exception ex){
                        LoadUSBDrives();
                        TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_error")), ex.Message));
                    }
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_protect_off")));
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
                TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_process_start_info")));
                return;
            }
            //
            string rootPath = DataMainTable.SelectedRows[0].Cells[0].Value.ToString().Trim();
            //
            if (Directory.Exists(rootPath)){
                DriveInfo driveInfo = new DriveInfo(rootPath);
                // Check file system
                if (driveInfo.DriveFormat != "NTFS"){
                    DialogResult result = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_warning")), "\n\n", "\n\n"));
                    if (result == DialogResult.Yes){
                        try{
                            string userInput = Interaction.InputBox(
                                TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_info")),
                                TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name")),
                                TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_name")),
                                -1,
                                -1
                            );
                            if (!string.IsNullOrEmpty(userInput)){
                                string pattern = @"[^\w\s]";
                                bool containsSpecialCharacters = Regex.IsMatch(userInput, pattern);
                                //
                                if (!containsSpecialCharacters) {
                                    // Formatting process
                                    Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode) + " - " + TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_format_title"));
                                    Task formatNtfs = Task.Run(() => FormatDrive(driveInfo.RootDirectory.ToString(), TSFormatTurkishLangType(userInput.Trim())));
                                }else{
                                    TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_info")));
                                }
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_new_usb_name_null")));
                            }
                        }catch (Exception){ }
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_not_ntfs_file_system_not")));
                }
            }else{
                TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_root_directory_not_null")));
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
                        Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
                        LoadUSBDrives();
                        //
                        if (process.ExitCode == 0){
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_format_success")));
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_ntfs_format_error")), error));
                        }
                    }
                }
                // Clear temporary file
                File.Delete(scriptFilePath);
            }catch (Exception ex){
                Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
                LoadUSBDrives();
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_default_error")), ex.Message));
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
                int set_attribute = theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { theme == 1 ? 0 : 1 }, 4);
                }
                if (theme == 1){
                    settingsToolStripMenuItem.Image = Properties.Resources.header_settings_light;
                    themeToolStripMenuItem.Image = Properties.Resources.header_theme_light;
                    languageToolStripMenuItem.Image = Properties.Resources.header_language_light;
                    initialViewToolStripMenuItem.Image = Properties.Resources.header_initial_light;
                    checkforUpdatesToolStripMenuItem.Image = Properties.Resources.header_update_light;
                    refreshToolStripMenuItem.Image = Properties.Resources.header_refresh_light;
                    aboutToolStripMenuItem.Image = Properties.Resources.header_about_light;
                }else if (theme == 0){
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
                        button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightColorHover");
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
            if (lang != "en"){ lang_preload(ts_lang_en, "en"); select_lang_active(sender); }
        }

        private void turkishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "tr"){ lang_preload(ts_lang_tr, "tr"); select_lang_active(sender); }
        }
        private void lang_preload(string lang_type, string lang_code){
            lang_engine(lang_type, lang_code);
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_code);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult lang_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("LangChange", "lang_change_notification")), "\n\n", "\n\n"));
            if (lang_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        private void lang_engine(string lang_type, string lang_code){
            try{
                lang_path = lang_type;
                lang = lang_code;
                // GLOBAL ENGINE
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                // SETTINGS
                settingsToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_settings"));
                // THEMES
                themeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_theme"));
                lightThemeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderThemes", "theme_light"));
                darkThemeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderThemes", "theme_dark"));
                // LANGS
                languageToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_language"));
                englishToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_en"));
                turkishToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_tr"));
                // INITIAL VIEW
                initialViewToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_start"));
                windowedToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_windowed"));
                fullScreenToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_full_screen"));
                // UPDATE CHECK
                checkforUpdatesToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_update"));
                // REFRESH
                refreshToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_refresh"));
                // ABOUT
                aboutToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_about"));
                //
                DataMainTable.Columns[0].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_dgv_root_directory"));
                DataMainTable.Columns[1].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_dgv_device_name"));
                DataMainTable.Columns[2].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_dgv_file_system"));
                DataMainTable.Columns[3].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_dgv_size"));
                DataMainTable.Columns[4].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_dgv_protect_mode"));
                //
                BtnActiveProtect.Text = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_btn_protect_active"));
                BtnDisabledProtect.Text = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_btn_protect_disabled"));
                BtnFormatNTFS.Text = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_btn_ntfs_format"));
                //
                Label_NotUSB.Text = TS_String_Encoder(software_lang.TSReadLangs("Yamira", "y_not_usb_storage_device"));
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
            Ping check_ping = new Ping();
            try{
                PingReply check_ping_reply = check_ping.Send("www.google.com");
                if (check_ping_reply.Status == IPStatus.Success){
                    return true;
                }
            }catch (PingException){ }
            return false;
        }
        public void software_update_check(int _check_update_ui){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (!IsNetworkCheck()){
                    if (_check_update_ui == 1){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_not_connection")), "\n\n"), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }
                    return;
                }
                using (WebClient webClient = new WebClient()){
                    string client_version = TS_VersionEngine.TS_SofwareVersion(2, Program.ts_version_mode).Trim();
                    int client_num_version = Convert.ToInt32(client_version.Replace(".", string.Empty));
                    //
                    string[] version_content = webClient.DownloadString(TS_LinkSystem.github_link_lt).Split('=');
                    string last_version = version_content[1].Trim();
                    int last_num_version = Convert.ToInt32(last_version.Replace(".", string.Empty));
                    //
                    if (client_num_version < last_num_version){
                        // Update available
                        DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_available")), Application.ProductName, "\n\n", client_version, "\n", last_version, "\n\n"), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                        if (info_update == DialogResult.Yes){
                            Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link_lr){ UseShellExecute = true });
                        }
                    }else if (_check_update_ui == 1 && client_num_version == last_num_version){
                        // No update available
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_not_available")), Application.ProductName, "\n", client_version), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }else if (_check_update_ui == 1 && client_num_version > last_num_version){
                        // Access before public use
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_newer")), "\n\n", string.Format("v{0}", client_version)), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }
                }
            }catch (Exception ex){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_error")), "\n\n", ex.Message), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
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
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_about"))));
                }
            }catch (Exception){ }
        }
        // EXIT
        // ======================================================================================================
        private void software_exit(){ Application.Exit(); }
        private void Yamira_FormClosing(object sender, FormClosingEventArgs e){ software_exit(); }
    }
}