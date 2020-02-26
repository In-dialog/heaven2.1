using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display_Text_GRBL : MonoBehaviour
{
    Text GRBL_text_container;
    public SendToArduino ar;
    string Buffer_Text;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        GRBL_text_container = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("YOYOOYOYOYOYOYO");
        //SendToArduino ar = GetComponent<SendToArduino>();
        if (GRBL_text_container.cachedTextGenerator.lines.Count == 40)
        {
            Buffer_Text = "";
        }
        int max = ar._positionsToSend[0].Count;
        //Debug.Log(max);
        if (max > 0)
        {
            //    for (int i = 0; i < max; i++)
            //    {
            if (i == 0)
            {
                Buffer_Text = ar._positionsToSend[0][0];
            }
            else if (!Buffer_Text.Contains(ar._positionsToSend[0][0]))
            {
                Buffer_Text = AddLine(Buffer_Text, ar._positionsToSend[0][0]);
                Debug.Log(GRBL_text_container.cachedTextGenerator.lines.Count);
            }
            //else if (Buffer_Text != ar._positionsToSend[0][i])
            //{
            //    Buffer_Text = AddLine(Buffer_Text, ar._positionsToSend[0][i]);
            //}
            //}
            i++;
        }
        GRBL_text_container.text = Buffer_Text;
    }

    public string AddLine(string oldString, string newLine)
    {
        oldString += "\n" + newLine;
        return oldString;
    }
}
