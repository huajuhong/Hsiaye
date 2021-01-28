using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Extensions
{
    /// <summary>
    /// 16进制计算
    /// </summary>
    public class HexCalc
    {
        public const int Base = 16;

        public static byte[] ToBytes(string hexString)
        {
            hexString = hexString.Replace("-", string.Empty).Replace(" ", string.Empty);
            byte[] buff = new byte[hexString.Length / 2];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(hexString.Substring(i * 2, 2), Base);
            }
            return buff;
        }
        public static string ToString(byte[] input, bool isSpace = false)
        {
            StringBuilder str = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                str.Append(input[i].ToString("X2"));
                if (isSpace)
                    str.Append(" ");
            }
            return str.ToString();
        }
        public static string Add(string numA, string numB)
        {
            int intSum = Convert.ToInt32(numA, Base) + Convert.ToInt32(numB, Base);
            return Convert.ToString(intSum, Base).ToUpper();
        }
        public static string Mult(string numA, string numB)
        {
            int intMult = Convert.ToInt32(numA, Base) * Convert.ToInt32(numB, Base);
            return Convert.ToString(intMult, Base).ToUpper();
        }
        /// <summary>
        /// 高低位数据转换
        /// </summary>
        /// <param name="bigEndianOrLittleEndian">高位或低位在前数据</param>
        /// <returns></returns>
        public static string GetReverse(string bigEndianOrLittleEndian)
        {
            byte[] bytes = ToBytes(bigEndianOrLittleEndian);
            Array.Reverse(bytes);
            string hexValues = ToString(bytes);
            return hexValues;
        }
    }
}
