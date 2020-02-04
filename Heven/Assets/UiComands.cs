using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UiComands : MonoBehaviour
{
    public SendToArduino ToArduino;
    public int arduinNr;
    public int step = 1;
    public Slider mainSlider;
    // Start is called before the first frame update
    private void Start()
    {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    public int ValueChangeCheck()
    {
        arduinNr = (int)mainSlider.value;
        return 1;
    }
    public void MoveUp()
    {
        ToArduino.arCom[arduinNr].MoveForword(step);
    }

    public void MoveDowm()
    {
        ToArduino.arCom[arduinNr].MoveBack(step);
    }

    public void MoveRight()
    {
        ToArduino.arCom[arduinNr].MoveRight(step);
    }
    public void MoveLeft()
    {
        ToArduino.arCom[arduinNr].MoveLeft(step);
    }
}
