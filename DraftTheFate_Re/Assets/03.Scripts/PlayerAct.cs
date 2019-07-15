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
        arrow.Active(true);
    }

    public void CardDragEnd(Card card)
    {
        if (!canDrag && !isSelected)
            return;
        arrow.Active(false);

        isSelected = false;
        nowCard = null;

        if (result != null)
        {
            if (!result.CompareTag("Panel") && !card.isTargeting)
            {
                if (card.UseSkill())
                {
                    DropCard(card);
                    return;
                }
            }
            else if (result.CompareTag("Monster") && card.isTargeting)
            {
                if (card.UseSkill())
                {
                    DropCard(card);
                    return;
                }
            }
        }
        
        card.cardRect.anchoredPosition3D = CardPosition(card.siblingIndex);
        card.cardRect.eulerAngles = new Vector3(0, 0, 0);
        card.cardRect.localScale = Vector3.one * 2;
        card.transform.SetSiblingIndex(card.siblingIndex - 1);
    }
}
