using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager singleton;
    public DataManager DM;
    public RequestSystem RS;
    public ScreenManager SM;
    public CalendarManager CM;
    public bool debugging = false;
    // Start is called before the first frame update
    void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


}
