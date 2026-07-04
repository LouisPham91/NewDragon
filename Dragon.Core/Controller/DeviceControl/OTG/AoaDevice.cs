using LibUsbDotNet.LibUsb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Dragon.Controller.DeviceControl.OTG
{
    public class AoaDevice
    {
        [Key]
        public string DeviceId { get; set; } = "";
        public string InstanceId { get; set; } = "";
        public string Description { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string ClassGuid { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Status { get; set; } = "";
        public string DriverName { get; set; } = "";
        public string Service { get; set; } = "";
        public string ParentId { get; set; } = "";

        [NotMapped]
        public List<string> Children { get; set; } = new();

        [NotMapped]
        public ushort Vid { get; set; }

        [NotMapped]
        public ushort Pid { get; set; }

        [NotMapped]
        public IUsbDevice? UsbDevice { get; set; }

        [NotMapped] public bool IsPoweredOn => Status.Equals("Started", StringComparison.OrdinalIgnoreCase);

        // ---- HÀM MỚI ----
        public bool UpdateFromScan(AoaDevice fresh)
        {
            if (fresh is null) return false;

            // chỉ cho phép update khi đúng thiết bị
            if (!string.Equals(this.DeviceId, fresh.DeviceId, StringComparison.OrdinalIgnoreCase))
            {
                // tránh nhầm lung tung
                return false;
            }

            // Giữ nguyên DeviceId và UsbDevice, chỉ refresh thông tin từ Windows
            InstanceId = fresh.InstanceId;
            Description = fresh.Description;
            ClassName = fresh.ClassName;
            ClassGuid = fresh.ClassGuid;
            Manufacturer = fresh.Manufacturer;
            Status = fresh.Status;
            DriverName = fresh.DriverName;
            Service = fresh.Service;
            ParentId = fresh.ParentId;
            Vid = fresh.Vid;
            Pid = fresh.Pid;

            // Children chỉ update khi scan có dữ liệu (withRelations = true)
            if (fresh.Children != null && fresh.Children.Count > 0)
            {
                Children.Clear();
                Children.AddRange(fresh.Children);
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Instance ID: {InstanceId}");
            sb.AppendLine($"Device Description: {Description}");
            sb.AppendLine($"Class Name: {ClassName}");
            sb.AppendLine($"Class GUID: {ClassGuid}");
            sb.AppendLine($"Manufacturer Name: {Manufacturer}");
            sb.AppendLine($"Status: {Status}");
            sb.AppendLine($"Driver Name: {DriverName}");
            sb.AppendLine($"Service: {Service}");
            sb.AppendLine($"Parent: {ParentId}");
            foreach (var c in Children) sb.AppendLine($"Children: {c}");
            return sb.ToString();
        }
    }


}

