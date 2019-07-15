using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Player : MonoBehaviour, ICharacter
{
    public List<Card> cardList;

    private Queue<Card> deck = new Queue<Card>();
    public List<Card> hand = new List<Card>();

    public int cost;
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

    public bool canGetCost = true;

    public int woundSlashCount = 0;

    public int maxCount = 5;

    public Dice dice;
    public Arrow arrow;

    public new Transform collider;
    public RectTransform handTr, deckTr;

    public GameObject[] costbar;

    private Text shieldText;
    private Text healthText;
    private Image healthbarImage;

    public GameObject turnArrow;
    private Button turnButton;

    private Animation animation;

    private Camera uiCamera;
    public static Player instance;

    private void Awake()
    {
        instance = this;

        animation = transform.GetComponent<Animation>();
        gr = mycanvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);

        turnButton = GameObject.Find("TurnButton").GetComponent<Button>();
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        shieldText = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
        healthText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        healthbarImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();

        health = maxHealth;
        SetInfo();
    }

    private void Start()
    {
        foreach (string cardName in DataManager.instance.gameData.myDeck)
        {
            Card newCard = Instantiate(DataManager.instance.cardPrefabData[cardName], deckTr);
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
        healthbarImage.fillAmount = (float)health / maxHealth;
        for (int i = 0; i < 6; i++)
        {
            if (i < cost)
                costbar[i].SetActive(true);
            else
                costbar[i].SetActive(false);
        }
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

    public void StartTurn()
    {
        if (canGetCost)
        {
            turnArrow.SetActive(true);
            canDrag = true;
        }
        StartCoroutine(StartTurnAnim());
    }

    public void EndTurn()
    {
        Debug.Log(false);
        turnButton.interactable = false;
        canDrag = false;
        StartCoroutine(EndTurnAnim());
    }

    public IEnumerator StartTurnAnim()
    {
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

        if (canGetCost)
        {
            yield return StartCoroutine(dice.Roll());
            GetCost(dice.Index());

            turnButton.interactable = true;
        }
        else
        {
            canGetCost = true;
            EndTurn();
        }
    }

    public IEnumerator EndTurnAnim()
    {
        turnArrow.SetActive(false);
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
        card.PositionReset(deckTr.position, 0, 0.5f);

        CardPositionReset();
    }

    public void DrawCard(int index)
    {
        if (deck.Count < 1)
            return;
        float temp = AudioManager.instance.effectSlider.value;
        AudioManager.instance.SetEffectVolume(0.05f);
        AudioManager.instance.PlayEffect("Draw");
        AudioManager.instance.SetEffectVolume(temp);
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
                DamagerManager.instance.MakeToast("- " + damage, shieldText.transform.position + Vector3.up * 0.5f, Color.blue);
                damage = 0;
                break;
            }
            else //데미지 일부 상쇄
            {
                DamagerManager.instance.MakeToast("- " + shieldList[idx].shield, shieldText.transform.position + Vector3.up * 0.5f, Color.blue);
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
        DamagerManager.instance.MakeToast("- " + damage, healthText.transform.position + Vector3.up * 0.5f, Color.red);
        SetInfo();

        if (health <= 0)
            Death();
    }

    public void TakeHeal(int heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        DamagerManager.instance.MakeToast("+ " + heal, healthbarImage.transform.position + Vector3.up * 0.5f, Color.green);

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
        StartCoroutine(GameDirector.instance.GameEnd());
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