using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ilsFramework;
using UnityEngine.UI;
[UIPanelSetting(EUILayer.Bottom, 0, true, EAssetLoadMode.Resources, "Assets/Shili/Prefab/InHouseUI")]
public class InHouseUI : UIPanel
{
    //玩家1，十分基础的英语，不想写注释了
    [AutoUIElement("Panel1/GameObject1/Text")]
    private Text player1NameText;
    [AutoUIElement("Panel1/GameObject1/Image")]
    private Image player1Headshot;
    [AutoUIElement("Panel1/GameObject3")]
    private GameObject player1SkillSlotsObject;
    //玩家2
    [AutoUIElement("Panel2/GameObject1/Text")]
    private Text player2NameText;
    [AutoUIElement("Panel2/GameObject1/Image")]
    private Image player2Headshot;

    [AutoUIElement("Panel2/GameObject3")]
    private GameObject player2SkillSlotsObject;
    
    public override void InitUIPanel()
    {
        base.InitUIPanel();
    }
    public override void Open()
    {
        base.Open();
        Debug.Log("打开InHouseUI");
    }
    public override void Close()
    {
        base.Close();
        Debug.Log("关闭InHouseUI");
    }
    public override void Update()
    {
        base.Update();
        //player1HealthImage.fillAmount = 0到1;
        //player1EnergyImage.fillAmount = 0到1;
        //player2HealthImage.fillAmount = 0到1;
        //player2EnergyImage.fillAmount = 0到1;
    }
}
