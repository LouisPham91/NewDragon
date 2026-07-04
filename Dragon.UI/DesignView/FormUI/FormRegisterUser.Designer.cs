namespace Dragon.DesignView.FormUI
{
    partial class FormRegisterUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegisterUser));
            A = new AntdUI.Button();
            comboxCountry = new ComboBox();
            comboxGender = new ComboBox();
            txtAdress = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label5 = new Label();
            label4 = new Label();
            txtPhone = new TextBox();
            label3 = new Label();
            labelName = new Label();
            label2 = new Label();
            label1 = new Label();
            labelEmail = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label8 = new Label();
            comboxCity = new ComboBox();
            dateOfBirth = new DateTimePicker();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // A
            // 
            A.BadgeSvg = "";
            A.DefaultBack = Color.Gainsboro;
            A.ForeColor = Color.Black;
            A.IconGap = 0.7F;
            A.IconRatio = 1.2F;
            A.IconSvg = resources.GetString("A.IconSvg");
            A.Location = new Point(165, 422);
            A.Name = "A";
            A.Size = new Size(126, 40);
            A.TabIndex = 4;
            A.Text = "Register";
            A.Click += buttonRegister_Click;
            // 
            // comboxCountry
            // 
            comboxCountry.FormattingEnabled = true;
            comboxCountry.Location = new Point(114, 248);
            comboxCountry.Name = "comboxCountry";
            comboxCountry.Size = new Size(200, 23);
            comboxCountry.TabIndex = 15;
            // 
            // comboxGender
            // 
            comboxGender.FormattingEnabled = true;
            comboxGender.Items.AddRange(new object[] { "Male", "Female" });
            comboxGender.Location = new Point(114, 209);
            comboxGender.Name = "comboxGender";
            comboxGender.Size = new Size(200, 23);
            comboxGender.TabIndex = 14;
            // 
            // txtAdress
            // 
            txtAdress.Location = new Point(114, 327);
            txtAdress.Margin = new Padding(3, 4, 3, 3);
            txtAdress.Multiline = true;
            txtAdress.Name = "txtAdress";
            txtAdress.Size = new Size(282, 23);
            txtAdress.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(44, 330);
            label6.Margin = new Padding(5, 7, 3, 0);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 11;
            label6.Text = "Adress";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(44, 252);
            label7.Margin = new Padding(5, 7, 3, 0);
            label7.Name = "label7";
            label7.Size = new Size(50, 15);
            label7.TabIndex = 12;
            label7.Text = "Country";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(44, 213);
            label5.Margin = new Padding(5, 7, 3, 0);
            label5.Name = "label5";
            label5.Size = new Size(45, 15);
            label5.TabIndex = 10;
            label5.Text = "Gender";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(44, 174);
            label4.Margin = new Padding(5, 7, 3, 0);
            label4.Name = "label4";
            label4.Size = new Size(51, 15);
            label4.TabIndex = 8;
            label4.Text = "Birthday";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(114, 132);
            txtPhone.Margin = new Padding(3, 4, 3, 3);
            txtPhone.Multiline = true;
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(200, 23);
            txtPhone.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(44, 135);
            label3.Margin = new Padding(5, 7, 3, 0);
            label3.Name = "label3";
            label3.Size = new Size(41, 15);
            label3.TabIndex = 6;
            label3.Text = "Phone";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelName.Location = new Point(116, 57);
            labelName.Margin = new Padding(5, 7, 3, 0);
            labelName.Name = "labelName";
            labelName.Size = new Size(67, 15);
            labelName.TabIndex = 2;
            labelName.Text = "Pham Nam";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(44, 96);
            label2.Margin = new Padding(5, 7, 3, 0);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 1;
            label2.Text = "Email";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(44, 57);
            label1.Margin = new Padding(5, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelEmail.Location = new Point(116, 96);
            labelEmail.Margin = new Padding(5, 7, 3, 0);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(108, 15);
            labelEmail.TabIndex = 5;
            labelEmail.Text = "Tuoigi@gmail.com";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.683067F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.909091F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.1818161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7.091172F));
            tableLayoutPanel1.Controls.Add(labelEmail, 2, 2);
            tableLayoutPanel1.Controls.Add(label1, 1, 1);
            tableLayoutPanel1.Controls.Add(label2, 1, 2);
            tableLayoutPanel1.Controls.Add(labelName, 2, 1);
            tableLayoutPanel1.Controls.Add(label3, 1, 3);
            tableLayoutPanel1.Controls.Add(label4, 1, 4);
            tableLayoutPanel1.Controls.Add(label5, 1, 5);
            tableLayoutPanel1.Controls.Add(label7, 1, 6);
            tableLayoutPanel1.Controls.Add(comboxGender, 2, 5);
            tableLayoutPanel1.Controls.Add(comboxCountry, 2, 6);
            tableLayoutPanel1.Controls.Add(txtAdress, 2, 8);
            tableLayoutPanel1.Controls.Add(label6, 1, 8);
            tableLayoutPanel1.Controls.Add(label8, 1, 7);
            tableLayoutPanel1.Controls.Add(txtPhone, 2, 3);
            tableLayoutPanel1.Controls.Add(comboxCity, 2, 7);
            tableLayoutPanel1.Controls.Add(dateOfBirth, 2, 4);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 12;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(455, 408);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(44, 291);
            label8.Margin = new Padding(5, 7, 3, 0);
            label8.Name = "label8";
            label8.Size = new Size(28, 15);
            label8.TabIndex = 16;
            label8.Text = "City";
            // 
            // comboxCity
            // 
            comboxCity.FormattingEnabled = true;
            comboxCity.Location = new Point(114, 287);
            comboxCity.Name = "comboxCity";
            comboxCity.Size = new Size(200, 23);
            comboxCity.TabIndex = 17;
            // 
            // dateOfBirth
            // 
            dateOfBirth.CustomFormat = "dd/MM/yyyy";
            dateOfBirth.Format = DateTimePickerFormat.Custom;
            dateOfBirth.Location = new Point(114, 171);
            dateOfBirth.Margin = new Padding(3, 4, 3, 3);
            dateOfBirth.Name = "dateOfBirth";
            dateOfBirth.Size = new Size(200, 23);
            dateOfBirth.TabIndex = 18;
            // 
            // FormRegisterUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 250, 250);
            ClientSize = new Size(455, 498);
            Controls.Add(A);
            Controls.Add(tableLayoutPanel1);
            Name = "FormRegisterUser";
            Text = "FormRegisterUser";
            Load += FormRegisterUser_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Button A;
        private ComboBox comboxCountry;
        private ComboBox comboxGender;
        private TextBox txtAdress;
        private Label label6;
        private Label label7;
        private Label label5;
        private Label label4;
        private TextBox txtPhone;
        private Label label3;
        private Label labelName;
        private Label label2;
        private Label label1;
        private Label labelEmail;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label8;
        private ComboBox comboxCity;
        private DateTimePicker dateOfBirth;
    }
}