using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPress : MonoBehaviour
{
    public GameObject Menue;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            Menue.SetActive(active);
            active = !active;
        }
    }
}
