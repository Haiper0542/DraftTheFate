using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public Player player;
    public Monster[] monsters;

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
    }

    private void Start()
    {
        BattleStart();
    }

    public void BattleStart()
    {
        stageText.text = mode + " : Stage " + stage;
        coinText.text = coin + " 원";

        player = GameObject.Find("Player").GetComponent<Player>();
        Transform monsterParent = GameObject.Find("Monsters").transform;
        monsters = new Monster[monsterParent.childCount];
        for (int i = 0; i < monsterParent.childCount; i++)
            monsters[i] = monsterParent.GetChild(i).GetComponent<Monster>();
        SwitchTurn();
    }

    int idx = 0;
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
            if (idx < monsters.Length)
            {
                monsters[idx].StartTurn();
                Debug.Log(monsters[idx] + " Turn");
                idx++;
                if (idx >= monsters.Length)
                {
                    isPlayerTurn = true;
                    idx = 0;
                }
            }
        }
    }

    public void GetCoin(int coin)
    {
        coin += coin;
        coinText.text = coin + " 원";
    }
}
