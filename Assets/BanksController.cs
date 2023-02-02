using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanksController : MonoBehaviour
{
    public GameObject Container;
    public GameObject BankPrefab;
    public List<GameObject> instances = new List<GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {
        if (SingletonManager.singleton)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                Destroy(instances[i]);
            }
            instances.Clear();
            for (int i = 0; i < SingletonManager.singleton.DM.banks.Count; i++)
            {
                GameObject instance = Instantiate(BankPrefab);
                instance.transform.parent = Container.transform;
                instance.GetComponent<BankLogoController>().bankInfo = SingletonManager.singleton.DM.banks[i];
                instance.transform.localScale = Vector3.one;
                instances.Add(instance);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
