using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReplaceText : MonoBehaviour
{
    public GameObject _object;
    public GameObject satelite;

    public Text text,text2,text3,text4;
    string original;
    string original2;
    string original3;
    string original4;

    int count;
    float lenth;

    void Start()
    {
        original = text.text;
        original2 = text2.text;
        original3 = text3.text;
        original4 = text4.text;
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> temp = satelite.GetComponent<ControlSystem>()._wayPoints;
        
        if (temp.Count != count & temp.Count>1)
        {
            lenth +=Vector3.Distance(temp[0], temp[1]);
        }
        text.text = original + "X: [" + Mathf.Ceil(_object.transform.position.x).ToString()+ "]  Y: [" + Mathf.Ceil(_object.transform.position.y).ToString() + "]";
        text2.text = original2 + "" + temp.Count;
        text3.text = original3 + Mathf.Ceil(Time.realtimeSinceStartup).ToString() + "s";
        text4.text = original4 + (Mathf.Round(lenth)/100).ToString() + " km";


        count = temp.Count;

    }
}
