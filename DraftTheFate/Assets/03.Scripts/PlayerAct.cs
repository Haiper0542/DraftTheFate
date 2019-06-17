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

    private void Update()
    {
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if (results.Count != 0)
            result = results[0].gameObject;
        else
            result = null;
    }

    public void CardDragBegin(Card card)
    {
        isSelected = true;
        nowCard = card;
        arrow.transform.position = card.transform.position;
        arrow.gameObject.SetActive(true);
    }

    public void CardDragEnd(Card card)
    {
        arrow.gameObject.SetActive(false);
        if (result != null && result.CompareTag("Deck"))
            DropCard(card);

        isSelected = false;
        nowCard = null;
    }
}
