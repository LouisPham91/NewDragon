using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Controller.TaskDeviceManager.Model.Input
{

    public enum InputTextType
    {
        None,
        InputText,
        Random,
        TwoFactor,
        DataWrite,
        SaveText
    }
    public enum TypeOption
    {
        Typing,
        CopyPaste,
    }
    public enum ImeActionType
    {
        ClearText,
        SmartEnter,
        Keycode,
        EditorCode,
        Show,
        Hide,
        GetClipboard,
        GetClipboardBase64
    }

}
