using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        
        Debug.Log("App Closed");
    }
}