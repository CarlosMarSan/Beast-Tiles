using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TacticsMove : MonoBehaviour 
{
    public bool turn = false;
    public bool action = false;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;
    Vector3 previousPosition;

    public bool moving = false;
    public int move = 4;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    /*bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;*/

    protected static bool selectableTilesActivated = false;

    //**********************
    GameObject[] casillas;

    protected virtual void Start()
    {
        turn = false;
        action = false;

        path = new Stack<Tile>();
        
        moving = false;
        //move = 4;
        jumpHeight = 2;
        moveSpeed = 2;
        jumpVelocity = 4.5f;

        velocity = new Vector3();
        heading = new Vector3();

        selectableTilesActivated = false;
    }

    public void FindSelectableTiles()
    {
        if (!selectableTilesActivated)
        {
            GetCurrentTile();

            List<Tile> seleccionablesAbiertos = new List<Tile>();
            List<Tile> seleccionablesCerrados = new List<Tile>();
            seleccionablesCerrados.Add(currentTile);

            currentTile.FindNeighbors(0, currentTile);
            foreach (Tile t in currentTile.neighbors)
            {
                if (t.walkable)
                {
                    seleccionablesAbiertos.Add(t);
                }
            }

            for (int i = 1; i < move+1; i++)
            {
                List<Tile> aux = new List<Tile>();
                foreach (Tile abierta in seleccionablesAbiertos)
                {
                    abierta.FindNeighbors(0, abierta);

                    if (abierta != currentTile)
                    {
                        seleccionablesCerrados.Add(abierta);
                    }

                    foreach (Tile vecino in abierta.neighbors)
                    {
                        if (!aux.Contains(vecino) && vecino.walkable)
                        {
                            aux.Add(vecino);
                        }
                    }
                }

                seleccionablesAbiertos = aux;
            }


            casillas = GameObject.FindGameObjectsWithTag("Tile"); ;
            foreach (GameObject tile in casillas)
            {
                Tile t = tile.GetComponent<Tile>();
                if (seleccionablesCerrados.Contains(t))
                {
                    t.selectable = true;
                }
            }

            selectableTilesActivated = true;
        }
    }

    public void FindNPCPath(Tile target)
    {
        GameObject[] generateGrid = GameObject.FindGameObjectsWithTag("GenerateGrid");
        List<Tile> abiertos = new List<Tile>();
        GetCurrentTile();
        previousPosition = currentTile.transform.position;
        previousPosition.y = GenerateGrid.cubeSize-0.25f; // ¡¡¡¡NÚMERO MÁGICO!!!!

        currentTile.parents = new Stack<Tile>();
        currentTile.h = Mathf.Abs(target.row - currentTile.row) + Mathf.Abs(target.column - currentTile.column);
        currentTile.f = currentTile.h;
        abiertos.Add(currentTile);

        List<Tile> cerrados = new List<Tile>();

        if (currentTile == target)
        {
            MoveToTargetTile_NPC(target);
            return;
        }

        while (abiertos.Count > 0)
        {
            
            Tile q = FindLowestF(abiertos);

            q.FindNeighbors(0, q);

            cerrados.Add(q);

            foreach (Tile tile in q.neighbors)
            {
                
                if(tile == target)
                {
                    MoveToTargetTile_NPC(target);
                    return;
                }

                if (tile.walkable)
                {
                    if (abiertos.Contains(tile))
                    {
                        float tempG = q.g + 1;

                        if (tempG < tile.g)
                        {
                            tile.parents = new Stack<Tile>(new Stack<Tile>(q.parents));
                            //Debug.Log("ANTES: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);
                            tile.parents.Push(q);
                            //Debug.Log("DESPS: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);

                            tile.g = tempG;
                            tile.f = tile.g + tile.h;
                        }
                    }
                    else if (cerrados.Contains(tile))
                    {
                        // NADA
                    }
                    else
                    {
                        tile.parents = new Stack<Tile>(new Stack<Tile>(q.parents));
                        //Debug.Log("ANTES: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);
                        tile.parents.Push(q);
                        //Debug.Log("DESPS: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);

                        tile.g = q.g + 1;
                        tile.h = Mathf.Abs(target.row - tile.row) + Mathf.Abs(target.column - tile.column);
                        tile.f = tile.g + tile.h;
                        abiertos.Add(tile);
                    }
                }
            }

        }

        path.Clear();
        moving = true;
        path.Push(currentTile);
        return;
    }

    public void FindSelectedPath(Tile target)
    {
        GameObject[] generateGrid = GameObject.FindGameObjectsWithTag("GenerateGrid");
        List<Tile> abiertos = new List<Tile>();
        GetCurrentTile();
        previousPosition = currentTile.transform.position;
        previousPosition.y = 1.5f; // ¡¡¡¡NÚMERO MÁGICO!!!!

        currentTile.parents = new Stack<Tile>();
        currentTile.h = Mathf.Abs(target.row - currentTile.row) + Mathf.Abs(target.column - currentTile.column);
        currentTile.f = currentTile.h;
        abiertos.Add(currentTile);

        List<Tile> cerrados = new List<Tile>();

        if(currentTile == target)
        {
            MoveToTargetTile_Player(target);
            return;
        }

        while (abiertos.Count > 0)
        {

            Tile q = FindLowestF(abiertos);

            q.FindNeighbors(0, q);

            cerrados.Add(q);

            foreach (Tile tile in q.neighbors)
            {

                if (tile == target)
                {
                    MoveToTargetTile_Player(target);
                    return;
                }

                if (tile.walkable)
                {
                    if (abiertos.Contains(tile))
                    {
                        float tempG = q.g + 1;

                        if (tempG < tile.g)
                        {
                            tile.parents = new Stack<Tile>(new Stack<Tile>(q.parents));
                            //Debug.Log("ANTES: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);
                            tile.parents.Push(q);
                            //Debug.Log("DESPS: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);

                            tile.g = tempG;
                            tile.f = tile.g + tile.h;
                        }
                    }
                    else if (cerrados.Contains(tile))
                    {
                        // NADA
                    }
                    else
                    {
                        tile.parents = new Stack<Tile>(new Stack<Tile>(q.parents));
                        //Debug.Log("ANTES: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);
                        tile.parents.Push(q);
                        //Debug.Log("DESPS: " + tile.showStack(tile.parents) + " (" + tile.row + ", " + tile.column + ")" + "|| COUNT: " + tile.parents.Count);

                        tile.g = q.g + 1;
                        tile.h = Mathf.Abs(target.row - tile.row) + Mathf.Abs(target.column - tile.column);
                        tile.f = tile.g + tile.h;
                        abiertos.Add(tile);
                    }
                }
            }

        }

        return;
    }

    //**********************

    protected void Init()
    {
        halfHeight = GetComponent<Collider>().bounds.extents.y;

        //TurnManager.AddUnit(this);
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public static Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void MoveToTargetTile_NPC(Tile tile)
    {
        path.Clear();
        moving = true;
        Stack<Tile> temp;

        while (tile.parents.Count > 0)
        {
            if(tile.parents.Count == 1 && path.Count > 0)
            {
                tile.parents.Peek().walkable = true;
            }
            
            path.Push(tile.parents.Peek());
            tile.parents.Pop();

            if(path.Count > move+1)
            {
                temp = new Stack<Tile>(path);
                temp.Pop();
                path = new Stack<Tile>(temp);
            }
        }

        Stack<Tile> aux = new Stack<Tile>(path);
        aux.Peek().target = true;
        aux.Peek().walkable = false;
        //Debug.Log("PATH: " + path.Count);
        
    }

    public void MoveToTargetTile_Player(Tile tile)
    {
        path.Clear();
        moving = true;
        tile.target = true;
        tile.walkable = false;
        tile.parents.Push(tile);

        while (tile.parents.Count > 0)
        {
            if (tile.parents.Count == 1 && path.Count > 0)
            {
                tile.parents.Peek().walkable = true;
            }
            
            path.Push(tile.parents.Peek());
            tile.parents.Pop();
        }
    }

    public void Move()
    {
        if (selectableTilesActivated)
        {
            RemoveSelectableTiles();
            selectableTilesActivated = false;
        }
        
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of the target tile
            //target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
            target.y = GenerateGrid.cubeSize-0.25f;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                //bool jump = transform.position.y != target.y;

                //if (jump)
                //{
                    //Jump(target);
                //}
                //else
                //{
                    CalculateHeading(target);
                    SetHorizotalVelocity();
                //}

                //Locomotion
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            moving = false;
            ActionManager.SetActualTacticsMove(this);
        }
    }

    public void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (GameObject tile in casillas)
        {
            Tile t = tile.GetComponent<Tile>();
            t.Reset();
        }

    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizotalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    /*
    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizotalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }
    */
    
    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    public void BeginTurn()
    {
        turn = true;
        TurnManager.occupied = true;
    }

    public void EndTurn()
    {
        turn = false;
        TurnManager.occupied = false;
    }

    public void BackToPreviousTile()
    {
        GetCurrentTile();
        currentTile.walkable = true;
        transform.position = new Vector3(previousPosition.x, GenerateGrid.cubeSize-0.25f, previousPosition.z);
        GetCurrentTile();
        currentTile.walkable = false;
    }
}
