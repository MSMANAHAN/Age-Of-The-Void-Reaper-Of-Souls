﻿using UnityEngine;
using System.Collections;
public class ChaseState : IEnemyState
{
    private readonly StatePatternEnemy enemy;//Keeps track of the current enemy state
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject player;
    bool attack = true;
    [SerializeField]
    private float speed;

    public float DistanceToTarget = 0;
    public ChaseState(StatePatternEnemy unitedStatePattern)//constructor
    {
        enemy = unitedStatePattern;//when an instance is created pass in the current enemy state pattern.
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to patrol state from chase state");//can't go to patrol from chase
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;//transfer to alert state
        Debug.Log("Chase -> Alert");//Can't transition to same state, but must have this function for the interface
    }

    public void ToChaseState()
    {
        Debug.Log("In Chase State");//Can't transition to same state, but must have this function for the interface
    }

    private void Look()//In patrol state, ray cast 20 units from his eyes
    {
        RaycastHit hit;
        Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;//Gives direction from the eyes to the target
        if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))//If the enemy does see player go into chase state chasing the player
        {
            enemy.chaseTarget = hit.transform;//Make the target of the chase the thing the ray cast hit
            ToChaseState();//transfer to chase state
            Debug.DrawRay(enemy.eyes.transform.position, enemyToTarget, Color.blue);
            DistanceToTarget = hit.distance;
        }
        else//If out of line of sight
        {
            ToAlertState();//transfer back to alert state
        }
    }

    private void Chase()
    {
        //enemy.meshRendererFlag.material.color = Color.red;//While in the alert state turn red(not imperative)
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;//set destination to the position of the target being chased
        //enemy.navMeshAgent.Resume();
    }

}