using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private int health = 10;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    [Range(1, 20)][SerializeField] private float speed = 3.0f;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0f, 20.0f);
    }

    public void Initialize(EnemyDataSO data)
    {
        Health = (int)data.maxHealth;
        Speed = data.speed;
    }
}
