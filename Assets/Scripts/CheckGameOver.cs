using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckGameOver : MonoBehaviour
{

    public GameObject ResultsScreen;
    public GameObject panel;
    public TextMeshProUGUI resultsName;

    // Start is called before the first frame update
    void Start()
    {
        ResultsScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (BeginMapManager.generationFinished)
        {
            isGameOver();
        }
        
    }

    void isGameOver()
    {
        if (TurnManager.players.Count == 0)
        {
            ResultsScreen.SetActive(true);
            panel.GetComponent<Image>().color = Color.red;
            resultsName.text = "¡PERDISTE!";
        }

        if (TurnManager.enemies.Count == 0)
        {
            ResultsScreen.SetActive(true);
            panel.GetComponent<Image>().color = Color.blue;
            resultsName.text = "¡GANASTE!";
        }
    }
}
