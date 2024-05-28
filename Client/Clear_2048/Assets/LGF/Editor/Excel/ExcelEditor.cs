using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ExcelEditor : EditorWindow
{
    [MenuItem("Tools/Excel/Generate")]
    public static void ShowExample()
    {
        ExcelEditor wnd = GetWindow<ExcelEditor>();
        wnd.titleContent = new GUIContent("ExcelWindowTest");
    }

    private static string UXMLROOT = "Assets/LGF/Editor/UI/";
    private static string ExcelRoot = Environment.CurrentDirectory + "/数据表/";

    public void CreateGUI()
    {
        Debug.Log(ExcelRoot);

        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UXMLROOT}ExcelWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();

        var searchRoot = labelFromUXML.Q<VisualElement>("SearchElement");
        var scrollRoot = labelFromUXML.Q<ScrollView>("ScrollRoot");
        var field = searchRoot.Q<TextField>("SearchField");

        // 搜索框发生变化
        field.RegisterValueChangedCallback(Search);

        string[] allExcelFiles = Directory.GetFiles(ExcelRoot, "*.xlsx", SearchOption.AllDirectories);
        foreach (string str in allExcelFiles)
        {
            var newItem = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{UXMLROOT}Item.uxml").Instantiate();

            var nameBtn = newItem.Q<Button>("Btn_Name");
            // 过滤掉路径
            nameBtn.text = Path.GetFileNameWithoutExtension(str);

            nameBtn.clicked += () => { NameBtn_clicked(str); };

            scrollRoot.Add(newItem);
        }

        root.Add(labelFromUXML);
    }

    private void Search(ChangeEvent<string> evt)
    {
        Debug.Log(evt.newValue);
    }

    private void NameBtn_clicked(string str)
    {
        Debug.Log(str);
    }
}