using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Scripts;

public class UWBDistancaPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "uwb_distance";

    // The game object
    public GameObject[] Beacons;
    [SerializeField] public float publishMessageFrequency = 0.5f;
    [SerializeField] private float mu = 0.0f;
    [SerializeField] private float sigma = 0.05f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed = 0.0f;

    private float GaussRandom(float mu, float sigma)
    {
        float x1 = UnityEngine.Random.Range(0.0f, 1.0f);
        float x2 = UnityEngine.Random.Range(0.0f, 1.0f);

        float y1 = (float)(Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0f * Math.PI * x2) * sigma + mu);
        return y1;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<UWB_distanceMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            for (int i = 0; i < Beacons.Length; i++)
            {
                GameObject beacon = Beacons[i];
                float distance = Vector3.Distance(transform.position, beacon.transform.position) + GaussRandom(mu, sigma);

                UWB_distanceMsg msg = new UWB_distanceMsg((sbyte)(i+1), distance);
                Debug.Log(msg);
                //ros.Publish(topicName, msg);
            }

            timeElapsed = 0;
        }
    }
}
