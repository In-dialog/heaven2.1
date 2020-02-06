using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public bool Switch_Cameras = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Switch_Cameras == true)
        {
            Debug.Log("--------");
            Switch_BetweenCamera();
            Switch_Cameras = false;
        }
    }

    void Switch_BetweenCamera()
    {
        foreach (Transform item in this.transform)
        {
            Debug.Log(item);
            if (item.gameObject.activeSelf == true)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
