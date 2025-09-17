using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HideShowButton : MonoBehaviour
{
    
    public GameObject DisplayOfExplanation;
    public TextMeshProUGUI Texto;
    bool showExplanation = true;

    // Start is called before the first frame update
    void Start()
    {
        DisplayOfExplanation.SetActive(showExplanation);
        Texto.text = "Ocultar";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideShow()
    {
        showExplanation = !showExplanation;
        DisplayOfExplanation.SetActive(showExplanation);
        if (showExplanation)
        {
            Texto.text = "Ocultar";
        }
        else
        {
            Texto.text = "Mostrar";
        }
    }
}
