using LGF.Event;
using LGF.Res;
using LGF.UI;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static EventManager EventManager;
    public static ResManager ResManager;
    public static UIManager UIManager;
    public static DataManager DataManager;

    private void Awake()
    {
        EventManager = this.AddComponent<EventManager>();
        ResManager = this.AddComponent<ResManager>();
        UIManager = this.AddComponent<UIManager>();

        EventManager.Add(GameEvent.YooAssetInitialized, (message) =>
        {
            DataManager = this.AddComponent<DataManager>();
            Load();
        });
    }

    void Start()
    {
        // EventManager.Add(GameEvent.YooAssetInitialized, (message) => { Load(); });
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.J))
    //     {
    //         Load();
    //     }
    // }

    private void Load()
    {
        // UIManager.Open<MainView>();
        int[] a = new[] { 1, 1, 2, 2, 1 };
        // UIManager.Open<MainView>();
        UIManager.Open("MainView", a);
        Debug.Log("aaaa");
    }
}