using UnityEngine;
using UnityEngine.UI;
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
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            {

                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("!!!!" + pos );

                ToArduino.arCom[arduinNr].MoveTo(pos);

                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                //{
                //    Debug.Log(hit.point);
                //}
                //FindObjectOfType<s>
                //Instantiate(particle, transform.position, transform.rotation);
            }
        }
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

    public void HomePrinter()
    {
        ToArduino.arCom[arduinNr].SendComand("Home");
    }
    public void Reset()
    {
        ToArduino.arCom[arduinNr].SendComand("Reset");
    }

}
