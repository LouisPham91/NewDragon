namespace Dragon
{
    partial class FormSecurity
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            label1 = new Label();
            buttonEncodeV1 = new Button();
            encodeV1 = new TextBox();
            buttonDecodeV1 = new Button();
            decodeV1 = new TextBox();
            decodeV2 = new TextBox();
            button3 = new Button();
            encodeV2 = new TextBox();
            button4 = new Button();
            button5 = new Button();
            HashString = new TextBox();
            encodeV1Result = new TextBox();
            decodeV1Result = new TextBox();
            encodeV2Result = new TextBox();
            decodeV2Result = new TextBox();
            HashStringResult = new TextBox();
            publicKey = new TextBox();
            PrivateKey = new TextBox();
            button1 = new Button();
            label3 = new Label();
            label4 = new Label();
            encodeRSA = new TextBox();
            encodeRSAResult = new TextBox();
            button2 = new Button();
            decodeRSA = new TextBox();
            decodeRSAResult = new TextBox();
            button6 = new Button();
            HashFileV1Result = new TextBox();
            HashFileV1 = new TextBox();
            button7 = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(427, 7);
            label2.Name = "label2";
            label2.Size = new Size(149, 25);
            label2.TabIndex = 1;
            label2.Text = "Test Protect AOT";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(262, 7);
            label1.Name = "label1";
            label1.Size = new Size(149, 25);
            label1.TabIndex = 0;
            label1.Text = "Test Protect AOT";
            // 
            // buttonEncodeV1
            // 
            buttonEncodeV1.Location = new Point(10, 44);
            buttonEncodeV1.Margin = new Padding(3, 2, 3, 2);
            buttonEncodeV1.Name = "buttonEncodeV1";
            buttonEncodeV1.Size = new Size(166, 22);
            buttonEncodeV1.TabIndex = 2;
            buttonEncodeV1.Text = "EnCode V1";
            buttonEncodeV1.UseVisualStyleBackColor = true;
            buttonEncodeV1.Click += buttonEncodeV1_Click;
            // 
            // encodeV1
            // 
            encodeV1.Location = new Point(193, 44);
            encodeV1.Margin = new Padding(3, 2, 3, 2);
            encodeV1.Name = "encodeV1";
            encodeV1.Size = new Size(202, 23);
            encodeV1.TabIndex = 3;
            // 
            // buttonDecodeV1
            // 
            buttonDecodeV1.Location = new Point(10, 70);
            buttonDecodeV1.Margin = new Padding(3, 2, 3, 2);
            buttonDecodeV1.Name = "buttonDecodeV1";
            buttonDecodeV1.Size = new Size(166, 22);
            buttonDecodeV1.TabIndex = 4;
            buttonDecodeV1.Text = "DeCode V1";
            buttonDecodeV1.UseVisualStyleBackColor = true;
            buttonDecodeV1.Click += buttonDecodeV1_Click;
            // 
            // decodeV1
            // 
            decodeV1.Location = new Point(193, 71);
            decodeV1.Margin = new Padding(3, 2, 3, 2);
            decodeV1.Name = "decodeV1";
            decodeV1.Size = new Size(202, 23);
            decodeV1.TabIndex = 5;
            // 
            // decodeV2
            // 
            decodeV2.Location = new Point(193, 133);
            decodeV2.Margin = new Padding(3, 2, 3, 2);
            decodeV2.Name = "decodeV2";
            decodeV2.Size = new Size(202, 23);
            decodeV2.TabIndex = 9;
            // 
            // button3
            // 
            button3.Location = new Point(10, 131);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(166, 22);
            button3.TabIndex = 8;
            button3.Text = "DeCode V2";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // encodeV2
            // 
            encodeV2.Location = new Point(193, 105);
            encodeV2.Margin = new Padding(3, 2, 3, 2);
            encodeV2.Name = "encodeV2";
            encodeV2.Size = new Size(202, 23);
            encodeV2.TabIndex = 7;
            // 
            // button4
            // 
            button4.Location = new Point(10, 105);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(166, 22);
            button4.TabIndex = 6;
            button4.Text = "EnCode V2";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(10, 364);
            button5.Margin = new Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new Size(166, 22);
            button5.TabIndex = 10;
            button5.Text = "HashSHA256 String V1";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // HashString
            // 
            HashString.Location = new Point(193, 364);
            HashString.Margin = new Padding(3, 2, 3, 2);
            HashString.Name = "HashString";
            HashString.Size = new Size(202, 23);
            HashString.TabIndex = 11;
            // 
            // encodeV1Result
            // 
            encodeV1Result.Location = new Point(400, 44);
            encodeV1Result.Margin = new Padding(3, 2, 3, 2);
            encodeV1Result.Name = "encodeV1Result";
            encodeV1Result.Size = new Size(583, 23);
            encodeV1Result.TabIndex = 12;
            // 
            // decodeV1Result
            // 
            decodeV1Result.Location = new Point(400, 71);
            decodeV1Result.Margin = new Padding(3, 2, 3, 2);
            decodeV1Result.Name = "decodeV1Result";
            decodeV1Result.Size = new Size(583, 23);
            decodeV1Result.TabIndex = 13;
            // 
            // encodeV2Result
            // 
            encodeV2Result.Location = new Point(400, 105);
            encodeV2Result.Margin = new Padding(3, 2, 3, 2);
            encodeV2Result.Name = "encodeV2Result";
            encodeV2Result.Size = new Size(583, 23);
            encodeV2Result.TabIndex = 14;
            // 
            // decodeV2Result
            // 
            decodeV2Result.Location = new Point(400, 133);
            decodeV2Result.Margin = new Padding(3, 2, 3, 2);
            decodeV2Result.Name = "decodeV2Result";
            decodeV2Result.Size = new Size(583, 23);
            decodeV2Result.TabIndex = 15;
            // 
            // HashStringResult
            // 
            HashStringResult.Location = new Point(400, 364);
            HashStringResult.Margin = new Padding(3, 2, 3, 2);
            HashStringResult.Name = "HashStringResult";
            HashStringResult.Size = new Size(583, 23);
            HashStringResult.TabIndex = 16;
            // 
            // publicKey
            // 
            publicKey.Location = new Point(230, 186);
            publicKey.Margin = new Padding(3, 2, 3, 2);
            publicKey.Name = "publicKey";
            publicKey.Size = new Size(284, 23);
            publicKey.TabIndex = 19;
            // 
            // PrivateKey
            // 
            PrivateKey.Location = new Point(595, 186);
            PrivateKey.Margin = new Padding(3, 2, 3, 2);
            PrivateKey.Name = "PrivateKey";
            PrivateKey.Size = new Size(388, 23);
            PrivateKey.TabIndex = 18;
            // 
            // button1
            // 
            button1.Location = new Point(10, 186);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(133, 22);
            button1.TabIndex = 17;
            button1.Text = "Create";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(162, 188);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 20;
            label3.Text = "publickey";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(522, 188);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 21;
            label4.Text = "privatekey";
            // 
            // encodeRSA
            // 
            encodeRSA.Location = new Point(193, 218);
            encodeRSA.Margin = new Padding(3, 2, 3, 2);
            encodeRSA.Name = "encodeRSA";
            encodeRSA.Size = new Size(202, 23);
            encodeRSA.TabIndex = 24;
            // 
            // encodeRSAResult
            // 
            encodeRSAResult.Location = new Point(400, 218);
            encodeRSAResult.Margin = new Padding(3, 2, 3, 2);
            encodeRSAResult.Name = "encodeRSAResult";
            encodeRSAResult.Size = new Size(583, 23);
            encodeRSAResult.TabIndex = 23;
            // 
            // button2
            // 
            button2.Location = new Point(10, 218);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(133, 22);
            button2.TabIndex = 22;
            button2.Text = "EnCode RSA";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // decodeRSA
            // 
            decodeRSA.Location = new Point(193, 243);
            decodeRSA.Margin = new Padding(3, 2, 3, 2);
            decodeRSA.Name = "decodeRSA";
            decodeRSA.Size = new Size(202, 23);
            decodeRSA.TabIndex = 27;
            // 
            // decodeRSAResult
            // 
            decodeRSAResult.Location = new Point(400, 242);
            decodeRSAResult.Margin = new Padding(3, 2, 3, 2);
            decodeRSAResult.Name = "decodeRSAResult";
            decodeRSAResult.Size = new Size(583, 23);
            decodeRSAResult.TabIndex = 26;
            // 
            // button6
            // 
            button6.Location = new Point(10, 242);
            button6.Margin = new Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new Size(133, 22);
            button6.TabIndex = 25;
            button6.Text = "DeCode RSA";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // HashFileV1Result
            // 
            HashFileV1Result.Location = new Point(400, 305);
            HashFileV1Result.Margin = new Padding(3, 2, 3, 2);
            HashFileV1Result.Name = "HashFileV1Result";
            HashFileV1Result.Size = new Size(583, 23);
            HashFileV1Result.TabIndex = 30;
            // 
            // HashFileV1
            // 
            HashFileV1.Location = new Point(193, 305);
            HashFileV1.Margin = new Padding(3, 2, 3, 2);
            HashFileV1.Name = "HashFileV1";
            HashFileV1.Size = new Size(202, 23);
            HashFileV1.TabIndex = 29;
            // 
            // button7
            // 
            button7.Location = new Point(10, 305);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(166, 22);
            button7.TabIndex = 28;
            button7.Text = "HashSHA256 File V1";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(31, 31, 31);
            ClientSize = new Size(994, 440);
            Controls.Add(HashFileV1Result);
            Controls.Add(HashFileV1);
            Controls.Add(button7);
            Controls.Add(decodeRSA);
            Controls.Add(decodeRSAResult);
            Controls.Add(button6);
            Controls.Add(encodeRSA);
            Controls.Add(encodeRSAResult);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(publicKey);
            Controls.Add(PrivateKey);
            Controls.Add(button1);
            Controls.Add(HashStringResult);
            Controls.Add(decodeV2Result);
            Controls.Add(encodeV2Result);
            Controls.Add(decodeV1Result);
            Controls.Add(encodeV1Result);
            Controls.Add(HashString);
            Controls.Add(button5);
            Controls.Add(decodeV2);
            Controls.Add(button3);
            Controls.Add(encodeV2);
            Controls.Add(button4);
            Controls.Add(decodeV1);
            Controls.Add(buttonDecodeV1);
            Controls.Add(encodeV1);
            Controls.Add(buttonEncodeV1);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Button buttonEncodeV1;
        private TextBox encodeV1;
        private Button buttonDecodeV1;
        private TextBox decodeV1;
        private TextBox decodeV2;
        private Button button3;
        private TextBox encodeV2;
        private Button button4;
        private Button button5;
        private TextBox HashString;
        private TextBox encodeV1Result;
        private TextBox decodeV1Result;
        private TextBox encodeV2Result;
        private TextBox decodeV2Result;
        private TextBox HashStringResult;
        private TextBox publicKey;
        private TextBox PrivateKey;
        private Button button1;
        private Label label3;
        private Label label4;
        private TextBox encodeRSA;
        private TextBox encodeRSAResult;
        private Button button2;
        private TextBox decodeRSA;
        private TextBox decodeRSAResult;
        private Button button6;
        private TextBox HashFileV1Result;
        private TextBox HashFileV1;
        private Button button7;
    }
}
