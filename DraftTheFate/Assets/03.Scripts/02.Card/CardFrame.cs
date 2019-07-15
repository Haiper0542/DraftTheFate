using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFrame : MonoBehaviour
{
    private RectTransform cardRect;
    private Image card;
    private Text cardNameText;
    private Text costText;
    private Text explanationText;
    private Image cardMask;
    private Image cardImage;

    public void SetInfo(CardData cardData)
    {
        card = transform.GetComponent<Image>();
        cardRect = transform.GetComponent<RectTransform>();
        cardNameText = transform.Find("CardNameText").GetComponent<Text>();
        costText = transform.Find("CostText").GetComponent<Text>();
        explanationText = transform.Find("ExplainText").GetComponent<Text>();
        cardImage = transform.Find("CardImage").GetComponent<Image>();
        cardMask = transform.Find("Mask").GetComponent<Image>();

        cardNameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        explanationText.text = cardData.explanation;
        cardImage.sprite = cardData.cardSprite;
    }

    public void SelectCard()
    {
        card.raycastTarget = false;
        cardImage.color = new Color(0.5f, 0.5f, 0.5f, 1);
        cardMask.color = new Color(0.5f, 0.5f, 0.5f, 1);
        costText.color = new Color(0.5f, 0.5f, 0.5f, 1);
        explanationText.color = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void DeselectCard()
    {
        card.raycastTarget = true;
        cardImage.color = Color.white;
        cardMask.color = Color.white;
        costText.color = Color.white;
        explanationText.color = Color.white;
    }
}
