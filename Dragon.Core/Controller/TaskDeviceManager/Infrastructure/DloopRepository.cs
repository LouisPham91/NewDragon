using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.GlobalControl.Property;
using Dragon.Controller.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure
{
    public class DloopRepository
    {
        private readonly DloopApiService _api;
        private readonly string _email;

        public DloopRepository()
        {
            _api = new DloopApiService();
            _email = GetSettings.GetUserEmail();
        }

        // 1. LOAD từ server → convert → đổ vào Items
        public async Task<(bool ok, string err)> LoadAsync()
        {
            var r = await _api.MineAsync();
            if (!r.ok) return (false, r.err);

            DloopStore.Items.Clear();
            foreach (var srv in r.data!)
            {
                var local = DLoopMapper.ToLocal(srv);
                DloopStore.Items[local.Id] = local;
            }
            return (true, "");
        }

        // 2. SAVE (thêm/sửa) → update Items trước, rồi đẩy server
        public async Task<(bool ok, string err)> SaveAsync(DLoop local)
        {
            // update RAM ngay để UI mượt
            DloopStore.Items[local.Id] = local;

            var srv = DLoopMapper.ToServer(local);
            var r = await _api.SaveAsync(srv);
            if (!r.ok)
            {
                // rollback nếu fail
                DloopStore.Items.TryRemove(local.Id, out _);
                return (false, r.err);
            }
            // đồng bộ Id server trả về
            local.Id = r.id;
            DloopStore.Items[local.Id] = local;
            return (true, "");
        }

        // 3. DELETE
        public async Task<(bool ok, string err)> DeleteAsync(Guid id)
        {
            if (!DloopStore.Items.TryRemove(id, out var backup))
                return (false, "NOT_IN_CACHE");

            var r = await _api.DeleteAsync(id);
            if (!r.ok)
            {
                // rollback
                DloopStore.Items[id] = backup;
                return (false, r.err);
            }
            return (true, "");
        }

        // 4. TÌM KIẾM (chỉ RAM, không gọi mạng)
        public IEnumerable<DLoop> Search(string keyword, bool? isSingle = null)
        {
            var q = DloopStore.Items.Values.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(keyword))
                q = q.Where(x => x.Name?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true);
            if (isSingle.HasValue)
                q = q.Where(x => x.IsSingleLoop == isSingle.Value);
            return q.OrderByDescending(x => x.UpdatedAt);
        }

        public DLoop? Get(Guid id) => DloopStore.Items.TryGetValue(id, out var v) ? v : null;
    }
}
