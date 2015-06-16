namespace UnoProLyzer
{
    partial class DebugConsole
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
            this.rt_debug_text = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rt_debug_text
            // 
            this.rt_debug_text.Location = new System.Drawing.Point(41, 145);
            this.rt_debug_text.Name = "rt_debug_text";
            this.rt_debug_text.Size = new System.Drawing.Size(671, 96);
            this.rt_debug_text.TabIndex = 0;
            this.rt_debug_text.Text = "";
            // 
            // DebugConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 281);
            this.Controls.Add(this.rt_debug_text);
            this.Name = "DebugConsole";
            this.Text = "DebugConsole";
            this.Load += new System.EventHandler(this.DebugConsole_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rt_debug_text;
    }
}