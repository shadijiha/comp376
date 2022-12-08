using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Vector3 velocity            = Vector3.zero;
    private Vector3 rotation            = Vector3.zero;
    private Vector3 additionalRotation  = Vector3.zero;
    private Vector3 previousPosition;
    public  float   currentVelocity;
    public  float   currentHorizontalVelocity;

    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0.0f;

    [SerializeField] private float cameraRotationLimit = 85f;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }

    // Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();

        currentVelocity = Vector3.Distance(transform.position, previousPosition);
        Vector3 currentHorizontalPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 previousHorizontalPos = new Vector3(previousPosition.x, 0, previousPosition.z);
        currentHorizontalVelocity = Vector3.Distance(currentHorizontalPos, previousHorizontalPos);
        previousPosition = transform.position;
    }

    /// <summary>
    /// Gets a movement vector and replaces the old one
    /// </summary>
    /// <param name="vel">The velocity vector</param>
    public void Move(Vector3 vel)
    {
        this.velocity = vel;
    }

    /// <summary>
    ///  Gets a rotation vector and replaces the old one
    /// </summary>
    /// <param name="rot">The rotation vector</param>
    public void Rotate(Vector3 rot)
    {
        this.rotation = rot;
    }

    public void AddRotation (Vector3 rot)
    {
        additionalRotation += rot;
    }

    /// <summary>
    ///  Gets a CAMERA rotation vector and replaces the old one
    /// </summary>
    /// <param name="rot">The CAMERA rotation vector</param>
    public void RotateCamera(float cam_rot)
    {
        this.cameraRotationX = cam_rot;
    }

    public void Jump()
    {
        rb.AddForce(new Vector3(0.0f, 1000.0f, 0.0f));
    }

    /// <summary>
    /// Moves the player based on the velocity variable
    /// </summary>
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Rotates the player based on the rotation variable
    /// </summary>
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation + additionalRotation));

        if (cam != null) {
            // Set our rotation and Clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX + additionalRotation.x, -cameraRotationLimit, cameraRotationLimit);

            // Apply rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0.0f, 0.0f);
        }

        additionalRotation = Vector3.zero;
    }

    public float getRotation()
    {
        return this.currentCameraRotationX;
    }
}