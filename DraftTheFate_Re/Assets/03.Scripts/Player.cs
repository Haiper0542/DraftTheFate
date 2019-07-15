using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Player : MonoBehaviour, ICharacter
{
    public Card cardPrefab;

    private Queue<Card> deck = new Queue<Card>();
    public List<Card> hand = new List<Card>();

    public int cost, maxCost;
    public int maxHealth;
    private int health;
    public List<Shield> shieldList = new List<Shield>();
    public int shield
    {
        get
        {
            int shield = 0;
            foreach (Shield s in shieldList)
                shield += s.shield;
            return shield;
        }
    }

    private int maxCount = 4;

    public Arrow arrow;

    public new Transform collider;
    public RectTransform handTr, deckTr;

    public Text costText;
    public Image costImage;

    public Text shieldText;
    public Text healthText;
    public Image healthbarImage;

    public Button turnButton;

    private Animation animation;

    public static Player instance;

    private void Awake()
    {
        instance = this;

        animation = transform.GetComponent<Animation>();
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        health = maxHealth;
        SetInfo();
    }

    public void CreateCard(Card c)
    {
        Card newCard = Instantiate(c, deckTr);
        newCard.transform.SetAsFirstSibling();
        newCard.PositionReset(Vector3.zero, 0, 0.1f);
        newCard.transform.localScale = Vector3.one * 0.5f;
        deck.Enqueue(newCard);
    }

    private void SetInfo()
    {
        shieldText.text = shield.ToString();
        healthText.text = String.Format("{0}/{1}", health, maxHealth);
        healthbarImage.fillAmount = (float)health / maxHealth;

        costText.text = cost.ToString();
        costImage.fillAmount = (float)cost / maxCost;
    }

    private void GetCost(int cost)
    {
        this.cost += cost;
        this.cost = Mathf.Clamp(this.cost, 0, 6);
        SetInfo();
    }

    public void UseCost(int cost)
    {
        this.cost -= cost;
        this.cost = Mathf.Clamp(this.cost, 0, 6);
        SetInfo();
    }

    public void StartTurn(int maxCost)
    {
        GetCost(maxCost);
        this.maxCost = maxCost;
        StartTurn();
    }

    public void StartTurn()
    {
        canDrag = true;
        StartCoroutine(StartTurnAnim());
    }

    public void EndTurn()
    {
        turnButton.interactable = false;
        canDrag = false;
        StartCoroutine(EndTurnAnim());
    }

    public IEnumerator StartTurnAnim()
    {
        yield return new WaitForSeconds(0.5f);
        int idx = 0;
        while (idx < shieldList.Count)
        {
            shieldList[idx].duration--;
            if (shieldList[idx].duration <= 0)
                shieldList.RemoveAt(idx);
            else
                ++idx;
        }

        int handCount = hand.Count;
        for (int i = maxCount; i > handCount; i--)
        {
            DrawCard(i);
            CardPositionReset();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1.2f);
        
        turnButton.interactable = true;
    }

    public IEnumerator EndTurnAnim()
    {
        yield return new WaitForSeconds(1.0f);
        GameDirector.instance.SwitchTurn();
    }

    public void CardPositionReset()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].siblingIndex = i + 1;
            hand[i].PositionReset();
        }
    }

    public void DropCard(Card card)
    {
        hand.Remove(card);
        deck.Enqueue(card);
        card.ExitHand();
        card.transform.SetParent(deckTr);
        card.transform.SetAsFirstSibling();
        card.PositionReset(Vector3.zero, 0, 0.5f);

        CardPositionReset();
    }

    public void DrawCard(int index)
    {
        Card newCard = deck.Dequeue();
        newCard.siblingIndex = index;
        newCard.transform.SetParent(handTr);
        newCard.EnterHand();
        hand.Add(newCard);
    }

    public void Shuffle()
    {
        Card[] temp = deck.ToArray();
        for (int i = 0; i < temp.Length * 2; i++)
        {
            int r = UnityEngine.Random.Range(0, temp.Length);
            Card t = temp[r];
            temp[r] = temp[i % temp.Length];
            temp[i % temp.Length] = t;
        }
        deck = new Queue<Card>();
        for (int i = 0; i < temp.Length; i++)
            deck.Enqueue(temp[i]);
    }

    public void TakeDamage(int damage)
    {
        int idx = 0;
        while (idx < shieldList.Count)
        {
            if (shieldList[idx].shield > damage) //데미지 완전 상쇄
            {
                shieldList[idx].shield -= damage;
                DamagerManager.instance.MakeToast("- " + damage, shieldText.transform.position + Vector3.up * 30, Color.blue);
                damage = 0;
                break;
            }
            else //데미지 일부 상쇄
            {
                DamagerManager.instance.MakeToast("- " + shieldList[idx].shield, shieldText.transform.position + Vector3.up * 30, Color.blue);
                damage -= shieldList[idx].shield;
                shieldList[idx].shield = 0;

                if (shieldList[idx].shield <= 0)
                    shieldList.RemoveAt(idx);
                else
                    ++idx;
            }
        }
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if(damage > 0)
        DamagerManager.instance.MakeToast("- " + damage, healthText.transform.position + Vector3.up * 30, Color.red);
        SetInfo();

        if (health <= 0)
            Death();
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        DamagerManager.instance.MakeToast("+ " + heal, healthbarImage.transform.position + Vector3.up * 30, Color.green);

        SetInfo();
    }

    public void TakeShield(int duration, int shield)
    {
        shieldList.Add(new Shield(duration, shield));
        SetInfo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Death();

        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
            result = results[0].gameObject;
        else
            result = null;
    }

    public void Death()
    {
        StartCoroutine(DeathAnim());
    }

    public IEnumerator DeathAnim()
    {
        yield return new WaitForSeconds(0.1f);
        animation.Play("Death");
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0;
        GameDirector.instance.GameEnd();
    }
    
    public Vector3 CardPosition(int index) // 0,1,2,3
    {
        int handCount = hand.Count;
        float widthTerm = 200f;
        float widthStart = -widthTerm * 1.5f;
        return new Vector3(widthStart + widthTerm * (index - 1), 0);
    }
}