using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject BallPrefab;
    //[SerializeField] private GameObject Blocks;
    [SerializeField] private Rigidbody2D Pivot;
    [SerializeField] protected float detachDelay;
    [SerializeField] protected float respawnDelay;

    private Rigidbody2D currentBallRig;
    private SpringJoint2D currentBallSpring;

    private Camera mainCamera;
    private bool isDragging;
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
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
    private void SpawnNewBall()
    {
        GameObject BallInstance =
        Instantiate(BallPrefab, Pivot.position, Quaternion.identity);

        currentBallRig = BallInstance.GetComponent<Rigidbody2D>();
        currentBallSpring = BallInstance.GetComponent<SpringJoint2D>();

        currentBallSpring.connectedBody = Pivot;
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
        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
