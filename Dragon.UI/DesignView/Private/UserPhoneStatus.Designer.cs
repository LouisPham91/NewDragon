namespace Dragon.DesignView.Private
{
    partial class UserPhoneStatus
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelSerialOrModel = new Label();
            labelDragonStatus = new Label();
            labelTagNumber = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // labelSerialOrModel
            // 
            labelSerialOrModel.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelSerialOrModel.Location = new Point(3, 192);
            labelSerialOrModel.Name = "labelSerialOrModel";
            labelSerialOrModel.Size = new Size(534, 45);
            labelSerialOrModel.TabIndex = 4;
            labelSerialOrModel.Text = "ce061716c02cbc740d7e";
            labelSerialOrModel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelDragonStatus
            // 
            labelDragonStatus.Font = new Font("Arial", 28F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelDragonStatus.Location = new Point(3, 627);
            labelDragonStatus.Name = "labelDragonStatus";
            labelDragonStatus.Size = new Size(534, 45);
            labelDragonStatus.TabIndex = 14;
            labelDragonStatus.Text = "Passionately Loading...";
            labelDragonStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTagNumber
            // 
            labelTagNumber.Font = new Font("Arial", 60F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTagNumber.Location = new Point(149, 85);
            labelTagNumber.Name = "labelTagNumber";
            labelTagNumber.Size = new Size(242, 93);
            labelTagNumber.TabIndex = 16;
            labelTagNumber.Text = "9999";
            labelTagNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(160, 374);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 220);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            // 
            // UserPhoneStatus
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pictureBox1);
            Controls.Add(labelTagNumber);
            Controls.Add(labelDragonStatus);
            Controls.Add(labelSerialOrModel);
            Name = "UserPhoneStatus";
            Size = new Size(540, 960);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label labelSerialOrModel;
        private Label labelDragonStatus;
        private Label labelTagNumber;
        private PictureBox pictureBox1;
    }
}
