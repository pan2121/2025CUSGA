using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGameCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
    public void Setting()
    {
        Debug.Log("打开设置");
    }
    public void Resume()
    {
        Debug.Log("重新加载场景");
    }
    public void ExitGame()
    {
        Debug.Log("回到主页面");
    }
}
