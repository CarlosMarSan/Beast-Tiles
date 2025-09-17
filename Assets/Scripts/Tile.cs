using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    public int row = -1;
    public int column = -1;
    
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool objective = false;
    public bool underArrow = false;

    public List<Tile> neighbors = new List<Tile>();
    public Stack<Tile> parents = new Stack<Tile>();

    //Needed BFS (breadth first search)
    public int distance = 0;

    //For A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

    public Color defaultColor;

	// Use this for initialization
	void Start () 
	{

    }   
	
	// Update is called once per frame
	void Update () 
	{
        if (underArrow)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else
        {
            if (target)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (selectable)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (objective)
            {
                GetComponent<Renderer>().material.color = Color.grey;
            }
            else
            {
                GetComponent<Renderer>().material.color = defaultColor;
            }
        }
        
	}

    public void Reset()
    {
        neighbors.Clear();

        current = false;
        target = false;
        selectable = false;
        objective = false;

        distance = 0;

        f = 0;
        g = 0;
        h = 0;
    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        /*
        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
        */

        if ((row - 1) >= 0)
        {
            Tile tile = GenerateGrid.grid[row - 1, column].GetComponent<Tile>();
            tile.parents = new Stack<Tile>(new Stack<Tile>(this.parents));
            tile.parents.Push(this);
            neighbors.Add(tile);
        }

        if ((row + 1) <= (GenerateGrid.gridRows - 1))
        {
            GameObject[] generateGrid = GameObject.FindGameObjectsWithTag("GenerateGrid");
            Tile tile = GenerateGrid.grid[row + 1, column].GetComponent<Tile>();
            tile.parents = new Stack<Tile>(new Stack<Tile>(this.parents));
            tile.parents.Push(this);
            neighbors.Add(tile);
        }

        if ((column - 1) >= 0)
        {
            GameObject[] generateGrid = GameObject.FindGameObjectsWithTag("GenerateGrid");
            Tile tile = GenerateGrid.grid[row, column - 1].GetComponent<Tile>();
            tile.parents = new Stack<Tile>(new Stack<Tile>(this.parents));
            tile.parents.Push(this);
            neighbors.Add(tile);
        }

        if ((column + 1) <= (GenerateGrid.gridColumns - 1))
        {
            GameObject[] generateGrid = GameObject.FindGameObjectsWithTag("GenerateGrid");
            Tile tile = GenerateGrid.grid[row, column + 1].GetComponent<Tile>();
            tile.parents = new Stack<Tile>(new Stack<Tile>(this.parents));
            tile.parents.Push(this);
            neighbors.Add(tile);
        }

    }

    /*public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }*/

    /*public string showStack(Stack<Tile> stack)
    {
        Stack<Tile> tempStack = new Stack<Tile>(new Stack<Tile>(stack));
        string mensaje = "";
        while(tempStack.Count > 0)
        {
            Tile tile = tempStack.Pop();
            mensaje += "(" + tile.row + ", " + tile.column + "),";
        }
        return mensaje;
    }
    */

    public static TacticsMove GetTargetTacticsMove(GameObject target)
    {
        RaycastHit hit;
        TacticsMove unit = null;

        if (Physics.Raycast(target.transform.position, Vector3.up, out hit, 1))
        {
            unit = hit.collider.GetComponent<TacticsMove>();
        }

        return unit;
    }
}
