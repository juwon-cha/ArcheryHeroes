using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Empty,                  // �� ����
    Floor,                  // �ٴ�
    Wall,                   // ��
    Collision               // ���� �� ���� �ܰ���
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
        this.TileGrid = new TileType[Width, Height]; // ������ ũ��� ���� 2���� �迭�� �����Ѵ�.
    }    
}
