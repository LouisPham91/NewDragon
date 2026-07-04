using AntdUI;
using CountryData.Globalization.Services;
using Dragon.Controller.Firebase.Model;
using Dragon.Controller.GlobalControl.Security;
using Dragon.Controller.Server.Base;
using Dragon.Controller.Server.Model;
using Dragon.Controller.Server.Services;
using Dragon.DesignView.Public;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class FormRegisterUser : Form
    {
        string valueMaHoa = string.Empty;
        GoogleAccount? googleAccount = null;
        private CountryDataProvider provider;

        public FormRegisterUser(string ValueMaHoa, GoogleAccount GoogleAccount)
        {
            InitializeComponent();
            valueMaHoa = ValueMaHoa;
            googleAccount = GoogleAccount;
            labelName.Text = googleAccount.displayName;
            labelEmail.Text = googleAccount.email;
            provider = new CountryDataProvider();
            
        }

        private void FormRegisterUser_Load(object sender, EventArgs e)
        {
            var countries = provider.GetAllCountries().ToList();
            comboxCountry.DataSource = countries;
            comboxCountry.DisplayMember = "CountryName";
            comboxCountry.ValueMember = "CountryShortCode";
            comboxCountry.SelectedIndex = -1;
            comboxCountry.SelectedIndexChanged += ComboxCountry_SelectedIndexChanged;
        }

        private void ComboxCountry_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboxCountry.SelectedValue is string countryCode)
            {
                var regions = provider.GetRegionsByCountryCode(countryCode).ToList();
                comboxCity.DataSource = regions;
                comboxCity.DisplayMember = "Name";
                comboxCity.ValueMember = "ShortCode";
                comboxCity.SelectedIndex = -1;
            }
        }

        private async void buttonRegister_Click(object sender, EventArgs e)
        {
            string name = labelName.Text;
            string email = labelEmail.Text;
            string phone = txtPhone.Text;
            string countryCode = comboxCountry.SelectedValue?.ToString() ?? "";
            string cityCode = comboxCity.SelectedValue?.ToString() ?? "";
            string address = txtAdress.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(countryCode) ||
                string.IsNullOrWhiteSpace(cityCode) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please fill in all required information!");
                return;
            }

            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Invalid phone number!");
                return;
            }

            // Server mới chỉ cần email + name (mã hóa 1 chiều)
            var post = await HttpServices.PostJsonAsync(
                new EncryptedPayload2value
                {
                    Data = await RsaKeyPair.MaHoa(email),
                    Data2 = await RsaKeyPair.MaHoa(name)
                },
                JsonServer.Default.EncryptedPayload2value,
                "/api/User/register");

            if (post == null)
            {
                MessageBox.Show("Server maintenance");
                return;
            }

            if (post.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var userJson = await post.Content.ReadAsStringAsync();
                // server trả plain JSON user
                StaticValue.userHash = userJson;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string reason = await post.Content.ReadAsStringAsync();
                MessageBox.Show($"{post.StatusCode}: {reason}");
            }
        }

        private static readonly Regex PhoneRegex = new Regex(@"^[0-9]{7,15}$", RegexOptions.CultureInvariant);
        private bool IsValidPhone(string phone) => PhoneRegex.IsMatch(phone);
    }
}