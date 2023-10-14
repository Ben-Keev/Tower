using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    //https://gamedevbeginner.com/how-to-follow-the-player-with-a-camera-in-unity-with-or-without-cinemachine/

    // Object camera will follow
    public Transform target;

    public float x = 2;
    public float z = -10;
    
    // Distance from target's y position
    public float offset = 30;

    private void LateUpdate() {
        transform.position = new Vector3(
            x,
            // Follows the player's Y constantly.
            target.position.y + offset,
            z
            );
        }

    
}
