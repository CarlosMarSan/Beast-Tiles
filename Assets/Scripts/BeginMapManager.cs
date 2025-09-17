using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginMapManager : MonoBehaviour
{
    public GeneratePlayerUnits generatePlayerUnits;
    public GenerateEnemyUnits generateEnemyUnits;
    public GenerateGrid generateGrid;
    static public bool generationFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        generationFinished = false;

        generatePlayerUnits.Initialize();
        generateEnemyUnits.Initialize();
        generateGrid.Initialize();

        foreach (Vector3Int u in generatePlayerUnits.playerInitPositions)
        {
            GenerateGrid.grid[u.x, u.y].GetComponent<Tile>().walkable = false;
        }

        foreach (Vector3Int u in generateEnemyUnits.enemyInitPositions)
        {
            GenerateGrid.grid[u.x, u.y].GetComponent<Tile>().walkable = false;
        }

        generationFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
