namespace Yamira
{
    partial class YamiraMain
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YamiraMain));
            this.HeaderMenu = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lightThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.darkThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.polishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turkishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkforUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bmacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.Panel_BG = new System.Windows.Forms.Panel();
            this.DataMainTable = new System.Windows.Forms.DataGridView();
            this.Panel_Right = new System.Windows.Forms.Panel();
            this.FLP_Container = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_NotUSB = new System.Windows.Forms.Label();
            this.BtnActiveProtect = new Yamira.TSCustomButton();
            this.BtnDisabledProtect = new Yamira.TSCustomButton();
            this.BtnFormatNTFS = new Yamira.TSCustomButton();
            this.HeaderMenu.SuspendLayout();
            this.Panel_BG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataMainTable)).BeginInit();
            this.Panel_Right.SuspendLayout();
            this.FLP_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeaderMenu
            // 
            this.HeaderMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.tSWizardToolStripMenuItem,
            this.bmacToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.HeaderMenu.Location = new System.Drawing.Point(0, 0);
            this.HeaderMenu.Name = "HeaderMenu";
            this.HeaderMenu.Size = new System.Drawing.Size(1008, 24);
            this.HeaderMenu.TabIndex = 0;
            this.HeaderMenu.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.startupToolStripMenuItem,
            this.checkforUpdatesToolStripMenuItem});
            this.settingsToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lightThemeToolStripMenuItem,
            this.darkThemeToolStripMenuItem,
            this.systemThemeToolStripMenuItem});
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.themeToolStripMenuItem.Text = "Theme";
            // 
            // lightThemeToolStripMenuItem
            // 
            this.lightThemeToolStripMenuItem.Name = "lightThemeToolStripMenuItem";
            this.lightThemeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.lightThemeToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.lightThemeToolStripMenuItem.Text = "Light Theme";
            this.lightThemeToolStripMenuItem.Click += new System.EventHandler(this.LightThemeToolStripMenuItem_Click);
            // 
            // darkThemeToolStripMenuItem
            // 
            this.darkThemeToolStripMenuItem.Name = "darkThemeToolStripMenuItem";
            this.darkThemeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.darkThemeToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.darkThemeToolStripMenuItem.Text = "Dark Theme";
            this.darkThemeToolStripMenuItem.Click += new System.EventHandler(this.DarkThemeToolStripMenuItem_Click);
            // 
            // systemThemeToolStripMenuItem
            // 
            this.systemThemeToolStripMenuItem.Name = "systemThemeToolStripMenuItem";
            this.systemThemeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.systemThemeToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.systemThemeToolStripMenuItem.Text = "System Theme";
            this.systemThemeToolStripMenuItem.Click += new System.EventHandler(this.SystemThemeToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.polishToolStripMenuItem,
            this.turkishToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.englishToolStripMenuItem.Text = "English";
            // 
            // polishToolStripMenuItem
            // 
            this.polishToolStripMenuItem.Name = "polishToolStripMenuItem";
            this.polishToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.polishToolStripMenuItem.Text = "Polish";
            // 
            // turkishToolStripMenuItem
            // 
            this.turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
            this.turkishToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.turkishToolStripMenuItem.Text = "Turkish";
            // 
            // startupToolStripMenuItem
            // 
            this.startupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowedToolStripMenuItem,
            this.fullScreenToolStripMenuItem});
            this.startupToolStripMenuItem.Name = "startupToolStripMenuItem";
            this.startupToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.startupToolStripMenuItem.Text = "Startup";
            // 
            // windowedToolStripMenuItem
            // 
            this.windowedToolStripMenuItem.Name = "windowedToolStripMenuItem";
            this.windowedToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.windowedToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.windowedToolStripMenuItem.Text = "Windowed";
            this.windowedToolStripMenuItem.Click += new System.EventHandler(this.WindowedToolStripMenuItem_Click);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.FullScreenToolStripMenuItem_Click);
            // 
            // checkforUpdatesToolStripMenuItem
            // 
            this.checkforUpdatesToolStripMenuItem.Name = "checkforUpdatesToolStripMenuItem";
            this.checkforUpdatesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.checkforUpdatesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.checkforUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkforUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckforUpdatesToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // tSWizardToolStripMenuItem
            // 
            this.tSWizardToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tSWizardToolStripMenuItem.Name = "tSWizardToolStripMenuItem";
            this.tSWizardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.tSWizardToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.tSWizardToolStripMenuItem.Text = "TSWizard";
            this.tSWizardToolStripMenuItem.Click += new System.EventHandler(this.TSWizardToolStripMenuItem_Click);
            // 
            // bmacToolStripMenuItem
            // 
            this.bmacToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bmacToolStripMenuItem.Name = "bmacToolStripMenuItem";
            this.bmacToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.bmacToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.bmacToolStripMenuItem.Text = "Bmac";
            this.bmacToolStripMenuItem.Click += new System.EventHandler(this.BmacToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // Panel_BG
            // 
            this.Panel_BG.Controls.Add(this.DataMainTable);
            this.Panel_BG.Controls.Add(this.Panel_Right);
            this.Panel_BG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_BG.Location = new System.Drawing.Point(0, 24);
            this.Panel_BG.Name = "Panel_BG";
            this.Panel_BG.Padding = new System.Windows.Forms.Padding(5);
            this.Panel_BG.Size = new System.Drawing.Size(1008, 577);
            this.Panel_BG.TabIndex = 1;
            // 
            // DataMainTable
            // 
            this.DataMainTable.AllowUserToAddRows = false;
            this.DataMainTable.AllowUserToDeleteRows = false;
            this.DataMainTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.DataMainTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataMainTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataMainTable.BackgroundColor = System.Drawing.Color.White;
            this.DataMainTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataMainTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataMainTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataMainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataMainTable.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataMainTable.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataMainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataMainTable.EnableHeadersVisualStyles = false;
            this.DataMainTable.GridColor = System.Drawing.Color.Gray;
            this.DataMainTable.Location = new System.Drawing.Point(5, 5);
            this.DataMainTable.MultiSelect = false;
            this.DataMainTable.Name = "DataMainTable";
            this.DataMainTable.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Crimson;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataMainTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataMainTable.RowHeadersVisible = false;
            this.DataMainTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DataMainTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataMainTable.Size = new System.Drawing.Size(773, 567);
            this.DataMainTable.TabIndex = 0;
            // 
            // Panel_Right
            // 
            this.Panel_Right.BackColor = System.Drawing.Color.White;
            this.Panel_Right.Controls.Add(this.FLP_Container);
            this.Panel_Right.Controls.Add(this.Label_NotUSB);
            this.Panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel_Right.Location = new System.Drawing.Point(778, 5);
            this.Panel_Right.Name = "Panel_Right";
            this.Panel_Right.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.Panel_Right.Size = new System.Drawing.Size(225, 567);
            this.Panel_Right.TabIndex = 1;
            // 
            // FLP_Container
            // 
            this.FLP_Container.AutoSize = true;
            this.FLP_Container.Controls.Add(this.BtnActiveProtect);
            this.FLP_Container.Controls.Add(this.BtnDisabledProtect);
            this.FLP_Container.Controls.Add(this.BtnFormatNTFS);
            this.FLP_Container.Dock = System.Windows.Forms.DockStyle.Top;
            this.FLP_Container.Location = new System.Drawing.Point(5, 0);
            this.FLP_Container.Name = "FLP_Container";
            this.FLP_Container.Size = new System.Drawing.Size(220, 119);
            this.FLP_Container.TabIndex = 0;
            // 
            // Label_NotUSB
            // 
            this.Label_NotUSB.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Label_NotUSB.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Label_NotUSB.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Label_NotUSB.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label_NotUSB.Location = new System.Drawing.Point(5, 432);
            this.Label_NotUSB.Margin = new System.Windows.Forms.Padding(3);
            this.Label_NotUSB.Name = "Label_NotUSB";
            this.Label_NotUSB.Padding = new System.Windows.Forms.Padding(5);
            this.Label_NotUSB.Size = new System.Drawing.Size(220, 135);
            this.Label_NotUSB.TabIndex = 1;
            this.Label_NotUSB.Text = "Herhangi bir USB depolama aygıtı tespit edilemedi.";
            this.Label_NotUSB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_NotUSB.Visible = false;
            // 
            // BtnActiveProtect
            // 
            this.BtnActiveProtect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnActiveProtect.BackColor = System.Drawing.Color.Crimson;
            this.BtnActiveProtect.BackgroundColor = System.Drawing.Color.Crimson;
            this.BtnActiveProtect.BorderColor = System.Drawing.Color.Crimson;
            this.BtnActiveProtect.BorderRadius = 10;
            this.BtnActiveProtect.BorderSize = 0;
            this.BtnActiveProtect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnActiveProtect.FlatAppearance.BorderSize = 0;
            this.BtnActiveProtect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnActiveProtect.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.BtnActiveProtect.ForeColor = System.Drawing.Color.White;
            this.BtnActiveProtect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnActiveProtect.Location = new System.Drawing.Point(0, 0);
            this.BtnActiveProtect.Margin = new System.Windows.Forms.Padding(0);
            this.BtnActiveProtect.Name = "BtnActiveProtect";
            this.BtnActiveProtect.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BtnActiveProtect.Size = new System.Drawing.Size(220, 35);
            this.BtnActiveProtect.TabIndex = 0;
            this.BtnActiveProtect.Text = "Korumayı Aç";
            this.BtnActiveProtect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnActiveProtect.TextColor = System.Drawing.Color.White;
            this.BtnActiveProtect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnActiveProtect.UseVisualStyleBackColor = false;
            this.BtnActiveProtect.Click += new System.EventHandler(this.BtnActiveProtect_Click);
            // 
            // BtnDisabledProtect
            // 
            this.BtnDisabledProtect.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnDisabledProtect.BackColor = System.Drawing.Color.Crimson;
            this.BtnDisabledProtect.BackgroundColor = System.Drawing.Color.Crimson;
            this.BtnDisabledProtect.BorderColor = System.Drawing.Color.Crimson;
            this.BtnDisabledProtect.BorderRadius = 10;
            this.BtnDisabledProtect.BorderSize = 0;
            this.BtnDisabledProtect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDisabledProtect.FlatAppearance.BorderSize = 0;
            this.BtnDisabledProtect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDisabledProtect.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.BtnDisabledProtect.ForeColor = System.Drawing.Color.White;
            this.BtnDisabledProtect.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnDisabledProtect.Location = new System.Drawing.Point(0, 42);
            this.BtnDisabledProtect.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.BtnDisabledProtect.Name = "BtnDisabledProtect";
            this.BtnDisabledProtect.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BtnDisabledProtect.Size = new System.Drawing.Size(220, 35);
            this.BtnDisabledProtect.TabIndex = 1;
            this.BtnDisabledProtect.Text = "Korumayı Kaldır";
            this.BtnDisabledProtect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnDisabledProtect.TextColor = System.Drawing.Color.White;
            this.BtnDisabledProtect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnDisabledProtect.UseVisualStyleBackColor = false;
            this.BtnDisabledProtect.Click += new System.EventHandler(this.BtnDisabledProtect_Click);
            // 
            // BtnFormatNTFS
            // 
            this.BtnFormatNTFS.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BtnFormatNTFS.BackColor = System.Drawing.Color.Crimson;
            this.BtnFormatNTFS.BackgroundColor = System.Drawing.Color.Crimson;
            this.BtnFormatNTFS.BorderColor = System.Drawing.Color.Crimson;
            this.BtnFormatNTFS.BorderRadius = 10;
            this.BtnFormatNTFS.BorderSize = 0;
            this.BtnFormatNTFS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnFormatNTFS.FlatAppearance.BorderSize = 0;
            this.BtnFormatNTFS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFormatNTFS.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.BtnFormatNTFS.ForeColor = System.Drawing.Color.White;
            this.BtnFormatNTFS.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnFormatNTFS.Location = new System.Drawing.Point(0, 84);
            this.BtnFormatNTFS.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.BtnFormatNTFS.Name = "BtnFormatNTFS";
            this.BtnFormatNTFS.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BtnFormatNTFS.Size = new System.Drawing.Size(220, 35);
            this.BtnFormatNTFS.TabIndex = 2;
            this.BtnFormatNTFS.Text = "NTFS Formatla";
            this.BtnFormatNTFS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnFormatNTFS.TextColor = System.Drawing.Color.White;
            this.BtnFormatNTFS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnFormatNTFS.UseVisualStyleBackColor = false;
            this.BtnFormatNTFS.Click += new System.EventHandler(this.BtnFormatNTFS_Click);
            // 
            // YamiraMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.Panel_BG);
            this.Controls.Add(this.HeaderMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.HeaderMenu;
            this.MinimumSize = new System.Drawing.Size(1024, 640);
            this.Name = "YamiraMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yamira";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Yamira_FormClosing);
            this.Load += new System.EventHandler(this.Yamira_Load);
            this.HeaderMenu.ResumeLayout(false);
            this.HeaderMenu.PerformLayout();
            this.Panel_BG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataMainTable)).EndInit();
            this.Panel_Right.ResumeLayout(false);
            this.Panel_Right.PerformLayout();
            this.FLP_Container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip HeaderMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lightThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem darkThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turkishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkforUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.Panel Panel_BG;
        private System.Windows.Forms.Panel Panel_Right;
        private System.Windows.Forms.DataGridView DataMainTable;
        private TSCustomButton BtnDisabledProtect;
        private TSCustomButton BtnActiveProtect;
        private TSCustomButton BtnFormatNTFS;
        private System.Windows.Forms.Label Label_NotUSB;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tSWizardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bmacToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel FLP_Container;
        private System.Windows.Forms.ToolStripMenuItem systemThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem polishToolStripMenuItem;
    }
}

