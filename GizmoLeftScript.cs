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

    // Start is called before the first frame update
    void Start()
    {
        printIMUMessage();
    }

    private void Update()
    {
        Read();
        System.Threading.Thread.Sleep(10); //1 second
    }


    void printIMUMessage()
    {
        Debug.Log(111);
        initIMU();
    }

    void initIMU()
    {
        string name;
        string message;
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
                
                changeGizmoLeft(new Vector3(Convert.ToSingle(data[3]), Convert.ToSingle(data[4]), Convert.ToSingle(data[5])));
                // Debug.Log(GameObject.Find(objectName).transform.rotation);
            }
        }
            catch (TimeoutException) { }
        // }
    }

    public void changeGizmoLeft(Vector3 vector)
    {
        GameObject CustomTrackedObject = GameObject.Find(objectName);
        if (CustomTrackedObject != null)
        {
            Quaternion q = Quaternion.LookRotation(CustomTrackedObject.transform.position - vector);
            // Quaternion.Slerp(self.rotation, q, turnSpeed * Time.deltaTime);
            // Quaternion q1 = new Quaternion(1, 2, 3, 4);
            CustomTrackedObject.transform.rotation = Quaternion.Slerp(CustomTrackedObject.transform.rotation, q, Convert.ToSingle(0.5) * Time.time);  // 可以把 t * Time.time 理解成速度 * 时间 等于角位移
            Debug.Log(CustomTrackedObject.transform.rotation);
        }
    }

}
