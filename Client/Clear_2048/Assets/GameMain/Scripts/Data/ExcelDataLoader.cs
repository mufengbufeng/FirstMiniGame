using System.Collections.Generic;
using UnityEngine;

namespace LGF.Data
{
    public static partial class ExcelDataLoader
    {
        public static ExcelTestExData[] TestEx => _excelDataSerialize.TestEx;
        public static ExcelTestEx2Data[] TestEx2 => _excelDataSerialize.TestEx2;

        public static ExcelDataSerialize _excelDataSerialize;
        
        private static readonly Dictionary<int, ExcelTestExData> _TestExDict = new();
        private static readonly Dictionary<int, ExcelTestEx2Data> _TestEx2Dict = new();

        private static void InitDict<T>(Dictionary<int, T> dict, T[] datas) where T : AExData
        {
            dict.Clear();
            if (datas == null) return;
            foreach (var data in datas)
            {
                if (dict.ContainsKey(data.id))
                {
                    Debug.LogError($"{0} id:{data.id} is already exist");
                    return;
                }
                else
                {
                    dict.Add(data.id, data);
                }
            }
        }

        public static void Sericalize(string jsonData)
        {
            _excelDataSerialize = Json.FromJson<ExcelDataSerialize>(jsonData);
            
            if (_excelDataSerialize == null)
            {
                Debug.LogError("ExcelDataSerialize is null");
            }

            InitDict(_TestExDict, _excelDataSerialize.TestEx);
            InitDict(_TestEx2Dict, _excelDataSerialize.TestEx2);

            Game.EventManager.Trigger(GameEvent.ExcelDataLoadedFinish);
        }

        public static ExcelTestExData GetTestExData(int id) => _TestExDict.ContainsKey(id) ? _TestExDict[id] : null;
        public static ExcelTestEx2Data GetTestEx2Data(int id) => _TestEx2Dict.ContainsKey(id) ? _TestEx2Dict[id] : null;
    }
}