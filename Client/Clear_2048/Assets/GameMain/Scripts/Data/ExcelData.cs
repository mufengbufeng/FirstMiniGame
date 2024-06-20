using System.Collections.Generic;
using System;

namespace LGF.Data
{
    [Serializable]
    public abstract class AExData
    {
        public int id;
    }


    [Serializable]
    public class ExcelTestExData : AExData
    {
        /** a **/
        public string str;

        /** d **/
        public List<object> arr;

        /** d **/
        public bool boo;
    }

    [Serializable]
    public class ExcelTestEx2Data : AExData
    {
        /**  **/
        public string str;
    }

    [Serializable]
    public class ExcelDataSerialize
    {
        public ExcelTestExData[] TestEx;
        public ExcelTestEx2Data[] TestEx2;
    }
}