using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginController : MonoBehaviour
{
    public TMP_InputField UserIF;
    public TMP_InputField PassIF;
    public TextMeshProUGUI FeedbackTxt;
    public Button Login;
    public float timer = 0;
    public bool tried = false;

    private void OnEnable()
    {
        FeedbackTxt.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Login.onClick.AddListener(delegate {
            print("Click login");
            StartCoroutine(SingletonManager.singleton.RS.LoginRequest(UserIF.text, PassIF.text, FeedbackTxt));
        });

        UserIF.text = PlayerPrefs.GetString("Correo");
        PassIF.text = PlayerPrefs.GetString("Password");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 2.5f && !tried)
        {
            if (UserIF.text.Length > 0)
            {
                StartCoroutine(SingletonManager.singleton.RS.LoginRequest(UserIF.text, PassIF.text, FeedbackTxt));
            }
            tried = true;
        }
        timer += Time.fixedDeltaTime;
    }
}
