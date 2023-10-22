using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    public Colour Colour;

    private void FixedUpdate()
    {
        Despawn();
    }

    IEnumerable Despawn()
    {
        // https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
        // https://docs.unity3d.com/ScriptReference/Object.Destroy.html
        // Wait 1 second then despawn.
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
