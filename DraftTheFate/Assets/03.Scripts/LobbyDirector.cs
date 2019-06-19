using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class LobbyDirector : MonoBehaviour {
    
    [Header("MyDeckBuilding")]
    private RectTransform mydeckBuildingContent;
    private RectTransform mydeckListPanel;

    [Header("DeckBuilding")]
    public RectTransform deckBuildingPanel;
    private RectTransform deckBuildingContent;
    private RectTransform cardListPanel;

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

        if (adventureCount >= explainList.Length) adventureCount = explainList.Length - 1;
        string explain = explainList[adventureCount];
        
        float term = 0.04f;
        for(int i = 0;i< explain.Length; i+=2)
        {
            explainText.text = explain.Substring(0, i);
            yield return new WaitForSeconds(term);
        }
        explainText.text = explain.Substring(0, explain.Length - 1);

        yield return new WaitForSeconds(0.4f);
        startText.gameObject.SetActive(true);
    }

    public void NoteClose()
    {
        notePanel.SetActive(false);
        startText.gameObject.SetActive(false);
        RectTransform noteRect = notePanel.GetComponent<RectTransform>();
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
