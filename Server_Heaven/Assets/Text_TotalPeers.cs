using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_TotalPeers : MonoBehaviour
{
    public GameServer Server;
    Text Text_SatAbove;
    // Start is called before the first frame update
    void Start()
    {
        Text_SatAbove = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Text_SatAbove.text = "Nr Of Peers : " + Server.Count_Peers;
    }
}
