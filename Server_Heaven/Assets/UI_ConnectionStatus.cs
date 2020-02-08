using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConnectionStatus : MonoBehaviour
{
    Image Status;
    public GameServer Server;
    void Start()
    {
        Status = this.GetComponent<Image>();
        
    }

    void Update()
    {
        if (Server.Conection == true)
        {
            Status.color = Color.green;
        }
        else
        {
            Status.color = Color.red;
        }
    }
}
