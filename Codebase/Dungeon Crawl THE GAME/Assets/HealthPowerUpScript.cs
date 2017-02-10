﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUpScript : MonoBehaviour {
    Animator anim;
    public GameObject HealthPowerUp;
    bool notOpen = true;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }
    void OnTriggerStay(Collider Col)
    {
        if (Col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (notOpen == true)
                {
                    notOpen = false;
                    anim.SetBool("Open", true);
                    Vector3 spawnLocation = (transform.forward * 2) + transform.position;
                    //spawnLocation.x += 4;
                    Instantiate(HealthPowerUp, spawnLocation, Quaternion.identity);
                }
            }
        }
    }
}
