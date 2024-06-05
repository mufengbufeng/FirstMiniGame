using System.Collections.Generic;
using System.IO;
using System.Text;
using LGF.Util;
using UnityEngine;
using UnityEngine.LowLevelPhysics;

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
                if (match != null && match(file)) continue;
                var et = ExcelUtil.Read(file);
                if (et.clientCols.Count > 0)
                {
                    MakeDataFile(et, outDataPath);
                }
            }

            MakeConfigFile(outDataPath, urlPath);
        }

        static void MakeConfigFile(string path, string urlPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            var files = FileUtil.GetFiles(path, "*.json");
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.EndsWith("Config.json")) continue;
                var url = $"{urlPath}/{PathUtil.BaseName(file)}";
                sb.AppendLine(i != files.Count - 1 ? $"\"{url}\"," : $"\"{url}\"");
            }

            sb.AppendLine("]");
            File.WriteAllText($"{path}/DataConfig.json", sb.ToString());
        }

        static void MakeDTSFile()
        {
        }

        static void MakeDataFile(ExcelTable et, string path)
        {
            Debug.Log($"MakeDataFile{et.name}");
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendFormat($"\"name\":\"{et.name}\",").AppendLine();
            var keys = new List<string>();
            for (int i = 0; i < et.keyCount; i++)
            {
                keys.Add(et.clientCols[i].name);
            }

            sb.Append($"\"keys\":[\"{string.Join("\",\"", keys)}\"],").AppendLine();
            sb.AppendLine("\"rows\":[");

            // 主数据
            for (int rowIndex = 0; rowIndex < et.dataRowCount; rowIndex++)
            {
                sb.Append("{");
                for (int colIndex = 1; colIndex < et.clientCols.Count; colIndex++)
                {
                    var col = et.clientCols[colIndex];
                    var val = et.GetValue(rowIndex, col);
                    //if (val.StartsWith("\"")) val = string.Concat("\"", StringUtil.EncodeUnicode(val.Trim('"')), "\"");
                    if (val.StartsWith("\"")) val = string.Concat("\"", val.Trim('"'), "\"");
                    if (colIndex == 1) sb.AppendFormat("\"{0}\":{1}", col.name, val);
                    else sb.AppendFormat(",\"{0}\":{1}", col.name, val);
                }

                sb.AppendLine(rowIndex != et.dataRowCount - 1 ? "}," : "}");
            }

            sb.AppendLine("]\n}");
            var jsonText = sb.ToString();
            Debug.Log($"{path}/{et.name}.json");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            // 清除文件防止重复
            if (File.Exists($"{path}/{et.name}.json")) File.Delete($"{path}/{et.name}.json");
            File.WriteAllText($"{path}/{et.name}.json", jsonText);
        }
    }
}