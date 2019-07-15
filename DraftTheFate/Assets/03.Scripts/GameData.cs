using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> cardDeselectedInventory;
    public List<string> cardSelectedInventory;
    public List<string> myDeck;
}

[System.Serializable]
public class CardData
{
    public string wordIndex;

    public string cardName;

    public Sprite cardSprite;

    public bool[] activeDice = new bool[6];

    public int damage, shield;
    public int duration;

    public int cost;

    [TextArea]
    public string explanation;
}

public class Shield
{
    public int duration;
    public int shield;

    public Shield(int duration, int shield)
    {
        this.duration = duration;
        this.shield = shield;
    }
}