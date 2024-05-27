using System.IO;
using OfficeOpenXml;
namespace LGF.Editor
{
    public static class ExcelUtil
    {
        public static ExcelTable Read(string fileName, string sheetName = null)
        {
            using FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); // 打开文件
            using ExcelPackage package = new ExcelPackage(stream);  // 创建包
            ExcelWorksheet ws = null;   // 表
            ExcelTable et = new ExcelTable();   // 创建表
            et.name = System.IO.Path.GetFileNameWithoutExtension(fileName); // 文件名
            et.sysName = $"sys_{et.name}";  // 系统名
            if(!string.IsNullOrEmpty(sheetName)) ws = package.Workbook.Worksheets[sheetName];   // 读取指定表
            ws ??= package.Workbook.Worksheets[0]; // 读取第一个表
            et.Parse(ws);// 解析表
            return et; 
        }
        
        
    }
}