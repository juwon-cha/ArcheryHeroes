using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerVisualData
    {
        public Sprite baseSprites;
        public RuntimeAnimatorController controllers;
        public bool isGet;
    }

    private SpriteRenderer spriteRenderer;
    private RuntimeAnimatorController controller;

    public PlayerVisualData[] playerVisualData;

    private int visualIndex = 4;
    public int PlayerVisualIndex
    {
        get => visualIndex;
        set
        {
            if (visualIndex != value)
            {
                visualIndex = value;
                ChangeVisual(visualIndex);
            }
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponentInChildren<RuntimeAnimatorController>();
    }

    private void ChangeVisual(int index)
    {
        spriteRenderer.sprite = playerVisualData[index].baseSprites;
        controller = playerVisualData[index].controllers;
    }
}