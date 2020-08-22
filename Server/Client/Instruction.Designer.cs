namespace Client
{
    partial class Instruction
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
            this.lvHuongDan = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvHuongDan
            // 
            this.lvHuongDan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHuongDan.Location = new System.Drawing.Point(0, 0);
            this.lvHuongDan.Name = "lvHuongDan";
            this.lvHuongDan.Size = new System.Drawing.Size(691, 397);
            this.lvHuongDan.TabIndex = 0;
            this.lvHuongDan.UseCompatibleStateImageBehavior = false;
            this.lvHuongDan.View = System.Windows.Forms.View.List;
            // 
            // Instruction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 397);
            this.Controls.Add(this.lvHuongDan);
            this.Name = "Instruction";
            this.Text = "Instruction";
            this.Load += new System.EventHandler(this.Instruction_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvHuongDan;
    }
}