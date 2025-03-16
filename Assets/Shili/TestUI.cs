using ilsFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.GetUIPanel<InHouseUI>();
    }
}
