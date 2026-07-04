using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public static class DLoopMapper
    {
        public static ServerDloop ToServer(DLoop local)
        {
            return new ServerDloop
            {
                Id = local.Id == Guid.Empty ? Guid.NewGuid() : local.Id,
                OwnerGmail = local.UserGmail, // chỉ để hiển thị, server sẽ bỏ qua
                Name = local.Name ?? "",
                IsSingleLoop = local.IsSingleLoop,
                PhoneModel = local.PhoneModel ?? "",
                DloopData = DLoopCompressor.Pack(local),
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static DLoop ToLocal(ServerDloop srv)
        {
            var d = DLoopCompressor.Unpack(srv.DloopData);
            d.Id = srv.Id;
            d.Name = srv.Name;
            d.IsSingleLoop = srv.IsSingleLoop;
            d.PhoneModel = srv.PhoneModel;
            d.UserGmail = srv.OwnerGmail; // lưu owner gốc để UI hiện “Shared by A”
            d.UpdatedAt = srv.UpdatedAt;
            return d;
        }
    }
}
