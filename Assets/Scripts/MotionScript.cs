using UnityEngine;
using UnityEngine.InputSystem;

public class MotionScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the drone movement
    [SerializeField] private float rotationSpeed = 2f; // Speed of the drone rotation
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

            drone.transform.position += move * Time.deltaTime * speed;

            if (keyboard.eKey.isPressed)
            {
                drone.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            if (keyboard.qKey.isPressed)
            {
                drone.transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }            
        }
    }
}
