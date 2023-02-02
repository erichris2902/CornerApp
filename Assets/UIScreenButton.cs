using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIScreenButton : MonoBehaviour
{
    public MyScreen next_screen;
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate {
            SingletonManager.singleton.SM.ChangeScreen(next_screen);
        });
    }
}
