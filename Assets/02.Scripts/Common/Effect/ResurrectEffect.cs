using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Effect/ResurrectEffect")]
public class ResurrectEffect : EffectSO
{
    [Range(0f, 1f)]
    [SerializeField] float resurrectChance = 0.5f; // 50% 확률로 부활
    private PlayerController playerController;

    public override void Initialize()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
    }

    public override void Execute(EffectContext effectContext = null)
    {

    }
}
