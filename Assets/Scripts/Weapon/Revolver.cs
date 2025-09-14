using UnityEngine;

public class Revolver : Weapon
{
    public override void ExecuteShoot()
    {
        currentClipSize -= 1;
        RaycastHit hit;
        Vector3 rayOrigin = camera.position;
        Vector3 rayDirection = camera.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range, targetLayer))
        {
            Enemy enemy = GetEnemyFromHit(hit.collider.gameObject);
            if(enemy != null){
                enemy.GetHit(damage, hit.collider.tag);
            }
        }

        Debug.DrawRay(rayOrigin, rayDirection * range, Color.red, 2.0f);
        base.ExecuteShoot();
    }
}
