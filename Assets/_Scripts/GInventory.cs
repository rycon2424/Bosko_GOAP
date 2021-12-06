using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GInventory
{
    [SerializeField]
    List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject g)
    {
        items.Add(g);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach (GameObject i in items)
        {
            if (i.tag == tag)
            {
                return i;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject g)
    {
        items.Remove(g);
    }
}
