using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour,ICharacter {

    public string monsterName;
    public int maxHealth;
    public int health;

    public Transform body;
    public new Transform collider;

    public Sprite monsterIcon;

    private Text nameText;
    private Text healthText;
    private Image healthbarImage;

    protected new Animation animation;

    public bool isDead = false;

    public abstract IEnumerator StartPattern();

    protected virtual void Awake()
    {
        animation = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Death()
    {
        if (isDead) return;
        isDead = true;
        StopCoroutine(Player.instance.EndTurnAnim());
        Player.instance.EndTurn();
        StartCoroutine(DeathAnim());
    }

    public IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(1.0f);

        animation.Play("Death");
        yield return StartCoroutine(GameDirector.instance.BattleEnd());

        Destroy(gameObject);
    }

    public void StartTurn()
    {
        StartCoroutine(StartPattern());
    }

    public void EndTurn()
    {
        GameDirector.instance.SwitchTurn();
    }
}
