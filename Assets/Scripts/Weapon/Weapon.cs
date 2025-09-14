using System.Collections;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public new Transform camera;
    [SerializeField]
    protected LayerMask targetLayer;

    [Header("Params")]
    [SerializeField]
    protected float damage = 25f;
    [SerializeField]
    protected float range = 100f;
    [SerializeField]
    public float fireRate = 0.25f;
    public int clipSize = 6;
    [SerializeField]
    protected float reloadSpeed = 0.5f;
    [SerializeField]
    protected float swapTime = 0.1f;

    public event Action OnShoot;
    public event Action OnReload;
    public event Action OnClipChange;
    public event Action OnSwap;

    public event Action OnShotFinished;
    public event Action OnReloadFinished;
    public event Action OnSwapFinished;

    [HideInInspector]
    public int currentClipSize;
    [HideInInspector]
    public bool isSwapped = false;

    private Coroutine swapCoroutine;

    public virtual void OnEnable()
    {
        OnSwap?.Invoke();
    }

    public virtual void Start()
    {
        currentClipSize = clipSize;
        OnClipChange?.Invoke();
    }

    public void Shoot()
    {
        OnShoot?.Invoke();
    }

    public void Reload()
    {
        OnReload?.Invoke();
    }

    public virtual void ExecuteShoot()
    {
        OnClipChange?.Invoke();
        StartCoroutine(ShotTime());
    }

    public virtual void ExecuteReload()
    {
        StartCoroutine(ReloadTime());
    }

    public virtual void Swap()
    {
        swapCoroutine = StartCoroutine(SwapTime());
    }

    public virtual void StopSwap()
    {
        StopCoroutine(swapCoroutine);
    }

    public virtual IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(reloadSpeed);
        currentClipSize = clipSize;
        OnClipChange?.Invoke();
        OnReloadFinished?.Invoke();
    }

    public virtual IEnumerator ShotTime()
    {
        yield return new WaitForSeconds(1f / fireRate);
        OnShotFinished?.Invoke();
    }

    public virtual IEnumerator SwapTime()
    {
        yield return new WaitForSeconds(swapTime);
        OnSwapFinished?.Invoke();
    }

    public static Enemy GetEnemyFromHit(GameObject hitObj)
    {
        Enemy enemy = hitObj.GetComponent<Enemy>();
        if (enemy == null)
        {
            enemy = hitObj.GetComponentInParent<Enemy>();
            if (enemy == null)
            {
                enemy = hitObj.GetComponentInChildren<Enemy>();
                if (enemy == null)
                {
                    Debug.Log("no enemy in hit obj???");
                }
            }
        }

        return enemy;
    }
}
