using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace InputRecorder
{
    partial class App
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        private static LowLevelProc _keypProc = KeyBoardProc;
        private static IntPtr _keyHookID = IntPtr.Zero;

        private const int WH_MOUSE_LL = 14;
        private static LowLevelProc _mouseProc = MouseProc;
        private static IntPtr _mouseHookID = IntPtr.Zero;
        
        //Set hooks
        private void InitializeSystemHook()
        {
            _keyHookID = SetKeyboardHook(_keypProc);
            _mouseHookID = SetMouseHook(_mouseProc);
        }

        //Unhook
        private void CloseSystemHook()
        {
            UnhookWindowsHookEx(_keyHookID);
            UnhookWindowsHookEx(_mouseHookID);
        }

        //Hooks the keyboard procedure 
        private static IntPtr SetKeyboardHook(LowLevelProc proc)
        {
            using(Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        //Hooks the mouse procedure 
        private static IntPtr SetMouseHook(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
        
        //The funtion called when keyboard event occurs
        private static IntPtr KeyBoardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBLLHOOKSTRUCT hookStruct = (KBLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBLLHOOKSTRUCT));// gets the structure pointed at by lParam

                if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                    KeyDown(hookStruct);

                if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                    KeyUp(hookStruct);
                
            }
            return CallNextHookEx(_keyHookID, nCode, wParam, lParam);

        }

        //The funtion called when mouse event occurs
        private static IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));// gets the structure pointed at by lParam
                MouseMessages message = (MouseMessages)wParam;

                if (MouseMessages.WM_LBUTTONDOWN == message)
                    MouseLBDown(hookStruct);
                if (MouseMessages.WM_LBUTTONUP == message)
                    MouseLBUp(hookStruct);
                if (MouseMessages.WM_RBUTTONDOWN == message)
                    MouseRBDown(hookStruct);
                if (MouseMessages.WM_RBUTTONUP == message)
                    MouseRBUp(hookStruct);
                if (MouseMessages.WM_MOUSEMOVE == message)
                    MouseMove(hookStruct);
                if (MouseMessages.WM_MOUSEWHEEL == message)
                    MouseScroll(hookStruct);
                if (MouseMessages.WM_XBUTTONDOWN == message)
                    MouseXDown(hookStruct);
                if (MouseMessages.WM_XBUTTONUP == message)
                    MouseXUp(hookStruct);
                 if (MouseMessages.WM_MBUTTONDOWN == message)
                    MouseMBDown(hookStruct);
                if (MouseMessages.WM_MBUTTONUP == message)
                    MouseMBUp(hookStruct);
            }
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);

        }

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201, 
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A, 
            WM_RBUTTONDOWN = 0x0204, 
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C
            
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public int mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KBLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        //external functions
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError=true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, int dwThreadID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hkk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
