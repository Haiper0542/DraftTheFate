using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagerManager : MonoBehaviour {

    public Transform pooler;
    public Transform damageFont;

    public static DamagerManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void MakeToast(string str, Vector3 postion, Color color)
    {
        Transform newToast = Instantiate(damageFont, postion, Quaternion.identity, pooler);
        Text newText =  newToast.GetChild(0).GetComponent<Text>();
        newText.text = str;
        newText.color = color;
        Destroy(newToast.gameObject, 1.5f);
    }
}
