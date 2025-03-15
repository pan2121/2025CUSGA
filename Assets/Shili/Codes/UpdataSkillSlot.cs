using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdataSkillSlot : MonoBehaviour
{
    public Transform player1Object;
    public Transform player2Object;
    public string prefabPath = "Assets/Prefabs/MyPrefab.prefab";
    private GameObject prefab;
    private void Start()
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void OnUpdataSkillSlots()
    {
        //加载一个预制体
        GameObject pf = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        pf.GetComponent<Image>().sprite = null;
        pf.transform.SetParent(player1Object);
    }
}
