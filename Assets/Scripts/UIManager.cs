using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    bool letActionButtonsShow;
    bool letPassButtonShow;
    static public bool letAttack;
    static public bool letHeal;
    
    public Button WaitButton;
    public Button AttackButton;
    public Button PassTurnButton;
    public Button HealButton;

    // Start is called before the first frame update
    void Start()
    {
        letActionButtonsShow = false;
        letPassButtonShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        letActionButtonsShow = (TurnManager.playerTurn && TurnManager.occupied && ActionManager.playerActionActivated);
        letPassButtonShow = TurnManager.playerTurn;
        WaitButton.interactable = letActionButtonsShow;
        WaitButton.gameObject.SetActive(letActionButtonsShow);
        AttackButton.interactable = letActionButtonsShow && letAttack;
        AttackButton.gameObject.SetActive(letActionButtonsShow && letAttack);
        HealButton.interactable = letActionButtonsShow && letHeal;
        HealButton.gameObject.SetActive(letActionButtonsShow && letHeal);
        PassTurnButton.gameObject.SetActive(letPassButtonShow);
    }

    public void SetWait_UI(bool b)
    {
        ActionManager.SetWait(b);
    }

    public void SetAttack_UI(bool b)
    {
        ActionManager.SetAttack(b);
    }

    public void SetHeal_UI(bool b)
    {
        ActionManager.SetHeal(b);
    }

    public void PassTurn_UI()
    {
        TurnManager.PassTurn();
    }
}
