using System.Collections.Generic;
using OfficeOpenXml;
namespace LGF.Editor
{
    /// <summary>
    ///  Excel列
    /// </summary>
    public class ExcelColumn
    {
        public int col; // 列
        public int index; // 索引
        public string desc; // 描述
        public string name; // 名称
        public string type; // 类型
        public int typeInt => TypeStrToTypeInt[type]; // 类型
        public bool isServer; // 是否服务器字段

        static Dictionary<string, int> TypeStrToTypeInt = new()
        {
            { "uint", 1 }, // 无符号整数
            { "int", 2 }, // 整数
            { "short", 3 }, // 短整数
            { "byte", 4 }, // 字节
            { "bool", 5 }, // 布尔
            { "string", 6 }, // 字符串
            { "array", 10 }, // 数组
        };
    }

    public class ExcelTable
    {
        public string name;
        public string sysName;
        public string desc;
        public string ext;
        public int keyCount = 1;

        public List<ExcelColumn> clientCols = new();
        // public List<ExcelColumn> serverCols = new List<ExcelColumn>(); TODO: 服务器字段
        public List<List<string>> dataRows = new();
        
        public void Parse(ExcelWorksheet sheet)
        {
            var rowCount = sheet.Dimension.Rows;
            var colCount = sheet.Dimension.Columns;
            keyCount = 1;
            for (int row = 1; row <= rowCount; row++)
            {
                    
            }
        }
    }
}