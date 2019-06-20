using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private Transform monsterParent;
    public Monster[] monsterList;
    public int wave = 0;

    public Player player;
    public Monster monster;

    [HideInInspector]
    public bool isPlayerTurn = true;
    [HideInInspector]
    public bool isBattle = false;

    private Text coinText;
    private Text stageText;

    private string mode = "Endless Mode";
    private int stage = 1;
    public int coin = 0;

    [Header("AdventureNote")]
    private GameObject notePanel;
    private Image monsterImage;
    private Text explainText;
    private Text endText;

    public string[] endExplain;

    public static GameDirector instance;

    private void Awake()
    {
        instance = this;

        Input.multiTouchEnabled = false;

        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        stageText = GameObject.Find("StageText").GetComponent<Text>();
        monsterParent = GameObject.Find("Monsters").transform;

        notePanel = GameObject.Find("NotePanel");
        monsterImage = GameObject.Find("MonsterImage").GetComponent<Image>();
        endText = GameObject.Find("EndText").GetComponent<Text>();
        explainText = GameObject.Find("ExplainText").GetComponent<Text>();
        endText.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(BattleStart());
    }

    public IEnumerator BattleStart()
    {
        Debug.Log("BattleStart");

        isBattle = true;
        isPlayerTurn = true;
        stageText.text = mode + " : Stage " + stage++;
        coinText.text = coin + " 원";

        player = GameObject.Find("Player").GetComponent<Player>();
        player.Shuffle();

        yield return new WaitForSeconds(2.0f);
        monster = Instantiate(monsterList[Random.Range(0,monsterList.Length)], monsterParent);
        RectTransform monsterRect = monster.GetComponent<RectTransform>();
        monsterRect.anchoredPosition = new Vector2(550, monsterRect.anchoredPosition.y);
        while (monsterRect.anchoredPosition.x >= 20)
        {
            monsterRect.anchoredPosition += Vector2.left * 1100 * Time.deltaTime;
            yield return null;
        }
        monsterRect.anchoredPosition =  new Vector2(0, monsterRect.anchoredPosition.y);

        yield return new WaitForSeconds(1.0f);

        isPlayerTurn = false;
        player.StartTurn();
        Debug.Log("Player Turn");
    }

    public IEnumerator BattleEnd()
    {
        isBattle = false;
        Debug.Log("BattleEnd");

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(BattleStart());
    }

    public IEnumerator GameEnd()
    {
        Time.timeScale = 0;
        monsterImage.sprite = monster.monsterIcon;
        RectTransform noteRect = notePanel.GetComponent<RectTransform>();
        float speed = 3000.0f;
        while (noteRect.anchoredPosition.y > 0)
        {
            noteRect.anchoredPosition += Vector2.down * speed * Time.fixedDeltaTime;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        noteRect.anchoredPosition = Vector2.zero;

        yield return new WaitForSecondsRealtime(0.7f);

        float term = 0.04f;
        string explain = endExplain[Random.Range(0, endExplain.Length)];
        for (int i = 0; i <= explain.Length; i++)
        {
            explainText.text = explain.Substring(0, i);
            yield return new WaitForSecondsRealtime(term);
        }

        yield return new WaitForSecondsRealtime(0.4f);
        endText.gameObject.SetActive(true);
    }

    public void BackToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LobbyScene");
    }

    public void SwitchTurn()
    {
        if (!isBattle) return;
        Debug.Log("SwitchTurn");

        if (isPlayerTurn)
        {
            isPlayerTurn = false;
            player.StartTurn();
            Debug.Log("Player Turn");
        }
        else
        {
            monster.StartTurn();
            Debug.Log(monster + " Turn");
            isPlayerTurn = true;
        }
    }

    public void GetCoin(int coin)
    {
        this.coin += coin;
        coinText.text = this.coin + " 원";
    }

    public bool UseCoin(int coin)
    {
        if (this.coin >= coin)
        {
            this.coin -= coin;
            coinText.text = this.coin + " 원";
            return true;
        }
        return false;
    }
}
