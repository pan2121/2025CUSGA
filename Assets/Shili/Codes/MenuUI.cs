using ilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[UIPanelSetting(EUILayer.Bottom, 0, true, EAssetLoadMode.Resources, "Assets/Shili/Prefab/BasePanel")]
public class MenuUI : UIPanel
{
    /*[AutoUIElement("Panel/GameObject/StartGame")]
    private Button startGameButton;
    [AutoUIElement("Panel/GameObject/Setting")]
    private Button settingButton;
    [AutoUIElement("Panel/GameObject/AboutOur")]
    private Button aboutOurButton;
    [AutoUIElement("Panel/GameObject/ExitGame")]
    private Button exitGameButton;
    public override void InitUIPanel()
    {
        base.InitUIPanel();
       // startGameButton.onClick.AddListener(OnOpenGame);
       // settingButton.onClick.AddListener(OnOpenSetting);
       // aboutOurButton.onClick.AddListener(OnOpenAboutOur);
       // exitGameButton.onClick.AddListener(OnOpenExitGame);
    }
    public override void Open()
    {
        base.Open();
        Debug.Log("打开MenuUI");
    }
    public override void Close()
    {
        base.Close();
        Debug.Log("关闭MenuUI");
    }
    private void OnOpenGame()
    {
        Debug.Log("开始游戏");
    }
    private void OnOpenSetting()
    {
        Debug.Log("打开设置");
    }
    private void OnOpenAboutOur()
    {
        Debug.Log("关于我们");
    }
    private void OnOpenExitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }*/
}
