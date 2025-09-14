using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public static event Action OnLocationEnter;

    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private GameObject barriers;

    private void Start()
    {
        barriers.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            spawner.StartSpawn();
            barriers.SetActive(true);
            OnLocationEnter?.Invoke();
        }
    }
}
