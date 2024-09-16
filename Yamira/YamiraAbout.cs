﻿using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using static Yamira.TSModules;

namespace Yamira{
    public partial class YamiraAbout : Form{
        public YamiraAbout(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // MEDIA LINK SYSTEM
        // ======================================================================================================
        TS_LinkSystem TS_LinkSystem = new TS_LinkSystem();
        private void YamiraAbout_Load(object sender, EventArgs e){
            try{
                // GET PRELOAD SETTINGS
                about_preloader();
                // IMAGES
                About_Image.Image = Properties.Resources.yamira_logo;
            }catch (Exception){ }
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void about_preloader(){
            try{
                // COLOR SETTINGS
                if (Yamira.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Yamira.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "PageContainerBGColor");
                About_BG_Panel.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "HeaderBGColor2");
                About_L1.ForeColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "TextboxFEColor");
                About_L2.ForeColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "TextboxFEColor");
                //
                About_WebsiteBtn.ForeColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "DynamicThemeActiveBtnBGColor");
                About_WebsiteBtn.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_WebsiteBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_WebsiteBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                //
                About_GitHubBtn.ForeColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "DynamicThemeActiveBtnBGColor");
                About_GitHubBtn.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_GitHubBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_GitHubBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                //
                About_XBtn.ForeColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "DynamicThemeActiveBtnBGColor");
                About_XBtn.BackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_XBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                About_XBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Convert.ToInt32(Yamira.theme), "ContentLabelRightColor");
                // ======================================================================================================
                TSGetLangs software_lang = new TSGetLangs(Yamira.lang_path);
                TS_VersionEngine glow_version = new TS_VersionEngine();
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_title").Trim())), Application.ProductName);
                About_L1.Text = glow_version.TS_SofwareVersion(0, Yamira.ts_version_mode);
                About_L2.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_copyright").Trim())), "\u00a9", DateTime.Now.Year, Application.CompanyName);
                About_WebsiteBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_website_page").Trim()));
                About_GitHubBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_github_page").Trim()));
                About_XBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_twitter_page").Trim()));
            }catch (Exception){ }
        }

        // WEBSITE LINK
        // ======================================================================================================
        private void About_WebsiteBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.website_link);
            }catch (Exception){ }
        }
        // X LINK
        // ======================================================================================================
        private void About_XBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.twitter_link);
            }catch (Exception){ }
        }
        // GITHUN LINK
        // ======================================================================================================
        private void About_GitHubBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.github_link + "/yamira");
            }catch (Exception){ }
        }
    }
}