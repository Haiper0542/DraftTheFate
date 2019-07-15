using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class FlowManager : MonoBehaviour {
    
    [Header("MyDeckBuilding")]
    private RectTransform mydeckBuildingContent;
    private RectTransform mydeckListPanel;

    [Header("DeckBuilding")]
    public RectTransform deckBuildingPanel;
    private RectTransform deckBuildingContent;
    private RectTransform deckPanelBack;
    private RectTransform cardListPanel;
    public GameObject cardObj;

    public Text[] deckListText;

    [Header("Scene Panel")]
    public GameObject mainPanel;
    public GameObject ingamePanel;
    public GameObject mapPanel;

    public GameObject toast;
    public Text toastText;

    [Header("AdventureNote")]
    private Text stageText;
    private string stageName = "Purple Castle";

    private bool isOpendeckBuilding = false;

    public static FlowManager instance;

    private void Awake()
    {
        instance = this;

        deckBuildingPanel = GameObject.Find("DeckBuildingPanel").GetComponent<RectTransform>();
        deckBuildingContent = GameObject.Find("DeckBuildingContent").GetComponent<RectTransform>();
        deckPanelBack = GameObject.Find("DeckPanelBack").GetComponent<RectTransform>();
        cardListPanel = GameObject.Find("CardList").GetComponent<RectTransform>();

        mydeckBuildingContent = GameObject.Find("MyDeckBuildingContent").GetComponent<RectTransform>();
        mydeckListPanel = GameObject.Find("MyDeckList").GetComponent<RectTransform>();
        deckBuildingPanel.gameObject.SetActive(false);

        stageText = GameObject.Find("StageText").GetComponent<Text>();
    }

    private void Start()
    {
        CardSetting();
    }

    private void Update()
    {
        if (isOpendeckBuilding && Input.GetKeyDown(KeyCode.Escape))
        {
            isOpendeckBuilding = false;
            deckBuildingPanel.gameObject.SetActive(false);
        }
        deckBuildingContent.sizeDelta = new Vector2(deckBuildingContent.sizeDelta.x, cardListPanel.sizeDelta.y);
        //deckPanelBack.sizeDelta = new Vector2(deckBuildingContent.sizeDelta.x, cardListPanel.sizeDelta.y);
    }

    public void AdventureStart()
    {
        StartCoroutine(StartAnim());
    }

    public void AdventureEnd()
    {
        StartCoroutine(EndAnim());
    }

    public IEnumerator StartAnim()
    {
        yield return StartCoroutine(EffectManager.instance.FadeOutAnim(1.5f));
        AudioManager.instance.PlayEffect("FootstepSound");
        yield return new WaitForSeconds(2.5f);
        AudioManager.instance.PlayBackground("Rafael_Bgm");
        mainPanel.SetActive(false);
        //mapPanel.SetActive(true);
        ingamePanel.SetActive(true);
        StartCoroutine(GameDirector.instance.GameStart());
        yield return StartCoroutine(EffectManager.instance.FadeInAnim(2));
    }

    public IEnumerator EndAnim()
    {
        yield return StartCoroutine(EffectManager.instance.FadeInAnim(1));
        yield return new WaitForSeconds(2.5f);
        mainPanel.SetActive(true);
        //mapPanel.SetActive(true);
        ingamePanel.SetActive(false);
        yield return StartCoroutine(EffectManager.instance.FadeOutAnim(2));
        AudioManager.instance.StopBackground();
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

    private void CardSetting()
    {
        List<string> cardInventory = DataManager.instance.gameData.cardDeselectedInventory;

        for (int i = 0; i < cardInventory.Count; i++)
        {
            GameObject newCard = Instantiate(cardObj, cardListPanel);
            newCard.GetComponent<CardFrame>().SetInfo(DataManager.instance[cardInventory[i]]);

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
            card.SetInfo(DataManager.instance[cardInventory[i]]);

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
            deckListText[idx++].text = n;
    }

    Dictionary<string, List<CardFrame>> selectedList = new Dictionary<string, List<CardFrame>>();
    public void PickupCard(string cardName, CardFrame card)
    {
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
