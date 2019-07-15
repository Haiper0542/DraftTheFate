using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Transform mapParent;
    public RectTransform room, corridor;
    public RectTransform gateIcon, playerIcon;

    public int roomCount = 10;
    public float roomSize;

    public void Awake()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        Vector2 nowPos = Vector2.zero;
        RectTransform newRoom = Instantiate(room, mapParent);
        newRoom.anchoredPosition = nowPos;
        RectTransform newGate = Instantiate(gateIcon, mapParent);
        newGate.anchoredPosition = nowPos + Vector2.down * roomSize * 0.5f;

        while(roomCount > 0)
        {
            newRoom = Instantiate(room, mapParent);
            nowPos += Vector2.up * roomSize;
            newRoom.anchoredPosition = nowPos;
            roomCount--;
        }
    }
}
