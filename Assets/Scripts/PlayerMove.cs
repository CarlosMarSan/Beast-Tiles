using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : GeneralStats 
{
    public bool underArrow = false;

    static AudioManager audioManager = null;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Use this for initialization
    protected override void Start () 
	{
        underArrow = false;

        base.Start();
        Init();

        //lightComp = gameObject.AddComponent<Light>();
        //lightComp.color = Color.red;
        //lightComp.intensity = Mathf.PingPong(Time.time, 8);
        //lightComp.enabled = false;
    }
	
	// Update is called once per frame
	protected override void Update () 
	{
        base.Update();
        PlayerBehaviour();
	}

    
    void PlayerBehaviour()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            //PlayerLights();

            CheckIfSelected();

            return;
        }

        if (!moving)
        {
            ActionManager.playerActionActivated = action;

            if (!action)
            {
                CheckUnitSelection();
                CheckUnitSelectionBack();
            }
            else
            {
                CheckMovementDecisionBack();

                if (ActionManager.EndOfAction())
                {
                    action = false;
                }
                else
                {
                    CheckAttackManagement();
                    CheckHealManagement();
                }

            }
        }
        else
        {
            Move();
            action = true; ;
        }
    }

    void CheckIfSelected()
    {
        if (TurnManager.playerTurn && Input.GetMouseButtonUp(0))
        {
            //Debug.Log("¿CONTAINS?: " + TurnManager.currentGroup.Contains(this));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    PlayerMove p = hit.collider.GetComponent<PlayerMove>();
                    if (TurnManager.currentGroup.Contains(p) && !TurnManager.occupied)
                    {
                        p.BeginTurn();
                        UIManager.letAttack = p.canAttack;
                        UIManager.letHeal = p.canHeal;
                        audioManager.PlaySFX(audioManager.selection);
                    }
                }
            }
        }
    }

    void CheckUnitSelection()
    {
        FindSelectableTiles();
        CheckMouse();
        //FindSelectableTiles(); // Quitar al final
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        //MoveToTile(t);
                        FindSelectedPath(t);
                    }
                }
            }
        }
    }

    void CheckUnitSelectionBack()
    {
        if (Input.GetMouseButtonUp(1))
        {
            RemoveSelectableTiles();
            turn = false;
            TurnManager.occupied = false;
            selectableTilesActivated = false;
        }
    }

    void CheckMovementDecisionBack()
    {
        if (Input.GetMouseButtonUp(1))
        {
            this.BackToPreviousTile();
            action = false;
        }
    }

    void CheckAttackManagement()
    {
        if (ActionManager.attack)
        {
            ActionManager.CalculateRangeOfAttack(stats.range);

            CheckAttackConfirm();

            CheckAttackBack();
        }
    }

    void CheckAttackConfirm()
    {
        if (TurnManager.playerTurn && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (gameObject != null && hit.collider.gameObject != null && ActionManager.targetUnits.Find(x => x == hit.collider.gameObject.GetComponent<TacticsMove>()))
                {
                    ActionManager.InflictDamage(gameObject, hit.collider.gameObject, true);
                }
            }
        }
    }

    void CheckAttackBack()
    {
        if (TurnManager.playerTurn && Input.GetMouseButtonUp(1))
        {
            ActionManager.SetAttack(false);

            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (GameObject t in tiles)
            {
                t.GetComponent<Tile>().objective = false;
            }

            ActionManager.targetUnits.Clear();
        }
    }
    
    void CheckHealManagement()
    {
        if (ActionManager.heal)
        {
            ActionManager.CalculateRangeOfHealing();

            CheckHealConfirm();

            CheckHealBack();
        }
    }

    void CheckHealConfirm()
    {
        if (TurnManager.playerTurn && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (gameObject != null && hit.collider.gameObject != null && ActionManager.targetUnits.Find(x => x == hit.collider.gameObject.GetComponent<TacticsMove>()))
                {
                    ActionManager.InflictHealing(gameObject, hit.collider.gameObject, true);
                }
            }
        }
    }

    void CheckHealBack()
    {
        if (TurnManager.playerTurn && Input.GetMouseButtonUp(1))
        {
            ActionManager.SetHeal(false);

            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (GameObject t in tiles)
            {
                t.GetComponent<Tile>().objective = false;
            }

            ActionManager.targetUnits.Clear();
        }
    }
}
