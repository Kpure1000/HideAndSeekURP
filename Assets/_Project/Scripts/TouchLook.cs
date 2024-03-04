using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLook : MonoBehaviour
{
    [SerializeField] Transform playerBodyTrans;
    [SerializeField] TouchPad touchPad;
    [SerializeField] float touchSensitivity;

    public float TouchSensitivity { get => touchSensitivity; set => touchSensitivity = value; }

    [Header("Looking Status")]
    [SerializeField] float xAxisPitch = 0.0f;
    [SerializeField]float yAxisYaw = 0.0f;

    void Update()
    {
        // calculate rotate from touchpad
        Vector2 rotation = touchPad.DeltaPosition * TouchSensitivity * Time.deltaTime;

        // rotate camera X axis (pitch)
        xAxisPitch -= rotation.y;
        xAxisPitch = Mathf.Clamp(xAxisPitch, -89.99f, 89.99f);
        transform.localRotation = Quaternion.Euler(xAxisPitch, 0f, 0f);

        // rotate plyer Y axis (yaw)
        yAxisYaw = rotation.x;
        playerBodyTrans.Rotate(Vector3.up * yAxisYaw);

    }
}
