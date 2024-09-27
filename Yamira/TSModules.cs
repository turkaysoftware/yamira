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
            twitter_link = "https://x.com/turkaysoftware",
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
        // LANG PATHS
        // ======================================================================================================
        public static string yamira_lf = @"y_langs";                                 // Main Path
        public static string yamira_lang_en = yamira_lf + @"\English.ini";            // English      | en
        public static string yamira_lang_tr = yamira_lf + @"\Turkish.ini";            // Turkish      | tr
        public class TSGetLangs{
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _saveFilePath;
            public TSGetLangs(string filePath){ _saveFilePath = filePath; }
            public string TSReadLangs(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(512);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 511, _saveFilePath);
                return stringBuilder.ToString();
            }
        } 
        // TS THEME ENGINE
        // ======================================================================================================
        public class TS_ThemeEngine{
            // Light Theme Colors
            public static readonly Dictionary<string, Color> LightTheme = new Dictionary<string, Color>{
                // HEADER MENU COLOR MODE
                { "HeaderBGColor", Color.FromArgb(222, 222, 222) },
                { "HeaderFEColor", Color.FromArgb(31, 31, 31) },
                { "HeaderFEColor2", Color.FromArgb(32, 32, 32) },
                { "HeaderBGColor2", Color.FromArgb(235, 235, 235) },
                // UI COLOR
                { "PageContainerBGColor", Color.WhiteSmoke },
                { "ContentLabelRightColor", Color.FromArgb(114, 19, 42) },
                { "DataGridBGColor", Color.White },
                { "DataGridFEColor", Color.FromArgb(32, 32, 32) },
                { "DataGridGridColor", Color.FromArgb(217, 217, 217) },
                { "DataGridAlternatingColor", Color.FromArgb(235, 235, 235) },
                { "DataGridHeaderBGColor", Color.FromArgb(114, 19, 42) },
                { "DataGridHeaderFEColor", Color.WhiteSmoke },
                { "DynamicThemeActiveBtnBGColor", Color.WhiteSmoke }
            };
            // Dark Theme Colors
            public static readonly Dictionary<string, Color> DarkTheme = new Dictionary<string, Color>{
                // HEADER MENU COLOR MODE
                { "HeaderBGColor", Color.FromArgb(31, 31, 31) },
                { "HeaderFEColor", Color.FromArgb(222, 222, 222) },
                { "HeaderFEColor2", Color.WhiteSmoke },
                { "HeaderBGColor2", Color.FromArgb(24, 24, 24) },
                // UI COLOR MODES
                { "PageContainerBGColor", Color.FromArgb(31, 31, 31) },
                { "ContentLabelRightColor", Color.FromArgb(151, 27, 57) },
                { "DataGridBGColor", Color.FromArgb(24, 24, 24) },
                { "DataGridFEColor", Color.WhiteSmoke },
                { "DataGridGridColor", Color.FromArgb(50, 50, 50) },
                { "DataGridAlternatingColor", Color.FromArgb(31, 31, 31) },
                { "DataGridHeaderBGColor", Color.FromArgb(151, 27, 57) },
                { "DataGridHeaderFEColor", Color.WhiteSmoke },
                { "DynamicThemeActiveBtnBGColor", Color.WhiteSmoke }
            };
            // Method to get color for the current theme
            public static Color ColorMode(int theme, string key){
                if (theme == 0){
                    return DarkTheme.ContainsKey(key) ? DarkTheme[key] : Color.Transparent;
                }else if (theme == 1){
                    return LightTheme.ContainsKey(key) ? LightTheme[key] : Color.Transparent;
                }
                return Color.Transparent;
            }
        }
        // SAVE PATHS
        // ======================================================================================================
        public static string ts_df = Application.StartupPath;
        public static string ts_sf = ts_df + @"\YamiraSettings.ini";
        public static string ts_settings_container = Path.GetFileNameWithoutExtension(ts_sf);
        // GLOW SETTINGS SAVE CLASS
        // ======================================================================================================
        public class TSSettingsSave{
            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _saveFilePath;
            public TSSettingsSave(string filePath){ _saveFilePath = filePath; }
            public string TSReadSettings(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(512);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 511, _saveFilePath);
                return stringBuilder.ToString();
            }
            public long TSWriteSettings(string episode, string settingName, string value){
                return WritePrivateProfileString(episode, settingName, value, _saveFilePath);
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