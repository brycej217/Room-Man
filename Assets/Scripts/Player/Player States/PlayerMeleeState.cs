using UnityEngine;

public class PlayerMeleeState : PlayerState
{
    protected string name = "MELEE";
    
    public override void EnterState()
    {
        base.EnterState();
        player.OnMeleeCooldown += OnMeleeCooldownHandler;
        Melee();
        player.Melee();
    }

    public override void ExitState()
    {
        base.ExitState();
        player.OnMeleeCooldown -= OnMeleeCooldownHandler;
    }

    private void Melee()
    {
        RaycastHit hit;
        Vector3 rayOrigin = player.camera.position;
        Vector3 rayDirection = player.camera.forward;

        if (Physics.SphereCast(rayOrigin, 5f, rayDirection, out hit, player.meleeRange, player.enemyMask))
        {
            Debug.Log("hit");
            Enemy enemy = Weapon.GetEnemyFromHit(hit.collider.gameObject);
            if (enemy != null)
            {
                enemy.GetHit(player.meleeDamage, hit.collider.tag);
            }
        }
        Debug.DrawRay(rayOrigin, rayDirection * player.meleeRange, Color.red, 2.0f);
    }

    private void OnMeleeCooldownHandler()
    {
        context.SwitchStates<PlayerReadyState>(this);
    }
}
