namespace FormsTalker
{
    partial class Form1
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.InputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(260, 165);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // InputRichTextBox
            // 
            this.InputRichTextBox.Location = new System.Drawing.Point(12, 183);
            this.InputRichTextBox.Name = "InputRichTextBox";
            this.InputRichTextBox.Size = new System.Drawing.Size(260, 37);
            this.InputRichTextBox.TabIndex = 1;
            this.InputRichTextBox.Text = "";
            this.InputRichTextBox.TextChanged += new System.EventHandler(this.InputRichTextBox_TextChanged);
            this.InputRichTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InputRichTextBox_KeyUp);
            this.InputRichTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.InputRichTextBox_PreviewKeyDown);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(197, 226);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputRichTextBox);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Talker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox InputRichTextBox;
        private System.Windows.Forms.Button SendButton;
    }
}

