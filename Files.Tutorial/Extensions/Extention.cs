using System;

namespace Files.Tutorial.Extensions
{
    public static class Extention
    {
        public static string ToBytesCount(this long bytes)
        {
            int unit = 1024;
            string unitStr = "b";
            if (bytes < unit) return string.Format("{0} {1}", bytes, unitStr);
            else unitStr = unitStr.ToUpper();
            int exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return string.Format("{0:##.##} {1}{2}", bytes / Math.Pow(unit, exp), "KMGTPEZY"[exp - 1], unitStr);
        }
    }
}