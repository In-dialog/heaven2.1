using UnityEngine;

public class StateOfMachine : MonoBehaviour
{
    public bool state;
    private static StateOfMachine instance;

    public bool SetSate
    {
        set
        {

            state = value;
        }
        get
        {
            return (state);
        }
    }
    public static StateOfMachine Instance
    {
        get
        {
            return instance ?? (instance = new GameObject("StateOfMachine").AddComponent<StateOfMachine>());
        }
    }


}