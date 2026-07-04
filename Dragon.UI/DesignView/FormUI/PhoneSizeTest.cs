using Dragon.Controller.Database.Services;
using Dragon.DesignView.Private;
using Dragon.DesignView.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dragon.DesignView.FormUI
{
    public partial class PhoneSizeTest : Form
    {
        public PhoneSizeTest()
        {
            InitializeComponent();
            ThemeHelper.SetTheme(ThemeMode.Dark, ThemeStyle.ThemeRed);
            var phones = PhoneRepository.LoadAll();
            UserPhoneStatus userPhoneStatus = new UserPhoneStatus(phones.First().DeviceID, ControlHelper.PhoneStatus.Resizing);
            userPhoneStatus.Dock = DockStyle.Fill;
            this.Controls.Add(userPhoneStatus);
        }
    }
}
