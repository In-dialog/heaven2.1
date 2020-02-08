using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Restart : MonoBehaviour
{
    public GameObject Settings_UI;
    public GameObject Server_UI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (Settings_UI.gameObject.activeInHierarchy == false &&
                Server_UI.gameObject.activeInHierarchy == false)
            {
                Settings_UI.gameObject.SetActive(true);
                Server_UI.gameObject.SetActive(true);
            }
            else
            {
                Settings_UI.gameObject.SetActive(false);
                Server_UI.gameObject.SetActive(false);
            }
            
        }
    }
}
