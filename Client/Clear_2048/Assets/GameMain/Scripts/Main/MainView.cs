using LGF.MVC;
using LGF.Path;
using UnityEngine;
using UnityEngine.UI;

public class MainView : LGFView
{
    private Image TImge;

    protected override void OnInit(string uiName, int uiDepth, object userData)
    {
        foreach (var info in (int[])userData)
        {
            Debug.Log(info);
        }

        TImge = transform.Find("TImg").GetComponent<Image>();
        if (TImge != null)
        {
            TImge.sprite = Game.ResManager.LoadSprite("Main", "Img_01");
        }
    }

    public override void OnClose()
    {
    }

    public override void OnDestroy()
    {
    }
}