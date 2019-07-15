using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour,ICharacter {

    public string monsterName;
    public int maxHealth;
    private int health;

    public Transform body;
    public new Transform collider;

    public GameObject turnArrow;

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
        healthbarImage.fillAmount = (float)health / maxHealth;
    }

    public void Death()
    {
        if (isDead) return;
        isDead = true;
        StopCoroutine(Player.instance.EndTurnAnim());
        StopCoroutine(StartPattern());
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
        DamagerManager.instance.MakeToast("- " + damage, healthText.transform.position + Vector3.up * 0.5f, Color.red);
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        SetInfo();

        if (health <= 0)
            Death();
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);

        SetInfo();
    }
}
