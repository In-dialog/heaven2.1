using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArduinoUI : MonoBehaviour
{
    public SendToArduino ToArduino;
    public GameObject prefacDisplay;
    public List<GameObject> containers = new List<GameObject>();
     bool atStart;
    public Image img;
   public GameClient gc;
    // Start is called before the first frame update
    void Start()
    {
 
       

    }
  public  void Activate(bool value)
    {
        atStart = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.conected)
            img.color = new Color(0, 1, 0, 0.4f);
        else
            img.color = new Color(0, 0.5f, 0.5f, 0.4f);
        //print(ToArduino.arCom.Count);
        if (atStart)
        {
            Vector3 temp = prefacDisplay.transform.localPosition;
            for (int i = 0; i < ToArduino.arCom.Count; i++)
            {
                GameObject gm = Instantiate(prefacDisplay);
                gm.transform.SetParent(this.transform);
                gm.transform.localPosition = new Vector3(temp.x, temp.y - (i * 50), temp.z);
                containers.Add(gm);
            }
            atStart = false;
        }
 

        for (int i = 0; i < containers.Count; i++)
        {
            containers[i].GetComponentInChildren<Text>().text = ToArduino.arCom[i].port;
            if(ToArduino.arCom[i].connectedOn)
                 containers[i].GetComponentInChildren<Image>().color = new Color(0,0,1,0.4f);
            else
                containers[i].GetComponentInChildren<Image>().color = new Color(1, 0, 0, 0.4f);

            //print(ToArduino.arCom[i].port);
        }

    }
    void DisplayStatus()
    {

    }
}
