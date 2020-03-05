using UnityEngine;
using System.Collections.Generic;

public class SendToArduino : MonoBehaviour
{
    ///-----------------------
    //public List<SerialController> serialController = new List<SerialController>();
    public List<List<string>> _positionsToSend = new List<List<string>>();
    public List<List<string>> msgArrived = new List<List<string>>();
    public List<ArduinoConmander> arCom = new List<ArduinoConmander>();
    public List<ArduinoConmander> arduinoConmanders = new List<ArduinoConmander>();

    GameObject SerailControl;
    ///-----------------------
    public float speed = 15000;
    bool initiateObject = true;
    public bool startProcess;
    int count;
    int delay;
    ///-----------------------

    private void Start()
    {
        speed= PlayerPrefs.GetFloat("Arduino");

        _positionsToSend.Add(new List<string>());
        _positionsToSend.Add(new List<string>());

        msgArrived.Add(new List<string>());
        msgArrived.Add(new List<string>());

    }
    public void setBool(bool value)
    {
        startProcess = value;
    }


    private void Update()
    {
        List<string> ports = ExtensionMethods.GetPortNames();
        if (ports.Count <=0 | !startProcess) return;

        if (initiateObject)
        {
            //////////////////////////////////---------------------------------->>>>>>> Initialize conection to arduino 
            Destroy(SerailControl);
            //serialController = new List<SerialController>();
            for (int i = 0; i < ports.Count; i++)
            {
                SerailControl = new GameObject("serial");
                SerailControl.transform.gameObject.AddComponent<SerialController>().enabled = false;
                SerialController sr = SerailControl.GetComponent<SerialController>();
                sr.portName = ports[i];
                sr.enabled = false;
                sr.enabled = true;

                ArduinoConmander arduino = new ArduinoConmander();
                arduino.SetNumber = 1;
                arduino.SetPot = ports[i];
                arduino.sr = sr;
                arCom.Add(arduino);
            }
            FindObjectOfType<ArduinoUI>().Activate(true);
            initiateObject = false;
        }
        else if(delay==60)
        {
            for (int i = 0; i < arCom.Count; i++)
            {
                SendData(arCom[i].SetSr, i);
                //print("-----------> machine:  " + arCom[i].nrOfMachine + "string to send " + _positionsToSend[i][0]);
            }
            if (arCom[0].nrOfMachine != 0 & count == 2)
            {
                _positionsToSend.Clear();
                _positionsToSend.Add(new List<string>());
                _positionsToSend.Add(new List<string>());

                msgArrived.Clear();
                msgArrived.Add(new List<string>());
                msgArrived.Add(new List<string>());
                arCom.Reverse();
            }
            delay = 0;
        }
        delay++;
    }
    void SendData(SerialController _serialController, int i)
    {
        ReciveData(_serialController, i);

        if(msgArrived[i].Count>=1)
        if (msgArrived[i][0].Length-1>=1)
        {
            print("----------->arduino msg:  " + msgArrived[i][0] + "machine number: " + i);
            ///////
            if (msgArrived[i][0].Contains("Grbl ")|| msgArrived[i][0].Contains("Marlin"))
            {
                if (msgArrived[i][0].Contains("XY") || msgArrived[i][0].Contains("Marlin"))
                {
                    arCom[i].SetNumber = 0;
                }
                //else
                //{
                //    arCom[i].SetNumber = 1;
                //}
                _serialController.SendSerialMessage("G00X1Y1" + "F" + speed);
                arCom[i].connectedOn = true;
                count++;
                msgArrived[i].RemoveAt(0);
                return;
            }
            ////////
            else if (msgArrived[i][0].Contains("ok") & _positionsToSend[i].Count-1 > 1)
            {
                    if (_positionsToSend[i][0] == "null") 
                    {
                        _positionsToSend[i].RemoveAt(0);
                        return;

                    }
                    string temp = _positionsToSend[i][0] + "F" + speed;
                _serialController.SendSerialMessage(temp);
                _positionsToSend[i].RemoveAt(0);
                msgArrived[i].RemoveAt(0);
                print(temp);
                return;
            }
            ////////
            else if (msgArrived[i][0].Contains("error: "))
            {
                Debug.Log("coruptdata " + i + "grbl= "+ _positionsToSend[i]);
                if (_positionsToSend[i].Count > 1)
                {
                    _positionsToSend[i].RemoveAt(0);
                }
                msgArrived[i].RemoveAt(0);
                _serialController.SendSerialMessage("G0");
                return;
            }

        }





        //if (msgArrived[i].Count >= 1)
        //{
        //    print("----------->  " + "arduino msg  "+msgArrived[i][0]+"inndex"+i);
        //    if (msgArrived[i][0].Contains("Grbl"))
        //    {
        //        if (msgArrived[i][0].Contains("XY"))
        //        {
        //            arCom[i].SetNumber = 0;

        //        }
        //        msgArrived[i].RemoveAt(0);
        //        _serialController.SendSerialMessage("G00X00Y00" + "F" + speed);
        //        arCom[i].connectedOn = true;
        //        count++;
        //        return;
        //    }
        //    else if (msgArrived[i][0].Contains("ok") & _positionsToSend[i].Count > 0)
        //    {
        //        string temp = _positionsToSend[i][0] + "F" + speed;
        //        _serialController.SendSerialMessage(temp);
        //        print(temp);
        //        _positionsToSend[i].RemoveAt(0);
        //        msgArrived[i].RemoveAt(0);
        //        return;
        //    }
        //    //else if (msgArrived[i][0].Contains("error"))
        //    else if (msgArrived[i][0].Contains("error: "))
        //    {
        //        Debug.Log("coruptdata" +i);
        //        _serialController.SendSerialMessage("G0");
        //        if (_positionsToSend[i].Count > 1)
        //        {
        //            _positionsToSend[i].RemoveAt(0);
        //        }
        //        msgArrived[i].RemoveAt(0);
        //        //msgArrived[i][0] = "ok";
        //        return;
        //    }
        //    else
        //    {
        //        msgArrived[i].RemoveAt(0);
        //    }
        //}
    }
    string ReciveData(SerialController _serialController,int i)
    {
        if (_serialController.portName == "")
            return "NoPort";
        string message = _serialController.ReadSerialMessage();
        if (message == null)
            return  "NoMsg";
        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        {
            Debug.Log("Connection established");
            return "ConectedToDev";
        }
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        {
            Debug.Log("Connection attempt failed or disconnection detected");
            return "NoDev";
        }
        else
        {
            Debug.LogWarning("Message arrived: " + message);
            if (message.Length-1>=1 & msgArrived[i].Contains(message)==false & message.Contains("SD card")==false & (message.Contains("Marlin") || message.Contains("Grbl ")|| message.Contains("ok") || message.Contains("error: ")))
            {
            
                    msgArrived[i].Add(message);
            }
            return message;
        }
    }
}