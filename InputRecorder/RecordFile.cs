using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InputRecorder
{
    public struct RecordInstruction
    {
        public uint timing;//timing of the instruction after replay start
        public byte instructionType;//Type of instruction : 0=KeyDown, 1=KeyUp, 2=LMBDown, 3=LMBUp, 4=RMBDown, 5=RMBUp, 6=MouseMove, 7=XDown, 8= XUp, 9=MBDown, 10=MBUo, 11=MouseWheel
        public uint keyCode;//enum KeyCode of the pressed or released key, NULL for mouse instructions
        public uint scanCode;
        public int x;//X coordinate of the mouse , NULL for keyboard and mouse button instructions
        public int y;//Y coordinate of the mouse , NULL for keyboard and mouse button instructions
        public int mouseData;//scrollData
    }

    public class RecordFile
    {
        public event EventHandler EndOffStream;
        private Stream readStream;
        private BinaryReader fileReader;
        private Stream writeStream;
        private BinaryWriter fileWriter;
        public bool isWriting;
        private string path;

        public RecordFile(string filePath,bool create)
        {
            path = filePath;
            if (create)
            {
                StartWrite();
            }
            else
            {
                StartRead();
            }
        }

        public void StartRead()
        {
            if (File.Exists(path))
            {
                readStream = new FileStream(path, FileMode.Open);
                fileReader = new BinaryReader(readStream);
                ReadHeader();
            }
        }

        private bool ReadHeader()
        {
            if (!isWriting)
            {
                string marker = fileReader.ReadString();
                string timeUnit = fileReader.ReadString();
                return (marker == "REC");
            }
            else
                return false;
        }

        public void StopRead()
        {
            if (!isWriting)
            {
                readStream.Close();
            }
        }
        public RecordInstruction ReadNextInstruction()
        {
            RecordInstruction instruction = new RecordInstruction();
            if (!isWriting)
            {
                if (readStream.Position != readStream.Length)
                {
                    instruction.timing = fileReader.ReadUInt32();
                    instruction.instructionType = fileReader.ReadByte();
                    if (instruction.instructionType == 0 || instruction.instructionType == 1)
                    {
                        instruction.keyCode = fileReader.ReadUInt32();
                        instruction.scanCode = fileReader.ReadUInt32();
                    }
                    else if (instruction.instructionType == 6)
                    {
                        instruction.x = fileReader.ReadInt32();
                        instruction.y = fileReader.ReadInt32();
                    }
                    else if (instruction.instructionType == 11)
                    {
                        instruction.mouseData = fileReader.ReadInt32();
                    }
                }
                else
                    EndOffStream(this,new EventArgs());
            }
            return instruction;
        }

        public void StartWrite()
        {
            isWriting = true;
            writeStream = new FileStream(path, FileMode.Create);
            fileWriter = new BinaryWriter(writeStream);
            WriteHeader();
        }

        private void WriteHeader()
        {
            if (isWriting)
            {
                fileWriter.Write("REC");//Marker string
                fileWriter.Write("ms");//Indicator that timing units are in ms (ignored)
            }
        }

        public void StopWrite()
        {
            if (isWriting)
            {
                isWriting = false;
                writeStream.Close();
            }
        }

        public void WriteInstruction(RecordInstruction instruction)
        {
            if (isWriting)
            {
                fileWriter.Write(instruction.timing);
                fileWriter.Write(instruction.instructionType);
                if (instruction.instructionType == 0 || instruction.instructionType == 1)
                {
                    fileWriter.Write(instruction.keyCode);
                    fileWriter.Write(instruction.scanCode);
                }
                else if (instruction.instructionType == 6)
                {
                    fileWriter.Write(instruction.x);
                    fileWriter.Write(instruction.y);
                }
                if (instruction.instructionType == 11)
                {
                    fileWriter.Write(instruction.mouseData);
                }
            }
        }

        public void WriteLControlUp()
        {
            if (isWriting)
            {
                fileWriter.Write((uint)0);
                fileWriter.Write((byte)1);

                fileWriter.Write((uint)162);
                fileWriter.Write((uint)29);
            }
        }
    }
}
