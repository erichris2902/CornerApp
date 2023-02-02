using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
using TMPro;

public class RequestSystem : MonoBehaviour
{
    public string baseURL = "localhost:8000/";
    public string conatiner1 = "localhost:8000/";
    public string container2 = "localhost:8000/";
    public string mediaURL = "localhost:8000/";
    public bool statusComplete = true;
    public GameObject canvas;

    private void Update()
    {
        if (statusComplete == false)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public IEnumerator setImage(string url, RawImage rawImg)
    {
        Debug.Log("Starting..." + mediaURL + url);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(mediaURL + url);
        yield return www.SendWebRequest();
        //Debug.Log(www.downloadHandler.text);
        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            //Debug.Log("Loading... " + url);
            rawImg.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }

    }

    public bool hasInternet()
    {
        if ((Application.internetReachability == NetworkReachability.NotReachable))
        {
            Debug.Log("Error de conexión, tu celular no está conectado a internet");
            return false;
        }
        Debug.Log("Has internet");
        return true;
    }

    public IEnumerator LoginRequest(string user, string password, TextMeshProUGUI FeedbackTxt)
    {
        if (!hasInternet())
        {
            yield break;
        }

        statusComplete = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("USERNAME", user));
        formData.Add(new MultipartFormDataSection("PASSWORD", password));

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "LOGIN", formData))
        {
            www.timeout = 10;
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                JSONObject receivedPackage = (JSONObject)JSON.Parse(www.downloadHandler.text);
                if (receivedPackage["STATUS"] == 1)
                {
                    //FALLO LOGICA
                    Debug.Log(receivedPackage["ERROR_MESSAGE"]);
                    FeedbackTxt.text = receivedPackage["ERROR_MESSAGE"];
                    FeedbackTxt.gameObject.SetActive(true);
                }
                else if (receivedPackage["STATUS"] == 0)
                {
                    //EXITO LOGICA
                    Debug.Log("Login Succesfull");
                    PlayerPrefs.SetString("Correo", user);
                    PlayerPrefs.SetString("Password", password);
                    SingletonManager.singleton.DM.actualUserInfo.id = receivedPackage["USERID"];
                    SingletonManager.singleton.SM.ChangeScreen(MyScreen.BANKS);
                }
            }
            Debug.Log(www.error);
        }
        statusComplete = true;
    }

    public IEnumerator GetBankImgsRequest()
    {
        if (!hasInternet())
        {
            yield break;
        }

        statusComplete = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("ACTION", "GETBANKIMGS"));

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "GETBANKIMGS", formData))
        {
            www.timeout = 10;
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                JSONObject receivedPackage = (JSONObject)JSON.Parse(www.downloadHandler.text);
                if (receivedPackage["STATUS"] == 1)
                {
                    //FALLO LOGICA
                    Debug.Log(receivedPackage["ERROR_MESSAGE"]);
                }
                else if (receivedPackage["STATUS"] == 0)
                {
                    //EXITO LOGICA
                    Debug.Log("GetImgs Succesfull");
                    SingletonManager.singleton.DM.banks.Clear();
                    for (int i = 0; i < receivedPackage["BANKIMGS"].Count; i++)
                    {
                        BankInfo bank = new BankInfo();
                        Debug.Log(receivedPackage["BANKIMGS"]);
                        bank.id = receivedPackage["BANKIMGS"][i]["id"];
                        bank.url = receivedPackage["BANKIMGS"][i]["img_url"];
                        bank.name = receivedPackage["BANKIMGS"][i]["bank"];
                        SingletonManager.singleton.DM.banks.Add(bank);
                    }
                }
            }
            Debug.Log(www.error);
        }
        statusComplete = true;
    }


    public IEnumerator GetBankInfoRequest(string bankId, BanksInfoController BIC)
    {
        if (!hasInternet())
        {
            yield break;
        }

        statusComplete = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("ACTION", "GETBANKINFO"));
        formData.Add(new MultipartFormDataSection("BANK_ID", bankId));

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "GETBANKINFO", formData))
        {
            www.timeout = 10;
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                JSONObject receivedPackage = (JSONObject)JSON.Parse(www.downloadHandler.text);
                if (receivedPackage["STATUS"] == 1)
                {
                    //FALLO LOGICA
                    Debug.Log(receivedPackage["ERROR_MESSAGE"]);
                }
                else if (receivedPackage["STATUS"] == 0)
                {
                    //EXITO LOGICA
                    Debug.Log("GetBankInfo Succesfull");
                    Debug.Log(receivedPackage["date"].Value.ToString().Split('/')[0]);
                    SingletonManager.singleton.CM.day = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[0]);
                    SingletonManager.singleton.CM.month = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[1]);
                    SingletonManager.singleton.CM.year = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[2]);
                    SingletonManager.singleton.CM.s_month = receivedPackage["month"];

                    SingletonManager.singleton.CM.total_day = int.Parse(receivedPackage["DIA"]);
                    SingletonManager.singleton.CM.total_month = int.Parse(receivedPackage["MES"]);
                    SingletonManager.singleton.CM.total_day_pendientes = int.Parse(receivedPackage["DIA_SIN_FACTURA"]);
                    SingletonManager.singleton.CM.total_last_month = int.Parse(receivedPackage["LAST_MONTH"]);
                    SingletonManager.singleton.CM.total_two_months_ago = int.Parse(receivedPackage["TWO_MONTHS_AGO"]);

                    SingletonManager.singleton.CM.infoZone.Clear();
                    for (int i = 0; i < receivedPackage["ZONA"].Count; i++)
                    {
                        BankInfo bank = new BankInfo();
                        Debug.Log(receivedPackage["ZONA"]);
                        ZonaInfo infoZone = new ZonaInfo();
                        infoZone.name = receivedPackage["ZONA"][i]["NOMBRE"];
                        infoZone.day = receivedPackage["ZONA"][i]["DIA"];
                        infoZone.month = receivedPackage["ZONA"][i]["MES"];
                        SingletonManager.singleton.CM.infoZone.Add(infoZone);
                    }
                    BIC.UpdateUI();
                    BIC.BLC.gameObject.SetActive(true);
                }
            }
            Debug.Log(www.error);
        }
        statusComplete = true;
    }

    public IEnumerator GetBankInfoWithDateRequest(string bankId, BanksInfoController BIC, MyDate new_date)
    {
        if (!hasInternet())
        {
            yield break;
        }

        statusComplete = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("ACTION", "GETBANKINFO"));
        formData.Add(new MultipartFormDataSection("BANK_ID", bankId));
        formData.Add(new MultipartFormDataSection("YEAR", new_date.year.ToString()));
        formData.Add(new MultipartFormDataSection("MONTH", new_date.month.ToString()));
        formData.Add(new MultipartFormDataSection("DAY", new_date.day.ToString()));

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "GETBANKINFO", formData))
        {
            www.timeout = 10;
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                JSONObject receivedPackage = (JSONObject)JSON.Parse(www.downloadHandler.text);
                if (receivedPackage["STATUS"] == 1)
                {
                    //FALLO LOGICA
                    Debug.Log(receivedPackage["ERROR_MESSAGE"]);
                }
                else if (receivedPackage["STATUS"] == 0)
                {
                    //EXITO LOGICA
                    Debug.Log("GetBankInfo Succesfull");
                    Debug.Log(receivedPackage["date"].Value.ToString().Split('/')[0]);
                    SingletonManager.singleton.CM.day = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[0]);
                    SingletonManager.singleton.CM.month = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[1]);
                    SingletonManager.singleton.CM.year = int.Parse(receivedPackage["date"].Value.ToString().Split('/')[2]);
                    SingletonManager.singleton.CM.s_month = receivedPackage["month"];

                    SingletonManager.singleton.CM.total_day = int.Parse(receivedPackage["DIA"]);
                    SingletonManager.singleton.CM.total_month = int.Parse(receivedPackage["MES"]);
                    SingletonManager.singleton.CM.total_day_pendientes = int.Parse(receivedPackage["DIA_SIN_FACTURA"]);
                    SingletonManager.singleton.CM.total_last_month = int.Parse(receivedPackage["LAST_MONTH"]);
                    SingletonManager.singleton.CM.total_two_months_ago = int.Parse(receivedPackage["TWO_MONTHS_AGO"]);

                    SingletonManager.singleton.CM.infoZone.Clear();
                    for (int i = 0; i < receivedPackage["ZONA"].Count; i++)
                    {
                        BankInfo bank = new BankInfo();
                        Debug.Log(receivedPackage["ZONA"]);
                        ZonaInfo infoZone = new ZonaInfo();
                        infoZone.name = receivedPackage["ZONA"][i]["NOMBRE"];
                        infoZone.day = receivedPackage["ZONA"][i]["DIA"];
                        infoZone.month = receivedPackage["ZONA"][i]["MES"];
                        SingletonManager.singleton.CM.infoZone.Add(infoZone);
                    }
                    BIC.UpdateUI();
                    BIC.BLC.gameObject.SetActive(true);
                }
            }
            Debug.Log(www.error);
        }
        statusComplete = true;
    }

    public IEnumerator GetPendientesRequest(string bankId, PendientesController PC)
    {
        if (!hasInternet())
        {
            yield break;
        }

        statusComplete = false;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("ACTION", "GETPENDIENTES"));
        formData.Add(new MultipartFormDataSection("BANK_ID", bankId));

        using (UnityWebRequest www = UnityWebRequest.Post(baseURL + "GETPENDIENTES", formData))
        {
            www.timeout = 10;
            yield return www.SendWebRequest();
            if (!www.isNetworkError && !www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                JSONObject receivedPackage = (JSONObject)JSON.Parse(www.downloadHandler.text);
                if (receivedPackage["STATUS"] == 1)
                {
                    //FALLO LOGICA
                    Debug.Log(receivedPackage["ERROR_MESSAGE"]);
                }
                else if (receivedPackage["STATUS"] == 0)
                {
                    //EXITO LOGICA
                    Debug.Log("GetPendientes Succesfull");

                    SingletonManager.singleton.DM.selectedPendientes.Clear();
                    for (int i = 0; i < receivedPackage["POLIZAS_PENDIENTES"].Count; i++)
                    {
                        BankInfo bank = new BankInfo();
                        Debug.Log(receivedPackage["POLIZAS_PENDIENTES"]);
                        PendientesInfo infoPendientes= new PendientesInfo();
                        infoPendientes.cliente = receivedPackage["POLIZAS_PENDIENTES"][i]["CLIENTE"];
                        infoPendientes.ejecutivo = receivedPackage["POLIZAS_PENDIENTES"][i]["EJECUTIVO"];
                        infoPendientes.region = receivedPackage["POLIZAS_PENDIENTES"][i]["DIVISION"];
                        SingletonManager.singleton.DM.selectedPendientes.Add(infoPendientes);
                    }
                    PC.UpdateUI();
                    PC.BLC.gameObject.SetActive(true);
                }
            }
            Debug.Log(www.error);
        }
        statusComplete = true;
    }

}

/*
if (SingletonManager.singleton.DM.actualUser == UserType.OPERATOR)
                    {
                        SingletonManager.singleton.Utl.sendDoozyEvent("GoToOperador");
                    }
                    if (SingletonManager.singleton.DM.actualUser == UserType.CLIENT)
                    {
                        SingletonManager.singleton.Utl.sendDoozyEvent("GoToClient");
                    }
                    */
