using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Player : MonoBehaviour , ICharacter
{
    public List<Card> cardList;

    private Queue<Card> deck = new Queue<Card>();
    public List<Card> hand = new List<Card>();

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

    public Dice dice;
    public Arrow arrow;

    public RectTransform handTr, deckTr;

    private Text shieldText;
    private Text healthText;
    private Image healthbarImage;

    public GameObject turnArrow;
    private GameObject turnButton;

    private Camera uiCamera;
    public static Player instance;

    private void Awake()
    {
        instance = this;

        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        turnButton = GameObject.Find("DiceButton");
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        shieldText = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
        healthText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        healthbarImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();

        health = maxHealth;
        SetInfo();
    }

    private void Start()
    {
        foreach (Card c in cardList)
        {
            Card newCard = Instantiate(c, deckTr);
            newCard.transform.SetAsFirstSibling();
            newCard.PositionReset(deckTr.position, 0, 0.5f);
            newCard.transform.localScale = Vector3.one * 0.5f;
            deck.Enqueue(newCard);
        }
    }

    private void SetInfo()
    {
        shieldText.text = shield.ToString();
        healthText.text = String.Format("{0}/{1}", health, maxHealth);
    }

    public void StartTurn()
    {
        turnArrow.SetActive(true);
        turnButton.SetActive(true);

        int handCount = hand.Count;
        for (int i = 5; i > handCount; i--)
            DrawCard(i);
        CardPositionReset();
    }

    public void EndTurn()
    {
        turnButton.SetActive(false);
        StartCoroutine(EndTurnAnim());
    }

    IEnumerator EndTurnAnim()
    {
        yield return StartCoroutine(dice.Roll());
        int idx = 0;
        while (idx < hand.Count)
        {
            if (hand[idx].UseSkill(dice.Index() - 1))
            {
                DropCard(hand[idx]);
            }
            else
                ++idx;
        }
        yield return new WaitForSeconds(3.0f);
        turnArrow.SetActive(false);
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
        card.PositionReset(deckTr.position, 0, 0.5f);

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
                damage = 0;
                break;
            }
            else //데미지 일부 상쇄
            {
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

        SetInfo();
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);

        SetInfo();
    }

    public void TakeShield(int duration, int shield)
    {
        shieldList.Add(new Shield(duration, shield));
        SetInfo();
    }

    public void Death()
    {

    }

    float heightTerm = 5;
    float widthTerm = 120;
    public Vector3 CardPosition(int index)
    {
        int handCount = hand.Count;
        float widthStart = -widthTerm * (handCount - 1) * 0.5f;
        return handTr.position + new Vector3(widthStart + widthTerm * (index - 1), -heightTerm * Mathf.Abs(index - handCount / 2 - 1));
    }

    float angleTerm = 1.5f;
    public float CardRotation(int index)
    {
        int handCount = hand.Count;
        if (handCount <= 1) return 0;
        return (angleTerm * (handCount - 1) * 0.5f) - angleTerm * (index - 1);
    }
}
