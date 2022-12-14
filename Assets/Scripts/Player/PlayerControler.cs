using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] public  float baseSpeed        = 5f;
    [SerializeField] private float sprintSpeed      = 7.5f;
    [SerializeField] private float speedMultiplier  = 1f;
    [SerializeField] public float sensitivity       = 3.5f; // In the future this should be in the game settings
    [SerializeField] private float jumpScanDist     = 1.5f;

    public GameObject weaponRotation;


    private bool isEnabled;

    private PlayerMotor motor;
    private bool jumpOnCooldown;
    private bool grounded;
    public readonly float JUMP_COOLDOWN = 0.1f;    // In Seconds

    //Animatior
    public Animator animator;
    public bool isSprinting = false;
    public bool isJumping = false;

    private Vector3 movHorizontal;
    private Vector3 movVertical;

    float xMov;
    float zMov;

    // sound
    enum WalkingSoundState { WALK, SPRINT, NONE };
    WalkingSoundState currentWalkingSoundState = WalkingSoundState.NONE;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip sprintSound;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the movement velocity as a 3D vector
        xMov = Input.GetAxisRaw("Horizontal");
        zMov = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && zMov >= 0.01)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        movHorizontal = transform.right * xMov;
        movVertical = transform.forward * (zMov);

        HandleSound();

        // Final movement vector
        Vector3 velocity = (movHorizontal + movVertical).normalized * (isSprinting? sprintSpeed : baseSpeed) * speedMultiplier;

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
        ///
        ///

        // Apply CAMERA rotation
        motor.RotateCamera(cameraRotationX);
        //Weapon rotation
        Vector3 rotationW = new Vector3(0, 0, -motor.getRotation());
        weaponRotation.transform.localRotation = Quaternion.Euler(rotationW);

        if (!jumpOnCooldown)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, jumpScanDist);
        }

        // Listen for the space bar
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            isJumping = true;
            motor.Jump();
            jumpOnCooldown = true;
            Invoke("ClearCooldown", JUMP_COOLDOWN);
        }
        else
        {
            isJumping =false;
        }

        //Animation
        animator.SetFloat("xSpeed", xMov);
        animator.SetFloat("zSpeed", zMov);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isJumping", isJumping);
    }

    private void HandleSound()
    {
        WalkingSoundState previous = currentWalkingSoundState;

        if (isSprinting)
        {
            currentWalkingSoundState = WalkingSoundState.SPRINT;
        }
        else if (motor.currentHorizontalVelocity < 0.001 && motor.currentHorizontalVelocity > -0.001)
        {
            currentWalkingSoundState = WalkingSoundState.NONE;
        }
        else
        {
            currentWalkingSoundState = WalkingSoundState.WALK;
        }

        if (currentWalkingSoundState == WalkingSoundState.NONE)
        {
            audioSrc.Stop();
        } 
        
        else if (previous != currentWalkingSoundState)
        {
            audioSrc.Stop();

            if (currentWalkingSoundState == WalkingSoundState.WALK)
            {
                audioSrc.PlayOneShot(walkSound);
            }
            
            else if (currentWalkingSoundState == WalkingSoundState.SPRINT)
            {
                audioSrc.PlayOneShot(sprintSound);
            }
        }

        else if (previous == currentWalkingSoundState)
        {
            if (!audioSrc.isPlaying)
            {
                if (currentWalkingSoundState == WalkingSoundState.WALK)
                {
                    audioSrc.PlayOneShot(walkSound);
                }

                else if (currentWalkingSoundState == WalkingSoundState.SPRINT)
                {
                    audioSrc.PlayOneShot(sprintSound);
                }
            }
        }
    }

    /// <summary>
    /// This function clears the cooldown of the jump
    /// </summary>
    void ClearCooldown()
    {
        jumpOnCooldown = false;
    }

    public void EnableMovement(bool enable)
    {
        isEnabled = enable;
    }

    public void SetSpeedMultiplier(float speed)
    {
        this.speedMultiplier = speed;
    }
}