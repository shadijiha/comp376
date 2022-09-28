using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivity = 3.0f; // In the future this should be in the game settings

    private PlayerMotor motor;
    private bool jumpOnCooldown;
    public readonly float JUMP_COOLDOWN = 1.0f;    // In Seconds

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock the cursor
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;


        // Calculate the movement velocity as a 3D vector
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        // Final movement vector
        Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

        // Apply movement
        motor.Move(velocity);

        // Calculate rotation as a 3D vector FOR TURNING AROUND
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0.0f, yRot, 0.0f) * sensitivity;

        // Apply rotation
        motor.Rotate(rotation);

        // Calculate CAMERA rotation as a 3D vector FOR AIMING AROUND
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * sensitivity;

        // Apply CAMERA rotation
        motor.RotateCamera(cameraRotationX);

        // Listen for the space bar
        if (Input.GetKeyDown(KeyCode.Space) && !jumpOnCooldown)
        {
            motor.Jump();
            jumpOnCooldown = true;
            Invoke("ClearCooldown", JUMP_COOLDOWN);
        }
    }

    /// <summary>
    /// This function clears the cooldown of the jump
    /// </summary>
    void ClearCooldown()
    {
        jumpOnCooldown = false;
    }
}