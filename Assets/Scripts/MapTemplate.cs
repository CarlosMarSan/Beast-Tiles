using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapTemplate : ScriptableObject
{
    public string mapName;
    public int maxRows;
    public int maxCols;
    public List<Vector2Int> unwalkableTiles;
    public List<Vector2Int> playerInitPositions;
    public List<Vector2Int> enemyInitPositions;
    public string map;
    
    public int GetMaxRows()
    {
        return maxRows;
    }

    public int GetMaxCols()
    {
        return maxCols;
    }

    public List<Vector2Int> GetUnwalkableTiles()
    {
        return unwalkableTiles;
    }

    public List<Vector2Int> GetPlayerInitPositions()
    {
        return playerInitPositions;
    }

    public List<Vector2Int> GetEnemyInitPositions()
    {
        return enemyInitPositions;
    }

    public string GetMap()
    {
        return map;
    }
}
