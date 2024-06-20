using System.Collections.Generic;
using System.IO;
using System.Text;
using LGF.Path;
using LGF.Util;
using UnityEditor;
using UnityEngine;
using FileUtil = LGF.Util.FileUtil;


namespace LGF.Editor
{
    public class ExcelToJson
    {
        public static void Make(string inPath, System.Predicate<string> match, string outDataPath, string urlPath)
        {
            List<ExcelTable> etList = new List<ExcelTable>();
            if (!Directory.Exists(outDataPath)) Directory.CreateDirectory(outDataPath); // 创建文件夹
            var files = FileUtil.GetFiles(inPath, "*.xlsx");
            //过滤 ~ 临时文件 
            files = files.FindAll(s => !s.Contains("~"));
            var sb = new StringBuilder();
            sb.AppendLine("{");
            int len = files.Count;
            for (var index = 0; index < files.Count; index++)
            {
                var file = files[index];
                Debug.Log(len);
                if (file.Contains('~'))
                {
                    len--;
                    continue;
                }

                if (match != null && match(file)) continue;
                var et = ExcelUtil.Read(file);
                if (et.clientCols.Count > 0)
                {
                    string infos = (index == len - 1) ? MakeDataFile(et, outDataPath) : $"{MakeDataFile(et, outDataPath)},";
                    Debug.LogError(index);
                    Debug.LogError(len - 1);
                    sb.AppendLine(infos);
                    etList.Add(et);
                }
            }

            sb.AppendLine("}");
            if (!Directory.Exists($"{outDataPath}/ExcelData.json")) Directory.CreateDirectory(outDataPath);
            File.WriteAllText($"{outDataPath}/ExcelData.json", sb.ToString());

            MakeCSFile(etList, PathConfig.ExDataScriptPath);
            MakeConfigFile(outDataPath, urlPath);
            // 刷新unity编辑器
            AssetDatabase.Refresh();
        }

        static void MakeConfigFile(string path, string urlPath)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{\"paths\": [");
            // var files = FileUtil.GetFiles(path, "*.json");
            // for (int i = 0; i < files.Count; i++)
            // {
            //     var file = files[i];
            //     if (file.EndsWith("Config.json")) continue;
            //     var url = $"{urlPath}/{PathUtil.BaseName(file)}";
            //     // sb.AppendLine(i != files.Count - 1 ? $"{{\"path\": \"{url}\"}}," : $"{{\"path\": \"{url}\"}}");
            //     sb.AppendLine(i != files.Count - 1 ? $"\"{url}\"," : $"\"{url}\"");
            // }

            sb.AppendLine($"{{\"path\": \"{path}/ExcelData.json\"}}");

            sb.AppendLine("]}");
            File.WriteAllText($"{path}/DataConfig.json", sb.ToString());
        }

        static void MakeCSFile(List<ExcelTable> etList, string path)
        {
            var sb = new StringBuilder();

            var allExGetSb = new StringBuilder();
            var allExGetByIdSb = new StringBuilder();
            var allExDictSb = new StringBuilder();
            var allExDictInitSb = new StringBuilder();

            var nameSb = new StringBuilder();
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System;");
            sb.AppendLine("namespace LGF.Data\n{");
            sb.AppendLine(ExcelTemplate.AExDataTemplate);
            foreach (var et in etList)
            {
                Debug.Log($"MakeDataFile{et.name}");
                var typeSb = new StringBuilder();
                foreach (var col in et.clientCols)
                {
                    var colType = col.type;
                    if (col.name == "id") continue;
                    if (colType == "array") colType = "List<object>";
                    typeSb.AppendLine($"\t/** {col.desc} **/");
                    typeSb.AppendLine($"\tpublic {colType} {col.name};");
                }

                var excelName = $"Excel{et.name}Data";


                allExGetSb.AppendLine($"public static {excelName}[] {et.name} => _excelDataSerialize.{et.name};");
                allExDictSb.AppendLine($"private static readonly Dictionary<int, {excelName}> _{et.name}Dict = new();");
                allExDictInitSb.AppendLine($"InitDict(_{et.name}Dict, _excelDataSerialize.{et.name});");

                allExGetByIdSb.AppendLine($"public static {excelName} Get{et.name}Data(int id) => _{et.name}Dict.ContainsKey(id)? _{et.name}Dict[id]:null;");

                nameSb.AppendLine($"\t\tpublic {excelName}[] {et.name};");
                sb.AppendFormat(ExcelTemplate.ExcelDataTemplate, et.name, typeSb);
            }

            sb.AppendFormat(ExcelTemplate.ExcelDataSerializeTemplate, nameSb);
            sb.AppendLine("}");

            // 清除文件防止重复
            if (File.Exists($"{path}/ExcelData.cs"))
            {
                File.Delete($"{path}/ExcelData.cs");
            }

            string loader = string.Format(ExcelTemplate.ExcelLoaderTemplate, allExGetSb, allExDictSb, allExDictInitSb, allExGetByIdSb);
            File.WriteAllText($"{path}/ExcelDataLoader.cs", loader, Encoding.UTF8);
            // File.WriteAllText(, Encoding.UTF8);
            File.WriteAllText($"{path}/ExcelData.cs", sb.ToString(), Encoding.UTF8);
        }


        static string MakeDataFile(ExcelTable et, string path)
        {
            Debug.Log($"MakeDataFile{et.name}");
            var sb = new StringBuilder();
            // sb.AppendLine("{");
            // sb.AppendFormat($"\"name\":\"{et.name}\",").AppendLine();
            // var keys = new List<string>();
            // for (int i = 0; i < et.keyCount; i++)
            // {
            //     keys.Add(et.clientCols[i].name);
            // }

            // sb.Append($"\"keys\":[\"{string.Join("\",\"", keys)}\"],").AppendLine();
            sb.AppendLine($"\"{et.name}\":[");

            // 主数据
            for (int rowIndex = 0; rowIndex < et.dataRowCount; rowIndex++)
            {
                sb.Append("{");
                for (int colIndex = 0; colIndex < et.clientCols.Count; colIndex++)
                {
                    var col = et.clientCols[colIndex];
                    var val = et.GetValue(rowIndex, col);
                    //if (val.StartsWith("\"")) val = string.Concat("\"", StringUtil.EncodeUnicode(val.Trim('"')), "\"");
                    if (val.StartsWith("\"")) val = string.Concat("\"", val.Trim('"'), "\"");
                    if (colIndex == 0) sb.AppendFormat("\"{0}\":{1}", col.name, val);
                    else sb.AppendFormat(",\"{0}\":{1}", col.name, val);
                }

                sb.AppendLine(rowIndex != et.dataRowCount - 1 ? "}," : "}");
            }

            sb.AppendLine("]");
            var jsonText = sb.ToString();
            return jsonText;
            // Debug.Lg($"{path}/{et.name}.json");
            // 清除文件防止重复
            // if (File.Exists($"{path}/{et.name}.json")) File.Delete($"{path}/{et.name}.json");

            // if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            // File.WriteAllText($"{path}/{et.name}.json", jsonText);
        }
    }
}