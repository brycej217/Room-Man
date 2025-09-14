using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class BoidScript : Enemy
{
    private Rigidbody rb; // for when it dies
    public Vector3 position; // track these for outside use
    public Vector3 forward; // track these for outside use
    Vector3 velocity; // track these for outside use
    public Vector3 avgFlockHeading; // received from hivemind
    public Vector3 avgAvoidanceHeading; // received from hivemind
    public Vector3 centerOfFlockmates; // received from hivemind
    public int numPerceivedFlockmates; // received from hivemindCompute
    private BoidSpawner nest;
    private bool init = false; 
    private Player player;
    public HiveMind hiveMind;

    public bool disabled = false;

    public void Initialize(BoidSpawner spawner, HiveMind hive){
        nest = spawner;

        velocity = transform.forward * nest.minSpeed;
        position = this.transform.position;
        forward = this.transform.forward;
        init = true;
        player = Player.Instance;

        hiveMind = hive;

        maxHealth = 1f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void BoidStep(){ // called in update by hivemind which gives info to this before calling
        if(!init && !disabled) return;
        Vector3 accl = Vector3.zero;

        if (numPerceivedFlockmates != 0) {
            centerOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = centerOfFlockmates - transform.position;

            Vector3 alignmentForce = Steer(avgFlockHeading) * nest.alignmentBias;
            Vector3 cohesionForce = Steer(offsetToFlockmatesCentre) * nest.cohesionBias;
            Vector3 seperationForce = Steer(avgAvoidanceHeading) * nest.seperationBias;

            Vector3 playerDirection = player.transform.position - position;
            Vector3 agressionForce = Steer(playerDirection) * nest.aggressionBias;

            accl += alignmentForce;
            accl += cohesionForce;
            accl += seperationForce;
            accl += agressionForce;
        }

        if(CollisionCourse()){ // dont hit terrain
            Vector3 avoidCollisionDirection = ObstacleRaycast();
            Vector3 avoidanceForce = Steer(avoidCollisionDirection) * nest.obstacleAvoidBias;
            accl += avoidanceForce;
        }

        velocity += accl * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 direction = velocity/speed;
        speed = Mathf.Clamp(speed, nest.minSpeed, nest.speed);
        velocity = direction * speed;

        this.transform.position += velocity * Time.deltaTime;
        this.transform.forward = direction;

        position = this.transform.position;
        forward = this.transform.forward;
    }

    Vector3 ObstacleRaycast(){ // find unobstructed way forward
        Vector3[] fov = BoidObstacleHelper.directions;

        for(int i = 0; i < fov.Length; i++){
            Vector3 dir = transform.TransformDirection(fov[i]);
            Ray ray = new Ray(position, dir);
            if(!Physics.SphereCast(ray, BoidObstacleHelper.boundsRad, nest.obstacleAvoidDist, nest.obstacles)){
                return dir;
            }
        }

        return forward;
    }

    bool CollisionCourse(){ // check if we will collide with terrain
        RaycastHit hit;
        if (Physics.SphereCast (position, BoidObstacleHelper.boundsRad, forward, out hit, nest.obstacleAvoidDist, nest.obstacles)) {
            return true;
        }else{
            return false;
        }
    }

    Vector3 Steer(Vector3 vec){
        Vector3 v = vec.normalized * nest.speed - velocity;
        return Vector3.ClampMagnitude(v, nest.rotationSpeed);
    }

    // debug
    [ContextMenu("killem")] // ui setup
    public override void Die(){ // ui setup
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = velocity;
        disabled = true;
    }

    public void Cleanup(){
        
    }
    //void OnDrawGizmos(){
        //Gizmos.color = Color.red;
        //Vector3 arrowEnd = transform.position + transform.forward * 5f;
        //Gizmos.DrawLine(transform.position, arrowEnd);

        //Gizmos.color = Color.yellow;
        //foreach(Vector3 pos in gizmos){
            //Gizmos.DrawLine(transform.position, pos);
        //}
        //gizmos.Clear();
    //}
}
