using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    private static PlayerProjectileManager instance;
    public static PlayerProjectileManager Instance { get { return instance; } }

    [SerializeField] private GameObject[] projectilePrefabs;

    private void Awake()
    {
        instance = this;
    }

    public void ShootBullet(PlayerRangeWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];
        GameObject gameObject = Instantiate(origin, startPosition, Quaternion.identity);

        PlayerProjectileController projectileController = gameObject.GetComponent<PlayerProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler);
    }
}
