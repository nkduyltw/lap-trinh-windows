namespace Client
{
    partial class ChatRoom
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
            this.btnsend = new System.Windows.Forms.Button();
            this.txtinput = new System.Windows.Forms.TextBox();
            this.lvchat = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(196, 279);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(57, 38);
            this.btnsend.TabIndex = 5;
            this.btnsend.Text = "Send";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // txtinput
            // 
            this.txtinput.Location = new System.Drawing.Point(1, 279);
            this.txtinput.Multiline = true;
            this.txtinput.Name = "txtinput";
            this.txtinput.Size = new System.Drawing.Size(189, 38);
            this.txtinput.TabIndex = 4;
            // 
            // lvchat
            // 
            this.lvchat.Location = new System.Drawing.Point(1, 3);
            this.lvchat.Name = "lvchat";
            this.lvchat.Size = new System.Drawing.Size(252, 270);
            this.lvchat.TabIndex = 3;
            this.lvchat.UseCompatibleStateImageBehavior = false;
            this.lvchat.View = System.Windows.Forms.View.List;
            // 
            // ChatRoom
            // 
            this.AcceptButton = this.btnsend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 319);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.txtinput);
            this.Controls.Add(this.lvchat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(1028, 345);
            this.Name = "ChatRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ChatRoom";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatRoom_FormClosed);
            this.Load += new System.EventHandler(this.ChatRoom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.TextBox txtinput;
        private System.Windows.Forms.ListView lvchat;
    }
}