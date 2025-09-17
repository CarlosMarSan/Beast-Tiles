using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    static TacticsMove actualTacticsMove;
    public static bool playerActionActivated = false;
    static public bool wait = false;
    static public bool attack = false;
    static public bool heal = false;
    static public List<TacticsMove> targetUnits = new List<TacticsMove>();

    static AudioManager audioManager = null;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerActionActivated = false;
        wait = false;
        attack = false;
        heal = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetActualTacticsMove(TacticsMove newActual)
    {
        actualTacticsMove = newActual;
        //Debug.Log(actualTacticsMove);
    }

    public static void SetWait(bool b)
    {
        if (actualTacticsMove != null)
        {
            wait = b;
        }
    }

    public static void SetAttack(bool b)
    {
        if (actualTacticsMove != null)
        {
            attack = b;
        }
    }

    public static void SetHeal(bool b)
    {
        if (actualTacticsMove != null)
        {
            heal = b;
        }
    }

    public static void InflictDamage(GameObject attackerObject, GameObject defenderObject, bool isPlayer)
    {
        
        if (isPlayer)
        {
            PlayerStats attacker = null;
            NPCStats defender = null;

            if (attackerObject.tag == "Player")
            {
                attacker = attackerObject.GetComponent<PlayerStats>();
            }

            if (defenderObject.tag == "Enemy")
            {
                defender = defenderObject.GetComponent<NPCStats>();
            }

            Debug.Log(attacker + "HA INFLINGIDO A " + defender + " " + attacker.attack + " PUNTOS DE DAÑO");
            defender.health -= attacker.attack;
        }
        else
        {
            NPCStats attacker = null;
            PlayerStats defender = null;

            if (attackerObject.tag == "Enemy")
            {
                attacker = attackerObject.GetComponent<NPCStats>();
            }

            if (defenderObject.tag == "Player")
            {
                defender = defenderObject.GetComponent<PlayerStats>();
            }

            Debug.Log(attacker + " HA INFLINGIDO A " + defender + " " + attacker.attack + " PUNTOS DE DAÑO");
            defender.health -= attacker.attack;
        }

        audioManager.PlaySFX(audioManager.attack);

        attack = false;
        wait = true;
    }

    public static void InflictHealing(GameObject attackerObject, GameObject defenderObject, bool isPlayer)
    {

        if (isPlayer)
        {
            PlayerStats attacker = null;
            PlayerStats defender = null;

            if (attackerObject.tag == "Player")
            {
                attacker = attackerObject.GetComponent<PlayerStats>();
            }

            if (defenderObject.tag == "Player")
            {
                defender = defenderObject.GetComponent<PlayerStats>();
            }

            Debug.Log(attacker + "HA CURADO A " + defender + " " + attacker.magic + " PUNTOS DE DAÑO");
            defender.health = Mathf.Min(defender.health + attacker.magic, defender.maxHealth);
        }
        else
        {
            NPCStats attacker = null;
            NPCStats defender = null;

            if (attackerObject.tag == "Enemy")
            {
                attacker = attackerObject.GetComponent<NPCStats>();
            }

            if (defenderObject.tag == "Enemy")
            {
                defender = defenderObject.GetComponent<NPCStats>();
            }

            Debug.Log(attacker + " HA CURADO A " + defender + " " + attacker.magic + " PUNTOS DE DAÑO");
            defender.health = Mathf.Min(defender.health + attacker.magic, defender.maxHealth);
        }


        audioManager.PlaySFX(audioManager.healing);
        heal = false;
        wait = true;
    }

    public static bool EndOfAction()
    {
        /*if (WaitManagement())
        {
            return true;
        }

        //AttackManagement();

        return false;*/
        return WaitManagement();
    }

    public static bool WaitManagement()
    {
        if (wait)
        {
            actualTacticsMove.GetComponent<Renderer>().material.color = Color.grey;

            TurnManager.EndTurn(actualTacticsMove);

            wait = false;
            attack = false;
            heal = false;
            actualTacticsMove = null;
            targetUnits.Clear();
            GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

            foreach (GameObject t in tiles)
            {
                t.GetComponent<Tile>().objective = false;
            }
            return true;
        }

        return false;
    }

    public static void CalculateRangeOfAttack(int range)
    {
        if (attack)
        {
            actualTacticsMove.GetCurrentTile();
            List<Tile> targetTiles = new List<Tile>(GetRange(range, actualTacticsMove.currentTile));
            
            foreach (Tile t in targetTiles)
            {
                t.objective = true;
                if (!t.walkable)
                {
                    // Para NPC
                    TacticsMove unit = Tile.GetTargetTacticsMove(t.gameObject);
                    //Debug.Log(unit);
                    if (unit != null)
                    {
                        if (unit.gameObject.tag != actualTacticsMove.gameObject.tag)
                        {
                            targetUnits.Add(unit);
                        }

                    }
                }
            }
        }
    }

    public static void CalculateRangeOfHealing()
    {
        if (heal)
        {
            actualTacticsMove.GetCurrentTile();
            actualTacticsMove.currentTile.FindNeighbors(0, actualTacticsMove.currentTile);

            foreach (Tile t in actualTacticsMove.currentTile.neighbors)
            {
                if (!t.walkable)
                {
                    // Para NPC
                    TacticsMove unit = Tile.GetTargetTacticsMove(t.gameObject);
                    //Debug.Log(unit);
                    if (unit != null)
                    {
                        if (unit.gameObject.tag == actualTacticsMove.gameObject.tag)
                        {
                            targetUnits.Add(unit);
                            t.objective = true;
                        }

                    }
                }
            }
        }
    }

    static List<Tile> GetRange(int range, Tile actualTile)
    {
        List<Tile> targetList = new List<Tile>();

        foreach(GameObject go in GenerateGrid.grid)
        {
            Tile t = go.GetComponent<Tile>();
            int calculatedRange = Mathf.Abs(t.row - actualTile.row) + Mathf.Abs(t.column - actualTile.column);
            if (calculatedRange <= range && t!=actualTile)
            {
                targetList.Add(t);
            }
        }

        return targetList;
    }
}
