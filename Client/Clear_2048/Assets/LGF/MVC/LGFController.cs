using LGF.Event;

namespace LGF.MVC
{
    public class LGFController : MonoSingleton<LGFController>
    {
        protected EventManager eventManager;

        protected override void Awake()
        {
            base.Awake();
            eventManager = EventManager.Instance;
        }
    }
}