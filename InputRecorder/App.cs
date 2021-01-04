using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace InputRecorder
{
    public partial class App : ApplicationContext
    {
        #region declarations
        public static bool isRecoding = false;
        public static bool isReplaying = false;
        private static RecordInstruction currentInstruction;
        private static RecordFile recordFile;
        private static uint startTime;
        #endregion
        
        public App()
        {
            InitializeComponents();
            InitializeEvents();
        }

        private void OnAppExit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            CloseSystemHook();
        }

        private void menuItemExit_Clicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Input Functions
        private static void KeyDown(KBLLHOOKSTRUCT keyInfo)
        {
            Keys key = (Keys)keyInfo.vkCode;

            #region Strart & Stop Recording
            //Start recording
            if (key == Keys.F1 && Keys.Control == Control.ModifierKeys && !isRecoding)
            {
                notifyIcon.ShowBalloonTip(1000, "Recording", "Mouse and keyboard input is now being recorded", ToolTipIcon.None);
                isRecoding = true;
                recordFile = new RecordFile(Application.StartupPath + "\\Profile1.rec", true);
                recordFile.EndOffStream += new EventHandler(StopReplay);
                startTime = keyInfo.time;
                return;
            }
            //Stop recording
            else if (key == Keys.F1 && Keys.Control == Control.ModifierKeys && isRecoding)
            {
                notifyIcon.ShowBalloonTip(1000, "Stoped recording", "Input recording has now stopped", ToolTipIcon.None);
                isRecoding = false;
                startTime = 0;
                recordFile.WriteLControlUp();
                recordFile.StopWrite();
                return;
            }
            #endregion
            #region Strart & Stop Replay
            if (!isRecoding)
            {
                //Start replay
                if (key == Keys.F1 && Keys.Shift == Control.ModifierKeys && !isReplaying)
                {
                    notifyIcon.ShowBalloonTip(1000, "Replaying", "Mouse and keyboard input is now being replayed", ToolTipIcon.None);
                    isReplaying = true;
                    recordFile.StartRead();
                    currentInstruction = recordFile.ReadNextInstruction();
                    timerReplay.Start();
                    return;
                }
                //Stop replay
                else if (key == Keys.F1 && Keys.Shift == Control.ModifierKeys && isReplaying)
                {
                    StopReplay(new object(),new EventArgs());
                    return;
                }
            }
            #endregion

            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 0;
                instruction.timing = keyInfo.time - startTime;
                instruction.keyCode = keyInfo.vkCode;
                instruction.scanCode = keyInfo.scanCode;             
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void KeyUp(KBLLHOOKSTRUCT keyInfo)
        {
            Keys key = (Keys)keyInfo.vkCode;

            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 1;
                instruction.timing = keyInfo.time - startTime;
                instruction.keyCode = keyInfo.vkCode;
                instruction.scanCode = keyInfo.scanCode;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseMove(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 6;
                instruction.timing = mouseInfo.time - startTime;
                instruction.x = mouseInfo.pt.x;
                instruction.y = mouseInfo.pt.y;
                instruction.mouseData = mouseInfo.mouseData;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseLBDown(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 2;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseLBUp(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 3;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseRBDown(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 4;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseRBUp(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 5;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseXDown(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 7;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseXUp(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 8;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseMBDown(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 9;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseMBUp(MSLLHOOKSTRUCT mouseInfo)
        {
            if (isRecoding)
            {
                RecordInstruction instruction = new RecordInstruction();
                instruction.instructionType = 10;
                instruction.timing = mouseInfo.time - startTime;
                recordFile.WriteInstruction(instruction);
            }
        }

        private static void MouseScroll(MSLLHOOKSTRUCT mouseInfo)
        {
            RecordInstruction instruction = new RecordInstruction();
            instruction.instructionType = 11;
            instruction.timing = mouseInfo.time - startTime;
            instruction.mouseData = mouseInfo.mouseData;
            recordFile.WriteInstruction(instruction);
        }
        #endregion

        private static void StopReplay(object sender, EventArgs e)
        {
            recordFile.StopRead();
            isReplaying = false;
            notifyIcon.ShowBalloonTip(1000, "Stoped replay", "Input replay has now stopped", ToolTipIcon.None);
            timerReplay.Stop();
        }

        private void timerReplay_Tick(EventTimerArgs e)
        {
            if (isReplaying)
            {
                while(currentInstruction.timing <= e.ElapsedMilliSeconds)
                {
                    if (currentInstruction.instructionType == 0)
                    {
                        MessageSender.SendKeyInput((ushort)currentInstruction.keyCode, (ushort)currentInstruction.scanCode, 0, 0);
                    }
                    else if (currentInstruction.instructionType == 1)
                    {
                        MessageSender.SendKeyInput((ushort)currentInstruction.keyCode, (ushort)currentInstruction.scanCode, 2, 0);
                    }
                    else if (currentInstruction.instructionType == 2)
                    {
                        MessageSender.SendLMBDown(0);
                    }
                    else if (currentInstruction.instructionType == 3)
                    {
                        MessageSender.SendLMBUp(0);
                    }
                    else if (currentInstruction.instructionType == 4)
                    {
                        MessageSender.SendRMBDown(0);
                    }
                    else if (currentInstruction.instructionType == 5)
                    {
                        MessageSender.SendRMBUp(0);
                    }
                    else if (currentInstruction.instructionType == 6)
                    {
                        MessageSender.SendMouseMove(currentInstruction.x, currentInstruction.y, 0);
                    }
                    else if (currentInstruction.instructionType == 7)
                    {
                        MessageSender.SendXDown(0);
                    }
                    else if (currentInstruction.instructionType == 8)
                    {
                        MessageSender.SendXUp(0);
                    }
                    else if (currentInstruction.instructionType == 9)
                    {
                        MessageSender.SendMBDown(0);
                    }
                    else if (currentInstruction.instructionType == 10)
                    {
                        MessageSender.SendMBUp(0);
                    }
                    else if (currentInstruction.instructionType == 11)
                    {
                        MessageSender.SendMouseWheel(currentInstruction.mouseData,0);
                    }
                    currentInstruction = recordFile.ReadNextInstruction();
                }
            }
        }
    }
}
