using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using System.Linq;

[RequireComponent(typeof(AIAiming))]
[RequireComponent(typeof(AIAttacks))]
[RequireComponent(typeof(AIMovement))]

public class AIBrain : MonoBehaviour
{
    public enum AIBehaviours
    {
        Idle,
        Roam,
        Attack,
        Forage,
        Flee
    }

    public AIBehaviours behaviours;

    // Agent health and state values
    public float health;
    private float maxHealth;
    public string agentName;

    // Agent movement and nav values
    public float speed;
    private float rotationSpeed = 5.0f;
    
    public GameObject target;
    private Quaternion targetRotation;
    public float targetRadius = 150f;

    // Agent attack variables
    private Vector3 attackPos;
    public GameObject attackProjectile;
    private float attackTimer;
    public float attackFireRate = 4f;

    // Agent flee variables
    private Vector3 fleePos;

    // Agent forage variables
    private Vector3 foragePos;
    public List<GameObject> foundPickUps;
    public List<GameObject> usedPickups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // BEHAVIOUR: Roam, move between random patrol points
    [Task]
    public void Roam()
    {

    }

    [Task]
    public void Attack()
    {

    }

    [Task] public void Flee()
    {

    }

    [Task] public void Forage()
    {

    }
}
