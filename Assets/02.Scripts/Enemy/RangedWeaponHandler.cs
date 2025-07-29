using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponHandler : WeaponHandler
{
    [Header("Range Attack Info")]
    [SerializeField] private Transform projectileSpawnPosition;

    [SerializeField] private int bulletIndex;
    public int BulletIndex { get { return bulletIndex; } }

    [SerializeField] private float bulletSize = 1f;
    public float BulletSize { get { return bulletSize; } }

    [SerializeField] private float duration;
    public float Duration { get { return duration; } }

    [SerializeField] private float spread; // 탄 퍼짐
    public float Spread { get { return spread; } }

    [SerializeField] private int numberOfProjectilesPerShot; // 발사할 총알 개수
    public int NumberOfProjectilesPerShot { get { return numberOfProjectilesPerShot; } }

    [SerializeField] private float multipleProjectileAngle; // 각각의 탄이 발사되는 각도
    public float MultipleProjectileAngle { get { return multipleProjectileAngle; } }

    [SerializeField] private Color projectileColor;
    public Color ProjectileColor { get { return projectileColor; } }

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();

        float projectileAngleSpace = multipleProjectileAngle;
        int numberOfProjectilePerShot = numberOfProjectilesPerShot;

        float minAngle = -(numberOfProjectilePerShot / 2f) * projectileAngleSpace;

        for (int i = 0; i < numberOfProjectilePerShot; ++i)
        {
            float angle = minAngle + (i * projectileAngleSpace);
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;

            CreateProjectile(EnemyController.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 lookDirection, float angle)
    {
        ProjectileManager.Instance.ShootBullet(
            this,
            projectileSpawnPosition.position,
            RotateVector2(lookDirection, angle)
            );
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        // Quaternion이 가지는 회전의 수치 만큼 v를 회전시킴
        // 행렬 곱셈을 통해 벡터를 회전시키는 방법 -> 교환 법칙이 성립하지 않아서 순서에 유의
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
