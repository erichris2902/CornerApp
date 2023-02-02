using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PendientesController : MonoBehaviour
{
    public TextMeshProUGUI TotalDia;
    public TextMeshProUGUI TotalMes;
    public GameObject container;
    public GameObject infoPrefab;
    public GameObject infoPrefabHeader;
    public List<GameObject> instances = new List<GameObject>();
    public BankLogoController BLC;


    // Start is called before the first frame update
    void OnEnable()
    {
        if (SingletonManager.singleton)
        {
            StartCoroutine(SingletonManager.singleton.RS.GetPendientesRequest(SingletonManager.singleton.DM.selectedBank.id.ToString(), this));
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

    }

    private void OnDisable()
    {
        BLC.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        BLC.bankInfo = SingletonManager.singleton.DM.selectedBank;

        TotalDia.text = SingletonManager.singleton.CM.total_day.ToString();
        TotalMes.text = SingletonManager.singleton.CM.total_month.ToString();



        for (int i = 0; i < instances.Count; i++)
        {
            Destroy(instances[i]);
        }
        instances.Clear();

        ZonaInfo headers = new ZonaInfo();
        headers.name = "Cliente";
        headers.day = "Ejecutivo";
        headers.month = "División";

        GameObject _header = Instantiate(infoPrefabHeader);
        _header.GetComponent<InfoContainer>().zonaInfo = headers;
        _header.transform.parent = container.transform;
        _header.transform.localScale = Vector3.one;
        instances.Add(_header);

        for (int i = 0; i < SingletonManager.singleton.DM.selectedPendientes.Count; i++)
        {
            GameObject info = Instantiate(infoPrefab);
            PendientesInfo infoZone = SingletonManager.singleton.DM.selectedPendientes[i];
            info.GetComponent<PendienteContainer>().pendientesInfo = infoZone;
            info.transform.parent = container.transform;
            info.transform.localScale = Vector3.one;
            instances.Add(info);
        }

        GameObject _pendientes = Instantiate(infoPrefabHeader);
        //_pendientes.GetComponent<PendienteContainer>().pendientesInfo = pendientes;
        _pendientes.transform.parent = container.transform;
        _pendientes.transform.localScale = Vector3.one;
        instances.Add(_pendientes);
    }
}
