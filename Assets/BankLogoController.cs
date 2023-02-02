using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BankLogoController : MonoBehaviour
{
    public BankInfo bankInfo;
    public RawImage bankImg;
    public Button BankSelectBtn;
    private void Start()
    {
        UpdateUI();
        BankSelectBtn.onClick.RemoveAllListeners();
        BankSelectBtn.onClick.AddListener(delegate {
            SingletonManager.singleton.DM.selectedBank = bankInfo;
            SingletonManager.singleton.SM.ChangeScreen(MyScreen.DATABANKS);
        });
    }

    private void OnEnable()
    {
        if (SingletonManager.singleton)
        {
            UpdateUI();
            BankSelectBtn.onClick.RemoveAllListeners();
            BankSelectBtn.onClick.AddListener(delegate {
                SingletonManager.singleton.DM.selectedBank = bankInfo;
                SingletonManager.singleton.SM.ChangeScreen(MyScreen.DATABANKS);
            });
        }
    }

    public void UpdateUI()
    {
        StartCoroutine(SingletonManager.singleton.RS.setImage(bankInfo.url, bankImg));
    }
}
