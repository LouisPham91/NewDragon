using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.HATX.Core.Model
{
    public record TouchCmd
    (
        string operation,
        int index = 0,
        float pressure = 0.5f,
        float xP = 0f,
        float yP = 0f
    );
}
