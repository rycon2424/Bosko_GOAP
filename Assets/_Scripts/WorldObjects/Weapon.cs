using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    void Start()
    {
        GWorld.Instance.AddWeapon(gameObject);
    }
}
