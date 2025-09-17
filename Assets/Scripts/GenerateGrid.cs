using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public List<GameObject> models3D;
    static public int gridRows; // Número de filas
    static public int gridColumns; // Número de columnas
    static public float cubeSize = 1f; // Tamaño de cada cubo
    [SerializeField] public Material material;
    public Texture2D texture; // La textura que quieres asignar al cubo
    List<GameObject> aestheticGuidelines = new List<GameObject>();
    string guideline;

    static public GameObject[,] grid;

    public void Initialize()
    {
        gridRows = GetActualMapTemplate().GetMaxRows();
        gridColumns = GetActualMapTemplate().GetMaxCols();
        guideline = GetActualMapTemplate().GetMap();
        GenerateAG();
        GenerateCubeMatrix();
    }

    public void GenerateAG()
    {
        foreach (char c in guideline)
        {
            switch (c)
            {
                case 'M':
                    aestheticGuidelines.Add(models3D[2]);
                    break;
                case 'W':
                    aestheticGuidelines.Add(models3D[1]);
                    break;
                case 'G':
                    aestheticGuidelines.Add(models3D[0]);
                    break;
                default:
                    aestheticGuidelines.Add(models3D[0]);
                    break;
            }
        }
    }

    public MapTemplate GetActualMapTemplate()
    {
        return GameObject.Find("MapsManagerObject").GetComponent<MapsManager>().GetActualMap();
    }

    void GenerateCubeMatrix()
    {
        grid = new GameObject[gridRows, gridColumns];
        int indexAG = 0;
        for (int row = gridRows-1; row >= 0; row--)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                // Crea un nuevo cubo en la posición calculada
                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject figure = Instantiate(aestheticGuidelines[indexAG]);
                

                figure.tag = "Tile";
                grid[row, col] = figure;

                AssignFile(figure, row, col);
                figure.GetComponent<Tile>().defaultColor = aestheticGuidelines[indexAG].GetComponent<Renderer>().sharedMaterial.color;
                figure.GetComponent<Tile>().walkable = GetWalkabilityFromGuideline(indexAG);

                AssignPosition(figure, row, col);

                BoxCollider boxCollider = figure.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1.0f, 1.0f, 1.0f); // Ajusta la altura del cubeCollider

                indexAG++;
            }
        }
    }

    void AssignPosition(GameObject cube, int row, int column)
    {
        Vector3 position = new Vector3(column * cubeSize, 0f, row * cubeSize);

        cube.transform.position = position;
        /*Vector2Int v = GetActualMapTemplate().GetUnwalkableTiles().Find(vec => vec.x == column && vec.y == row);
        //Debug.Log(v);
        if (v.x == column && v.y == row && GetActualMapTemplate().GetUnwalkableTiles().Exists(vec => vec == v))
        {
            //Debug.Log("(" + v.x + ", " + v.y + ")");
            cube.transform.position = new Vector3(column * cubeSize, 1.5f, row * cubeSize);
            cube.GetComponent<Tile>().walkable = false;
        }*/
        cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
    }

    void AssignTexture(GameObject cube, string textureToAssign)
    {
        // Obtenemos el componente Renderer del cubo
        Renderer rend = cube.GetComponent<Renderer>();

        // Cargamos la textura "Madera.png" desde los recursos del proyecto
        texture = Resources.Load<Texture2D>(textureToAssign);

        if (texture == null)
        {
            Debug.LogError("No se pudo cargar la textura 'CasillaNormal.png'");
            return;
        }

        // Creamos un nuevo material y asignamos la textura a este material
        Material material = new Material(Shader.Find("Standard")); // Selecciona el shader adecuado
        material.mainTexture = texture;

        // Asignamos el material al cubo
        rend.material = material;
    }

    void AssignFile(GameObject cube, int row, int column)
    {
        cube.AddComponent<Tile>();
        cube.GetComponent<Tile>().row = row;
        cube.GetComponent<Tile>().column = column;

        /*if (Tile.GetTargetTacticsMove(cube) != null)
        {
            cube.GetComponent<Tile>().walkable = false;
        }*/
    }


    bool GetWalkabilityFromGuideline(int indexAG)
    {
        bool canWalk = true;
        switch (guideline[indexAG])
        {
            case 'W':
            case 'M':
                canWalk = false;
                break;
            default:
                break;
        }
        return canWalk;
    }
}
