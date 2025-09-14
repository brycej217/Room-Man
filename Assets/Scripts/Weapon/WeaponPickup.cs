using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPickup
{
    [SerializeField]
    private GameObject weapon;
    private Player player;

    public void PickUp()
    {
        weapon.transform.SetParent(player.hand, false);
        weapon.transform.position = player.hand.position;
        weapon.SetActive(true);
        player.ApplyWeapon(weapon);
        weapon.GetComponent<Weapon>().camera = player.camera;
        player.animator.Play("AIM");
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
