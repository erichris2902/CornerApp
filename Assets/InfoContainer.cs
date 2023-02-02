using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoContainer : MonoBehaviour
{
    public ZonaInfo zonaInfo;
    public TextMeshProUGUI header;
    public TextMeshProUGUI day;
    public TextMeshProUGUI month;

    // Start is called before the first frame update
    void Start()
    {
        header.text = zonaInfo.name;
        day.text = zonaInfo.day.ToString();
        month.text = zonaInfo.month.ToString();    
    }
}
