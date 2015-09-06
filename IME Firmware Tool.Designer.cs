namespace IME_Firmware_Tool
{
    partial class IMEFirmwareTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IMEFirmwareTool));
            this.InstalledMEFirmwareVersionLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.FirmwareUpdateCheckingTab = new System.Windows.Forms.TabPage();
            this.StartupLoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.FirmwareType = new System.Windows.Forms.Label();
            this.ServerVersionLabel = new System.Windows.Forms.Label();
            this.DownloadUpdatePictureBox = new System.Windows.Forms.PictureBox();
            this.FirmwareUpdateCheckingLoadingPictureBox = new System.Windows.Forms.PictureBox();
            this.FlashUpdateButton = new System.Windows.Forms.Button();
            this.DownloadUpdateButton = new System.Windows.Forms.Button();
            this.CheckServerButton = new System.Windows.Forms.Button();
            this.FirmwareFlashingTab = new System.Windows.Forms.TabPage();
            this.LockPictureBox = new System.Windows.Forms.PictureBox();
            this.FlashingPictureBox = new System.Windows.Forms.PictureBox();
            this.SelectFiletoFlashLinkLabel = new System.Windows.Forms.LinkLabel();
            this.PreferencesTab = new System.Windows.Forms.TabPage();
            this.AllowFlashingOfSameFirmwareVersionCheckBox = new System.Windows.Forms.CheckBox();
            this.ForceResetCheckBox = new System.Windows.Forms.CheckBox();
            this.AboutTab = new System.Windows.Forms.TabPage();
            this.ProgramUpdatePictureBox = new System.Windows.Forms.PictureBox();
            this.MyWebsiteLink = new System.Windows.Forms.LinkLabel();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.CreatedByLabel = new System.Windows.Forms.Label();
            this.BuildDateLabel = new System.Windows.Forms.Label();
            this.ProgramVersionLabel = new System.Windows.Forms.Label();
            this.Tabs.SuspendLayout();
            this.FirmwareUpdateCheckingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartupLoadingPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadUpdatePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirmwareUpdateCheckingLoadingPictureBox)).BeginInit();
            this.FirmwareFlashingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LockPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FlashingPictureBox)).BeginInit();
            this.PreferencesTab.SuspendLayout();
            this.AboutTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramUpdatePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // InstalledMEFirmwareVersionLabel
            // 
            this.InstalledMEFirmwareVersionLabel.AutoSize = true;
            this.InstalledMEFirmwareVersionLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.InstalledMEFirmwareVersionLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InstalledMEFirmwareVersionLabel.ForeColor = System.Drawing.Color.Blue;
            this.InstalledMEFirmwareVersionLabel.Location = new System.Drawing.Point(250, 3);
            this.InstalledMEFirmwareVersionLabel.Name = "InstalledMEFirmwareVersionLabel";
            this.InstalledMEFirmwareVersionLabel.Size = new System.Drawing.Size(202, 13);
            this.InstalledMEFirmwareVersionLabel.TabIndex = 1;
            this.InstalledMEFirmwareVersionLabel.Text = "Installed IME Firmware Version: 9.999999";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.ForeColor = System.Drawing.Color.Blue;
            this.StatusLabel.Location = new System.Drawing.Point(-2, 252);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "Status";
            this.StatusLabel.Visible = false;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.FirmwareUpdateCheckingTab);
            this.Tabs.Controls.Add(this.FirmwareFlashingTab);
            this.Tabs.Controls.Add(this.PreferencesTab);
            this.Tabs.Controls.Add(this.AboutTab);
            this.Tabs.HotTrack = true;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(463, 291);
            this.Tabs.TabIndex = 4;
            // 
            // FirmwareUpdateCheckingTab
            // 
            this.FirmwareUpdateCheckingTab.Controls.Add(this.StartupLoadingPictureBox);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.FirmwareType);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.ServerVersionLabel);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.DownloadUpdatePictureBox);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.FirmwareUpdateCheckingLoadingPictureBox);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.StatusLabel);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.FlashUpdateButton);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.DownloadUpdateButton);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.CheckServerButton);
            this.FirmwareUpdateCheckingTab.Controls.Add(this.InstalledMEFirmwareVersionLabel);
            this.FirmwareUpdateCheckingTab.ForeColor = System.Drawing.Color.Blue;
            this.FirmwareUpdateCheckingTab.Location = new System.Drawing.Point(4, 22);
            this.FirmwareUpdateCheckingTab.Name = "FirmwareUpdateCheckingTab";
            this.FirmwareUpdateCheckingTab.Padding = new System.Windows.Forms.Padding(3);
            this.FirmwareUpdateCheckingTab.Size = new System.Drawing.Size(455, 265);
            this.FirmwareUpdateCheckingTab.TabIndex = 0;
            this.FirmwareUpdateCheckingTab.Text = "Firmware Update Checking";
            this.FirmwareUpdateCheckingTab.UseVisualStyleBackColor = true;
            // 
            // StartupLoadingPictureBox
            // 
            this.StartupLoadingPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.StartupLoadingPictureBox.Enabled = false;
            this.StartupLoadingPictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.Update;
            this.StartupLoadingPictureBox.Location = new System.Drawing.Point(211, 116);
            this.StartupLoadingPictureBox.Name = "StartupLoadingPictureBox";
            this.StartupLoadingPictureBox.Size = new System.Drawing.Size(32, 32);
            this.StartupLoadingPictureBox.TabIndex = 16;
            this.StartupLoadingPictureBox.TabStop = false;
            // 
            // FirmwareType
            // 
            this.FirmwareType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FirmwareType.Location = new System.Drawing.Point(252, 16);
            this.FirmwareType.Name = "FirmwareType";
            this.FirmwareType.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.FirmwareType.Size = new System.Drawing.Size(200, 13);
            this.FirmwareType.TabIndex = 6;
            this.FirmwareType.Text = "Firmware Type: 1.5MB";
            // 
            // ServerVersionLabel
            // 
            this.ServerVersionLabel.AutoSize = true;
            this.ServerVersionLabel.Location = new System.Drawing.Point(-2, 239);
            this.ServerVersionLabel.Name = "ServerVersionLabel";
            this.ServerVersionLabel.Size = new System.Drawing.Size(76, 13);
            this.ServerVersionLabel.TabIndex = 5;
            this.ServerVersionLabel.Text = "Server Version";
            this.ServerVersionLabel.Visible = false;
            // 
            // DownloadUpdatePictureBox
            // 
            this.DownloadUpdatePictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.LoadingStyle1;
            this.DownloadUpdatePictureBox.Location = new System.Drawing.Point(293, 200);
            this.DownloadUpdatePictureBox.Name = "DownloadUpdatePictureBox";
            this.DownloadUpdatePictureBox.Size = new System.Drawing.Size(16, 16);
            this.DownloadUpdatePictureBox.TabIndex = 4;
            this.DownloadUpdatePictureBox.TabStop = false;
            this.DownloadUpdatePictureBox.Visible = false;
            // 
            // FirmwareUpdateCheckingLoadingPictureBox
            // 
            this.FirmwareUpdateCheckingLoadingPictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.LoadingStyle1;
            this.FirmwareUpdateCheckingLoadingPictureBox.Location = new System.Drawing.Point(293, 239);
            this.FirmwareUpdateCheckingLoadingPictureBox.Name = "FirmwareUpdateCheckingLoadingPictureBox";
            this.FirmwareUpdateCheckingLoadingPictureBox.Size = new System.Drawing.Size(16, 16);
            this.FirmwareUpdateCheckingLoadingPictureBox.TabIndex = 4;
            this.FirmwareUpdateCheckingLoadingPictureBox.TabStop = false;
            this.FirmwareUpdateCheckingLoadingPictureBox.Visible = false;
            // 
            // FlashUpdateButton
            // 
            this.FlashUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FlashUpdateButton.BackColor = System.Drawing.SystemColors.Control;
            this.FlashUpdateButton.Enabled = false;
            this.FlashUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlashUpdateButton.ForeColor = System.Drawing.Color.Red;
            this.FlashUpdateButton.Location = new System.Drawing.Point(315, 149);
            this.FlashUpdateButton.Name = "FlashUpdateButton";
            this.FlashUpdateButton.Size = new System.Drawing.Size(138, 38);
            this.FlashUpdateButton.TabIndex = 0;
            this.FlashUpdateButton.Text = "Flash Update";
            this.FlashUpdateButton.UseVisualStyleBackColor = false;
            this.FlashUpdateButton.Click += new System.EventHandler(this.FlashUpdateButton_Click);
            // 
            // DownloadUpdateButton
            // 
            this.DownloadUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadUpdateButton.BackColor = System.Drawing.SystemColors.Control;
            this.DownloadUpdateButton.Enabled = false;
            this.DownloadUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DownloadUpdateButton.ForeColor = System.Drawing.Color.DarkGreen;
            this.DownloadUpdateButton.Location = new System.Drawing.Point(315, 188);
            this.DownloadUpdateButton.Name = "DownloadUpdateButton";
            this.DownloadUpdateButton.Size = new System.Drawing.Size(138, 38);
            this.DownloadUpdateButton.TabIndex = 0;
            this.DownloadUpdateButton.Text = "Download Update";
            this.DownloadUpdateButton.UseVisualStyleBackColor = false;
            this.DownloadUpdateButton.EnabledChanged += new System.EventHandler(this.DownloadUpdateButton_EnabledChanged);
            this.DownloadUpdateButton.Click += new System.EventHandler(this.DownloadUpdateButton_Click);
            // 
            // CheckServerButton
            // 
            this.CheckServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckServerButton.BackColor = System.Drawing.SystemColors.Control;
            this.CheckServerButton.Enabled = false;
            this.CheckServerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckServerButton.ForeColor = System.Drawing.Color.DarkGreen;
            this.CheckServerButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CheckServerButton.Location = new System.Drawing.Point(315, 227);
            this.CheckServerButton.Name = "CheckServerButton";
            this.CheckServerButton.Size = new System.Drawing.Size(138, 38);
            this.CheckServerButton.TabIndex = 0;
            this.CheckServerButton.Text = "Check Server";
            this.CheckServerButton.UseVisualStyleBackColor = false;
            this.CheckServerButton.EnabledChanged += new System.EventHandler(this.CheckServerButton_EnabledChanged);
            this.CheckServerButton.Click += new System.EventHandler(this.CheckServerButton_Click);
            // 
            // FirmwareFlashingTab
            // 
            this.FirmwareFlashingTab.Controls.Add(this.LockPictureBox);
            this.FirmwareFlashingTab.Controls.Add(this.FlashingPictureBox);
            this.FirmwareFlashingTab.Controls.Add(this.SelectFiletoFlashLinkLabel);
            this.FirmwareFlashingTab.ForeColor = System.Drawing.Color.Blue;
            this.FirmwareFlashingTab.Location = new System.Drawing.Point(4, 22);
            this.FirmwareFlashingTab.Name = "FirmwareFlashingTab";
            this.FirmwareFlashingTab.Padding = new System.Windows.Forms.Padding(3);
            this.FirmwareFlashingTab.Size = new System.Drawing.Size(455, 265);
            this.FirmwareFlashingTab.TabIndex = 1;
            this.FirmwareFlashingTab.Text = "Firmware Flashing";
            this.FirmwareFlashingTab.UseVisualStyleBackColor = true;
            // 
            // LockPictureBox
            // 
            this.LockPictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.WhiteLockedIcon;
            this.LockPictureBox.Location = new System.Drawing.Point(274, 111);
            this.LockPictureBox.Name = "LockPictureBox";
            this.LockPictureBox.Size = new System.Drawing.Size(32, 32);
            this.LockPictureBox.TabIndex = 6;
            this.LockPictureBox.TabStop = false;
            // 
            // FlashingPictureBox
            // 
            this.FlashingPictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.LoadingStyle1;
            this.FlashingPictureBox.Location = new System.Drawing.Point(158, 124);
            this.FlashingPictureBox.Name = "FlashingPictureBox";
            this.FlashingPictureBox.Size = new System.Drawing.Size(16, 16);
            this.FlashingPictureBox.TabIndex = 5;
            this.FlashingPictureBox.TabStop = false;
            this.FlashingPictureBox.Visible = false;
            // 
            // SelectFiletoFlashLinkLabel
            // 
            this.SelectFiletoFlashLinkLabel.ActiveLinkColor = System.Drawing.Color.Black;
            this.SelectFiletoFlashLinkLabel.AutoSize = true;
            this.SelectFiletoFlashLinkLabel.ForeColor = System.Drawing.Color.Red;
            this.SelectFiletoFlashLinkLabel.LinkColor = System.Drawing.Color.Red;
            this.SelectFiletoFlashLinkLabel.Location = new System.Drawing.Point(179, 126);
            this.SelectFiletoFlashLinkLabel.Name = "SelectFiletoFlashLinkLabel";
            this.SelectFiletoFlashLinkLabel.Size = new System.Drawing.Size(96, 13);
            this.SelectFiletoFlashLinkLabel.TabIndex = 0;
            this.SelectFiletoFlashLinkLabel.TabStop = true;
            this.SelectFiletoFlashLinkLabel.Text = "Select File to Flash";
            this.SelectFiletoFlashLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SelectFiletoFlashLinkLabel_LinkClicked);
            this.SelectFiletoFlashLinkLabel.EnabledChanged += new System.EventHandler(this.SelectFiletoFlashLinkLabel_EnabledChanged);
            // 
            // PreferencesTab
            // 
            this.PreferencesTab.Controls.Add(this.AllowFlashingOfSameFirmwareVersionCheckBox);
            this.PreferencesTab.Controls.Add(this.ForceResetCheckBox);
            this.PreferencesTab.ForeColor = System.Drawing.Color.Blue;
            this.PreferencesTab.Location = new System.Drawing.Point(4, 22);
            this.PreferencesTab.Name = "PreferencesTab";
            this.PreferencesTab.Padding = new System.Windows.Forms.Padding(3);
            this.PreferencesTab.Size = new System.Drawing.Size(455, 265);
            this.PreferencesTab.TabIndex = 2;
            this.PreferencesTab.Text = "Preferences";
            this.PreferencesTab.UseVisualStyleBackColor = true;
            // 
            // AllowFlashingOfSameFirmwareVersionCheckBox
            // 
            this.AllowFlashingOfSameFirmwareVersionCheckBox.AutoSize = true;
            this.AllowFlashingOfSameFirmwareVersionCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AllowFlashingOfSameFirmwareVersionCheckBox.ForeColor = System.Drawing.Color.Orange;
            this.AllowFlashingOfSameFirmwareVersionCheckBox.Location = new System.Drawing.Point(3, 19);
            this.AllowFlashingOfSameFirmwareVersionCheckBox.Name = "AllowFlashingOfSameFirmwareVersionCheckBox";
            this.AllowFlashingOfSameFirmwareVersionCheckBox.Size = new System.Drawing.Size(215, 17);
            this.AllowFlashingOfSameFirmwareVersionCheckBox.TabIndex = 4;
            this.AllowFlashingOfSameFirmwareVersionCheckBox.Text = "Allow Flashing of Same Firmware Version";
            this.AllowFlashingOfSameFirmwareVersionCheckBox.UseVisualStyleBackColor = true;
            this.AllowFlashingOfSameFirmwareVersionCheckBox.CheckedChanged += new System.EventHandler(this.AllowFlashingOfSameFirmwareVersionCheckBox_CheckedChanged);
            // 
            // ForceResetCheckBox
            // 
            this.ForceResetCheckBox.AutoSize = true;
            this.ForceResetCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForceResetCheckBox.ForeColor = System.Drawing.Color.Orange;
            this.ForceResetCheckBox.Location = new System.Drawing.Point(3, 3);
            this.ForceResetCheckBox.Name = "ForceResetCheckBox";
            this.ForceResetCheckBox.Size = new System.Drawing.Size(216, 17);
            this.ForceResetCheckBox.TabIndex = 4;
            this.ForceResetCheckBox.Text = "Force System Reset After Firmware Flash";
            this.ForceResetCheckBox.UseVisualStyleBackColor = true;
            this.ForceResetCheckBox.CheckedChanged += new System.EventHandler(this.ForceResetCheckBox_CheckedChanged);
            // 
            // AboutTab
            // 
            this.AboutTab.Controls.Add(this.ProgramUpdatePictureBox);
            this.AboutTab.Controls.Add(this.MyWebsiteLink);
            this.AboutTab.Controls.Add(this.CopyrightLabel);
            this.AboutTab.Controls.Add(this.EmailLabel);
            this.AboutTab.Controls.Add(this.CreatedByLabel);
            this.AboutTab.Controls.Add(this.BuildDateLabel);
            this.AboutTab.Controls.Add(this.ProgramVersionLabel);
            this.AboutTab.Location = new System.Drawing.Point(4, 22);
            this.AboutTab.Name = "AboutTab";
            this.AboutTab.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTab.Size = new System.Drawing.Size(455, 265);
            this.AboutTab.TabIndex = 3;
            this.AboutTab.Text = "About";
            this.AboutTab.UseVisualStyleBackColor = true;
            // 
            // ProgramUpdatePictureBox
            // 
            this.ProgramUpdatePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ProgramUpdatePictureBox.Image = global::IME_Firmware_Tool.Properties.Resources.Update;
            this.ProgramUpdatePictureBox.Location = new System.Drawing.Point(423, 233);
            this.ProgramUpdatePictureBox.Name = "ProgramUpdatePictureBox";
            this.ProgramUpdatePictureBox.Size = new System.Drawing.Size(32, 32);
            this.ProgramUpdatePictureBox.TabIndex = 15;
            this.ProgramUpdatePictureBox.TabStop = false;
            this.ProgramUpdatePictureBox.Click += new System.EventHandler(this.ProgramUpdatePictureBox_Click);
            // 
            // MyWebsiteLink
            // 
            this.MyWebsiteLink.ActiveLinkColor = System.Drawing.Color.Black;
            this.MyWebsiteLink.AutoSize = true;
            this.MyWebsiteLink.ForeColor = System.Drawing.Color.Blue;
            this.MyWebsiteLink.LinkColor = System.Drawing.Color.Indigo;
            this.MyWebsiteLink.Location = new System.Drawing.Point(-1, 66);
            this.MyWebsiteLink.Name = "MyWebsiteLink";
            this.MyWebsiteLink.Size = new System.Drawing.Size(63, 13);
            this.MyWebsiteLink.TabIndex = 14;
            this.MyWebsiteLink.TabStop = true;
            this.MyWebsiteLink.Text = "My Website";
            this.MyWebsiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MyWebsiteLink_LinkClicked);
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.ForeColor = System.Drawing.Color.Blue;
            this.CopyrightLabel.Location = new System.Drawing.Point(-1, 53);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(268, 13);
            this.CopyrightLabel.TabIndex = 9;
            this.CopyrightLabel.Text = "Copyright: GlitchyHack 2014-2015 All Rights Reserved.";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.ForeColor = System.Drawing.Color.Blue;
            this.EmailLabel.Location = new System.Drawing.Point(-1, 40);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(156, 13);
            this.EmailLabel.TabIndex = 10;
            this.EmailLabel.Text = "Email: GlitchyHack@Gmail.com";
            // 
            // CreatedByLabel
            // 
            this.CreatedByLabel.AutoSize = true;
            this.CreatedByLabel.ForeColor = System.Drawing.Color.Blue;
            this.CreatedByLabel.Location = new System.Drawing.Point(-1, 27);
            this.CreatedByLabel.Name = "CreatedByLabel";
            this.CreatedByLabel.Size = new System.Drawing.Size(123, 13);
            this.CreatedByLabel.TabIndex = 11;
            this.CreatedByLabel.Text = "Created By: GlitchyHack";
            // 
            // BuildDateLabel
            // 
            this.BuildDateLabel.AutoSize = true;
            this.BuildDateLabel.ForeColor = System.Drawing.Color.Blue;
            this.BuildDateLabel.Location = new System.Drawing.Point(-1, 14);
            this.BuildDateLabel.Name = "BuildDateLabel";
            this.BuildDateLabel.Size = new System.Drawing.Size(56, 13);
            this.BuildDateLabel.TabIndex = 12;
            this.BuildDateLabel.Text = "Build Date";
            // 
            // ProgramVersionLabel
            // 
            this.ProgramVersionLabel.AutoSize = true;
            this.ProgramVersionLabel.ForeColor = System.Drawing.Color.Blue;
            this.ProgramVersionLabel.Location = new System.Drawing.Point(-1, 1);
            this.ProgramVersionLabel.Name = "ProgramVersionLabel";
            this.ProgramVersionLabel.Size = new System.Drawing.Size(84, 13);
            this.ProgramVersionLabel.TabIndex = 13;
            this.ProgramVersionLabel.Text = "Program Version";
            // 
            // IMEFirmwareTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 290);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "IMEFirmwareTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IME Firmware Tool";
            this.Load += new System.EventHandler(this.MEUpdateChecker_Load);
            this.Tabs.ResumeLayout(false);
            this.FirmwareUpdateCheckingTab.ResumeLayout(false);
            this.FirmwareUpdateCheckingTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartupLoadingPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadUpdatePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirmwareUpdateCheckingLoadingPictureBox)).EndInit();
            this.FirmwareFlashingTab.ResumeLayout(false);
            this.FirmwareFlashingTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LockPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FlashingPictureBox)).EndInit();
            this.PreferencesTab.ResumeLayout(false);
            this.PreferencesTab.PerformLayout();
            this.AboutTab.ResumeLayout(false);
            this.AboutTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramUpdatePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CheckServerButton;
        private System.Windows.Forms.Label InstalledMEFirmwareVersionLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage FirmwareUpdateCheckingTab;
        private System.Windows.Forms.TabPage FirmwareFlashingTab;
        private System.Windows.Forms.TabPage PreferencesTab;
        private System.Windows.Forms.Button DownloadUpdateButton;
        private System.Windows.Forms.Button FlashUpdateButton;
        private System.Windows.Forms.LinkLabel SelectFiletoFlashLinkLabel;
        private System.Windows.Forms.PictureBox FirmwareUpdateCheckingLoadingPictureBox;
        private System.Windows.Forms.CheckBox ForceResetCheckBox;
        private System.Windows.Forms.PictureBox DownloadUpdatePictureBox;
        private System.Windows.Forms.CheckBox AllowFlashingOfSameFirmwareVersionCheckBox;
        private System.Windows.Forms.PictureBox FlashingPictureBox;
        private System.Windows.Forms.Label ServerVersionLabel;
        private System.Windows.Forms.TabPage AboutTab;
        private System.Windows.Forms.LinkLabel MyWebsiteLink;
        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label CreatedByLabel;
        private System.Windows.Forms.Label BuildDateLabel;
        private System.Windows.Forms.Label ProgramVersionLabel;
        private System.Windows.Forms.PictureBox ProgramUpdatePictureBox;
        private System.Windows.Forms.PictureBox LockPictureBox;
        private System.Windows.Forms.Label FirmwareType;
        private System.Windows.Forms.PictureBox StartupLoadingPictureBox;
    }
}

