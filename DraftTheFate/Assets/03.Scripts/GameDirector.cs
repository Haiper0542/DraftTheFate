using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private Transform monsterParent;
    public Monster[] monsterList;
    public int wave = 0;

    public Player player;
    public Monster monster;

    private bool isPlayerTurn = true;

    private Text coinText;
    private Text stageText;

    private string mode = "Endless Mode";
    private int stage = 1;
    public int coin = 0;

    public static GameDirector instance;

    private void Awake()
    {
        instance = this;
        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        stageText = GameObject.Find("StageText").GetComponent<Text>();
        monsterParent = GameObject.Find("Monsters").transform;
    }

    private void Start()
    {
        StartCoroutine(BattleStart());
    }

    public IEnumerator BattleStart()
    {
        stageText.text = mode + " : Stage " + stage;
        coinText.text = coin + " 원";

        player = GameObject.Find("Player").GetComponent<Player>();
        player.Shuffle();

        yield return new WaitForSeconds(5.0f);
        monster = Instantiate(monsterList[wave], monsterParent);
        SwitchTurn();
    }

    public void BattleEnd()
    {

    }

    public void SwitchTurn()
    {
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
