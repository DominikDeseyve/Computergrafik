using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class ErdeRotation : MonoBehaviour
{
    /// Reference object
    public GameObject referenceObject;

    /// Duration of rotation in seconds
    public float durationSeconds;

    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html
        transform.RotateAround(referenceObject.transform.position, Vector3.up, (360.0F / durationSeconds) * Time.deltaTime);
    }
}
