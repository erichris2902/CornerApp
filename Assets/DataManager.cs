using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DataManager : MonoBehaviour
{
    public UserInfo actualUserInfo;
    public Calendario calendario = new Calendario();
    public List<BankInfo> banks = new List<BankInfo>();
    public BankInfo selectedBank;
    public List<PendientesInfo> selectedPendientes;

    private void Awake()
    {
        calendario.Meses.Add(1, "Enero");
        calendario.Meses.Add(2, "Febrero");
        calendario.Meses.Add(3, "Marzo");
        calendario.Meses.Add(4, "Abril");
        calendario.Meses.Add(5, "Mayo");
        calendario.Meses.Add(6, "Junio");
        calendario.Meses.Add(7, "Julio");
        calendario.Meses.Add(8, "Agosto");
        calendario.Meses.Add(9, "Septiembre");
        calendario.Meses.Add(10, "Octubre");
        calendario.Meses.Add(11, "Noviembre");
        calendario.Meses.Add(12, "Diciembre");
        calendario.month = DateTime.Now.Month;
        calendario.year = DateTime.Now.Year;
    }

    private void Start()
    {
        StartCoroutine(SingletonManager.singleton.RS.GetBankImgsRequest());
    }

}

[Serializable]
public class PendientesInfo
{
    public string cliente;
    public string ejecutivo;
    public string region;
}

[Serializable]
public class BankInfo
{
    public int id;
    public string name;
    public string url;
}

[Serializable]
public class UserInfo
{
    public int id;
    public string name;
    public string password;
}

[Serializable]
public class Calendario
{
    public Dictionary<int, string> Meses = new Dictionary<int, string>();
    public int year;
    public int month;
}


