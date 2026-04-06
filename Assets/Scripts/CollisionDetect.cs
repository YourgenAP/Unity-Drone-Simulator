using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollisionDetect : MonoBehaviour
{

    [SerializeField] GameObject groundPlane;
    [SerializeField] float rangeFinderDistance = 15.0f;
    [SerializeField] float minimalAllowedDistance = 5.0f;

    [SerializeField] float sensorBias = 0.0f;
    [SerializeField] float sensorStdDeviation = 0.05f;

    private Rigidbody rb;

    private Helpers helper = new Helpers();

    private class Sensor
    {
        public Vector3 direction;
        public LineRenderer line;
    }

    private Sensor[] sensors;

    private void Awake()
    {
        sensors = new Sensor[4];
        for (int i = 0; i < sensors.Length; i++)
            sensors[i] = new Sensor();
    }

    private float HandleRangeSensors(Vector3 start, Sensor sensor)
    {
        if (sensor == null) return Mathf.Infinity;
        if (sensor.line == null) return Mathf.Infinity;

        RaycastHit hit;

        Vector3 dir = transform.TransformDirection(sensor.direction);
        start = start + dir * 2.0f;
        

        if (Physics.Raycast(start, dir, out hit, rangeFinderDistance))
        {
            sensor.line.SetPosition(0, transform.position);
            sensor.line.SetPosition(1, hit.point);
            sensor.line.enabled = true;
            float error = helper.GaussRandom(sensorBias, sensorStdDeviation);
            return hit.distance + error;
        }
        else
        {
            sensor.line.enabled = false;
        }
        return Mathf.Infinity;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == groundPlane)
        {
            Vector3 vel = rb.linearVelocity;
            if (vel.magnitude > 1.0f)
            {
                Debug.LogError("Crash");
                Application.Quit();
                Debug.Break();
            }
            else
            {
                Debug.Log("Hit ground plane");
            }
        }
        else
        {
            Debug.LogError("Crash");
            Application.Quit();
            Debug.Break();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3[] dirs = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        rb = GetComponent<Rigidbody>();


        for (int i = 0; i < dirs.Length; i++) 
        {
            sensors[i] = new Sensor();
            sensors[i].direction = dirs[i];

            // Create GameObject
            GameObject obj = new GameObject("Sensor_" + i);
            obj.transform.parent = transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            LineRenderer lr = obj.AddComponent<LineRenderer>();
            if (lr == null)
            {
                Debug.LogError("Failed to add LineRenderer to Sensor_" + i);
                continue;
            }
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.red;
            lr.startWidth = 0.1f;
            lr.positionCount = 2;

            sensors[i].line = lr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        foreach (Sensor s in sensors)
        {
            float range = HandleRangeSensors(transform.position, s);
            Debug.Log(range);
            if (range <= minimalAllowedDistance)
            {
                transform.position -= transform.TransformDirection(s.direction) * Time.deltaTime;
            }
        }
    }
}
