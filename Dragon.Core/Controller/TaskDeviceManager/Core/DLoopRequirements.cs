using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    public class DLoopRequirements
    {
        public bool NeedUHID { get; set; } = false;
        public bool NeedScrcpy { get; set; } = false;
        public bool NeedATX { get; set; } = false;
        public bool NeedACC { get; set; } = false;

        public bool IsCanRunDloop()
        {
            if (NeedUHID && NeedScrcpy) return false; // xung đột
            if (NeedACC) return false; // chưa hỗ trợ
            return true;
        }

        // --- Write ---
        public bool GetRequirement_GetColumnDataArgs(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.ATX: NeedATX = true; break;
                case ControlMode.Scrcpy: NeedScrcpy = true; break;
                case ControlMode.HDI: NeedUHID = true; break;
                case ControlMode.ACC: NeedACC = true; break;
            }
            return IsCanRunDloop();
        }

        // --- Read ---
        public bool GetRequirement_SetColumnDataArgs(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.ATX: NeedATX = true; break;
                case ControlMode.Scrcpy: NeedScrcpy = true; break;
                case ControlMode.HDI: NeedUHID = true; break;
                case ControlMode.ACC: NeedACC = true; break;
            }
            return IsCanRunDloop();
        }

        // --- Mouse ---
        public bool GetRequirement_Mouse(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.ATX: NeedATX = true; break;
                case ControlMode.Scrcpy: NeedScrcpy = true; break;
                case ControlMode.HDI: NeedUHID = true; break;
                case ControlMode.ACC: NeedACC = true; break;
                    // ADB, ADBEvent, None -> luôn có sẵn
            }
            return IsCanRunDloop();
        }

        // --- Input ---
        public bool GetRequirement_SendTextArgs(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.ATX: NeedATX = true; break;
                case ControlMode.Scrcpy: NeedScrcpy = true; break;
                case ControlMode.HDI: NeedUHID = true; break;
                case ControlMode.ACC: NeedACC = true; break;
            }
            return IsCanRunDloop();
        }

        // --- KeyPress ---
        public bool GetRequirement_KeyPress(ControlMode mode)
        {
            switch (mode)
            {
                case ControlMode.ATX: NeedATX = true; break;
                case ControlMode.ACC: NeedACC = true; break;
            }
            return IsCanRunDloop();
        }

        // --- Copy File ---
        public bool GetRequirement_FileArgs(ControlMode mode)
        {
            if (mode == ControlMode.ATX) NeedATX = true;
            return IsCanRunDloop();
        }

        // --- Keyboard ---
        public bool GetRequirement(KeyboardType mode)
        {
            if (mode == KeyboardType.ACC) NeedACC = true;
            return IsCanRunDloop();
        }
        // --- AppArgs ---
        public bool GetRequirement_AppArgs(ControlMode mode)
        {
            if (mode == ControlMode.ACC) NeedACC = true;
            if (mode == ControlMode.ATX) NeedATX = true;

            return IsCanRunDloop();
        }
    }
}
