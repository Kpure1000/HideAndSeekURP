using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FPSCameraShake : MonoBehaviour
{

    [Header("Player Information")]
    [SerializeField] 
    private PlayerMovement playerMovement;

    [Header("Shake")]
    [SerializeField] 
    private bool m_shakeWhileRunning = true;
    [SerializeField]
    private bool m_shakeAfterFalling = true;
    [SerializeField]
    private bool m_shakeWhileGettingHurt = true;

    private Camera m_mainCamera;
    private Transform cameraTrans;

    // Start is called before the first frame update
    void Start()
    {
        m_mainCamera = GetComponent<Camera>();
        cameraTrans = m_mainCamera.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_shakeWhileRunning)
        {
            
        }
    }
}
