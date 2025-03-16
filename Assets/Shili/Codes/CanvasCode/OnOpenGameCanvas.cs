using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OnOpenGameCanvas : MonoBehaviour
{
    public Text m_Text;
    float countdown;
    private void OnEnable()
    {
        countdown = 3f;
        Invoke("RemoveSelf", 3f);
    }
    private void Update()
    {
        countdown -= Time.deltaTime;
        m_Text.text = Math.Truncate(countdown).ToString();
    }
    public void RemoveSelf()
    {
        Destroy(gameObject);
    }
}
