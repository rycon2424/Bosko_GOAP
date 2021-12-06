using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    void Start()
    {
        GWorld.Instance.AddGuns(gameObject);
    }
}
