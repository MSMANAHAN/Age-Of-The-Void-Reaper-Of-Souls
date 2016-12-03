﻿using UnityEngine;
using System.Collections;

public enum playerStates
{
    normal,
    throwing
}


public class BasePlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public int HP = 100;
    public float speed = 6.0F;
    public float sprintSpeed = 10.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float rotationSpeed = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    public Rigidbody marioRb;
    public Rigidbody wowserRb;
    public playerStates currentState = playerStates.normal;
    public int numberOfJumps = 3;
    private int currentJump = 3;
    public bool canGrabTail = false;
    private bool tailGrabbed = false;
    public float tossSpeed;
    public GameObject getTail;
    public GameObject Wowser;

    private bool hasThrown = false;



    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        switch (currentState)
        {
            case playerStates.normal:
                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
                    moveDirection = transform.TransformDirection(moveDirection);
                }
                break;
            case playerStates.throwing:
                moveDirection = new Vector3(0, 0, 0);
                moveDirection = transform.TransformDirection(moveDirection);
                break;
            default:
                break;
        }

        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                moveDirection *= sprintSpeed;
            else
                moveDirection *= speed;

            currentJump = numberOfJumps;
        }

        if (Input.GetKeyDown("space") && currentJump > 0)
        {
            currentJump--;
            moveDirection.y = jumpSpeed;
        }

        if (Input.GetKeyDown(KeyCode.E) && canGrabTail)
        {
            if (tailGrabbed)
            {
                Wowser.transform.parent = null;
                tailGrabbed = false;

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !hasThrown)
                {
                    StartCoroutine("tossTime");

                    //Need to find wowser's front vector and move him in that direction
                }
                currentState = playerStates.normal;
            }
            else
            {
                currentState = playerStates.throwing;
                Wowser.transform.parent = transform;
                tailGrabbed = true;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        controller.Move(moveDirection * Time.deltaTime);
    }

    void TakeDamage(int damage)
    {
        HP -= damage;
    }
    void TakeFireDamage(int damage)

    {
        //TunOnParticle Burn Here.
        //GameObject.Find("")
        TakeDamage(damage);
    }

    IEnumerator tossTime()
    {
        hasThrown = true;
        Wowser.GetComponent<Rigidbody>().isKinematic = false;
        Wowser.GetComponent<Rigidbody>().velocity = (transform.forward * tossSpeed);
        yield return new WaitForSeconds(3);
        Wowser.GetComponent<Rigidbody>().isKinematic = true;
        hasThrown = false;

    }
}