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
    public Image AllGood;
    public ControlSystem cs;
    public GameClient gc;
    public  Slider slider;
    private void Start()
    {
        //FindObjectOfType<Mover>().rottationSpeed = PlayerPrefs.GetFloat("PSpeed");
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        slider.value = PlayerPrefs.GetFloat("PSpeed");
    }

    public void ValueChangeCheck()
    {
        Mover mv = FindObjectOfType<Mover>();
        if (mv)
            mv.rottationSpeed = slider.value;
        PlayerPrefs.SetFloat("PSpeed", slider.value);

        SendToArduino sendToArduino = FindObjectOfType<SendToArduino>();
        sendToArduino.speed = slider.value.Remap(0, 80, 200, 600);
        PlayerPrefs.SetFloat("Arduino", sendToArduino.speed);

    }

    public  void Activate(bool value)
    {
        atStart = value;
    }

    void Update()
    {
  
        if (gc.conected)
        {
            img.color = new Color32(1, 233,248,120);
            }
            else
            img.color = new Color(0.1f, 0.1f, 0.1f, 0.4f);
        if (atStart)
        {
            Vector3 temp = prefacDisplay.transform.localPosition;
            for (int i = 0; i < ToArduino.arCom.Count; i++)
            {
                GameObject gm = Instantiate(prefacDisplay);
                gm.transform.SetParent(this.transform);
                gm.transform.localPosition = new Vector3(temp.x, temp.y - (i * 25), temp.z);
                containers.Add(gm);
            }
            atStart = false;
        }
        int count = 0;
        for (int i = 0; i < containers.Count; i++)
        {
            if (ToArduino.arCom[i].SetNumber == 0)
            {
                containers[i].GetComponentInChildren<Text>().text = "XY -plotter";
            }
            else
            {
                containers[i].GetComponentInChildren<Text>().text = "V -plotter";
            }

            if (ToArduino.arCom[i].connectedOn)
            {
                 containers[i].GetComponentInChildren<Image>().color = new Color32(1, 233, 248, 120);
                count++;
            }
            else
            {
                containers[i].GetComponentInChildren<Image>().color = new Color(0.5f, 0, 0.2f, 0.5f);
            }


        }

        if (gc.conected & cs._wayPoints.Count > 1  & count==2)
        {
            AllGood.color = new Color32(1, 233, 248, 120);
        }else if (gc.conected & cs._wayPoints.Count > 1 & count == 1)
        {
            AllGood.color = new Color32(1, 233, 248, 120);

        }
    }
}
