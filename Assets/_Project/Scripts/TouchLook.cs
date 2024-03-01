using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLook : MonoBehaviour
{
    [SerializeField] Transform playerBodyTrans;

    [SerializeField] float touchSensitivity;

    [SerializeField] TouchPad touchPad;

    public float TouchSensitivity { get => touchSensitivity; set => touchSensitivity = value; }

    float xAxisRotation = 0.0f;

    void Update()
    {
        // rotate
        Vector2 rotation = (touchPad.DeltaPosition) * TouchSensitivity * Time.deltaTime;

        xAxisRotation -= rotation.y;
        xAxisRotation = Mathf.Clamp(xAxisRotation, -89.99f, 89.99f);

        transform.localRotation = Quaternion.Euler(xAxisRotation, 0f, 0f);
        playerBodyTrans.Rotate(Vector3.up * rotation.x);

    }
}
