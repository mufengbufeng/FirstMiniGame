using LGF.MVC;
using UnityEngine;

public class MainView : LGFView
{
    protected override void OnInit(string uiName, int uiDepth, object userData)
    {
        foreach (var info in (int[])userData)
        {
            Debug.Log(info);
        }
    }

    public override void OnClose()
    {
    }

    public override void OnDestroy()
    {
    }
}