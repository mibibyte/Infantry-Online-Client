namespace FreeInfantryClient.Windows
{
    partial class frmZoneList
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
            this.boxList = new System.Windows.Forms.GroupBox();
            this.listZones = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAlias = new System.Windows.Forms.Label();
            this.alias = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.boxList.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // boxList
            // 
            this.boxList.AutoSize = true;
            this.boxList.Controls.Add(this.listZones);
            this.boxList.Location = new System.Drawing.Point(241, 12);
            this.boxList.Name = "boxList";
            this.boxList.Size = new System.Drawing.Size(332, 394);
            this.boxList.TabIndex = 0;
            this.boxList.TabStop = false;
            this.boxList.Text = "Zone List";
            // 
            // listZones
            // 
            this.listZones.FormattingEnabled = true;
            this.listZones.Location = new System.Drawing.Point(13, 20);
            this.listZones.Name = "listZones";
            this.listZones.Size = new System.Drawing.Size(302, 355);
            this.listZones.TabIndex = 0;
            this.listZones.SelectedIndexChanged += new System.EventHandler(this.listZones_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.lblAlias);
            this.groupBox1.Controls.Add(this.alias);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 157);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Account Info";
            // 
            // lblAlias
            // 
            this.lblAlias.AutoSize = true;
            this.lblAlias.Location = new System.Drawing.Point(93, 16);
            this.lblAlias.Name = "lblAlias";
            this.lblAlias.Size = new System.Drawing.Size(29, 13);
            this.lblAlias.TabIndex = 1;
            this.lblAlias.Text = "Alias";
            // 
            // alias
            // 
            this.alias.Location = new System.Drawing.Point(37, 31);
            this.alias.Name = "alias";
            this.alias.Size = new System.Drawing.Size(147, 20);
            this.alias.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Location = new System.Drawing.Point(12, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 177);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zone Description";
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 358);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(223, 44);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // frmZoneList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(584, 405);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.boxList);
            this.Name = "frmZoneList";
            this.ShowIcon = false;
            this.Text = "ZoneList";
            this.Load += new System.EventHandler(this.frmZoneList_Load);
            this.boxList.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox boxList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listZones;
        private System.Windows.Forms.Label lblAlias;
        private System.Windows.Forms.TextBox alias;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPlay;
    }
}