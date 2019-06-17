using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour,ICharacter {

    public string monsterName;
    public int maxHealth;
    private int health;

    public GameObject turnArrow;

    private Text nameText;
    private Text healthText;
    private Image healthbarImage;

    public bool isDead = false;

    public abstract IEnumerator StartPattern();

    protected virtual void Awake()
    {
        nameText = transform.GetChild(2).GetComponent<Text>();
        healthText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        healthbarImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        health = maxHealth;

        SetInfo();
    }

    protected void SetInfo()
    {
        nameText.text = monsterName;
        healthText.text = String.Format("{0}/{1}", health, maxHealth);
    }

    public void Death()
    {

    }

    public void StartTurn()
    {
        turnArrow.SetActive(true);
        StartCoroutine(StartPattern());
    }

    public void EndTurn()
    {
        turnArrow.SetActive(false);
        GameDirector.instance.SwitchTurn();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        SetInfo();
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);

        SetInfo();
    }
}
