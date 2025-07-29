using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Empty,                  // 빈 공간
    Floor,                  // 바닥
    Wall,                   // 벽
    Collision               // 나갈 수 없는 외곽선
}
public class RoomData : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public TileType[,] TileGrid { get; private set; }

    public RoomData(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.TileGrid = new TileType[Width, Height]; // 설정한 크기로 방을 2차원 배열에 저장한다.
    }    
}
