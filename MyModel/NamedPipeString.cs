using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModel
{
    public class NamedPipeString
    {
        private Stream NamedPipeStream { get; }

        private Encoding Encoding => Encoding.UTF8;

        // 2 byte = 256, 255
        // head 2 byte is contents max lenth ushort.MaxValue(65535)
        public const int MaxLenth = ushort.MaxValue;

        public const short Quotient = 256;
        public const short Remainder = ushort.MaxValue / Quotient;

        public NamedPipeString(Stream namedPipeStream)
        {
            NamedPipeStream = namedPipeStream;
        }

        public static byte GetQuotientByte(int lenth) => (byte)(lenth / Quotient);
        public static byte GetRemainderByte(int lenth) => (byte)(lenth & Remainder);

        public static int GetQuotientLenth(int b) => b * Quotient;

        /// <summary>
        /// send to
        /// </summary>
        /// <param name="text">message</param>
        /// <returns>total lenth</returns>
        public int Write(string text)
        {
            // text to encoding.utf8
            byte[] textBuffer = Encoding.GetBytes(text);

            // maximum 65535
            if (textBuffer.Length > MaxLenth)
                throw new OverflowException();

            // first byte is lenth quotient
            NamedPipeStream.WriteByte(GetQuotientByte(textBuffer.Length));
            // second byte is lenth remainder
            NamedPipeStream.WriteByte(GetRemainderByte(textBuffer.Length));
            
            // last byte array is text paramster
            NamedPipeStream.Write(textBuffer, 0, textBuffer.Length);
            NamedPipeStream.Flush();

            // first byte (1) + second byte (1) + last byte array
            return 1 + 1 + textBuffer.Length;
        }
        /// <summary>
        /// receive from
        /// </summary>
        /// <returns>message</returns>
        public string Read()
        {
            // first byte is lenth quotient
            int lenth = GetQuotientLenth(NamedPipeStream.ReadByte());
            // second byte is lenth remainder
            lenth += NamedPipeStream.ReadByte();

            byte[] text = new byte[lenth];
            NamedPipeStream.Read(text, 0, text.Length);

            return Encoding.GetString(text);
        }

        
    }
}
