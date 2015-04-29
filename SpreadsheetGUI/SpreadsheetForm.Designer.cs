namespace SS
{
    partial class spreadsheetWinForm
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
            this.components = new System.ComponentModel.Container();
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            this.contentsBox = new System.Windows.Forms.TextBox();
            this.Cell = new System.Windows.Forms.Label();
            this.cellName = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.valueBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contentButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hEROMODEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.jukeboxPlay = new System.Windows.Forms.Button();
            this.jukeboxOpen = new System.Windows.Forms.Button();
            this.jukeboxNameLabel = new System.Windows.Forms.Label();
            this.jukeboxTitle = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel2.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel.Location = new System.Drawing.Point(2, 62);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(780, 491);
            this.spreadsheetPanel.TabIndex = 0;
            // 
            // contentsBox
            // 
            this.contentsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentsBox.Location = new System.Drawing.Point(250, 36);
            this.contentsBox.MinimumSize = new System.Drawing.Size(100, 20);
            this.contentsBox.Name = "contentsBox";
            this.contentsBox.Size = new System.Drawing.Size(441, 20);
            this.contentsBox.TabIndex = 1;
            this.contentsBox.WordWrap = false;
            this.contentsBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.contentsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentsBox_KeyDown);
            this.contentsBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.contentsBox_KeyPress);
            this.contentsBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.contentsBox_KeyUp);
            // 
            // Cell
            // 
            this.Cell.AutoSize = true;
            this.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cell.Location = new System.Drawing.Point(3, 6);
            this.Cell.Name = "Cell";
            this.Cell.Size = new System.Drawing.Size(32, 13);
            this.Cell.TabIndex = 2;
            this.Cell.Text = "Cell:";
            // 
            // cellName
            // 
            this.cellName.AutoSize = true;
            this.cellName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cellName.Location = new System.Drawing.Point(41, 6);
            this.cellName.Name = "cellName";
            this.cellName.Size = new System.Drawing.Size(0, 13);
            this.cellName.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.valueBox);
            this.panel2.Controls.Add(this.Cell);
            this.panel2.Controls.Add(this.cellName);
            this.panel2.Location = new System.Drawing.Point(2, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(184, 26);
            this.panel2.TabIndex = 5;
            // 
            // valueBox
            // 
            this.valueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.valueBox.Location = new System.Drawing.Point(74, 3);
            this.valueBox.MinimumSize = new System.Drawing.Size(94, 20);
            this.valueBox.Name = "valueBox";
            this.valueBox.ReadOnly = true;
            this.valueBox.Size = new System.Drawing.Size(107, 20);
            this.valueBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Contents:";
            // 
            // contentButton
            // 
            this.contentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.contentButton.Location = new System.Drawing.Point(697, 36);
            this.contentButton.Name = "contentButton";
            this.contentButton.Size = new System.Drawing.Size(75, 20);
            this.contentButton.TabIndex = 7;
            this.contentButton.Text = "Enter";
            this.contentButton.UseVisualStyleBackColor = true;
            this.contentButton.Click += new System.EventHandler(this.contentButton_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.saveAsMenuItem,
            this.exotToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Enabled = false;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsMenuItem.Text = "Save As";
            this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // exotToolStripMenuItem
            // 
            this.exotToolStripMenuItem.Name = "exotToolStripMenuItem";
            this.exotToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exotToolStripMenuItem.Text = "Exit";
            this.exotToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.helpToolStripMenuItem.Text = "About";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.extrasToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "menuStrip1";
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hEROMODEToolStripMenuItem});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            this.extrasToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.extrasToolStripMenuItem.Text = "Extras";
            // 
            // hEROMODEToolStripMenuItem
            // 
            this.hEROMODEToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hEROMODEToolStripMenuItem.Name = "hEROMODEToolStripMenuItem";
            this.hEROMODEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hEROMODEToolStripMenuItem.Text = "Hero Mode";
            this.hEROMODEToolStripMenuItem.Click += new System.EventHandler(this.hEROMODEToolStripMenuItem_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // jukeboxPlay
            // 
            this.jukeboxPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jukeboxPlay.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.jukeboxPlay.Location = new System.Drawing.Point(697, 0);
            this.jukeboxPlay.Name = "jukeboxPlay";
            this.jukeboxPlay.Size = new System.Drawing.Size(75, 23);
            this.jukeboxPlay.TabIndex = 9;
            this.jukeboxPlay.Text = "Play";
            this.jukeboxPlay.UseVisualStyleBackColor = false;
            this.jukeboxPlay.Click += new System.EventHandler(this.jukeboxPlay_Click);
            // 
            // jukeboxOpen
            // 
            this.jukeboxOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jukeboxOpen.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.jukeboxOpen.Location = new System.Drawing.Point(616, 0);
            this.jukeboxOpen.Name = "jukeboxOpen";
            this.jukeboxOpen.Size = new System.Drawing.Size(75, 23);
            this.jukeboxOpen.TabIndex = 9;
            this.jukeboxOpen.Text = "Open";
            this.jukeboxOpen.UseVisualStyleBackColor = false;
            this.jukeboxOpen.Click += new System.EventHandler(this.jukeboxOpen_Click);
            // 
            // jukeboxNameLabel
            // 
            this.jukeboxNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jukeboxNameLabel.AutoSize = true;
            this.jukeboxNameLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.jukeboxNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jukeboxNameLabel.Location = new System.Drawing.Point(482, 5);
            this.jukeboxNameLabel.Name = "jukeboxNameLabel";
            this.jukeboxNameLabel.Size = new System.Drawing.Size(58, 13);
            this.jukeboxNameLabel.TabIndex = 10;
            this.jukeboxNameLabel.Text = "Jukebox:";
            // 
            // jukeboxTitle
            // 
            this.jukeboxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.jukeboxTitle.AutoSize = true;
            this.jukeboxTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.jukeboxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jukeboxTitle.Location = new System.Drawing.Point(536, 5);
            this.jukeboxTitle.Name = "jukeboxTitle";
            this.jukeboxTitle.Size = new System.Drawing.Size(31, 13);
            this.jukeboxTitle.TabIndex = 10;
            this.jukeboxTitle.Text = "none";
            this.toolTip.SetToolTip(this.jukeboxTitle, "No Song Loaded");
            // 
            // spreadsheetWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 556);
            this.Controls.Add(this.jukeboxTitle);
            this.Controls.Add(this.jukeboxNameLabel);
            this.Controls.Add(this.jukeboxOpen);
            this.Controls.Add(this.jukeboxPlay);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.contentButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.contentsBox);
            this.Controls.Add(this.spreadsheetPanel);
            this.MinimumSize = new System.Drawing.Size(475, 200);
            this.Name = "spreadsheetWinForm";
            this.Text = "untitled - Spreadsheet";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.contentsBox_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.contentsBox_KeyUp);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.TextBox contentsBox;
        private System.Windows.Forms.Label Cell;
        private System.Windows.Forms.Label cellName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox valueBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button contentButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.ToolStripMenuItem exotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hEROMODEToolStripMenuItem;
        private System.Windows.Forms.Label jukeboxNameLabel;
        private System.Windows.Forms.Button jukeboxOpen;
        private System.Windows.Forms.Button jukeboxPlay;
        private System.Windows.Forms.Label jukeboxTitle;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

