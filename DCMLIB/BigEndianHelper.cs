using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DCMLIB
{
    public static class CxEndian
    {
        public static byte[] ReverseForBigEndian(this byte[]
                    byteArray, int startIndex, int count)
        {
            byte[] ret = new byte[count];
            if (BitConverter.IsLittleEndian)
            {
                for (int i = startIndex + (count - 1); i >= startIndex; --i)
                    ret[(startIndex + (count - 1)) - i] = byteArray[i];
            }
            else
            {
                for (int i = 0; i < count; ++i)
                    ret[i] = byteArray[i + startIndex];
            }
            return ret;
        }
    }

    public class BinaryReaderBE : BinaryReader
    {
        private byte[] a16 = new byte[2];
        private byte[] a32 = new byte[4];
        private byte[] a64 = new byte[8];
        public BinaryReaderBE(System.IO.Stream stream) : base(stream) { }

        public override UInt16 ReadUInt16()
        {
            a16 = base.ReadBytes(2);
            Array.Reverse(a16);
            return BitConverter.ToUInt16(a16, 0);
        }
        public override Int16 ReadInt16()
        {
            a16 = base.ReadBytes(2);
            Array.Reverse(a16);
            return BitConverter.ToInt16(a16, 0);
        }
        public override UInt32 ReadUInt32()
        {
            a32 = base.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }
        public override Int32 ReadInt32()
        {
            a32 = base.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }
        public override Single ReadSingle()
        {
            a32 = base.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToSingle(a32, 0);
        }
        public override Double ReadDouble()
        {
            a64 = base.ReadBytes(8);
            Array.Reverse(a64);
            return BitConverter.ToDouble(a64, 0);
        }
    }
}
