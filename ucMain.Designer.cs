
namespace Mess
{
    partial class ucMain
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucMain));
            this.panelBar = new System.Windows.Forms.Panel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picBox2 = new System.Windows.Forms.PictureBox();
            this.lblUsersCount = new System.Windows.Forms.Label();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.listUsers = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.fileTransferLabel = new System.Windows.Forms.Label();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.sendFile = new MaterialSkin.Controls.MaterialRaisedButton();
            this.addFile = new MaterialSkin.Controls.MaterialRaisedButton();
            this.fileIcon = new System.Windows.Forms.PictureBox();
            this.filePath = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.panelBar.SuspendLayout();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBar
            // 
            this.panelBar.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panelBar.Controls.Add(this.panelMenu);
            this.panelBar.Controls.Add(this.lblUsersCount);
            this.panelBar.Controls.Add(this.picStatus);
            this.panelBar.Controls.Add(this.lblUsername);
            this.panelBar.Location = new System.Drawing.Point(0, 551);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(330, 40);
            this.panelBar.TabIndex = 5;
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.pictureBox1);
            this.panelMenu.Controls.Add(this.picBox2);
            this.panelMenu.Location = new System.Drawing.Point(20, 40);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(40, 0);
            this.panelMenu.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // picBox2
            // 
            this.picBox2.Image = ((System.Drawing.Image)(resources.GetObject("picBox2.Image")));
            this.picBox2.Location = new System.Drawing.Point(0, 0);
            this.picBox2.Name = "picBox2";
            this.picBox2.Size = new System.Drawing.Size(40, 40);
            this.picBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBox2.TabIndex = 8;
            this.picBox2.TabStop = false;
            // 
            // lblUsersCount
            // 
            this.lblUsersCount.AutoSize = true;
            this.lblUsersCount.Font = new System.Drawing.Font("Lato", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsersCount.ForeColor = System.Drawing.SystemColors.Menu;
            this.lblUsersCount.Location = new System.Drawing.Point(235, 25);
            this.lblUsersCount.Name = "lblUsersCount";
            this.lblUsersCount.Size = new System.Drawing.Size(80, 15);
            this.lblUsersCount.TabIndex = 7;
            this.lblUsersCount.Text = "Online Users: ";
            // 
            // picStatus
            // 
            this.picStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picStatus.Image = ((System.Drawing.Image)(resources.GetObject("picStatus.Image")));
            this.picStatus.Location = new System.Drawing.Point(20, 0);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(40, 40);
            this.picStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picStatus.TabIndex = 6;
            this.picStatus.TabStop = false;
            this.picStatus.Click += new System.EventHandler(this.picStatus_Click);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Lato", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUsername.ForeColor = System.Drawing.SystemColors.Menu;
            this.lblUsername.Location = new System.Drawing.Point(66, 4);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(170, 34);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Default Text";
            // 
            // listUsers
            // 
            this.listUsers.BackColor = System.Drawing.Color.White;
            this.listUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listUsers.Font = new System.Drawing.Font("Lato", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listUsers.ForeColor = System.Drawing.Color.Black;
            this.listUsers.HideSelection = false;
            this.listUsers.Location = new System.Drawing.Point(20, 60);
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(290, 290);
            this.listUsers.TabIndex = 7;
            this.listUsers.UseCompatibleStateImageBehavior = false;
            this.listUsers.View = System.Windows.Forms.View.SmallIcon;
            this.listUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listUsers_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(330, 40);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lato", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(125, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chat";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Controls.Add(this.fileTransferLabel);
            this.panel3.Location = new System.Drawing.Point(0, 371);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(330, 40);
            this.panel3.TabIndex = 9;
            // 
            // fileTransferLabel
            // 
            this.fileTransferLabel.AutoSize = true;
            this.fileTransferLabel.Font = new System.Drawing.Font("Lato", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.fileTransferLabel.ForeColor = System.Drawing.Color.White;
            this.fileTransferLabel.Location = new System.Drawing.Point(74, 2);
            this.fileTransferLabel.Name = "fileTransferLabel";
            this.fileTransferLabel.Size = new System.Drawing.Size(183, 36);
            this.fileTransferLabel.TabIndex = 0;
            this.fileTransferLabel.Text = "File Transfer";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(7, 422);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(53, 24);
            this.materialLabel1.TabIndex = 10;
            this.materialLabel1.Text = "Path:";
            // 
            // sendFile
            // 
            this.sendFile.Depth = 0;
            this.sendFile.Location = new System.Drawing.Point(221, 505);
            this.sendFile.MouseState = MaterialSkin.MouseState.HOVER;
            this.sendFile.Name = "sendFile";
            this.sendFile.Primary = true;
            this.sendFile.Size = new System.Drawing.Size(94, 29);
            this.sendFile.TabIndex = 11;
            this.sendFile.Text = "send file";
            this.sendFile.UseVisualStyleBackColor = true;
            this.sendFile.Click += new System.EventHandler(this.sendFile_Click);
            // 
            // addFile
            // 
            this.addFile.Depth = 0;
            this.addFile.Location = new System.Drawing.Point(7, 505);
            this.addFile.MouseState = MaterialSkin.MouseState.HOVER;
            this.addFile.Name = "addFile";
            this.addFile.Primary = true;
            this.addFile.Size = new System.Drawing.Size(94, 29);
            this.addFile.TabIndex = 13;
            this.addFile.Text = "Add file";
            this.addFile.UseVisualStyleBackColor = true;
            this.addFile.Click += new System.EventHandler(this.addFile_Click);
            // 
            // fileIcon
            // 
            this.fileIcon.Location = new System.Drawing.Point(125, 470);
            this.fileIcon.Name = "fileIcon";
            this.fileIcon.Size = new System.Drawing.Size(64, 64);
            this.fileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fileIcon.TabIndex = 14;
            this.fileIcon.TabStop = false;
            // 
            // filePath
            // 
            this.filePath.Depth = 0;
            this.filePath.Hint = "";
            this.filePath.Location = new System.Drawing.Point(67, 416);
            this.filePath.MouseState = MaterialSkin.MouseState.HOVER;
            this.filePath.Name = "filePath";
            this.filePath.PasswordChar = '\0';
            this.filePath.SelectedText = "";
            this.filePath.SelectionLength = 0;
            this.filePath.SelectionStart = 0;
            this.filePath.Size = new System.Drawing.Size(248, 28);
            this.filePath.TabIndex = 15;
            this.filePath.UseSystemPasswordChar = false;
            // 
            // ucMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.filePath);
            this.Controls.Add(this.fileIcon);
            this.Controls.Add(this.addFile);
            this.Controls.Add(this.sendFile);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listUsers);
            this.Location = new System.Drawing.Point(3, 0);
            this.Name = "ucMain";
            this.Size = new System.Drawing.Size(330, 590);
            this.Load += new System.EventHandler(this.ucMain_Load);
            this.panelBar.ResumeLayout(false);
            this.panelBar.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.PictureBox picStatus;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUsersCount;
        private System.Windows.Forms.ListView listUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label fileTransferLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialRaisedButton sendFile;
        private MaterialSkin.Controls.MaterialRaisedButton addFile;
        private System.Windows.Forms.PictureBox fileIcon;
        private MaterialSkin.Controls.MaterialSingleLineTextField filePath;
    }
}
