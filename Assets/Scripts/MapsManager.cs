using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsManager : MonoBehaviour
{
    //public List<MapTemplate> mapsLibrary;
    public MapTemplate actualMap;

    public void SetActualMap(MapTemplate newActualMap)
    {
        actualMap = newActualMap;
    }

    public MapTemplate GetActualMap()
    {
        return actualMap;
    }

    /*public List<MapTemplate> GetMapsLibrary()
    {
        return mapsLibrary;
    }*/
}
