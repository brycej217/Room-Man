using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HiveMind : MonoBehaviour
{
    // SINGLETON CLASS FOR CALCULATING TELEMETRY OF ALL BOIDS IN SCENE
    public ComputeShader boidMind; // gaming
    BoidScript[] boids;
    // on start and when boids get added track each
    void Start()
    {
       boids = FindObjectsOfType<BoidScript>();
    }

    public void UpdateBoidList(){
       boids = FindObjectsOfType<BoidScript>();
    }
    public void KillBoid(BoidScript deadBoid){
        BoidScript[] updatedBoids = new BoidScript[boids.Length - 1];
        for(int i = 0, j = 0; i < boids.Length; i++){
            if(boids[i] != deadBoid){
                updatedBoids[j] = boids[i];
            }else{
                j++;
            }
        }

        boids = updatedBoids;
        Destroy(deadBoid);
    }

    private void PruneBoids(){
        List<BoidScript> updatedBoids = new List<BoidScript>();
        for(int i = 0; i < boids.Length; i++){
            if(!boids[i].disabled){
                updatedBoids.Add(boids[i]);
            }else{
                boids[i].Cleanup();
            }
        }

        boids = updatedBoids.ToArray();
    }

    public float neighborDistance = 5f;
    public float separationDistance = 1f;

    void Update()
    {
        PruneBoids();
        if(boids.Length > 0){
            int numBoids = boids.Length;
            Data[] data = new Data[numBoids];
            for(int i = 0; i < numBoids; i++){
                data[i].position = boids[i].position;
                data[i].direction = boids[i].forward;
            }

            ComputeBuffer buff = new ComputeBuffer(numBoids, Data.Size);
            buff.SetData(data);
            boidMind.SetBuffer(0, "boids", buff);
            boidMind.SetInt("numBoids", boids.Length);
            boidMind.SetFloat("viewRadius", neighborDistance);
            boidMind.SetFloat("avoidRadius", separationDistance);

            int threads = Mathf.CeilToInt(numBoids / 1024f);

            boidMind.Dispatch(0, threads, 1, 1);
            buff.GetData(data);

            for (int i = 0; i< numBoids; i++){
                boids[i].avgFlockHeading = data[i].flockHeading;
                boids[i].centerOfFlockmates = data[i].flockCenter;
                boids[i].avgAvoidanceHeading = data[i].avoidanceHeading;
                boids[i].numPerceivedFlockmates = data[i].numFlockmates;

                boids[i].BoidStep();
            }

            buff.Release();
        }
    }

    public struct Data{
        public Vector3 position;
        public Vector3 direction;
        public Vector3 flockHeading;
        public Vector3 flockCenter;
        public Vector3 avoidanceHeading;
        public int numFlockmates;

        public static int Size {
            get {
                return sizeof (float) * 3 * 5 + sizeof (int);
            }
        }
    }
}
