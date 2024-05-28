using System;
using LGF.MVC;
using UnityEngine;

public class MainView : LGFView
{
    protected override void OnInit(string uiName, int uiDepth, object userData)
    {
        Debug.Log("MainView OnInit");
    }

    public override void OnClose()
    {
        Debug.Log("MainView OnClose");
    }

    public override void OnDestroy()
    {
        Debug.Log("MainView OnDestroy");
    }
}