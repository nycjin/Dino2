using UnityEditor;
using UnityEngine;

public class GlobalLogic2 : MonoBehaviour
{
    // Singleton
    static GlobalLogic2 instance; 
    
    // Property named "Current Logic"
    // It can be accessed by "GlobalLogic.CurrentLogic"
    public static ILogic CurrentLogic { get; private set; }


    void Awake()
    {
        if ( instance != null && instance != this )
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    
    
}