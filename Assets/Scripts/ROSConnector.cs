using System.Text.RegularExpressions;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.Windows;

public class ROSConnector : MonoBehaviour
{
    [SerializeField] string IPAddress = "127.0.0.1";
    [SerializeField] int Port = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ROSConnection ros = new ROSConnection();
        string pattern = @"^((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.){3}(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)$";

        bool isValid = Regex.IsMatch(IPAddress, pattern);

        if (isValid)
        {
            ros.Connect(IPAddress, Port);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
