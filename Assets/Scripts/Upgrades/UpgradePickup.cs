using UnityEngine;

public class UpgradePickup : MonoBehaviour, IPickup
{
    [SerializeField]
    private Upgrade upgrade;
    private Player player;

    public void PickUp()
    {
        player.ApplyUpgrade(upgrade);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            PickUp();
        }
    }
}
