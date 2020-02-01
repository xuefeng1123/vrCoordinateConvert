using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

public class GizmoLeftScript : MonoBehaviour
{
    static SerialPort _serialPort;
    static bool _continue = true;
    //private GameObject CustomTrackedObject = null;
    static string objectName = "GizmoLeft";
    static string focusName = "focus";
    GameObject GizmoLeft = null;
    Rigidbody rb = null;
    GameObject Focus = null;

    // Start is called before the first frame update
    void Start()
    {
        printIMUMessage();
        initGameObject();
    }

    private void Update()
    {
        Read();
        // System.Threading.Thread.Sleep(10); //1 second
    }

    public void initGameObject()
    {
        GizmoLeft = GameObject.Find(objectName);
        rb = GizmoLeft.GetComponent<Rigidbody>();
        Focus = GameObject.Find(focusName);
    }


    void printIMUMessage()
    {
        Debug.Log(111);
        initIMU();
    }

    void initIMU()
    {
        string name;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        // Thread readThread = new Thread(Read);

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);


        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500; 
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        // readThread.Start();

        Console.Write("Name: ");
        name = Console.ReadLine();

        Console.WriteLine("Type QUIT to exit");



        // readThread.Join();
        // _serialPort.Close();
    }

    public void Read()
    {
        //while (_continue)
        //{
            try
            {
                if(_serialPort != null)
                {
                    string message = _serialPort.ReadLine();
                    // Debug.Log(message);
                string[] data = Regex.Split(message, "\\s+", RegexOptions.IgnoreCase);//\s表示匹配任何空白字符,+表示匹配一次或多次
                
                //数组size = 7, 最后一个项为空
                //int index = 0;
                //foreach (string i in data)
                //{
                //    Debug.Log(index + i);
                //    index++;
                //}
                
                changeGizmoLeftRotation(new Vector3(Convert.ToSingle(data[3]), Convert.ToSingle(data[4]), Convert.ToSingle(data[5])));
                // changeGizmoLeftPosition(new Vector3(Convert.ToSingle(data[0]), Convert.ToSingle(data[1]), Convert.ToSingle(data[2])));
                changeFocusPosition();
            }
        }
            catch (TimeoutException) { }
        // }
    }

    public void changeGizmoLeftRotation(Vector3 vector)
    {
        if (GizmoLeft != null)
        {
            Quaternion q = Quaternion.LookRotation(GizmoLeft.transform.position - vector);
            GizmoLeft.transform.rotation = Quaternion.Slerp(GizmoLeft.transform.rotation, q, Convert.ToSingle(0.5) * Time.time);  // 可以把 t * Time.time 理解成速度 * 时间 等于角位移
        }
    }

    public void changeGizmoLeftPosition(Vector3 vector)
    {
        //rb = GizmoLeft.GetComponent<Rigidbody>();
        //if(rb != null)
        //{
        //    rb.MovePosition(GizmoLeft.transform.position + vector);
        //}
        Debug.Log("-------------------------------------------");
        Debug.Log("vector:   " + vector);
        Debug.Log("position: " + GizmoLeft.transform.position);
        // GizmoLeft.transform.position = GizmoLeft.transform.position + vector;
    }

    public void changeFocusPosition()
    {
        //rb = GizmoLeft.GetComponent<Rigidbody>();
        //if(rb != null)
        //{
        //    rb.MovePosition(GizmoLeft.transform.position + vector);
        //}
        // Debug.Log("-------------------------------------------");
        // Debug.Log("vector:   " + vector);
        // Debug.Log("position: " + GizmoLeft.transform.position);
        Focus.transform.position = GizmoLeft.transform.forward * 0.5f;
        Debug.Log(Focus.transform.position);
    }
}
