using System.IO;
using LGF.Util;

namespace LGF.Editor
{
    public class ExcelToJson
    {
        public static void Make(string inPath, System.Predicate<string> match, string outDataPath, string urlPath)
        {
            if (!Directory.Exists(outDataPath)) Directory.CreateDirectory(outDataPath); // 创建文件夹
            var files = FileUtil.GetFiles(inPath, "*.xlsx");
            foreach (var file in files)
            {
                if (file.Contains('~')) continue;
                if (match != null && !match(file)) continue;
                var et = ExcelUtil.Read(file);
                if (et.clientCols.Count > 0)
                {
                     
                } 
            }
        }

        static void MakeDataFile(ExcelTable et, string path)
        {
            
        }
    }
}