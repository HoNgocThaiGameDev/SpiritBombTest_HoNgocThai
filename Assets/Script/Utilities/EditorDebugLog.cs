using UnityEngine;
using System.Collections;

public static class EditorDebugLog {

    public static void Log( string message)
    {
        
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
