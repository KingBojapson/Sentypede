
using UnityEngine;


// camera follow player
public class FollowCamera : MonoBehaviour
{
    float followCamera;
    private void FixedUpdate()
    {
        followCamera = GameplayManager.instance.camMoveSpeed;
    }
}
