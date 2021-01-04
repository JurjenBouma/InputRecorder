using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace InputRecorder
{
    class MessageSender
    {
        [DllImport("user32.dll",SetLastError=true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetric smIndex);

        public static void SendKeyInput(ushort vkCode, ushort scanCode, uint Flags, uint Time)
        {
            INPUT input = new INPUT { Type = 1 };
            input.Data.Keyboard = new KEYBDINPUT();
            input.Data.Keyboard.Vk = vkCode;
            input.Data.Keyboard.Scan = scanCode;
            input.Data.Keyboard.Flags = Flags;//0 = keydown 2 = keyup
            input.Data.Keyboard.Time = Time;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[] { input };
            if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                throw new Exception();
            }
        }

        public static void SendMouseMove(int x, int y,uint Time)
        {
            INPUT input = new INPUT { Type = 0};
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
            input.Data.Mouse.y = (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_MOVE |  MOUSEINPUTFLAGS.MOUSEEVENTF_ABSOLUTE);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;
            INPUT[] inputs = new INPUT[] { input };
            if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                throw new Exception();
            }
        }

        public static void SendLMBDown(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTDOWN);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
        public static void SendLMBUp(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_LEFTUP);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendRMBDown(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTDOWN);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
        public static void SendRMBUp(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_RIGHTUP);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendXDown(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_XDOWN);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
        public static void SendXUp(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_XUP);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendMBDown(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_MIDDLEDOWN);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
        public static void SendMBUp(uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.MouseData = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_MIDDLEUP);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendMouseWheel(int mouseData,uint Time)
        {
            INPUT input = new INPUT { Type = 0 };
            input.Data.Mouse = new MOUSEINPUT();
            input.Data.Mouse.x = 0;
            input.Data.Mouse.y = 0;
            input.Data.Mouse.Flags = (uint)(MOUSEINPUTFLAGS.MOUSEEVENTF_WHEEL);
            input.Data.Mouse.Time = Time;
            input.Data.Mouse.ExtraInfo = IntPtr.Zero;

            if (mouseData >0)
                input.Data.Mouse.MouseData = 120;
            else if(mouseData <0)
                input.Data.Mouse.MouseData = -120;

            INPUT[] inputs = new INPUT[] { input };
            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT HardWare;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int x;
            public int y;
            public int MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
        
        enum MOUSEINPUTFLAGS : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        enum SystemMetric
        {
            SM_CXSCREEN =0,
            SM_CYSCREEN =1
        }
        #endregion
    }
}
