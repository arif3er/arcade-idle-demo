using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraLook : MonoBehaviour
{
    private CinemachineFreeLook cinemachine;
    private PlayerInput playerInput;

    [SerializeField]
    private float lookSpeed = 1f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        Vector2 delta = playerInput.PlayerMain.Look.ReadValue<Vector2>();
        cinemachine.m_XAxis.Value += delta.x * lookSpeed * 200 *Time.deltaTime;
        cinemachine.m_YAxis.Value += delta.y * lookSpeed * Time.deltaTime;
    }
}
