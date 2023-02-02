using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BanksInfoController : MonoBehaviour
{
    public TextMeshProUGUI TotalDia;
    public TextMeshProUGUI TotalMes;
    public GameObject container;
    public GameObject infoPrefab;
    public GameObject infoPrefabHeader;
    public List<GameObject> instances = new List<GameObject>();
    public BankLogoController BLC;

    public TextMeshProUGUI LastMonth;
    public TextMeshProUGUI TwoMonthsAgo;

    public Button nextDay;
    public Button lastDay;

    public TextMeshProUGUI lastMonthTxt;
    public TextMeshProUGUI LastMonth2Txt;

    public Button pendientesBtn;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (SingletonManager.singleton)
        {
            StartCoroutine(SingletonManager.singleton.RS.GetBankInfoRequest(SingletonManager.singleton.DM.selectedBank.id.ToString(), this));
        }
        TotalDia.text = "";
        TotalMes.text = "";

        for (int i = 0; i < instances.Count; i++)
        {
            Destroy(instances[i]);
        }
        instances.Clear();
    }

    private void Start()
    {
        lastDay.onClick.AddListener(delegate {
            StartCoroutine(SingletonManager.singleton.RS.GetBankInfoWithDateRequest(SingletonManager.singleton.DM.selectedBank.id.ToString(), this, SingletonManager.singleton.CM.GetYesterday()));
        });
        nextDay.onClick.AddListener(delegate {
            StartCoroutine(SingletonManager.singleton.RS.GetBankInfoWithDateRequest(SingletonManager.singleton.DM.selectedBank.id.ToString(), this, SingletonManager.singleton.CM.GetTomorrow()));
        });
        pendientesBtn.onClick.AddListener(delegate {
            SingletonManager.singleton.SM.ChangeScreen(MyScreen.PENDIENTES);
        });
    }

    private void OnDisable()
    {
        BLC.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        BLC.bankInfo = SingletonManager.singleton.DM.selectedBank;

        TotalDia.text = SingletonManager.singleton.CM.total_day.ToString();
        TotalMes.text = SingletonManager.singleton.CM.total_month.ToString();

        if(SingletonManager.singleton.CM.total_last_month < SingletonManager.singleton.CM.total_month)
        {
            LastMonth.color = Color.green;
        }
        else
        {
            LastMonth.color = Color.red;
        }

        if (SingletonManager.singleton.CM.total_two_months_ago < SingletonManager.singleton.CM.total_month)
        {
            TwoMonthsAgo.color = Color.green;
        }
        else
        {
            TwoMonthsAgo.color = Color.red;
        }

        LastMonth.text = SingletonManager.singleton.CM.total_last_month.ToString();
        TwoMonthsAgo.text = SingletonManager.singleton.CM.total_two_months_ago.ToString();

        for (int i = 0; i < instances.Count; i++)
        {
            Destroy(instances[i]);
        }
        instances.Clear();

        ZonaInfo headers = new ZonaInfo();
        headers.name = "Zona/Region";
        headers.day = SingletonManager.singleton.CM.day + "/" + SingletonManager.singleton.CM.month + "/"  + SingletonManager.singleton.CM.year;
        headers.month = SingletonManager.singleton.CM.s_month;

        GameObject _header = Instantiate(infoPrefabHeader);
        _header.GetComponent<InfoContainer>().zonaInfo = headers;
        _header.transform.parent = container.transform;
        _header.transform.localScale = Vector3.one;
        instances.Add(_header);

        for (int i = 0; i < SingletonManager.singleton.CM.infoZone.Count; i++)
        {
            GameObject info = Instantiate(infoPrefab);
            ZonaInfo infoZone = SingletonManager.singleton.CM.infoZone[i];
            info.GetComponent<InfoContainer>().zonaInfo = infoZone;
            info.transform.parent = container.transform;
            info.transform.localScale = Vector3.one;
            instances.Add(info);
        }

        ZonaInfo pendientes = new ZonaInfo();
        pendientes.name = "Pendientes";
        pendientes.day = SingletonManager.singleton.CM.total_day_pendientes.ToString();
        pendientes.month = "";

        GameObject _pendientes = Instantiate(infoPrefabHeader);
        _pendientes.GetComponent<InfoContainer>().zonaInfo = pendientes;
        _pendientes.transform.parent = container.transform;
        _pendientes.transform.localScale = Vector3.one;
        instances.Add(_pendientes);
    }
}
