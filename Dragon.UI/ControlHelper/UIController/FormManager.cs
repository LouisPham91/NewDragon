using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AntdUI;
using Dragon.ControlHelper.ScrcpyNet.DesignDevice;
using Dragon.ControlHelper.ScrcpyNet.Services;
using Dragon.Controller.Database.Services;
using Dragon.Controller.DeviceControl.ScrcpyNet.Services;
using Dragon.Controller.GlobalControl;
using Dragon.Controller.GlobalControl.Helper;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Database.Models;
using Dragon.DesignView.FormUI;
using Dragon.DesignView.Private;
using Dragon.DesignView.Public;
using Dragon.DesignView.Public.NormalMode;
using TesseractOCR.Font;

namespace Dragon.ControlHelper.UIController
{
    public class FormManager
    {

        private static readonly Lazy<FormManager> instance = new(() => new FormManager { });
        public static FormManager Instance => instance.Value;

        public bool IsDongBo = false;
        public UserPhoneSwitch? _UserPhoneSwitch;
        public FormDeviceControl? formDeviceControl { get; private set; }
        public FormAutoRecord? formAutoRecord { get; private set; }
        public FormActionIntent? formActionIntent { get; private set; }
        public FormSettings? formSettings { get; private set; }
        public VirtualFlowHelper? virtualFlowHelper { get; private set; }
        public FormMain? formMain { get; private set; }
        public FormOTGLoopEditor? formAoaLoopEditor { get; set; }
        public Bitmap dragonArt = new Bitmap(Path.Combine(AppContext.BaseDirectory, "Extension", "Gif", "dragonArt.jpg"));

        // Khởi tạo form chính
        #region ======================= FormMain =======================
        public void LoadFormMain(FormMain formMain)
        {
            this.formMain = formMain;
        }
        public void LoadvirtualFlowHelper(DoubleBufferedFlowLayoutPanel flowLayoutPanel)
        {
            virtualFlowHelper?.Dispose();
            virtualFlowHelper = new VirtualFlowHelper(flowLayoutPanel);
        }
        #endregion 
        #region ======================= UserPhoneSwitch =======================
        public UserPhoneSwitch GetUserPhoneSwitch(Phone phone)
        {
            _UserPhoneSwitch = new UserPhoneSwitch(phone);
            _UserPhoneSwitch.CreateControl();
            _UserPhoneSwitch.AutoSizeChange(phone);
            return _UserPhoneSwitch;

        }
        #endregion
        #region ======================= formDeviceControl =======================
        private string aaptPath = Path.Combine(Directory.GetCurrentDirectory(), "Extension", "ScrcpyNet", "aapt.exe");
        public async Task labelnstallApk_Mouseclick()
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extension", "Apk");
            if (!Directory.Exists(folder)) return;

            var extensions = new[] { "*.apk", "*.xapk", "*.apks" };
            var filesOnDisk = extensions
                .SelectMany(ext => Directory.GetFiles(folder, ext, SearchOption.TopDirectoryOnly))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 1. Lấy DB 1 lần
            var dbApks = InstallApkRepository.LoadAll();
            var dbPaths = dbApks.Select(x => x.Path).ToHashSet(StringComparer.OrdinalIgnoreCase);

            // 2. Xóa cái không còn trên đĩa
            var toDelete = dbPaths.Except(filesOnDisk).ToList();
            foreach (var p in toDelete)
                InstallApkRepository.DeleteByPath(p);

            // 3. Thêm cái mới — ĐÂY LÀ CHỖ CẬU CẦN
            var toAdd = filesOnDisk.Except(dbPaths).ToList();
            if (!toAdd.Any()) return;

            // chạy nền, đừng block UI
            await Task.Run(() =>
            {
                foreach (var path in toAdd)
                {
                    try
                    {
                        // tự parse thông tin APK, đừng gọi lại form
                        var stringDump = CMD.ExecuteAaptCMD(aaptPath, $"dump badging \"{path}\"");
                        if (!string.IsNullOrEmpty(stringDump))
                        {
                            // AppName: bắt mọi ký tự cho đến dấu '
                            string AppName = RegexHelper.RegexGet(stringDump, @"label='([^']+)'");

                            // PackageName: chuẩn
                            string packageName = RegexHelper.RegexGet(stringDump, @"package: name='([^']+)'");

                            // VersionName: bắt mọi ký tự cho đến dấu '
                            string verName = RegexHelper.RegexGet(stringDump, @"versionName='([^']+)'");

                            // VersionCode: số nguyên
                            string verCode = RegexHelper.RegexGet(stringDump, @"versionCode='(?<value>\d+)'", "value");

                            // MinAPI: số nguyên
                            string minAPI = RegexHelper.RegexGet(stringDump, @"sdkVersion:'(?<value>\d+)'", "value");

                            // MaxAPI: số nguyên
                            string maxAPI = RegexHelper.RegexGet(stringDump, @"maxSdkVersion:'(?<value>\d+)'", "value");

                            // ABIs: bắt toàn bộ chuỗi trong native-code
                            var ABIs = RegexHelper.RegexGetABI(stringDump, @"native-code: '([^']+)'");


                            var installApk = new InstallApk
                            {
                                AppName = AppName,
                                PackageName = packageName,
                                VersionCode = verCode,
                                VersionName = verName,
                                MinAPI = string.IsNullOrEmpty(minAPI) ? -1 : Convert.ToInt32(minAPI),
                                MaxAPI = string.IsNullOrEmpty(maxAPI) ? -1 : Convert.ToInt32(maxAPI),
                                ABIs = ABIs,
                                Path = path
                            };

                            installApk.ABIs = string.IsNullOrEmpty(ABIs) ? "universal" : ABIs;

                            InstallApkRepository.Add(installApk);
                        }
                    }
                    catch { /* log lỗi, bỏ qua file hỏng */ }
                }
            });
        }

        public void DraggingformDeviceControl(Point point)
        {
            if (formDeviceControl != null)
            {
                //WindowHelper.BringToFront(formDeviceControl);
                formDeviceControl.Location = point;
            }
        }
        public void Switch_DeviceControl_To_AutoRecord()
        {
            if (formMain == null) return;

            Switch_AutoRecord_To_Flow_OPEN(true);
            if (formDeviceControl == null) return;
            var panels = formDeviceControl.panel1.Controls.OfType<PanelDevice>();
            if (!panels.Any()) return;
            var panelDevice = panels.First();
            if (panelDevice == null) return;
            var phone = panelDevice.phone;

            formAutoRecord = new FormAutoRecord();

            var allsize = GetSettings.GetALLSize(phone);
            var newSize = GetSettings.ComputePanelSize(new Size(allsize.max.w, allsize.max.h), formAutoRecord.panelPhone.Height);
            var pad = panelDevice.BorderThickness * 2;
            var size = new Size(newSize.Width + pad, newSize.Height + pad);
            panelDevice.Size = new Size(size.Width, size.Height);

            formAutoRecord.panelPhone.Size = newSize;
            formAutoRecord.panelUnderBack.Width = newSize.Width + 4;
            formAutoRecord.picboxScreenShot.Size = newSize;

            formDeviceControl.panel1.Controls.Remove(panelDevice);
            formDeviceControl.Close();

            formAutoRecord.panelPhone.Controls.Add(panelDevice);


            formAutoRecord.Show();

        }
        public void Switch_DeviceControl_To_Flow_OPEN(bool Isclose = false)
        {
            if (formDeviceControl == null || formDeviceControl.panel1 == null) return;
            var panels = formDeviceControl.panel1.Controls.OfType<PanelDevice>();
            if (!panels.Any()) return;
            var panelDevice = panels.First();
            if (panelDevice == null) return;
            if (_UserPhoneSwitch == null) return;
            var flowpanel = _UserPhoneSwitch.Parent as FlowLayoutPanel;
            if (flowpanel == null) return;
            var phone = panelDevice.phone;

            int index = flowpanel.Controls.GetChildIndex(_UserPhoneSwitch);
            flowpanel.SuspendLayout();
            if (index >= 0)
            {
                var allsize = GetSettings.GetALLSize(phone);
                panelDevice.Size = new Size(allsize.min.w, allsize.min.h);
                flowpanel.Controls.Add(panelDevice);
                flowpanel.Controls.SetChildIndex(panelDevice, index);

                flowpanel.Controls.Remove(_UserPhoneSwitch);
            }
            flowpanel.ResumeLayout();
            if (Isclose)
            {
                formDeviceControl.Close();
                formDeviceControl = null;
            }
        }
        public void Switch_AutoRecord_To_Flow_OPEN(bool Isclose = false)
        {
            if (formAutoRecord == null) return;
            if (formDeviceControl == null || formDeviceControl.panel1 == null) return;
            var panels = formDeviceControl.panel1.Controls.OfType<PanelDevice>();
            if (!panels.Any()) return;
            var panelDevice = panels.First();
            if (panelDevice == null) return;
            if (_UserPhoneSwitch == null) return;
            var flowpanel = _UserPhoneSwitch.Parent as FlowLayoutPanel;
            if (flowpanel == null) return;
            var phone = panelDevice.phone;

            int index = flowpanel.Controls.GetChildIndex(_UserPhoneSwitch);
            flowpanel.SuspendLayout();
            if (index >= 0)
            {
                var allsize = GetSettings.GetALLSize(phone);
                panelDevice.Size = new Size(allsize.min.w, allsize.min.h);
                flowpanel.Controls.Add(panelDevice);
                flowpanel.Controls.SetChildIndex(panelDevice, index);

                flowpanel.Controls.Remove(_UserPhoneSwitch);
            }
            flowpanel.ResumeLayout();
            if (Isclose)
            {
                formAutoRecord.Close();
            }
        }
        public void Switch_Flow_To_DeviceControl(PanelDevice panelDevice, Point? location = null)
        {
            if (formMain == null) return;
            var phone = panelDevice.phone;
            Switch_AutoRecord_To_Flow_OPEN(true);

            // --- phần flow giữ nguyên ---
            if (!IsExitPhoneInFormDeviceControl(phone.DeviceID))
            {
                Switch_DeviceControl_To_Flow_OPEN(true);
                var userPhoneSwitch = GetUserPhoneSwitch(phone);
                if (panelDevice.Parent is FlowLayoutPanel flowpanel)
                {
                    flowpanel.SuspendLayout();
                    int index = flowpanel.Controls.GetChildIndex(panelDevice);
                    if (index >= 0)
                    {
                        userPhoneSwitch.AutoSizeChange(phone);
                        flowpanel.Controls.Add(userPhoneSwitch);
                        flowpanel.Controls.SetChildIndex(userPhoneSwitch, index);
                        flowpanel.Controls.Remove(panelDevice);
                    }
                    flowpanel.ResumeLayout();
                }
            }

            // --- LUÔN tạo mới hoặc reset hoàn toàn ---
            if (formDeviceControl == null || formDeviceControl.phone?.DeviceID != phone.DeviceID)
            {
                formDeviceControl?.Close();
                formDeviceControl = new FormDeviceControl(phone.DeviceID);
                formDeviceControl.Loading();
            }

            int pad = panelDevice.BorderThickness * 2;
            var all = GetSettings.GetALLSize(phone);

            var sizeMax = new Size(all.max.w + pad, all.max.h + pad);
            if (panelDevice.Width > panelDevice.Height)
            {
                sizeMax = new Size(all.max.h + pad, all.max.w + pad);
            }

            // dùng ClientSize để không bị cộng viền Windows 2 lần
            var formClient = new Size(sizeMax.Width + 10 + 190, sizeMax.Height);
            formDeviceControl.ClientSize = formClient;

            // reset panel1 đúng cả 2 chiều
            formDeviceControl.panel1.SuspendLayout();
            formDeviceControl.panel1.Controls.Clear(); // tránh chồng
            formDeviceControl.panel1.Size = new Size(sizeMax.Width + 5, sizeMax.Height);

            panelDevice.Size = sizeMax;
            panelDevice.Dock = DockStyle.None;
            panelDevice.Location = Point.Empty;

            formDeviceControl.panel1.Controls.Add(panelDevice);
            formDeviceControl.panel1.ResumeLayout();

            formDeviceControl.labelPhoneTagNumber.Text = phone.PhoneTagNumber.ToString("00");
            formDeviceControl.labelModelName.Text = phone.Model;

            if (location.HasValue) formDeviceControl.Location = location.Value;
            else formDeviceControl.StartPosition = FormStartPosition.CenterScreen;

            if (!formDeviceControl.Visible)
            {
                formMain.StartFormDevice(formDeviceControl);
            }

        }
        public void Switch_Flow_To_AutoRecord(PanelDevice panelDevice)
        {
            if (formMain == null) return;
            Switch_AutoRecord_To_Flow_OPEN(true);
            Switch_DeviceControl_To_Flow_OPEN(true);
            var phone = panelDevice.phone;
            var userPhoneSwitch = GetUserPhoneSwitch(phone);
            var flowpanel = panelDevice.Parent as FlowLayoutPanel;
            if (flowpanel == null) return;

            flowpanel.SuspendLayout();
            int index = flowpanel.Controls.GetChildIndex(panelDevice);
            if (index >= 0)
            {
                flowpanel.Controls.Add(userPhoneSwitch);
                flowpanel.Controls.SetChildIndex(userPhoneSwitch, index);
                userPhoneSwitch.Invalidate();
                userPhoneSwitch.Update();

                flowpanel.Controls.Remove(panelDevice);
            }
            flowpanel.ResumeLayout();

            formAutoRecord = new FormAutoRecord();

            var allsize = GetSettings.GetALLSize(phone);
            var newSize = GetSettings.ComputePanelSize(new Size(allsize.max.w, allsize.max.h), formAutoRecord.panelPhone.Height);
            var pad = panelDevice.BorderThickness * 2;
            var size = new Size(newSize.Width + pad, newSize.Height + pad);
            panelDevice.Size = new Size(size.Width, size.Height);

            formAutoRecord.panelPhone.Size = newSize;
            formAutoRecord.panelUnderBack.Width = newSize.Width + 4;
            formAutoRecord.picboxScreenShot.Size = newSize;

            formAutoRecord.panelPhone.Controls.Add(panelDevice);


            formAutoRecord.Show();

        }



        public async Task Rotate_PanelDevice_FormDeviceControl()
        {
            //if (formDeviceControl != null)
            //{
            //    var RotateCheck = -1;

            //    formDeviceControl.SuspendLayout();
            //    var device = formDeviceControl.panel1.Controls.OfType<PanelDeviceView>().First();
            //    if (device != null && device.phone != null)
            //    {
            //        var IsRotated = await device.RotateAsync();
            //        if (IsRotated.Item1)
            //        {

            //            RotateCheck = IsRotated.Item2;
            //            device.RotateFinalAfterSuccsess(IsRotated.Item2);

            //            var sizeNonPad = GetSettings.getSize(device.phone);
            //            var pad = device.BorderThickness * 2;

            //            var sizeMax = new Size();
            //            if (IsRotated.Item2 == 0)
            //            {
            //                sizeMax = new Size(sizeNonPad.maxSize.Width + pad, sizeNonPad.maxSize.Height + pad);
            //            }
            //            else if (IsRotated.Item2 == 1)
            //            {
            //                sizeMax = new Size(sizeNonPad.maxSize.Height + pad, sizeNonPad.maxSize.Width + pad);
            //            }

            //            var formSize = new Size(
            //               sizeMax.Width + 10 + 2 + 190,
            //               sizeMax.Height + 1
            //           );

            //            formDeviceControl.Size = formSize;
            //            formDeviceControl.panel1.Size = new Size(sizeMax.Width + 5, formDeviceControl.panel1.Height);
            //            formDeviceControl.panelMenuRight.Size = new Size(190, formDeviceControl.panelMenuRight.Height);
            //        }
            //    }
            //    formDeviceControl.ResumeLayout();

            //}
        }
        public bool RemovePanelDeviceInFormDeviceControl(bool CloseForm = false)
        {
            if (formDeviceControl != null)
            {
                // 1. Tìm panel đầu tiên trong formDeviceControl.panel1
                var panel = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                if (panel == null) return false;

                formDeviceControl.panel1.Controls.Remove(panel);

                if (CloseForm)
                {
                    formDeviceControl.Close();
                    formDeviceControl = null;
                }
                return true;

            }
            return false;
        }
        public bool RemovePanelDeviceInFormDeviceControl(string deviceID, bool CloseForm = false)
        {
            if (formDeviceControl != null)
            {
                // 1. Tìm panel đầu tiên trong formDeviceControl.panel1
                var panel = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                if (panel == null || panel.phone == null) return false;
                if (panel.Name == deviceID || panel.phone.DeviceID == deviceID)
                {
                    formDeviceControl.panel1.Controls.Remove(panel);

                    return true;
                }
                if (CloseForm)
                {
                    formDeviceControl.Close();
                    formDeviceControl = null;
                }
            }
            return false;
        }
        public bool RemovePanelDeviceInFormDeviceControl(List<Phone> Phones, bool CloseForm = false)
        {
            if (Phones == null || Phones.Count() == 0)
                return false;
            if (formDeviceControl != null)
            {
                var devicePhone = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                if (devicePhone != null && devicePhone.phone != null)
                    Phones.RemoveAt(Phones.FindIndex(p => p.DeviceID == devicePhone?.phone?.DeviceID));

                if (CloseForm)
                {
                    formDeviceControl.Close();
                    formDeviceControl = null;
                }

                return true;

            }
            return false;
        }
        public PanelDevice? FindPanelDeviceInFormDeviceControl(List<PanelDevice> listSelecteds)
        {
            // Validate đầu vào
            if (listSelecteds == null || listSelecteds.Count == 0) return null;
            if (formDeviceControl == null) return null;


            var devicePhone = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
            if (devicePhone != null && devicePhone.phone != null)
            {
                var match = listSelecteds.FirstOrDefault(p => p.phone?.DeviceID == devicePhone.phone.DeviceID);
                if (match != null)
                {
                    return devicePhone; // Trả về ngay nếu tìm thấy
                }
            }

            // Không tìm thấy PanelDeviceView nào phù hợp
            return null;
        }
        public bool IsExiPanelDeviceInFormDeviceControl(List<Phone> listSelecteds)
        {
            // Validate đầu vào
            if (listSelecteds == null || listSelecteds.Count == 0) return false;
            if (formDeviceControl == null) return false;


            var devicePhone = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
            if (devicePhone != null && devicePhone.phone != null)
            {
                var match = listSelecteds.FirstOrDefault(p => p.DeviceID == devicePhone.phone.DeviceID);
                if (match != null)
                {
                    return true; // Trả về ngay nếu tìm thấy
                }
            }

            // Không tìm thấy PanelDeviceView nào phù hợp
            return false;
        }
        public bool IsExitPhoneInFormDeviceControl(string deviceID)
        {
            if (formDeviceControl != null)
            {
                // 1. Tìm panel đầu tiên trong formDeviceControl.panel1
                var panel = formDeviceControl.panel1.Controls.OfType<PanelDevice>().FirstOrDefault();
                if (panel == null || panel.phone == null) return false;
                if (panel.Name == deviceID || panel.phone.DeviceID == deviceID) return true;
            }
            return false;
        }
        public void CloseFormDeviceControl()
        {
            if (formDeviceControl != null)
            {
                formDeviceControl.Close();
                formDeviceControl = null;
            }
        }

        #endregion

        #region ======================= FormAutoRecord =======================
        public void SwitchToFormAutoRecord()
        {
            if (formDeviceControl == null) return;
            if (formDeviceControl.panel1 == null) return;

            if (formAutoRecord != null) return;

            formAutoRecord = new FormAutoRecord();
            var panelDevice = formDeviceControl.panel1.Controls.OfType<PanelDevice>().SingleOrDefault();
            if (panelDevice != null)
            {
                formDeviceControl.panel1.Controls.Remove(panelDevice);
                formDeviceControl.Close();
                DeviceManager.Instance.ChangePanelDevice(panelDevice, formAutoRecord);
                formAutoRecord.panelPhone.Controls.Add(panelDevice);
                formAutoRecord.Show();
            }
        }

        #endregion


        // ======================= FormActionIntent =======================
        public FormActionIntent CreateNewFormActionIntent(Phone phone)
        {
            return new FormActionIntent(phone);
        }

        public void ShowFormActionIntent(Phone phone)
        {
            formActionIntent = new FormActionIntent(phone);
            formActionIntent.Show();
        }

        public void CloseFormActionIntent()
        {
            if (formActionIntent != null)
            {
                formActionIntent.Close();
                formActionIntent.Dispose();
                formActionIntent = null;
            }
        }


        #region ======================= FlowLayoutPanel - VirtualFlowHelper =======================


        public void RemoveAllcontrolPhoneSelected(List<Phone> ListSeleteds)
        {
            virtualFlowHelper?.RemoveAllPhoneControl(ListSeleteds);
            RemovePanelDeviceInFormDeviceControl(ListSeleteds, true);
        }

        #endregion

        #region ======================= FlowLayoutPanel - VirtualFlowHelper =======================
        #endregion
        //FormActionIntent

    }
}

