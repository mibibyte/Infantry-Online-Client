namespace FreeInfantryClient.Windows
{
    partial class Game
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
            this.lstPlayers = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSend.Location = new System.Drawing.Point(816, 493);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(146, 33);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
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
            this.chatSend.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.chatSend.ForeColor = System.Drawing.Color.Aquamarine;
            this.chatSend.Location = new System.Drawing.Point(12, 493);
            this.chatSend.Multiline = true;
            this.chatSend.Name = "chatSend";
            this.chatSend.Size = new System.Drawing.Size(798, 33);
            this.chatSend.TabIndex = 4;
            this.chatSend.KeyUp += new System.Windows.Forms.KeyEventHandler(this.chatSend_KeyUp);
            // 
            // chatLog
            // 
            this.chatLog.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.chatLog.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chatLog.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chatLog.Location = new System.Drawing.Point(12, 32);
            this.chatLog.Name = "chatLog";
            this.chatLog.ReadOnly = true;
            this.chatLog.Size = new System.Drawing.Size(798, 457);
            this.chatLog.TabIndex = 5;
            this.chatLog.Text = "";
            // 
            // lstPlayers
            // 
            this.lstPlayers.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.lstPlayers.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstPlayers.ForeColor = System.Drawing.Color.DarkOrange;
            this.lstPlayers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstPlayers.HideSelection = false;
            this.lstPlayers.Location = new System.Drawing.Point(816, 32);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(145, 456);
            this.lstPlayers.TabIndex = 6;
            this.lstPlayers.UseCompatibleStateImageBehavior = false;
            this.lstPlayers.View = System.Windows.Forms.View.List;
            // 
            // Game
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(974, 538);
            this.Controls.Add(this.lstPlayers);
            this.Controls.Add(this.chatLog);
            this.Controls.Add(this.chatSend);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Game";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Infantry Online";
            this.Load += new System.EventHandler(this.Game_Load);
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
        private System.Windows.Forms.ListView lstPlayers;
    }
}