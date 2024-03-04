using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FPSCameraFovController : MonoBehaviour
{

    [Header("Player Information")]
    [SerializeField]
    private PlayerMovement playerMovement;


    [Header("Fov")]
    [SerializeField]
    private bool m_changeFovWhenAcceleration = true;
    
    [SerializeField][Range(0f,90f)]
    private float m_normalFov = 65f;
    
    [SerializeField][Range(0f,90f)]
    private float m_accelFov = 75f;
    
    [SerializeField][Range(0f, 3f)]
    private float m_smoothTime = 0.5f;


    [Header("Fov Status")]
    [SerializeField]
    private float m_currentFov = 0f;

    private Camera m_mainCamera;
    private float m_fovVelocity;

    // Start is called before the first frame update
    void Start()
    {
        m_currentFov = m_normalFov;
        m_mainCamera = GetComponent<Camera>();
        m_mainCamera.fieldOfView = m_currentFov;
    }

    // Update is called once per frame
    void Update()
    {
        m_accelFov = Mathf.Clamp(m_accelFov, m_normalFov, m_accelFov);
        m_normalFov = Mathf.Clamp(m_normalFov, m_normalFov, m_accelFov);

        if (m_changeFovWhenAcceleration)
        {
            float targetFov = (playerMovement.IsRunning & playerMovement.IsMoving) ? m_accelFov : m_normalFov;

            m_currentFov = Mathf.SmoothDamp(m_currentFov, targetFov, ref m_fovVelocity, m_smoothTime);

            m_mainCamera.fieldOfView = m_currentFov;
        }
    }
}
