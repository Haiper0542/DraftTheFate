using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class LobbyDirector : MonoBehaviour {
    
    [Header("MyDeckBuilding")]
    private RectTransform mydeckBuildingContent;
    private RectTransform mydeckListPanel;

    [Header("DeckBuilding")]
    public RectTransform deckBuildingPanel;
    private RectTransform deckBuildingContent;
    private RectTransform cardListPanel;
    public GameObject cardObj;

    public Text[] deckListText;

    public GameObject toast;
    public Text toastText;

    [Header("AdventureNote")]
    private GameObject notePanel;
    private Text explainText;
    private Text stageText;
    private Text startText;
    private string stageName = "야생숲";
    private int adventureCount = 0;

    [TextArea]
    public string[] explainList;

    private bool isOpendeckBuilding = false;

    private void Awake()
    {
        deckBuildingPanel = GameObject.Find("DeckBuildingPanel").GetComponent<RectTransform>();
        deckBuildingContent = GameObject.Find("DeckBuildingContent").GetComponent<RectTransform>();
        cardListPanel = GameObject.Find("CardList").GetComponent<RectTransform>();

        mydeckBuildingContent = GameObject.Find("MyDeckBuildingContent").GetComponent<RectTransform>();
        mydeckListPanel = GameObject.Find("MyDeckList").GetComponent<RectTransform>();
        deckBuildingPanel.gameObject.SetActive(false);

        notePanel = GameObject.Find("NotePanel");
        stageText = GameObject.Find("StageText").GetComponent<Text>();
        startText = GameObject.Find("StartText").GetComponent<Text>();
        explainText = GameObject.Find("ExplainText").GetComponent<Text>();
        startText.gameObject.SetActive(false);
        notePanel.SetActive(false);
    }

    private void Start()
    {
        CardSetting();
        AudioManager.instance.PlayBackground("LobbyBgm");
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

    public void NoteOpen()
    {
        stageText.text = stageName;
        adventureCount = PlayerPrefs.GetInt(stageName, 0);

        StartCoroutine(NoteOpenAnim());
    }

    IEnumerator NoteOpenAnim()
    {
        notePanel.SetActive(true);
        RectTransform noteRect = notePanel.GetComponent<RectTransform>();
        float speed = 3000.0f;
        while (noteRect.anchoredPosition.y > speed * Time.deltaTime)
        {
            noteRect.anchoredPosition += Vector2.down * speed* Time.deltaTime;
            yield return null;
        }
        noteRect.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(0.7f);
        
        string explain = explainList[Random.Range(0,explainList.Length)];
        
        float term = 0.04f;
        for(int i = 0;i< explain.Length; i+=2)
        {
            explainText.text = explain.Substring(0, i);
            yield return new WaitForSeconds(term);
        }
        explainText.text = explain.Substring(0, explain.Length);

        yield return new WaitForSeconds(0.4f);
        startText.gameObject.SetActive(true);
    }

    public void NoteClose()
    {
        notePanel.SetActive(false);
        startText.gameObject.SetActive(false);
        RectTransform noteRect = notePanel.GetComponent<RectTransform>();
        explainText.text = string.Empty;
        noteRect.anchoredPosition = new Vector2(0, 600);
    }

    public void AdventureStart()
    {
        PlayerPrefs.SetInt(stageName, ++adventureCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene("IngameScene");
    }

    public void OpenDeckBuilding()
    {
        if (isOpendeckBuilding)
        {
            SaveDeck();

        }
        else
        {
            isOpendeckBuilding = true;
            deckBuildingPanel.gameObject.SetActive(true);
        }
    }

    private void CardSetting()
    {
        List<string> cardInventory = DataManager.instance.gameData.cardDeselectedInventory;

        for (int i = 0; i < cardInventory.Count; i++)
        {
            GameObject newCard = Instantiate(cardObj, cardListPanel);
            Card card = DataManager.instance.cardPrefabData[cardInventory[i]];
            newCard.GetComponent<CardFrame>().SetInfo(card.cardData);

            EventTrigger et = newCard.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            string cardName = cardInventory[i];
            entry.callback.AddListener((data) => { PickupCard(cardName, newCard.GetComponent<CardFrame>()); });

            et.triggers.Add(entry);
        }

        cardInventory = DataManager.instance.gameData.cardSelectedInventory;

        for (int i = 0; i < cardInventory.Count; i++)
        {
            GameObject newCard = Instantiate(cardObj, cardListPanel);
            CardFrame card = newCard.GetComponent<CardFrame>();

            card.SetInfo(DataManager.instance.cardPrefabData[cardInventory[i]].cardData);

            string cardName = cardInventory[i];
            if (!selectedList.ContainsKey(cardName))
                selectedList.Add(cardName, new List<CardFrame>());
            selectedList[cardName].Add(card);

            card.SelectCard();

            EventTrigger et = newCard.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener((data) => { PickupCard(cardName, newCard.GetComponent<CardFrame>()); });

            et.triggers.Add(entry);
        }

        DeckSetting();
    }

    private void DeckSetting()
    {
        for (int i = 0; i < deckListText.Length; i++)
            deckListText[i].text = "";

        int idx = 0;
        foreach (string n in DataManager.instance.gameData.myDeck)
            deckListText[idx++].text = DataManager.instance.cardPrefabData[n].cardData.cardName;
    }

    Dictionary<string, List<CardFrame>> selectedList = new Dictionary<string, List<CardFrame>>();
    public void PickupCard(string cardName, CardFrame card)
    {
        if (DataManager.instance.gameData.myDeck.Count >= deckListText.Length)
            return;

        DataManager.instance.PickupCard(cardName);

        if (!selectedList.ContainsKey(cardName))
            selectedList.Add(cardName, new List<CardFrame>());
        selectedList[cardName].Add(card);

        card.SelectCard();
        DeckSetting();
    }

    public void DropCard(int index)
    {
        if (DataManager.instance.gameData.myDeck.Count <= index)
            return;
        string cardName = DataManager.instance.gameData.myDeck[index];
        CardFrame card = selectedList[cardName][0];

        selectedList[cardName].Remove(card);
        if (selectedList[cardName].Count <= 0)
            selectedList.Remove(cardName);

        DataManager.instance.DropCard(cardName);
        card.DeselectCard();
        DeckSetting();
    }

    public void SaveDeck()
    {
        if (DataManager.instance.gameData.myDeck.Count < 5)
        {
            toast.SetActive(true);
            toastText.text = "덱안에는 5장이상의 카드가 필요합니다";
            Invoke("ToastOff", 1);
            return;
        }
        isOpendeckBuilding = false;
        deckBuildingPanel.gameObject.SetActive(false);
        toast.SetActive(true);
        toastText.text = "덱이 저장되었습니다";
        Invoke("ToastOff", 1);
        DataManager.instance.SaveDeck();
    }

    public void ToastOff()
    {
        toast.SetActive(false);
    }
}
