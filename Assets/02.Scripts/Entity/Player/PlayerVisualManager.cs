using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        visualIndex = PlayerPrefs.GetInt(PlayerVisualIndexKey, 4);
        ChangeVisual(visualIndex);
    }

    private void Update()
    {
        for(int i = 0; i < playerVisualData.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && i != 0)
                PlayerVisualIndex = i - 1;
            else if (Input.GetKeyDown(KeyCode.Alpha0))
                PlayerVisualIndex = 9;
        }
    }

    private void ChangeVisual(int index)
    {
        spriteRenderer.sprite = playerVisualData[index].baseSprites;
        controller.runtimeAnimatorController = playerVisualData[index].controllers;
        PlayerPrefs.SetInt(PlayerVisualIndexKey, index);
    }
}