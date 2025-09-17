using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour 
{
    static public List<TacticsMove> players = new List<TacticsMove>();
    static public List<TacticsMove> enemies = new List<TacticsMove>();
    static public List<TacticsMove> currentGroup = new List<TacticsMove>();
    static public bool playerTurn = true;
    static public bool passTurn = true;    // Para pasar el turno al otro grupo
    static public bool occupied = false; // Para evitar actuar sobre varias unidades a la vez

    public TextMeshProUGUI turnText;


    // Use this for initialization
    void Start () 
	{
        players = new List<TacticsMove>();
        enemies = new List<TacticsMove>();
        currentGroup = new List<TacticsMove>();
        playerTurn = true;
        //Debug.Log("PlayerTurn: " + playerTurn);
        passTurn = true;
        //Debug.Log("PassTurn: " + passTurn);
        occupied = false;
        //Debug.Log("Occupied: " + occupied);
    }

    void Update()
    {
        
        if (passTurn && BeginMapManager.generationFinished)
        {
            passTurn = false;
            currentGroup.Clear();
            if (playerTurn)
            {
                currentGroup = new List<TacticsMove>(players);
                foreach(TacticsMove t in players)
                {
                    Debug.Log(t);
                }
                turnText.text = "TURNO JUGADOR";
                turnText.color = new Color(0, 0, 255, 255);
            }
            else
            {
                currentGroup = new List<TacticsMove>(enemies);
                foreach (TacticsMove t in enemies)
                {
                    Debug.Log(t);
                }
                turnText.text = "TURNO ENEMIGO";
                turnText.color = new Color(255, 0, 0, 255);
                StartTurn();
            }

        }
    }

    public static void StartTurn()
    {
        if(currentGroup.Count > 0)
        {
            //Debug.Log(currentGroup[0]);
            currentGroup[0].BeginTurn();
        }
    }

    public static void EndTurn(TacticsMove objeto)
    {
        if (objeto.tag == "Player")
        {
            TacticsMove unit = currentGroup.Find(x => x==objeto);
            unit.EndTurn();
            currentGroup.Remove(objeto);
            //Debug.Log("FIN TURNO: " + unit.identificador + " || CURRENTGROUP: " + currentGroup.Count);
        }
        else
        { 
            TacticsMove unit = currentGroup[0];
            unit.EndTurn();
            currentGroup.RemoveAt(0);

            if (currentGroup.Count > 0)
            {
                StartTurn();
            }
            else
            {
                //Debug.Log("FIN TURNO NPC");
                PassTurn();
            }
        }

    }

    public static void AddUnit(TacticsMove unit)
    {

        if (unit != null)
        {
            if (unit.tag == "Player")
            {
                players.Add(unit);
                //Debug.Log(unit + "añadida");
            }
            else if (unit.tag == "Enemy")
            {
                enemies.Add(unit);
                //Debug.Log(unit + "añadida");
            }
        }
    }

    public static void PassTurn()
    {
        if (!occupied)
        {
            /*if (playerTurn)
            {
                foreach (TacticsMove t in players)
                {
                    //t.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            else
            {
                foreach (TacticsMove t in enemies)
                {
                    //t.GetComponent<Renderer>().material.color = Color.green;
                }
            }*/
            playerTurn = !playerTurn;
            passTurn = true;

        }
    }
}
