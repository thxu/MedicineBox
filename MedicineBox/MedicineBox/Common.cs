using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineBox
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 十六进制字节转十六进制字符串
        /// </summary>
        /// <param name="bytes">十六进制字节</param>
        /// <param name="len">字节长度</param>
        /// <returns>十六进制字符串</returns>
        public static string ByteToHexStr(byte[] bytes, int len)       /*十六进制字节转十六进制字符串*/
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 十六进制字符串转十六进制字节
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <returns>十六进制字节</returns>
        public static byte[] HexStringToByte(String hex)
        {
            int len = (hex.Length / 2);
            byte[] result = new byte[len];
            char[] achar = hex.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                int pos = i * 2;
                result[i] = (byte)(ToByte(achar[pos]) << 4 | ToByte(achar[pos + 1]));
            }
            return result;
        }

        private static int ToByte(char c)
        {
            byte b = (byte)"0123456789abcdef".IndexOf(c);
            return b;
        }

        /// <summary>
        /// int转byte，最多只能转换2^16的数字
        /// </summary>
        /// <param name="num">要转的int</param>
        /// <returns>十六进制byte</returns>
        public static byte[] IntToHexByte(int num)
        {
            string str = num <= 255 ? Convert.ToString(num, 16).PadLeft(2, '0') : Convert.ToString(num, 16).PadLeft(4, '0');
            return HexStringToByte(str);
        }

        /// <summary>
        /// 十六进制字符串转ascii字符串
        /// </summary>
        /// <param name="data">十六进制字符串</param>
        /// <returns>ascii字符串</returns>
        public static string HexStrToAscii(string data)                /*十六进制字符串转ascii字符串*/
        {
            byte[] bytBuff = new byte[data.Length / 2];
            int index = 0;
            for (int i = 0; i < data.Length; i += 2)
            {
                bytBuff[index] = Convert.ToByte(data.Substring(i, 2), 16);
                ++index;
            }
            return Encoding.Default.GetString(bytBuff);
        }

        /// <summary>
        /// 检查数据是否完整
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="len">长度</param>
        /// <returns>完整=true</returns>
        public static bool IsDataComplete(byte[] data, int len)
        {
            if (data[0] == 0xEE && data[len - 1] == 0xFC)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取有效数据
        /// </summary>
        /// <param name="recv">接收到的数据</param>
        /// <returns></returns>
        public static string GetData(string recv)
        {
            return recv.Substring(2, recv.Length - 2);
        }

        /// <summary>
        /// 判断是否为小时
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static bool IsHour(int hour)
        {
            if (hour >= 1 && hour <= 24)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否为分钟
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static bool IsMinute(int minute)
        {
            if (minute >= 0 && minute <= 59)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 生成flg标记
        /// </summary>
        /// <returns></returns>
        public static byte[] BuildFlg()
        {
            int flg = (int)(DateTime.Now.Ticks % 65535);
            return IntToHexByte(flg);
        }
    }
}
