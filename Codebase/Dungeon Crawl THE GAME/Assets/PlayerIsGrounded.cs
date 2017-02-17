﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        EventSystem.PlayerGrounded(false);
    }

    // Update is called once per frame
    bool isGrounded = false;

    List<Collider> collisionList = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Added" + other.gameObject.name);

        collisionList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Removed" + other.gameObject.name);

        collisionList.Remove(other);
    }

    void Update()

    {
        collisionList.RemoveAll(IsEMpty);
        if (isGrounded)
        {
            if (collisionList.Count < 1)
            {
                EventSystem.PlayerGrounded(false);
                isGrounded = false;
            }

        }
        else
        {
            if (collisionList.Count > 0)
            {
                EventSystem.PlayerGrounded(true);
                isGrounded = true;
            }
        }
    }

    private bool IsEMpty(Collider col)
    {
        return col == null;
    }
}