using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlayerUnits : MonoBehaviour
{
    public List<Vector2Int> playerInitPositions;
    public List<StatsTemplate> statsList;
    public List<GameObject> models3D;
    public float figureSize = 1f; // Tamaño de cada figura
    [SerializeField] public Material material;
    int identificador = 1;

    // Start is called before the first frame update
    public void Initialize()
    {
        playerInitPositions = GetActualMapTemplate().GetPlayerInitPositions();
        PositionUnits();
    }

    public MapTemplate GetActualMapTemplate()
    {
        return GameObject.Find("MapsManagerObject").GetComponent<MapsManager>().GetActualMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PositionUnits()
    {
        int unitIndex = 0;
        foreach(Vector2Int u in playerInitPositions)
        {
            //GameObject figure = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            //GameObject figure = Instantiate(models3D[0]);
            UnitsTemplate actualUnit = GameObject.Find("PartyManagerObject").GetComponent<PartyManager>().GetUnits()[unitIndex];
            GameObject figure = Instantiate(actualUnit.GetModel());
            CapsuleCollider capsuleCollider = figure.AddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, 3.0f, 0); // Mueve el colisionador 1 unidad hacia arriba
            capsuleCollider.radius = 2.0f; // Ajusta el radio del CapsuleCollider
            capsuleCollider.height = 4.0f; // Ajusta la altura del CapsuleCollider
            /*BoxCollider boxCollider = figure.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(0, 3.0f, 0); // Mueve el colisionador 1 unidad hacia arriba
            boxCollider.size = new Vector3(4.0f, 8.0f, 4.0f);*/
            figure.AddComponent<MeshRenderer>();
            AssignPosition(figure, u.x, u.y, 0.25f);
            AssignPlayerFile(figure);
            AssignStats(figure, actualUnit.GetStats()) ;
            unitIndex++;
            TurnManager.AddUnit(figure.GetComponent<TacticsMove>());
        }
    }

    void AssignPosition(GameObject figure, int row, int column, float scale)
    {
        Vector3 position = new Vector3(column * figureSize, GenerateGrid.cubeSize-0.25f, row * figureSize);

        figure.transform.position = position;

        figure.transform.localScale = new Vector3(scale, scale, scale);
    }

    void AssignPlayerFile(GameObject figure)
    {
        figure.AddComponent<PlayerStats>();
        figure.tag = "Player";
        figure.name = "PlayerCapsule" + identificador;
        identificador++;
    }

    void AssignTexture(GameObject figure)
    {
        figure.GetComponent<Renderer>().material = material;
    }

    void AssignStats(GameObject figure, StatsTemplate stats)
    {
        //figure.GetComponent<PlayerStats>().stats = statsList[0];
        figure.GetComponent<PlayerStats>().stats = stats;
    }
}
