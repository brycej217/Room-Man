using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class ProceduralStepIKSolver : MonoBehaviour
{
    // solves ik target position, goes on ik target itself
    [SerializeField] Transform skeletonRoot;

    [SerializeField] ProceduralStepIKSolver otherSolver;
    [SerializeField] Vector3 footOffset = Vector3.zero;
    [SerializeField] IKSettings settings;
    Vector3 prevPos, currPos, newPos;
    Vector3 prevNormal, currNormal, newNormal;
    float lerp;
    float xOffset;
    Vector3 rayCastPos = Vector3.zero;
    Vector3 newTargetPos = Vector3.zero;


    void Start()
    {
        xOffset = transform.localPosition.x;
        currPos = newPos = prevPos = transform.position;
        currNormal = newNormal = prevNormal = transform.up;
        lerp = 1;
    }
    void Update()
    {
        // manually set position and normal to free transform from parent
        this.transform.position = currPos;    
        this.transform.up = currNormal;

        // raycast down from body to find new target pos
        Ray ray = new Ray(skeletonRoot.position + (skeletonRoot.right * xOffset), Vector3.down); 
        if(Physics.Raycast(ray, out RaycastHit hit, 10)){
            rayCastPos = hit.point;

            if(Vector3.Distance(newPos, hit.point) > settings.stepDist && !otherSolver.IsMoving() && lerp >= 1){
                lerp = 0;
                int dir = skeletonRoot.InverseTransformPoint(hit.point).z > skeletonRoot.InverseTransformPoint(newPos).z ? 1: -1;
                newPos = hit.point  + footOffset + (transform.forward * settings.stepLen * dir * -1);
                newTargetPos = newPos;
                Vector3 modNormal = hit.normal;
                modNormal.y *= -1;
                newNormal = modNormal;
            }
        }

        if(lerp < 1){
            Vector3 temp = Vector3.Lerp(prevPos, newPos, lerp);
            temp.y += Mathf.Sin(lerp * Mathf.PI) * settings.stepArc;
            currPos = temp;

            currNormal = Vector3.Lerp(prevNormal, newNormal, lerp);

            lerp += Time.deltaTime * settings.speed;
        }else{
            prevPos = newPos;
            prevNormal = newNormal;
        }
    }

    public bool IsMoving(){
        return lerp < 1;
    } 

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rayCastPos, .2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newTargetPos, .2f);
    }
}
