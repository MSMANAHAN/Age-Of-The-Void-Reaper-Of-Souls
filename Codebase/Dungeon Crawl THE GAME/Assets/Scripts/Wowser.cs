﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public enum BossStates
{
    Idle,
    Moving,
    Stomp,
    FireBreath,
    Stunned
}

public class Wowser : MonoBehaviour
{
    bool isCoroutineExecutingone = false;
    bool isCoroutineExecuting = false;
    public float timeToDodgeFire = 1;
    float timeElapsed = 0.0f;
    int bHealth = 3;
    public bool isFireBreath = false;
    Vector3 Position;
    float dist = 0;
    bool ResetTime;
    public BossStates CurrentState = BossStates.Idle;
    public GameObject Mario;
    public Collider wowser;
    public GameObject arena;
    public BasePlayer mariocontroller;

    void Start()
    {

    }

    void Update()
    {
        if (bHealth <= 0)
        {
            SceneManager.LoadScene("KillWowser");
        }
        dist = Vector3.Distance(transform.position, Mario.transform.position);

        Position = transform.position;
        switch (CurrentState)
        {
            case BossStates.Idle:
                IdleState();
                break;
            case BossStates.Moving:
                MovingState();
                break;
            case BossStates.Stomp:
                StompState();
                break;
            case BossStates.FireBreath:
                FirebreathState();
                break;
            case BossStates.Stunned:
                StunndedState();
                break;
            default:
                break;
        }
    }
    void IdleState()
    {


        //controls duration of IdleState // change hard coded 1 eventually
        if (timeElapsed > 1)


            //controls duration of IdleState // change hard coded 1 eventually
            if (timeElapsed > 5)

            {
                CurrentState = BossStates.Moving;
                timeElapsed = 0;
            }

        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;
            continue;
        }


    }


    void MovingState()
    {
        if (GetComponent<NavMeshAgent>().remainingDistance < 4f)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<NavMeshAgent>().speed = 0.5f;
        }
        else if (GetComponent<NavMeshAgent>().remainingDistance >= 5f)
        {
            GetComponent<NavMeshAgent>().speed = 3;        }

        GetComponent<NavMeshAgent>().SetDestination(Mario.transform.position);
    }
    void StompState()
    {

    }
    void FirebreathState()
    {
        isCoroutineExecuting = false;
        StartCoroutine(PreFireBreath(timeToDodgeFire));
    }

    void StunndedState()
    {

        int stunDuration = 3;
        if (timeElapsed > stunDuration)
        {
            CurrentState = BossStates.Moving;
            timeElapsed = 0;
        }
        Mario.GetComponent<BasePlayer>().TakeDamage();
        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;
            continue;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Explosive")
        {
            --bHealth;
            Destroy(col.gameObject);
        }
    }
    public void SetCurrentState(BossStates NewState)
    {
        CurrentState = NewState;
    }
    void ResetTimeElapsed()
    {
        timeElapsed = 0;
    }
    IEnumerator PostFireBreath(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;

        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<TriggerFireEvent>().DisableParticleSystem();
        Debug.Log("While loop broken");
        isFireBreath = false;
        isCoroutineExecutingone = false;
        StartCoroutine(WaitTransitionState(1.5f));

        isCoroutineExecuting = false;
    }
    IEnumerator PreFireBreath(float seconds)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        GetComponentInChildren<TriggerFireEvent>().EnableParticleSystem();
        yield return new WaitForSeconds(seconds);
        isFireBreath = true;

        isCoroutineExecuting = false;

        StartCoroutine(PostFireBreath(1.0f));
        isCoroutineExecuting = false;
    }
    IEnumerator WaitTransitionState(float seconds)
    {
        if (isCoroutineExecutingone)
            yield break;
        isCoroutineExecutingone = true;
        yield return new WaitForSeconds(seconds);
        CurrentState = BossStates.Moving;
        isCoroutineExecutingone = false;
    }
}