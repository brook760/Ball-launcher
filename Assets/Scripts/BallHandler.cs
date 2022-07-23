using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D currentBallRig;
    [SerializeField] private SpringJoint2D currentBallSpring;
    [SerializeField] protected float detachDelay;

    private Camera mainCamera;
    private bool isDragging;
    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if(currentBallRig == null) { return; }
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
                LaunchBall();

            isDragging = false;
            return; 
        }
        isDragging = true;
       Vector2 touchPos = 
            Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
        currentBallRig.position = worldPos;

        currentBallRig.isKinematic = true;
    }

    private void LaunchBall()
    {
        currentBallRig.isKinematic = false;
        currentBallRig = null;
        Invoke(nameof(DetachBall),detachDelay);
    }
    private void DetachBall()
    {
        currentBallSpring.enabled = false;
        currentBallSpring = null;
    }
}
