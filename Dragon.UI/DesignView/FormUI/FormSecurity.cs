using Dragon.Controller.GlobalControl.Security;
using System.Diagnostics;

namespace Dragon
{
    public partial class FormSecurity : Form
    {
        string Result = string.Empty;
        string TestString = "Hello World";
        public FormSecurity()
        {
            SetStyle(
              
              ControlStyles.DoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();


            InitializeComponent();

            Result = $"ABC";
            label2.Text = Result;
            label1.Text = getResult();

            //label1.Text = DecodeStringV1.EncodeV1("Hello World");

            string Direct = Directory.GetCurrentDirectory() + "\\Dragon.dll";
            HashFileV1.Text = Direct;
            label1.Text = TestString;
            if (File.Exists(Direct))
            {
                //var valueget = GetSettings.GetHash();
                //var test = HashValue.ComputeFileSHA256(Direct);
                //if (string.IsNullOrEmpty(valueget))
                //{
                //    GetSettings.SetHash(test);
                //}
                //else
                //{
                //    //var encodev1 = DecodeStringV1.EncodeV1(test);
                //    var check = DecodeStringV1.EncodeV1(valueget);
                //    if (check == DecodeStringV1.EncodeV1(test))
                //    {
                //       // MessageBox.Show("HashValue is correct!");
                //       Debug.WriteLine("HashValue is correct!");
                //    }
                //}            
            }
        }

        public string getResult()
        {
            return "MaxMad";
        }

        public class TestClass
        {
            public static string MyString = string.Empty;

        }

        private void buttonEncodeV1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(encodeV1.Text))
            {
                encodeV1Result.Text = DecodeStringV1.EncodeV1(encodeV1.Text);
            }

        }

        private void buttonDecodeV1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(decodeV1.Text))
            {
                decodeV1Result.Text = DecodeStringV1.DecodeV1(decodeV1.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(encodeV2.Text))
            {
                encodeV2Result.Text = DecodeStringV2.EncodeV2(encodeV2.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(decodeV2.Text))
            {
                decodeV2Result.Text = DecodeStringV2.DecodeV2(decodeV2.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var pair = RsaKeyPair.Generate();
            //publicKey.Text = pair.PublicKeyBase64;
            //PrivateKey.Text = pair.PrivateKeyBase64;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(encodeRSA.Text))
            {
                MessageBox.Show("Vui lòng nhập chuỗi cần mã hoá.");
                return;
            }
            if (string.IsNullOrEmpty(publicKey.Text))
            {
                MessageBox.Show("Vui lòng nhập public key.");
                return;
            }

            //encodeRSAResult.Text = RsaKeyPair.Encrypt(encodeRSA.Text, publicKey.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(decodeRSA.Text))
            {
                MessageBox.Show("Vui lòng nhập chuỗi cần mã hoá.");
                return;
            }

            if (string.IsNullOrEmpty(PrivateKey.Text))
            {
                MessageBox.Show("Vui lòng nhập PrivateKey key.");
                return;
            }

            //decodeRSAResult.Text = RsaKeyPair.Decrypt(decodeRSA.Text, PrivateKey.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HashString.Text))
            {
                MessageBox.Show("Vui lòng nhập HashString ");
                return;
            }

            HashStringResult.Text = HashValue.ComputeStringSHA256(HashString.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HashFileV1.Text))
            {
                MessageBox.Show("Vui lòng nhập HashString ");
                return;
            }
            if (!File.Exists(HashFileV1.Text))
            {
                MessageBox.Show("File không tồn tại.");
                return;
            }
            HashFileV1Result.Text = HashValue.ComputeFileSHA256(HashFileV1.Text);
        }
    }

}
