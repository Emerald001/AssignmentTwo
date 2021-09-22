using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject bullet;
    private List<GameObject> activeObjectPool = new List<GameObject>();
    private List<GameObject> inactiveObjectPool = new List<GameObject>();
    public int poolAmount;

    private void Start()
    {
        GameObject tmp;
        for (int i = 0; i < poolAmount; i++)
        {
            tmp = Instantiate(bullet);
            tmp.SetActive(false);
            inactiveObjectPool.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            if (!inactiveObjectPool[i].activeInHierarchy)
            {
                inactiveObjectPool[i].SetActive(true);
                return inactiveObjectPool[i];
            }
        }

        GameObject newItem = AddNewItemToPool();
        newItem.SetActive(true);

        return newItem;
    }

    private GameObject AddNewItemToPool()
    {
        GameObject instance = Instantiate(bullet);
        inactiveObjectPool.Add(instance);
        instance.SetActive(false);
        return instance;
    }

    public void ReturnItemToInactivePool(GameObject _item)
    {
        if (activeObjectPool.Contains(_item))
        {
            activeObjectPool.Remove(_item);
        }
        _item.SetActive(false);
        inactiveObjectPool.Add(_item);
    } 
}
