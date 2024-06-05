using System;
using LGF.Path;
using UnityEngine;

[Serializable]
public class DataConfig_Json
{
    public string url;
}

public class DataManager : MonoSingleton<DataManager>
{
    protected override void Awake()
    {
        base.Awake();
        Init("DataConfig");
    }

    public void Init(string fileName)
    {
        var res = Game.ResManager.LoadAsset<TextAsset>(PathConfig.GetJsonDataPath(fileName));
        Debug.Log(res);
    }
}