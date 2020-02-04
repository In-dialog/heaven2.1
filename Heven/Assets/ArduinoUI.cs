using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArduinoUI : MonoBehaviour
{
    public SendToArduino ToArduino;
    public GameObject prefacDisplay;
    List<GameObject> containers = new List<GameObject>();
    bool atStart;
    // Start is called before the first frame update
    void Start()
    {
 
       

    }

    // Update is called once per frame
    void Update()
    {

        if (ToArduino.arCom.Count - 1 > 0 & atStart)
        {
            Vector3 temp = prefacDisplay.transform.localPosition;
            for (int i = 0; i < ToArduino.arCom.Count - 1; i++)
            {
                GameObject gm = Instantiate(prefacDisplay);
                gm.transform.position = new Vector3(temp.x, temp.y - (i * 10), temp.z);
                gm.GetComponentInChildren<Text>
                containers.Add(gm);
            }
            atStart = false;
        }

        for (int i = 0; i < max; i++)
        {

        }


    }
    void DisplayStatus()
    {

    }
}
