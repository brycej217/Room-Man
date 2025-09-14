using UnityEngine;

[CreateAssetMenu(fileName = "Wisp Upgrade", menuName = "Upgrades/Wisp Upgrade")]
public class WispUpgrade : Upgrade
{
    [Header("Params")]
    [SerializeField]
    private float damage = 30f;
    [SerializeField]
    private float radius = 5.0f;

    [Header("Collision")]
    private Vector3 position;
    public LayerMask explosionMask;

    public override void ApplyUpgrade(Player player)
    {
        Enemy.OnDeath += CreateExplosion;
    }

    private void CreateExplosion(Transform enemy)
    {
        this.position = enemy.position;
        Collider[] hitColliders = Physics.OverlapSphere(position, radius, explosionMask);
        foreach (var hitCollider in hitColliders)
        {
            Enemy hitEnemy = hitCollider.GetComponent<Enemy>();
            if (hitEnemy)
            {
                hitEnemy.GetHit(damage, hitCollider.tag);
            }
        }
    }
}
