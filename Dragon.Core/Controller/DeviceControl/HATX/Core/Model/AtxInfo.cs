using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Dragon.Controller.Controller.DeviceControl.HATX.Core.Model
{
    public class AtxInfo
    {
        [JsonPropertyName("udid")] public string? Udid { get; set; }
        [JsonPropertyName("version")] public string? Version { get; set; }
        [JsonPropertyName("serial")] public string? Serial { get; set; }
        [JsonPropertyName("brand")] public string? Brand { get; set; }
        [JsonPropertyName("model")] public string? Model { get; set; }
        [JsonPropertyName("hwaddr")] public string? HwAddr { get; set; }
        [JsonPropertyName("sdk")] public int Sdk { get; set; }
        [JsonPropertyName("agentVersion")] public string? AgentVersion { get; set; }
        [JsonPropertyName("display")] public DisplayInfo? Display { get; set; }
        [JsonPropertyName("battery")] public BatteryInfo? Battery { get; set; }
        [JsonPropertyName("memory")] public MemoryInfo? Memory { get; set; }
        [JsonPropertyName("cpu")] public CpuInfo? Cpu { get; set; }
        [JsonPropertyName("arch")] public string? Arch { get; set; }
        [JsonPropertyName("owner")] public object? Owner { get; set; }
        [JsonPropertyName("presenceChangedAt")] public DateTime PresenceChangedAt { get; set; }
        [JsonPropertyName("usingBeganAt")] public DateTime UsingBeganAt { get; set; }
        [JsonPropertyName("product")] public object? Product { get; set; }
        [JsonPropertyName("provider")] public object? Provider { get; set; }
    }

    public class DisplayInfo
    {
        [JsonPropertyName("width")] public int Width { get; set; }
        [JsonPropertyName("height")] public int Height { get; set; }
    }

    public class BatteryInfo
    {
        [JsonPropertyName("acPowered")] public bool AcPowered { get; set; }
        [JsonPropertyName("usbPowered")] public bool UsbPowered { get; set; }
        [JsonPropertyName("wirelessPowered")] public bool WirelessPowered { get; set; }
        [JsonPropertyName("status")] public int Status { get; set; }
        [JsonPropertyName("health")] public int Health { get; set; }
        [JsonPropertyName("present")] public bool Present { get; set; }
        [JsonPropertyName("level")] public int Level { get; set; }
        [JsonPropertyName("scale")] public int Scale { get; set; }
        [JsonPropertyName("voltage")] public int Voltage { get; set; }
        [JsonPropertyName("temperature")] public int Temperature { get; set; }
        [JsonPropertyName("technology")] public string? Technology { get; set; }
    }

    public class MemoryInfo
    {
        [JsonPropertyName("total")] public long Total { get; set; }
        [JsonPropertyName("around")] public string? Around { get; set; }
    }

    public class CpuInfo
    {
        [JsonPropertyName("cores")] public int Cores { get; set; }
        [JsonPropertyName("hardware")] public string? Hardware { get; set; }
    }

   
}
