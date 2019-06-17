using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDirector : MonoBehaviour {

    [Header("MyDeckBuilding")]
    private RectTransform mydeckBuildingContent;
    private RectTransform mydeckListPanel;

    [Header("DeckBuilding")]
    public RectTransform deckBuildingPanel;
    private RectTransform deckBuildingContent;
    private RectTransform cardListPanel;

    private bool isOpendeckBuilding = false;

    private void Awake()
    {
        deckBuildingPanel = GameObject.Find("DeckBuildingPanel").GetComponent<RectTransform>();
        deckBuildingContent = GameObject.Find("DeckBuildingContent").GetComponent<RectTransform>();
        cardListPanel = GameObject.Find("CardList").GetComponent<RectTransform>();

        mydeckBuildingContent = GameObject.Find("MyDeckBuildingContent").GetComponent<RectTransform>();
        mydeckListPanel = GameObject.Find("MyDeckList").GetComponent<RectTransform>();
        deckBuildingPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isOpendeckBuilding && Input.GetKeyDown(KeyCode.Escape))
        {
            isOpendeckBuilding = false;
            deckBuildingPanel.gameObject.SetActive(false);
        }
        deckBuildingContent.sizeDelta = new Vector2(deckBuildingContent.sizeDelta.x, cardListPanel.sizeDelta.y + 160);
        mydeckBuildingContent.sizeDelta = new Vector2(mydeckBuildingContent.sizeDelta.x, mydeckListPanel.sizeDelta.y + 160);
    }

    public void AdventureStart()
    {
        SceneManager.LoadScene("IngameScene");
    }

    public void OpenDeckBuilding()
    {
        if (isOpendeckBuilding)
        {
            isOpendeckBuilding = false;
            deckBuildingPanel.gameObject.SetActive(false);
        }
        else
        {
            isOpendeckBuilding = true;
            deckBuildingPanel.gameObject.SetActive(true);
        }
    }
}
