using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;//.Ports;

public class SendToArduino : MonoBehaviour
{
    ///-----------------------
    public List<SerialController> serialController = new List<SerialController>();
    public List<List<string>> _positionsToSend = new List<List<string>>();
    public List<List<string>> msgArrived = new List<List<string>>();

    GameObject SerailControl;
    ///-----------------------
    public int speed = 15000;
    bool initiateObject = true;
    public bool startProcess;

    public int nrOfMachines;
    ///-----------------------

    private void Start()
    {
        _positionsToSend.Add(new List<string>());
        _positionsToSend.Add(new List<string>());
        msgArrived.Add(new List<string>());
        msgArrived.Add(new List<string>());

    }
    private void Update()
    {
        List<string> ports = ExtensionMethods.GetPortNames();
        if (ports.Count <=0 | !startProcess) return;
        if (initiateObject)
        {
            //////////////////////////////////---------------------------------->>>>>>> Initialize conection to arduino 
            Destroy(SerailControl);
            SerailControl = new GameObject("serial");
            serialController = new List<SerialController>();

            if (ports.Count-1 == 1)
                for (int i = 0; i < nrOfMachines; i++)
                {
                    serialController.Add(SerailControl.transform.gameObject.AddComponent<SerialController>());
                    serialController[i].enabled = false;
                    serialController[i].portName = ports[i];
                    serialController[i].enabled = true;
                }
            else
            {
                serialController.Add(SerailControl.transform.gameObject.AddComponent<SerialController>());
                serialController[0].enabled = false;
                serialController[0].portName = ports[0];
                serialController[0].enabled = true;
            }
            initiateObject = false;
        }
        else
        {
            //////////////////////////////////---------------------------------->>>>>>> Sends to arduino
            if (serialController.Count -1 == 1)
            {
                SendData(serialController[0], 0);
                SendData(serialController[1], 1);
            }
            else
            {
                SendData(serialController[0], 0);
            }
        }
        //////////////////////////////////---------------------------------->>>>>>> End of update
    }

    void SendData(SerialController _serialController,int i) 
        {
            ReciveData(_serialController, i);

        if (msgArrived[i].Count >= 1)
        {
            print(msgArrived[i].Count);
            if (msgArrived[i][0].Contains("Grbl"))
            {
                _serialController.SendSerialMessage("G00X00Y00" + "F" + speed);
                msgArrived[i].RemoveAt(0);

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