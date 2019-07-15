using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour {

    public CardData cardData;

    #region CardData
    protected string wordIndex;

    protected string cardName;

    protected Sprite cardSprite;

    private bool[] activeDice;

    [HideInInspector]
    public int cost;
    protected int damage, shield;
    protected int duration;

    protected string explanation;
    #endregion

    public int siblingIndex;

    public bool isTargeting = false;
    public bool isCursed = false;
    public Player owner;

    public RectTransform cardRect;
    public Image card;
    private Text cardNameText;
    private Text costText;
    protected Text explanationText;
    private Image cardImage;
    
    public abstract bool UseSkill();

    private void Awake()
    {
        owner = Player.instance;
        card = transform.GetComponent<Image>();
        cardRect = transform.GetComponent<RectTransform>();
        cardNameText = transform.Find("CardNameText").GetComponent<Text>();
        costText = transform.Find("CostText").GetComponent<Text>();
        explanationText = transform.Find("ExplainText").GetComponent<Text>();
        cardImage = transform.Find("CardImage").GetComponent<Image>();
        SetInfo();
    }

    protected void SetInfo()
    {
        wordIndex = cardData.wordIndex;
        cardName = cardData.cardName;
        cardSprite = cardData.cardSprite;
        activeDice = cardData.activeDice;
        damage = cardData.damage;
        shield = cardData.shield;
        duration = cardData.duration;
        explanation = cardData.explanation;
        cost = cardData.cost;

        cardNameText.text = cardName;
        costText.text = cost.ToString();
        explanationText.text = explanation;
        cardImage.sprite = cardSprite;

        EventTrigger et = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { CardEnter(); });

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((data) => { CardExit(); });

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.BeginDrag;
        entry3.callback.AddListener((data) => { Player.instance.CardDragBegin(this); });

        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.EndDrag;
        entry4.callback.AddListener((data) => { Player.instance.CardDragEnd(this); });

        et.triggers.Add(entry);
        et.triggers.Add(entry2);
        et.triggers.Add(entry3);
        et.triggers.Add(entry4);
    }

    private Vector3 targetPos = Vector3.zero;
    public float targetRotZ = 0, targetScale = 1;

    private bool isMovePos = false, isRotZ = false, isScale = false;

    float speed = 1;
    float posDead = 1, rotDead = 3, scaleDead = 0.2f;
    protected virtual void Update()
    {
        if (isMovePos)
        {
            cardRect.anchoredPosition3D = Vector3.Lerp(cardRect.anchoredPosition3D, targetPos, Time.deltaTime * 5 * speed);
            if (Vector3.SqrMagnitude(cardRect.anchoredPosition3D - targetPos) < posDead * posDead)
            {
                isMovePos = false;
                cardRect.anchoredPosition3D = targetPos;
            }
        }
        if (isRotZ)
        {
            cardRect.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(cardRect.eulerAngles.z, targetRotZ, Time.deltaTime * 5 * speed));
            if (Mathf.Abs(cardRect.eulerAngles.z - targetRotZ) < rotDead * rotDead)
            {
                isRotZ = false;
                cardRect.eulerAngles = new Vector3(0, 0, targetRotZ);
            }
        }
        if (isScale)
        {
            cardRect.localScale = Vector3.one * Mathf.Lerp(cardRect.localScale.x, targetScale, Time.deltaTime * 50 * speed);
            if (Mathf.Abs(cardRect.localScale.z - targetScale) < scaleDead * scaleDead)
            {
                isScale = false;
                cardRect.localScale = Vector3.one * targetScale;
            }
        }
    }

    public void EnterHand()
    {
        card.raycastTarget = true;
    }

    public void ExitHand()
    {
        card.raycastTarget = false;
        speed = 1;
    }

    public void CardEnter()
    {
        if (Player.instance.isSelected)
            return;
        transform.position += transform.up * 1.5f;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one * 1.1f;
        transform.SetAsLastSibling();
    }

    public void CardExit()
    {
        if (Player.instance.isSelected)
            return;
        cardRect.anchoredPosition3D = owner.CardPosition(siblingIndex);
        cardRect.eulerAngles = new Vector3(0, 0, owner.CardRotation(siblingIndex));
        cardRect.localScale = Vector3.one * 1;
        transform.SetSiblingIndex(siblingIndex - 1);
    }

    public void PositionReset()
    {
        isMovePos = isRotZ = isScale = true;

        targetPos = owner.CardPosition(siblingIndex);
        targetRotZ = owner.CardRotation(siblingIndex);
        targetScale = 1;
    }

    public void PositionReset(Vector3 pos, float rotZ, float scale)
    {
        isMovePos = isRotZ = isScale = true;

        targetPos = pos;
        targetRotZ = rotZ;
        targetScale = scale;
    }
}