using UnityEngine; 
using System.Collections; 
using UnityEngine.UI; 
using UnityEngine.EventSystems; 
using System.Collections.Generic;

public partial class Player : MonoBehaviour
{
    public Canvas mycanvas;
    GraphicRaycaster gr;
    PointerEventData ped;

    public Card nowCard;
    public bool isSelected = false;

    public GameObject result;


    private bool canDrag = true;
    public void CardDragBegin(Card card)
    {
        if (!canDrag)
            return;
        isSelected = true;
        nowCard = card;
        arrow.transform.position = card.transform.position;
        arrow.gameObject.SetActive(true);
    }

    public void CardDragEnd(Card card)
    {
        if (!canDrag && !isSelected)
            return;
        arrow.gameObject.SetActive(false);

        isSelected = false;
        nowCard = null;

        if (result != null && result.CompareTag("Deck"))
        {
            if (card.isCursed)
            {
                if (GameDirector.instance.UseCoin(card.GetComponent<CursedCard>().cost))
                {
                    DropCard(card);
                    return;
                }
            }
            else
            {
                GameDirector.instance.GetCoin(card.cost);
                DropCard(card);
                return;
            }
        }

        card.cardRect.anchoredPosition3D = CardPosition(card.siblingIndex);
        card.cardRect.eulerAngles = new Vector3(0, 0, CardRotation(card.siblingIndex));
        card.cardRect.localScale = Vector3.one * 1;
        card.transform.SetSiblingIndex(card.siblingIndex - 1);
    }
}
