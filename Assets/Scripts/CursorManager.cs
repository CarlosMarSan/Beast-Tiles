using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorManager : MonoBehaviour
{
    PlayerMove playerBelow = null;
    Tile tileBelow = null;
    GeneralStats generalStatsBelow = null;

    public GameObject DisplayOfStatsGO;
    public GameObject panel;
    public TextMeshProUGUI className;
    public TextMeshProUGUI attackAtribute;
    public TextMeshProUGUI magicAtribute;
    public TextMeshProUGUI rangeAtribute;
    public TextMeshProUGUI movAtribute;

    public Slider healthBarImage;
    public GameObject healthBarImageFill;
    public TextMeshProUGUI healthText;

    public int maxHealth;
    public int currentHealth;

    public GameObject haloPrefab; // Prefab del halo luminoso
    private GameObject currentHalo; // Referencia al halo instanciado

    Collider targetCollider;
    Collider lastCollider;

    // Start is called before the first frame update
    void Start()
    {
        healthBarImageFill.GetComponent<Image>().color = Color.green;
        DisplayOfStatsGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckArrow();
    }

    void CheckArrow()
    {
        Debug.DrawRay(transform.position, transform.forward);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray rayTile = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0.0f, 0.1f, 0.0f));

        RaycastHit hit;
        RaycastHit hitTile;

        if (Physics.Raycast(ray, out hit))
        {
            targetCollider = hit.collider;
            
            CheckPlayer(hit);
            if (hit.collider.tag == "Player" || hit.collider.tag == "Enemy")
            {
                // Si es en el turno del jugador
                if (TurnManager.playerTurn)
                {
                    DisplayOfStatsGO.SetActive(true);

                    if(hit.collider.tag == "Player")
                    {
                        if (TurnManager.currentGroup.Contains(hit.collider.GetComponent<TacticsMove>()))
                        {
                            panel.GetComponent<Image>().color = Color.blue;
                        }
                        else
                        {
                            panel.GetComponent<Image>().color = Color.black;
                        }
                    }

                    if (hit.collider.tag == "Enemy")
                    {
                        panel.GetComponent<Image>().color = Color.red;
                        //enemyCollider = hit.collider;
                        //enemyCollider.GetComponent<TacticsMove>().FindSelectableTiles();
                    }
                }

                generalStatsBelow = hit.collider.GetComponent<GeneralStats>();
                currentHealth = generalStatsBelow.health;
                maxHealth = generalStatsBelow.maxHealth;
                className.text = generalStatsBelow.stats.statsClass;
                attackAtribute.text = (generalStatsBelow.stats.attack).ToString();
                magicAtribute.text = (generalStatsBelow.stats.magic).ToString();
                rangeAtribute.text = (generalStatsBelow.stats.range).ToString();
                movAtribute.text = (generalStatsBelow.move).ToString();
                healthText.text = currentHealth + "/" + maxHealth;
                healthBarImage.value = (float)currentHealth / maxHealth;

                if(targetCollider != lastCollider)
                {
                    DestroyHalo();
                    //enemyCollider.GetComponent<TacticsMove>().RemoveSelectableTiles();
                }

                
                // Instanciar el halo si no está instanciado
                if (currentHalo == null)
                {
                    currentHalo = Instantiate(haloPrefab, hit.collider.transform);
                    //currentHalo.transform.localPosition = Vector3.zero; // Centrar el halo en el objeto
                    currentHalo.transform.localPosition = new Vector3(0, 10, 0); // Centrar el halo en el objeto
                }
            }
            else
            {
                DestroyHalo(); // Destruir el halo si no es un "Player" o "Enemy"
                //enemyCollider.GetComponent<TacticsMove>().RemoveSelectableTiles();
            }
        }
        else
        {
            if (!TurnManager.occupied)
            {
                DisplayOfStatsGO.SetActive(false);
            }
            DestroyHalo(); // Destruir el halo si no se golpea nada
            //enemyCollider.GetComponent<TacticsMove>().RemoveSelectableTiles();
        }

        if (Physics.Raycast(rayTile, out hitTile))
        {
            CheckTile(hitTile);
        }
    }

    void CheckPlayer(RaycastHit hit)
    {
        if (hit.collider.tag == "Player")
        {
            if (playerBelow != null)
            {
                playerBelow.underArrow = false;
            }

            playerBelow = hit.collider.GetComponent<PlayerMove>();
            playerBelow.underArrow = true;
        }
        else
        {
            if (playerBelow != null)
            {
                playerBelow.underArrow = false;
            }
        }
    }

    void CheckTile(RaycastHit hit)
    {
        if (hit.collider.tag == "Tile")
        {
            if (tileBelow != null)
            {
                tileBelow.underArrow = false;
            }

            tileBelow = hit.collider.GetComponent<Tile>();
            tileBelow.underArrow = true;
        }
        else
        {
            if (tileBelow != null)
            {
                tileBelow.underArrow = false;
            }
        }
    }

    void DestroyHalo()
    {
        if (currentHalo != null)
        {
            Destroy(currentHalo);
            currentHalo = null;
        }
    }

}
