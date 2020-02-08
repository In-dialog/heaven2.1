using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_TotalSats : MonoBehaviour
{
    public Satalite_manager Sat_Man;
    Text Text_sat;
    // Start is called before the first frame update
    void Start()
    {
        Text_sat = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Text_sat.text = "Nr Of Satelites : " + Sat_Man.nr_sat;
    }
}
