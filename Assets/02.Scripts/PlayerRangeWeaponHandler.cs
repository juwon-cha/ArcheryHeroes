using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeWeaponHandler : PlayerWeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread;
    public float Spread { get { return spread; } }

    [SerializeField] private int numberOfProjectilesPerShot;
    public int NumberofProjectilePerShot { get { return numberOfProjectilesPerShot; } }

    [SerializeField] private int multipleProjectileAngle;
    public int MultipleProjectileAngle { get { return multipleProjectileAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    private PlayerProjectileManager projectileManager;

    protected override void Start()
    {
        base.Start();
        projectileManager = PlayerProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        float projectilesAngleSpace = multipleProjectileAngle;
        int numberOfProjectiles = numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectiles / 2f) * projectilesAngleSpace;

        for(int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(controller.LookDirtection, angle);
        }
    }

    private void CreateProjectile(Vector2 lookDirection, float angle)
    {
        projectileManager.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(lookDirection, angle));
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
