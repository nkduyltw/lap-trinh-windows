namespace MC
{
    partial class MCChatRoom
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
            this.btnsend.Location = new System.Drawing.Point(197, 279);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(58, 41);
            this.btnsend.TabIndex = 8;
            this.btnsend.Text = "Send";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // txtinput
            // 
            this.txtinput.Location = new System.Drawing.Point(8, 279);
            this.txtinput.Multiline = true;
            this.txtinput.Name = "txtinput";
            this.txtinput.Size = new System.Drawing.Size(181, 41);
            this.txtinput.TabIndex = 7;
            this.txtinput.TextChanged += new System.EventHandler(this.txtinput_TextChanged);
            // 
            // lvchat
            // 
            this.lvchat.Location = new System.Drawing.Point(8, 3);
            this.lvchat.Name = "lvchat";
            this.lvchat.Size = new System.Drawing.Size(247, 270);
            this.lvchat.TabIndex = 6;
            this.lvchat.UseCompatibleStateImageBehavior = false;
            this.lvchat.View = System.Windows.Forms.View.List;
            // 
            // MCChatRoom
            // 
            this.AcceptButton = this.btnsend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 323);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.txtinput);
            this.Controls.Add(this.lvchat);
            this.Location = new System.Drawing.Point(1028, 345);
            this.Name = "MCChatRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MCChatRoom";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MCChatRoom_FormClosed);
            this.Load += new System.EventHandler(this.MCChatRoom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.TextBox txtinput;
        private System.Windows.Forms.ListView lvchat;
    }
}