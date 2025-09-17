using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : GeneralStats 
{
    GameObject target;
    bool attackActivated = false;

    // Use this for initialization
    protected override void Start () 
	{
        attackActivated = false;
        base.Start();
        Init();
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
        base.Update();
        NPCBehaviour();
	}

    void NPCBehaviour()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
            if (!action)
            {
                CheckNPCSelection();
            }
            else
            {
                if (ActionManager.EndOfAction())
                {
                    action = false;
                    attackActivated = false;
                }
                else
                {
                    CheckNPCMentality();
                }
            }
        }
        else
        {
            CheckMovementDecision();
        }
    }

    void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        //FindPath(targetTile);
        FindNPCPath(targetTile);
        //Debug.Log("Target Tile " + targetTile.row + ", " + targetTile.column);
    }

    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;

        //Debug.Log("ENCONTRÉ UN OBJETIVO: " + target);
    }

    void CheckNPCSelection()
    {
        FindNearestTarget();
        CalculatePath();
        FindSelectableTiles(); // Quitar en la versión final
    }

    void CheckMovementDecision()
    {
        Move();
        action = true;
        ActionManager.SetAttack(true);
    }

    void CheckNPCMentality()
    {
        if (!attackActivated)
        {
            ActionManager.CalculateRangeOfAttack(stats.range);
            attackActivated = true;
        }
        
        if (ActionManager.targetUnits.Count > 0)
        {
            PlayerStats bestTarget = (PlayerStats)ActionManager.targetUnits[0];
            foreach (TacticsMove t in ActionManager.targetUnits)
            {
                if (((PlayerStats)t).health < bestTarget.health)
                {
                    bestTarget = (PlayerStats)t;
                }
            }
            ActionManager.InflictDamage(gameObject, bestTarget.gameObject, false);
        }
        else
        {
            ActionManager.SetWait(true);
        }
    }
}
