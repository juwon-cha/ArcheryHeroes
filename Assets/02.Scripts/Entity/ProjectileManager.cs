using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private GameObject[] projectilePrefabs;

    public void ShootBullet(RangedWeaponHandler rangeWeaponHandler, Vector2 startPosition, Vector2 direction)
    {
        GameObject origin = projectilePrefabs[rangeWeaponHandler.BulletIndex];
        GameObject obj = ObjectPoolingManager.Instance.Get(origin, startPosition, Quaternion.identity);

        ProjectileController projectileController = obj.GetComponent<ProjectileController>();
        projectileController.Init(direction, rangeWeaponHandler);
    }
}
