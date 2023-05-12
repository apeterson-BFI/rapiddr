namespace RapidClient
{
    partial class ClientForm
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
            this.txtInputBox = new System.Windows.Forms.TextBox();
            this.roomPanel = new System.Windows.Forms.Panel();
            this.skillsPanel = new System.Windows.Forms.Panel();
            this.equipPanel = new System.Windows.Forms.Panel();
            this.systemMainPanel = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.systemMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInputBox
            // 
            this.txtInputBox.BackColor = System.Drawing.SystemColors.Control;
            this.txtInputBox.Location = new System.Drawing.Point(438, 749);
            this.txtInputBox.Name = "txtInputBox";
            this.txtInputBox.Size = new System.Drawing.Size(1693, 39);
            this.txtInputBox.TabIndex = 0;
            // 
            // roomPanel
            // 
            this.roomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.roomPanel.Location = new System.Drawing.Point(12, 12);
            this.roomPanel.Name = "roomPanel";
            this.roomPanel.Size = new System.Drawing.Size(400, 268);
            this.roomPanel.TabIndex = 1;
            // 
            // skillsPanel
            // 
            this.skillsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.skillsPanel.Location = new System.Drawing.Point(12, 310);
            this.skillsPanel.Name = "skillsPanel";
            this.skillsPanel.Size = new System.Drawing.Size(400, 252);
            this.skillsPanel.TabIndex = 2;
            // 
            // equipPanel
            // 
            this.equipPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipPanel.Location = new System.Drawing.Point(12, 588);
            this.equipPanel.Name = "equipPanel";
            this.equipPanel.Size = new System.Drawing.Size(400, 200);
            this.equipPanel.TabIndex = 3;
            // 
            // systemMainPanel
            // 
            this.systemMainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemMainPanel.Controls.Add(this.richTextBox1);
            this.systemMainPanel.Location = new System.Drawing.Point(438, 12);
            this.systemMainPanel.Name = "systemMainPanel";
            this.systemMainPanel.Size = new System.Drawing.Size(1693, 710);
            this.systemMainPanel.TabIndex = 4;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(13, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1675, 702);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2155, 819);
            this.Controls.Add(this.systemMainPanel);
            this.Controls.Add(this.equipPanel);
            this.Controls.Add(this.skillsPanel);
            this.Controls.Add(this.roomPanel);
            this.Controls.Add(this.txtInputBox);
            this.KeyPreview = true;
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ClientForm_KeyPress);
            this.systemMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtInputBox;
        private Panel roomPanel;
        private Panel skillsPanel;
        private Panel equipPanel;
        private Panel systemMainPanel;
        private RichTextBox richTextBox1;
    }
}