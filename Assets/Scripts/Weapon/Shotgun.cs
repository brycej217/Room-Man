using UnityEngine;

public class Shotgun : Weapon
{
    [Header("Shotgun Params")]
    [SerializeField]
    protected int pelletCount = 10;
    [SerializeField]
    public float spread = 5f;

public override void ExecuteShoot()
{
    currentClipSize -= 1;

    for (int i = 0; i < pelletCount; i++)
    {
        // Calculate random spread direction
        Vector3 spreadDirection = GetSpreadDirection(camera.forward, spread);
        RaycastHit hit;
        Vector3 rayOrigin = camera.position;

        if (Physics.Raycast(rayOrigin, spreadDirection, out hit, range, targetLayer))
        {
            Enemy enemy = GetEnemyFromHit(hit.collider.gameObject);
            if (enemy != null)
            {
                enemy.GetHit(damage / pelletCount, hit.collider.tag);
            }
        }

        Debug.DrawRay(rayOrigin, spreadDirection * range, Color.blue, 2.0f);
    }

    base.ExecuteShoot();
}


    // Method to calculate a random spread direction within the given spread angle
    private Vector3 GetSpreadDirection(Vector3 forward, float spreadAngle)
    {
        float spreadRadius = Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
        Vector3 spreadOffset = new Vector3(
            Random.Range(-spreadRadius, spreadRadius),
            Random.Range(-spreadRadius, spreadRadius),
            0
        );
        Vector3 spreadDirection = (forward + spreadOffset).normalized;
        return spreadDirection;
    }
}
