using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;//.Ports;

public class SendToArduino : MonoBehaviour
{
    ///-----------------------
    //public List<SerialController> serialController = new List<SerialController>();
    public List<List<string>> _positionsToSend = new List<List<string>>();
    public List<List<string>> msgArrived = new List<List<string>>();
    public List<ArduinoConmander> arCom = new List<ArduinoConmander>();
    GameObject SerailControl;
    ///-----------------------
    public int speed = 15000;
    bool initiateObject = true;
    public bool startProcess;
    ///-----------------------

    private void Start()
    {
        _positionsToSend.Add(new List<string>());
        _positionsToSend.Add(new List<string>());
        msgArrived.Add(new List<string>());
        msgArrived.Add(new List<string>());
        //arCom.Add()

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
                SerialController sr = SerailControl.transform.gameObject.AddComponent<SerialController>();
                sr.enabled = false;
                sr.portName = ports[i];
                sr.enabled = true;

                ArduinoConmander arduino = new ArduinoConmander();
                arduino.SetNumber = i;
                arduino.SetPot = ports[i];
                arduino.sr = sr;
                arCom.Add(arduino);
                //Debug.Log(arCom[i].port);


            }
            FindObjectOfType<ArduinoUI>().Activate(true);
            initiateObject = false;
        }
        else
        {
            //////////////////////////////////---------------------------------->>>>>>> Sends to arduino
            if (arCom.Count -1 == 1)
            {
                SendData(arCom[0].SetSr, 0);
                SendData(arCom[1].SetSr, 1);
            }
            else
            {
                SendData(arCom[0].SetSr, 0);
            }
        }
        //////////////////////////////////---------------------------------->>>>>>> End of update
    }

    void SendData(SerialController _serialController,int i) 
        {
            ReciveData(_serialController, i);

        if (msgArrived[i].Count >= 1)
        {
            //print(msgArrived[i].Count);
            if (msgArrived[i][0].Contains("Grbl"))
            {
                _serialController.SendSerialMessage("G00X00Y00" + "F" + speed);
                msgArrived[i].RemoveAt(0);
                arCom[i].connectedOn = true;

            }
            else if (msgArrived[i][0].Contains("ok") & _positionsToSend[i].Count > 0)
            {
                string temp = _positionsToSend[i][0] + "F" + speed;
                _serialController.SendSerialMessage(temp);
                print(temp);
                _positionsToSend[i].RemoveAt(0);
                msgArrived[i].RemoveAt(0);

            }
            else if (msgArrived[i][0].Contains("error") )
            {
                Debug.Log("coruptdata");
                //_positionsToSend[i].Remove(_positionsToSend[i][0]);
                _serialController.SendSerialMessage("G0");
                if(_positionsToSend[i].Count-1>0)
                _positionsToSend[i].RemoveAt(0);
                msgArrived[i].RemoveAt(0);

            }
        }
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
            if (message.Length-1>1)
            {
                msgArrived[i].Add(message);
            }
            return message;
        }
    }
}