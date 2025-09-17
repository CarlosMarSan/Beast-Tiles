using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitsTemplate : ScriptableObject
{
    public StatsTemplate stats;
    public GameObject model;

    public GameObject GetModel()
    {
        return model;
    }

    public StatsTemplate GetStats()
    {
        return stats;
    }
}
