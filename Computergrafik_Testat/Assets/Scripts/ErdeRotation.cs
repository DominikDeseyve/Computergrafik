using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 @brief Script performing orbiting of desired object arround given reference object.
        Script is to be attached to object which shall be transformed.
 */
public class ErdeRotation : MonoBehaviour
{
    //--- PUBLIC VARIABLES (appearing in Unity Inspector) ---//

    /// Reference object
    public GameObject referenceObject;

    /// Duration of rotation in seconds
    public float durationSeconds;

    /**
     @brief Update method is called once per frame
     */
    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Transform.RotateAround.html
        transform.RotateAround(referenceObject.transform.position, Vector3.up, (360.0F / durationSeconds) * Time.deltaTime);
    }
}
