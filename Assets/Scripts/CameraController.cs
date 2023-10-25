using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    //https://gamedevbeginner.com/how-to-follow-the-player-with-a-camera-in-unity-with-or-without-cinemachine/

    // Object camera will follow
    public Transform target;

    public float x;
    public float z;
    
    // Distance from target's y position
    public float offset;

    // Runs after Update()
    private void LateUpdate() {
        transform.position = new Vector3(
            // X remains fixed
            x,
            // Follows the player's Y constantly.
            target.position.y + offset,
            // Z remains fixed.
            z
        );
    }

    
}
