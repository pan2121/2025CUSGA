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
        Debug.Log("������");
    }
    public void Resume()
    {
        Debug.Log("���¼��س���");
    }
    public void ExitGame()
    {
        Debug.Log("�ص���ҳ��");
    }
}
