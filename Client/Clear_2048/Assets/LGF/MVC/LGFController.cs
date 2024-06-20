using LGF.Event;
using UnityEngine;

namespace LGF.MVC
{
    public class LGFController : MonoSingleton<LGFController>
    {
        protected EventManager EventManager;

        protected override void Awake()
        {
            base.Awake();
            EventManager = EventManager.Instance;
            Debug.LogError("aaaaaaaaaaaaaaaaa");
        }
    }
}