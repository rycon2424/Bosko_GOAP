using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool dead;
    public float speed = 3;

    private CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        cc.Move(new Vector3(x, 0, z) * Time.deltaTime * speed);
    }
}
