using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStats : TacticsMove
{
    public int health;
    public int maxHealth;
    public int attack;
    public int magic;
    public bool canAttack;
    public bool canHeal;
    public StatsTemplate stats;
    public WeaponTemplate weapon;
    int extradamage;

    static AudioManager audioManager = null;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        move = stats.mobility;
        base.Start();
        
        health = stats.health;
        maxHealth = health;
        magic = stats.magic;
        extradamage = 0;
        UpdateDamage();

        if (attack > 0)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        if (magic > 0)
        {
            canHeal = true;
        }
        else
        {
            canHeal = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateDamage();
        Faint();
    }

    protected void Faint()
    {
        if (health <= 0)
        {
            TurnManager.players.Remove(this);
            TurnManager.enemies.Remove(this);
            GetTargetTile(gameObject).walkable = true;
            Destroy(gameObject);
            audioManager.PlaySFX(audioManager.death);
        }
    }

    protected void UpdateDamage()
    {
        if (weapon != null)
        {
            extradamage = weapon.damage;
        }
        attack = stats.attack + extradamage;
    }
}
