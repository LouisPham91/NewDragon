using System;
using System.IO;
using System.Text;

namespace Dragon.Controller.DeviceControl.ScrcpyNet.Services
{
    public class BigEndianBinaryWriter : IDisposable
    {
        private readonly BinaryWriter _writer;

        public BigEndianBinaryWriter(Stream output)
        {
            _writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
        }

        public void Write(byte value) => _writer.Write(value);
        public void Write(byte[] buffer) => _writer.Write(buffer);

        public void Write(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void Write(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void Write(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void Write(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}