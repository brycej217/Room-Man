using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSettings : MonoBehaviour
{
    public float stepArc = 1;
    public float speed = 1;
    public float stepDist = 2.5f;
    public float stepLen = 4f;
    [SerializeField] public Transform orientation;
    public bool unfuck;
}
