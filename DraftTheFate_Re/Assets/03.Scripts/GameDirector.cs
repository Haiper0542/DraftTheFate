using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public Player player;
    public Monster monster;

    public bool isPlayerTurn = false;

    public Monster[] monsterList;

    public RectTransform monsterParent;

    public Image monsterHealthbar;
    public Text monsterHealthText;
    public Text monsterNameText;

    public GameObject SettingScreen;
    public bool isSettingOpen = false;

    public Text turnText;

    public int maxCost = 3;

    public int coin;
    public Text coinText;

    public new Animation animation;
    public static GameDirector instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingOpen = !isSettingOpen;
            SettingScreen.SetActive(isSettingOpen);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public IEnumerator GameStart()
    {
        monster = Instantiate(monsterList[Random.Range(0, monsterList.Length)], monsterParent);
        foreach (string cardName in DataManager.instance.gameData.myDeck)
            player.CreateCard(DataManager.instance.cardPrefabData[cardName]);

        yield return new WaitForSeconds(1);
        MonsterSetInfo();
        BattleStart();
    }

    public IEnumerator GoNextStage()
    {
        animation.Play("GoNextStage");

        monster = Instantiate(monsterList[Random.Range(0, monsterList.Length)], monsterParent);
        MonsterSetInfo();

        yield return new WaitForSeconds(2);
        animation.Play("EnterNextStage");
        yield return new WaitForSeconds(2);
        BattleStart();
    }

    public void BattleStart()
    {
        animation.Play("BattleStartAnim");
        SwitchTurn();
    }

    public IEnumerator BattleEnd()
    {
        animation.Play("BattleEndAnim");
        maxCost = 3;
        yield return new WaitForSeconds(4);
        StartCoroutine(GoNextStage());
        yield return null;
    }

    public void GameEnd()
    {
        FlowManager.instance.AdventureEnd();
    }

    public void SwitchTurn()
    {
        if (isPlayerTurn)
        {
            if (monster.isDead)
                return;
            isPlayerTurn = false;
            StartCoroutine(StartEnemyTurn());
        }
        else
        {
            isPlayerTurn = true;
            StartCoroutine(StartPlayerTurn());
        }
    }

    public IEnumerator StartPlayerTurn()
    {
        turnText.text = "PlayerTurn";
        yield return new WaitForSeconds(1.0f);
        animation.Play("PlayerTurn");
        yield return new WaitForSeconds(1.0f);
        player.StartTurn(maxCost++);
    }

    public IEnumerator StartEnemyTurn()
    {
        turnText.text = "EnemyTurn";
        yield return new WaitForSeconds(1.0f);
        animation.Play("EnemyTurn");
        yield return new WaitForSeconds(1.0f);
        monster.StartTurn();
    }

    public void MonsterSetInfo()
    {
        monsterHealthbar.fillAmount = (float)monster.health / monster.maxHealth;
        monsterHealthText.text = monster.health + "/" + monster.maxHealth;
        monsterNameText.text = monster.monsterName;
    }

    public void GiveDamage(int damage)
    {
        DamagerManager.instance.MakeToast("- " + damage, monster.transform.position + Vector3.up * 10f, Color.red);
        monster.health -= damage;
        monster.health = Mathf.Clamp(monster.health, 0, monster.maxHealth);

        if (monster.health <= 0)
            monster.Death();

        MonsterSetInfo();
    }

    public void TakeHeal(int heal)
    {
        monster.health += heal;
        monster.health = Mathf.Clamp(monster.health, 0, monster.maxHealth);
        MonsterSetInfo();
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
