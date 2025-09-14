using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public bool SpawnPlayer()
    {
        if (Instantiate(player))
        {
            return true;
        }
        return false;
    }
}
