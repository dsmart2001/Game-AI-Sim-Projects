using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class AIPlayer : Player
{
    // AI References
    public Player playerEnemy;

    // Attack variables
    public float attackRange;
    public float JumpPlatformRange;
    private GameObject nearestItem;

    private float attackOrFlee;
    private List<GameObject> currentStageItems;
    

    [Task]
    private bool ItemNearby()
    {

        return false;
    }

    [Task]
    private bool IsWeak()
    {

        return false;
    }

    [Task]
    private bool EnemyAhead()
    {

        return false;
    }

    [Task]
    private bool EnemyWeak()
    {

        return false;
    }

    [Task]
    private bool EnemyAbove()
    {

        return false;
    }

    [Task]
    private bool EnemyInRange()
    {

        return false;
    }

    [Task]
    private bool WaypointAbove()
    {

        return false;
    }

    [Task]
    private bool WaypointAhead()
    {

        return false;
    }
}
