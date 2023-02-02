using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class ScreenManager : MonoBehaviour
{
    public MyScreen current_screen = MyScreen.NONE;
    Popup current_popup = Popup.NONE;
    [SerializeField] MyScreen start_screen;
    [SerializeField] Popup start_popup;
    [SerializeField] List<MyScreen> screen_history = new List<MyScreen>();

    public event Action<MyScreen> OnScreenChange;
    public event Action<Popup> OnPopupShow;
    public event Action OnPopupClose;
    [SerializeField] UnityEvent OnCompleteScreenChange;
    [SerializeField] UnityEvent OnCompletePopupShow;

    private void Awake()
    {
        //Register event for swapping screens
        OnScreenChange += _ChangeScreen;
        OnPopupShow += _ShowPopup;
    }

    private void Start()
    {
        //Load start_screen
        OnScreenChange?.Invoke(start_screen);
        OnPopupClose?.Invoke();
    }

    //Event loaded OnScreenChange
    private void _ChangeScreen(MyScreen selected_screen)
    {
        if (current_screen == selected_screen)
            return;
        current_screen = selected_screen;
        screen_history.Add(current_screen);
        OnCompleteScreenChange?.Invoke();
    }

    //Change screen to the desired one
    public void ChangeScreen(MyScreen selected_screen)
    {
        OnScreenChange?.Invoke(selected_screen);
    }


    //Load the screen that was before the current one
    public void LoadLastScreen()
    {
        if (screen_history.Count > 1)
        {
            screen_history.RemoveAt(screen_history.Count - 1);
            ChangeScreen(screen_history[screen_history.Count - 1]);
            screen_history.RemoveAt(screen_history.Count - 1);
        }
    }

    public void RegisterScreen()
    {
        OnScreenChange?.Invoke(current_screen);
    }

    public void RegisterPopup()
    {
        OnPopupShow?.Invoke(current_popup);
    }

    private void _ShowPopup(Popup selected_popup)
    {
        if (current_popup == selected_popup && current_popup != Popup.NONE)
            return;
        current_popup = selected_popup;
        OnCompletePopupShow?.Invoke();
    }

    //Change screen to the desired one
    public void ShowPopup(Popup selected_popup)
    {
        OnPopupShow?.Invoke(selected_popup);
    }

    private void _HidePopup()
    {
        current_popup = Popup.NONE;
        OnPopupClose?.Invoke();
    }

    //Change screen to the desired one
    public void HidePopup()
    {
        OnPopupClose?.Invoke();
    }

    public void SesionReset()
    {
        screen_history = new List<MyScreen>();
        screen_history.Add(current_screen);
    }

    private void Update()
    {
    }
}

[Serializable]
public enum MyScreen
{
    NONE,
    LOGIN,
    BANKS,
    DATABANKS,
    PENDIENTES,

}

[Serializable]
public enum Popup
{
    NONE,
}