using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DimOnDistroy : MonoBehaviour
{
    public Transform[] temp;
   public  float count =1;
    public void DestroyMe()
    {
        count = 1;
        temp = new Transform[10];
        temp = GetComponentsInChildren<Transform>();
        foreach (var item in temp)
        {
            if (item.GetComponent<TextMeshPro>())
            {
                Destroy(this.gameObject, 10);

            }
            else
            {

                if (item.GetComponent<MeshRenderer>())
                {
                    MeshRenderer mr = item.GetComponent<MeshRenderer>();
                    mr.material.EnableKeyword("_EMISSION");
                    mr.material.SetColor("_EmissionColor", mr.material.GetColor("_EmissionColor") * count);
                    Destroy(this.gameObject, 10);
                    count -= 0.1f;
                    //Debug.Log("help");

                }
                else if (item.GetComponent<LineRenderer>())
                {
                    LineRenderer lr = item.GetComponent<LineRenderer>();
                    lr.material.EnableKeyword("_EMISSION");
                    lr.material.SetColor("_EmissionColor", lr.material.GetColor("_EmissionColor") * count);
                    Destroy(this.gameObject, 10);
                    count -= 0.1f;

                    //Debug.Log("help");

                }
            }

        }
    }

}




