using UnityEngine;
using TMPro;

public class TextPositon : MonoBehaviour
{
    TextMeshPro text;
        // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

        text.text = "X: " + Mathf.Ceil(this.transform.position.x).ToString()+ "Y: " + Mathf.Ceil(this.transform.position.y).ToString();
    }
}
