using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public Transform targetObj;  // obj that camera will be follow 
    public float offset2Target;   // offset from Cam to target

    void Start()
    {
        offset2Target = 1f;
    }
}
