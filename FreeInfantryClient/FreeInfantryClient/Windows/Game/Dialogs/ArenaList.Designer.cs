namespace FreeInfantryClient.Windows.Dialogs
{
    partial class dialog_ArenaList
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
            this.listArenas = new System.Windows.Forms.ListBox();
            this.lblArena = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listArenas
            // 
            this.listArenas.BackColor = System.Drawing.SystemColors.InfoText;
            this.listArenas.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.listArenas.FormattingEnabled = true;
            this.listArenas.Location = new System.Drawing.Point(3, 25);
            this.listArenas.Name = "listArenas";
            this.listArenas.Size = new System.Drawing.Size(308, 108);
            this.listArenas.TabIndex = 0;
            this.listArenas.DoubleClick += new System.EventHandler(this.listArenas_DoubleClick);
            // 
            // lblArena
            // 
            this.lblArena.AutoSize = true;
            this.lblArena.ForeColor = System.Drawing.Color.Orange;
            this.lblArena.Location = new System.Drawing.Point(21, 9);
            this.lblArena.Name = "lblArena";
            this.lblArena.Size = new System.Drawing.Size(35, 13);
            this.lblArena.TabIndex = 2;
            this.lblArena.Text = "Arena";
            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.ForeColor = System.Drawing.Color.Orange;
            this.lblPlayers.Location = new System.Drawing.Point(256, 9);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(41, 13);
            this.lblPlayers.TabIndex = 3;
            this.lblPlayers.Text = "Players";
            // 
            // dialog_ArenaList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(314, 138);
            this.Controls.Add(this.lblPlayers);
            this.Controls.Add(this.lblArena);
            this.Controls.Add(this.listArenas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dialog_ArenaList";
            this.Opacity = 0.7D;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArenaList";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ArenaList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dialog_ArenaList_KeyDown);
            this.Leave += new System.EventHandler(this.dialog_ArenaList_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listArenas;
        private System.Windows.Forms.Label lblArena;
        private System.Windows.Forms.Label lblPlayers;
    }
}