using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private Transform head;

    void Update()
    {
        transform.position = head.transform.position;
    }
}