using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayerCam : MonoBehaviour
{
    public Transform playercam;

    void Start()
    {
        playercam = FindObjectOfType<OrbitCamera>().transform;
    }
    
    void Update()
    {
        transform.LookAt(playercam);
    }
}
