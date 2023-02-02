using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PendienteContainer : MonoBehaviour
{
    public PendientesInfo pendientesInfo;
    public TextMeshProUGUI header;
    public TextMeshProUGUI day;
    public TextMeshProUGUI month;

    // Start is called before the first frame update
    void Start()
    {
        header.text = pendientesInfo.cliente;
        day.text = pendientesInfo.ejecutivo.ToString();
        month.text = pendientesInfo.region.ToString();
    }
}
