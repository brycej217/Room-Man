using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.GetAttack(other.gameObject.GetComponent<Player>());
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {

        }
    }
}
