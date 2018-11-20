namespace FreeInfantryClient.Windows
{
    public partial class Game
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
            this.btnSend = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chatSend = new System.Windows.Forms.TextBox();
            this.chatLog = new System.Windows.Forms.RichTextBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.lstPlayers = new System.Windows.Forms.TreeView();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSend.Location = new System.Drawing.Point(944, 508);
            this.btnSend.Margin = new System.Windows.Forms.Padding(0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(57, 27);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1001, 24);
            this.menuStrip1.TabIndex = 7;
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // chatSend
            // 
            this.chatSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatSend.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.chatSend.ForeColor = System.Drawing.Color.Aquamarine;
            this.chatSend.Location = new System.Drawing.Point(12, 508);
            this.chatSend.Multiline = true;
            this.chatSend.Name = "chatSend";
            this.chatSend.Size = new System.Drawing.Size(929, 27);
            this.chatSend.TabIndex = 4;
            this.chatSend.KeyUp += new System.Windows.Forms.KeyEventHandler(this.chatSend_KeyUp);
            // 
            // chatLog
            // 
            this.chatLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatLog.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.chatLog.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.chatLog.Location = new System.Drawing.Point(12, 30);
            this.chatLog.Name = "chatLog";
            this.chatLog.ReadOnly = true;
            this.chatLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.chatLog.Size = new System.Drawing.Size(820, 472);
            this.chatLog.TabIndex = 9;
            this.chatLog.Text = "";
            this.chatLog.Enter += new System.EventHandler(this.chatLog_Enter);
            this.chatLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chatLog_MouseDown);
            this.chatLog.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chatLog_MouseMove);
            this.chatLog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chatLog_MouseUp);
            // 
            // pnlRight
            // 
            this.pnlRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRight.Controls.Add(this.lstPlayers);
            this.pnlRight.Location = new System.Drawing.Point(835, 30);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(163, 469);
            this.pnlRight.TabIndex = 11;
            // 
            // lstPlayers
            // 
            this.lstPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPlayers.BackColor = System.Drawing.Color.Black;
            this.lstPlayers.ForeColor = System.Drawing.Color.White;
            this.lstPlayers.LineColor = System.Drawing.Color.Gold;
            this.lstPlayers.Location = new System.Drawing.Point(3, 3);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(157, 321);
            this.lstPlayers.TabIndex = 0;
            // 
            // Game
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1001, 535);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.chatLog);
            this.Controls.Add(this.chatSend);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "Game";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Infantry Online";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Game_FormClosing);
            this.Load += new System.EventHandler(this.Game_Load);
            this.Move += new System.EventHandler(this.Game_Move);
            this.Resize += new System.EventHandler(this.Game_Resize);
            this.pnlRight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TextBox chatSend;
        private System.Windows.Forms.RichTextBox chatLog;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.TreeView lstPlayers;
    }
}