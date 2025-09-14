using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public GameObject boid;
    int numBoids = 100;
    int bounds = 10;
    public float speed = 6f;
    public float minSpeed = 3f;
    public float speedFluc = 3f;
    public float rotationSpeed = 3f;
    public float alignmentBias = 1f;
    public float seperationBias = 1f;
    public float cohesionBias = 1f;
    public float aggressionBias = 1f;
    public HiveMind hive;
    public LayerMask obstacles;
    public float obstacleAvoidDist = 5f;
    public float obstacleAvoidBias = 10f;
    void Start()
    {
        hive = FindObjectOfType<HiveMind>();
        for(int i = 0; i < numBoids; i++){
            SpawnBoid();
        }
        hive.UpdateBoidList();
    }

    void SpawnBoid(){
        GameObject spawnedBoid = Instantiate(boid, this.transform.position, RandomRot());
        spawnedBoid.GetComponent<BoidScript>().Initialize(this, hive);
    }

    Vector3 RandomPosInBounds(){
        float randomX = Random.Range(-bounds, bounds);
        float randomY = Random.Range(-bounds, bounds);
        float randomZ = Random.Range(-bounds, bounds);

        return new Vector3(randomX, randomY, randomZ) + transform.position;
    }

    Quaternion RandomRot(){
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        return Quaternion.Euler(randomX, randomY, randomZ);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.grey;
        Gizmos.DrawCube(this.transform.position, new Vector3(1, .5f, 1));
    }
}
