using RosMessageTypes.Scripts;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.InputSystem;

public class MotionScript : MonoBehaviour
{
    ROSConnection ros;
    public string takeoffTopicName = "takeoff";
    public string flyVelocityTopicName = "fly_velocity";

    [SerializeField] private float speed = 5f; // Speed of the drone movement
    [SerializeField] private float rotationSpeed = 2f; // Speed of the drone rotation

    private float target_height;
    private float v_x;
    private float v_y;
    private float v_z;

    private void TakeoffCallback(TakeoffMsg msg)
    {
        target_height = msg.height;
    }

    private void FlyVelocityCallback(Fly_velocityMsg msg) {
        v_x = msg.v_x;
        v_y = msg.v_y;
        v_z = msg.v_z;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<TakeoffMsg>(takeoffTopicName, TakeoffCallback);
        ros.Subscribe<Fly_velocityMsg>(flyVelocityTopicName, FlyVelocityCallback);
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        GameObject drone = GameObject.Find("drone");
        if (drone != null)
        {
            Vector3 move = Vector3.zero;
            if (keyboard.wKey.isPressed)
            {
                move += drone.transform.forward;
            }
            if (keyboard.sKey.isPressed)
            {
                move -= drone.transform.forward;
            }
            if (keyboard.dKey.isPressed)
            {
                move += drone.transform.right;
            }
            if (keyboard.aKey.isPressed)
            {
                move -= drone.transform.right;
            }

            if (keyboard.spaceKey.isPressed)
            {
                move += drone.transform.up;
            }
            if (keyboard.leftCtrlKey.isPressed)
            {
                move -= drone.transform.up;
            }

            if (keyboard.eKey.isPressed)
            {
                drone.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            if (keyboard.qKey.isPressed)
            {
                drone.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
            
            if (drone.transform.position.z != target_height)
            {
                float diff = target_height - drone.transform.position.z;
                if (diff > 0)
                {
                    move += drone.transform.up;
                }
                else
                {
                    move -= drone.transform.up;
                }
            }

            drone.transform.position += move * Time.deltaTime * speed;

            move = Vector3.zero;

            if (v_x != 0.0f)
            {
                move += drone.transform.forward * v_x;
            }
            if (v_y != 0.0f)
            {
                move += drone.transform.right * v_y;
            }
            if (v_z != 0.0f)
            {
                move += drone.transform.up * v_z;
            }

            drone.transform.position += move * Time.deltaTime;
        }
    }
}
