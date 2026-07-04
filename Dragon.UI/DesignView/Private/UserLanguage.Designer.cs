
using Dragon.Controller.GlobalControl.Property;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    partial class UserLanguage
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
            if (disposing)
            {
                // 🔥 SỬA: Unsubscribe đúng cách
                

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            radioEnglish = new RadioButton();
            label1 = new Label();
            radioTiengViet = new RadioButton();
            SuspendLayout();
            // 
            // radioEnglish
            // 
            radioEnglish.AutoSize = true;
            radioEnglish.ForeColor = Color.White;
            radioEnglish.Location = new Point(28, 65);
            radioEnglish.Name = "radioEnglish";
            radioEnglish.Size = new Size(63, 19);
            radioEnglish.TabIndex = 0;
            radioEnglish.TabStop = true;
            radioEnglish.Text = "English";
            radioEnglish.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(28, 28);
            label1.Name = "label1";
            label1.Size = new Size(108, 15);
            label1.TabIndex = 2;
            label1.Text = "Choose Language :";
            // 
            // radioTiengViet
            // 
            radioTiengViet.AutoSize = true;
            radioTiengViet.ForeColor = Color.White;
            radioTiengViet.Location = new Point(28, 94);
            radioTiengViet.Name = "radioTiengViet";
            radioTiengViet.Size = new Size(77, 19);
            radioTiengViet.TabIndex = 3;
            radioTiengViet.TabStop = true;
            radioTiengViet.Text = "Tiếng Việt";
            radioTiengViet.UseVisualStyleBackColor = true;
            // 
            // UserLanguage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            Controls.Add(radioTiengViet);
            Controls.Add(label1);
            Controls.Add(radioEnglish);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UserLanguage";
            Size = new Size(585, 543);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton radioEnglish;
        private Label label1;
        private RadioButton radioTiengViet;
    }
}