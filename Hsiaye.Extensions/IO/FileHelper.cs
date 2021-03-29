using System.IO;

namespace Hsiaye.Extensions
{
    /// <summary>
    /// A helper class for File operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Checks and deletes given file if it does exists.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        public static void DeleteIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns>B，KB，GB，TB</returns>
        public static string GetSizeString(long size)
        {
            string strSize = "";
            long factSize = size;
            if (factSize < 1024.00)
                strSize = factSize.ToString("F2") + " 字节";
            else if (factSize >= 1024.00 && factSize < 1048576)
                strSize = (factSize / 1024.00).ToString("F2") + " KB";
            else if (factSize >= 1048576 && factSize < 1073741824)
                strSize = (factSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (factSize >= 1073741824)
                strSize = (factSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return strSize;
        }
        public static void Write(string filePath, byte[] buffer)
        {
            if (!File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                using (FileStream fs = file.Create())
                {
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
        }
        public static string Read(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
