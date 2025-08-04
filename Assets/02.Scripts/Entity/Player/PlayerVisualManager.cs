using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisualManager : Singleton<PlayerVisualManager>
{
    [System.Serializable]
    public class PlayerVisualData
    {
        public Sprite baseSprites;
        public RuntimeAnimatorController controllers;
        public bool isGet;
    }

    private SpriteRenderer spriteRenderer;
    private Animator controller;

    public PlayerVisualData[] playerVisualData;

    private int inputNum;

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

    private const string PlayerVisualIndexKey = "VisualIndexKey";

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        visualIndex = PlayerPrefs.GetInt(PlayerVisualIndexKey, 4);
        ChangeVisual(visualIndex);
    }

    public void ChangeVisual(int index)
    {
        spriteRenderer.sprite = playerVisualData[index].baseSprites;
        controller.runtimeAnimatorController = playerVisualData[index].controllers;
        PlayerPrefs.SetInt(PlayerVisualIndexKey, index);
    }
}