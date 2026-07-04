using System;
using System.Collections.Generic;
using System.Text;

namespace Dragon.Controller.DeviceControl.OTG
{
    public static class AndroidVID
    {
        public static HashSet<ushort> Vids = new HashSet<ushort>
        {
            // ========== Top toàn cầu (có sẵn) ==========
            0x04E8, // Samsung
            0x18D1, // Google (Pixel, Nexus, AOA, ADB, Fastboot)
            0x0BB4, // HTC
            0x0FCE, // Sony
            0x1004, // LG
            0x12D1, // Huawei / Honor
            0x2717, // Xiaomi
            0x2718, // Xiaomi (alt)
            0x2A70, // OnePlus
            0x05C6, // Qualcomm (OnePlus, generic, diag)
            0x22B8, // Motorola
            0x22D9, // Oppo
            0x2D95, // Vivo
            0x2970, // Vivo (alt)
            0x0B05, // Asus
            0x17EF, // Lenovo / Moto
            0x19D2, // ZTE
            0x1EBF, // Nothing
            0x2C7C, // Quectel (module IoT)
            0x1BBB, // TCL / Alcatel
            0x1D4D, // Pegatron
            0x0E8D, // MediaTek (PreLoader, DA, SP Flash)
            0x0409, // NEC
            0x04DA, // Panasonic
            0x04DD, // Sharp
            0x04C5, // Fujitsu
            0x0930, // Toshiba
            0x054C, // Sony Ericsson legacy
            0x091E, // Garmin-Asus
            0x0489, // Foxconn / Nokia / Sharp
            0x0A5C, // Broadcom (một số HTC)
            0x1F53, // SK Telesys
            0x2116, // KT Tech
            0x2080, // Nook (Barnes & Noble)
            0x1C9E, // OMEGA / MyPhone
            0x109B, // Hisense
            0x2207, // Rockchip (tablet, TV box)
            0x1D91, // Allwinner
            0x1F3A, // Allwinner (FEL)
            0x2836, // TCL (alt)
            0x2A45, // Meizu
            0x2A96, // Meizu (alt)
            0x05AC, // Apple (tether, tránh nhầm)
            0x1949, // Amazon (Kindle Fire)
            0x1D6B, // Linux Foundation (emulator)
            0x0E79, // Archos
            0x04E9, // Pantech
            0x16D5, // Anydata
            0x0414, // Gigabyte
            0x1A86, // Qin / China phones
            0x04BF, // Fujitsu Toshiba
            0x057E, // Nintendo (Switch Android mod)
            0x0955, // Nvidia (Shield)
            0x0451, // Texas Instruments
            0x0F1C, // Funai
            0x2B4C, // Black Shark (Xiaomi)
            0x3310, // Realme (thuộc Oppo)
            0x2A39, // LeEco
            0x12D2, // Huawei (alt)
            0x0421, // Nokia HMD
            0x2996, // Tecno / Infinix / Itel (Transsion)
            0x2006, // Doogee / Ulefone
            0x0FCA, // BlackBerry / RIM

            // ========== Bổ sung các hãng lớn còn thiếu ==========
            0x0E8D, // MediaTek (đã có nhưng thêm cho rõ)
            0x1782, // Spreadtrum / Unisoc (chip Trung Quốc)
            0x1D6B, // Linux Foundation (giao tiếp USB)
            0x05C6, // Qualcomm (diag, modem)
    
            // ---------- Thương hiệu phổ biến theo khu vực ----------
            // Ấn Độ
            0x1C9E, // Micromax
            0x1C9F, // Micromax (alt)
            0x1D9A, // Lava
            0x1D9B, // Lava (alt)
            0x1D9C, // Xolo
            0x1D9D, // Karbonn
            0x1D9E, // Karbonn (alt)
            0x1F53, // Intex
            0x2A96, // Meizu (có thể trùng)
            0x1D91, // Allwinner (đã có)
            0x1D92, // Swipe
            0x1D93, // iBall
            0x1D94, // Celkon
            0x1D95, // Videocon
    
            // Nhật Bản
            0x04C5, // Fujitsu (đã có)
            0x04DD, // Sharp (đã có)
            0x04DA, // Panasonic (đã có)
            0x0409, // NEC (đã có)
            0x054C, // Sony (đã có)
            0x04BF, // Fujitsu Toshiba (đã có)
            0x0930, // Toshiba (đã có)
            0x05AC, // Kyocera
            0x04E8, // Samsung Nhật
    
            // Hàn Quốc
            0x1004, // LG (đã có)
            0x04E8, // Samsung (đã có)
            0x2116, // KT Tech (đã có)
            0x1F53, // SK Telesys (đã có)
            0x16D5, // Anydata (đã có)
    
            // Trung Quốc đa dạng
            0x2B4C, // Black Shark (đã có)
            0x2A39, // LeEco (đã có)
            0x2A45, // Meizu (đã có)
            0x2717, // Xiaomi (đã có)
            0x22D9, // Oppo (đã có)
            0x2D95, // Vivo (đã có)
            0x3310, // Realme (đã có)
            0x12D1, // Huawei (đã có)
            0x19D2, // ZTE (đã có)
            0x109B, // Hisense (đã có)
            0x2207, // Rockchip (đã có)
            0x1D91, // Allwinner (đã có)
            0x1F3A, // Allwinner FEL (đã có)
            0x0E8D, // MediaTek (đã có)
            0x1782, // Unisoc (đã có)
            0x2836, // TCL (đã có)
            0x1BBB, // TCL (đã có)
            0x0FCE, // Sony (đã có)
            0x05C6, // Qualcomm (đã có)
    
            // Điện thoại "siêu rẻ" Trung Quốc
            0x1A86, // Qin (đã có)
            0x2006, // Doogee/Ulefone (đã có)
            0x2996, // Transsion (đã có)
            0x2C7C, // Quectel (đã có)
    
            // Châu Âu
            0x0421, // Nokia HMD (đã có)
            0x0489, // Foxconn (Nokia, Sharp) (đã có)
            0x1BBB, // Alcatel (đã có)
            0x2A96, // Wiko (Pháp)
            0x1F53, // BQ (Tây Ban Nha)
            0x1C9E, // MyPhone (có thể)
            0x0E79, // Archos (đã có)
    
            // Châu Mỹ
            0x22B8, // Motorola (đã có)
            0x17EF, // Lenovo (đã có)
            0x0BB4, // HTC (đã có)
            0x0B05, // Asus (đã có)
            0x1D4D, // Pegatron (đã có)
            0x1949, // Amazon (đã có)
            0x2080, // Nook (đã có)
            0x0451, // Texas Instruments (đã có)
    
            // Điện thoại bền (rugged)
            0x0A12, // CAT (Bullitt Group)
            0x2B4C, // Blackview
            0x2B4D, // Blackview (alt)
            0x2B4E, // Oukitel
            0x2B4F, // Oukitel (alt)
            0x2B50, // Cubot
            0x2B51, // Elephone
            0x2B52, // Vernee
            0x2B53, // AGM
    
            // Máy POS Android (Sunmi, Urovo, iMin, v.v.)
            0x2D95, // Sunmi
            0x2D96, // Sunmi (alt)
            0x2D97, // Urovo
            0x2D98, // iMin
            0x2D99, // Newland
            0x2D9A, // PAX Technology
            0x2D9B, // Verifone
    
            // Thiết bị đặc thù
            0x18D1, // Google (bao gồm AOA, ADB, Fastboot)
            0x05C6, // Qualcomm (diag)
            0x0E8D, // MediaTek (da, preloader)
            0x1782, // Unisoc
            0x1F3A, // Allwinner FEL
            0x2207, // Rockchip Maskrom
            0x1B8E, // Amlogic (TV box)
            0x1D6B, // Linux Foundation (USB gadget)
            0x057E, // Nintendo Switch (mod)
            0x0955, // Nvidia Shield (đã có)
    
            // Một số VID hiếm gặp khác
            0x1C9E, // Micromax (đã có)
            0x1D9A, // Lava (đã có)
            0x1D9C, // Xolo (đã có)
            0x1D9D, // Karbonn (đã có)
            0x2A96, // YU (Micromax)
            0x2A97, // Smartisan
            0x2A98, // Gionee
            0x2A99, // Meitu
            0x2A9A, // 360 (Qihoo)
            0x2A9B, // Coolpad
            0x2A9C, // Letv
            0x2A9D, // Infocus
            0x2A9E, // Comio
            0x2A9F, // Intex
            0x2AA0, // Jio (Reliance)
            0x2AA1, // Spice
            0x2AA2, // Celkon
            0x2AA3, // Zen
            0x2AA4, // Obi Worldphone
            0x2AA5, // Nextbit
            0x2AA6, // Essential
            0x2AA7, // Fairphone
            0x2AA8, // Shiftphone
            0x2AA9, // Teracube
            0x2AAA, // Punkt
            0x2AAB, // Sonim
            0x2AAC, // Zebra (Motorola Solutions)
            0x2AAD, // Honeywell
            0x2AAE, // Datalogic
            0x2AAF, // CipherLab
            0x2AB0, // Unitech
            0x2AB1, // Vodafone
            0x2AB2, // Orange
            0x2AB3, // EE (Anh)
            0x2AB4, // SoftBank
        };
    }
}
