using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Yamira{
    internal class TSModules{
        // LINK SYSTEM
        // ======================================================================================================
        public class TS_LinkSystem{
            public string
            website_link = "https://www.turkaysoftware.com",
            twitter_x_link = "https://x.com/turkaysoftware",
            instagram_link = "https://www.instagram.com/erayturkayy/",
            github_link = "https://github.com/turkaysoftware",
            //
            github_link_lt = "https://raw.githubusercontent.com/turkaysoftware/yamira/main/Yamira/SoftwareVersion.txt",
            github_link_lr = "https://github.com/turkaysoftware/yamira/releases/latest";
        }
        // VERSIONS
        // ======================================================================================================
        public class TS_VersionEngine{
            public string TS_SofwareVersion(int v_type, int v_mode){
                string version_mode = "";
                string versionSubstring = v_mode == 0 ? Application.ProductVersion.Substring(0, 5) : Application.ProductVersion.Substring(0, 7);
                switch (v_type){
                    case 0:
                        version_mode = v_mode == 0 ? $"{Application.ProductName} - v{versionSubstring}" : $"{Application.ProductName} - v{Application.ProductVersion.Substring(0, 7)}";
                        break;
                    case 1:
                        version_mode = $"v{versionSubstring}";
                        break;
                    case 2:
                        version_mode = versionSubstring;
                        break;
                    default:
                        break;
                }
                return version_mode;
            }
        }
        // TS SOFTWARE COPYRIGHT DATE
        // ======================================================================================================
        public class TS_SoftwareCopyrightDate{
            public static string ts_scd_preloader = string.Format("\u00a9 {0}{1}, {2}.", DateTime.Now.Year == 2025 ? "2024-" : "", DateTime.Now.Year, Application.CompanyName);
        }
        // SETTINGS SAVE PATHS
        // ======================================================================================================
        public static string ts_df = Application.StartupPath;
        public static string ts_sf = ts_df + @"\" + Application.ProductName + "Settings.ini";
        public static string ts_settings_container = Path.GetFileNameWithoutExtension(ts_sf);
        // SETTINGS SAVE CLASS
        // ======================================================================================================
        public class TSSettingsSave{
            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _settingFilePath;
            public TSSettingsSave(string filePath) { _settingFilePath = filePath; }
            public string TSReadSettings(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(2048);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 2047, _settingFilePath);
                return stringBuilder.ToString();
            }
            public long TSWriteSettings(string episode, string settingName, string value){
                return WritePrivateProfileString(episode, settingName, value, _settingFilePath);
            }
        }
        // READ LANG PATHS
        // ======================================================================================================
        public static string ts_lf = @"y_langs";                            // Main Path
        public static string ts_lang_en = ts_lf + @"\English.ini";          // English      | en
        public static string ts_lang_tr = ts_lf + @"\Turkish.ini";          // Turkish      | tr
        // READ LANG CLASS
        // ======================================================================================================
        public class TSGetLangs{
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _readFilePath;
            public TSGetLangs(string filePath) { _readFilePath = filePath; }
            public string TSReadLangs(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(2048);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 2047, _readFilePath);
                return stringBuilder.ToString();
            }
        }
        // TS STRING ENCODER
        // ======================================================================================================
        public static string TS_String_Encoder(string get_text){
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(get_text)).Trim();
        }
        // TS THEME ENGINE
        // ======================================================================================================
        public class TS_ThemeEngine{
            // LIGHT THEME COLORS
            // ====================================
            public static readonly Dictionary<string, Color> LightTheme = new Dictionary<string, Color>{
                // TS PRELOADER
                { "TSBT_BGColor", Color.FromArgb(236, 242, 248) },
                { "TSBT_BGColor2", Color.White },
                { "TSBT_AccentColor", Color.FromArgb(114, 19, 42) },
                { "TSBT_LabelColor1", Color.FromArgb(51, 51, 51) },
                { "TSBT_LabelColor2", Color.FromArgb(100, 100, 100) },
                { "TSBT_CloseBG", Color.FromArgb(200, 255, 255, 255) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColor", Color.White },
                { "HeaderFEColor", Color.FromArgb(51, 51, 51) },
                { "HeaderFEColor2", Color.FromArgb(51, 51, 51) },
                { "HeaderBGColor2", Color.FromArgb(236, 242, 248) },
                // UI COLOR
                { "PageContainerBGColor", Color.FromArgb(236, 242, 248) },
                { "ContentLabelRightColor", Color.FromArgb(114, 19, 42) },
                { "ContentLabelRightColorHover", Color.FromArgb(136, 24, 52) },
                { "DataGridBGColor", Color.White },
                { "DataGridFEColor", Color.FromArgb(51, 51, 51) },
                { "DataGridGridColor", Color.FromArgb(226, 226, 226) },
                { "DataGridAlternatingColor", Color.FromArgb(236, 242, 248) },
                { "DataGridHeaderBGColor", Color.FromArgb(114, 19, 42) },
                { "DataGridHeaderFEColor", Color.WhiteSmoke },
                { "DynamicThemeActiveBtnBGColor", Color.WhiteSmoke }
            };
            // DARK THEME COLORS
            // ====================================
            public static readonly Dictionary<string, Color> DarkTheme = new Dictionary<string, Color>{
                // TS PRELOADER
                { "TSBT_BGColor", Color.FromArgb(21, 23, 32) },
                { "TSBT_BGColor2", Color.FromArgb(25, 31, 42) },
                { "TSBT_AccentColor", Color.FromArgb(214, 0, 64) },
                { "TSBT_LabelColor1", Color.WhiteSmoke },
                { "TSBT_LabelColor2", Color.FromArgb(176, 184, 196) },
                { "TSBT_CloseBG", Color.FromArgb(210, 25, 31, 42) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColor", Color.FromArgb(25, 31, 42) },
                { "HeaderFEColor", Color.WhiteSmoke },
                { "HeaderFEColor2", Color.WhiteSmoke },
                { "HeaderBGColor2", Color.FromArgb(21, 23, 32) },
                // UI COLOR MODES
                { "PageContainerBGColor", Color.FromArgb(21, 23, 32) },
                { "ContentLabelRightColor", Color.FromArgb(151, 27, 57) },
                { "ContentLabelRightColorHover", Color.FromArgb(172, 31, 66) },
                { "DataGridBGColor", Color.FromArgb(25, 31, 42) },
                { "DataGridFEColor", Color.WhiteSmoke },
                { "DataGridGridColor", Color.FromArgb(36, 45, 61) },
                { "DataGridAlternatingColor", Color.FromArgb(21, 23, 32) },
                { "DataGridHeaderBGColor", Color.FromArgb(151, 27, 57) },
                { "DataGridHeaderFEColor", Color.WhiteSmoke },
                { "DynamicThemeActiveBtnBGColor", Color.WhiteSmoke }
            };
            // THEME SWITCHER
            // ====================================
            public static Color ColorMode(int theme, string key){
                if (theme == 0){
                    return DarkTheme.ContainsKey(key) ? DarkTheme[key] : Color.Transparent;
                }else if (theme == 1){
                    return LightTheme.ContainsKey(key) ? LightTheme[key] : Color.Transparent;
                }
                return Color.Transparent;
            }
        }
        // DYNAMIC SIZE COUNT ALGORITHM
        // ======================================================================================================
        public static string TS_FormatSize(double bytes){
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int suffixIndex = 0;
            double doubleBytes = bytes;
            while (doubleBytes >= 1024 && suffixIndex < suffixes.Length - 1){
                doubleBytes /= 1024;
                suffixIndex++;
            }
            return $"{doubleBytes:0.##} {suffixes[suffixIndex]}";
        }
        public static double TS_FormatSizeNoType(double bytes){
            while (bytes >= 1024){
                bytes /= 1024;
            }
            return Math.Round(bytes, 2);
        }
        // TITLE BAR SETTINGS DWM API
        // ======================================================================================================
        [DllImport("DwmApi")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        // DPI AWARE
        // ======================================================================================================
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }
}