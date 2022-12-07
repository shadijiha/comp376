using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] public  float baseSpeed        = 5f;
    [SerializeField] private float sprintSpeed      = 7.5f;
    [SerializeField] private float speedMultiplier  = 1f;
    [SerializeField] private float sensitivity      = 3.5f; // In the future this should be in the game settings
    [SerializeField] private float jumpScanDist     = 1.5f;
    
    public GameObject weaponRotation;


    private bool shootingBlock;

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

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        shootingBlock = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop any player movement if isEnabled is false;
        // Mainly used when game is pause.
        if (!isEnabled)
        {
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0);
            return;
        }

        // Calculate the movement velocity as a 3D vector
        xMov = Input.GetAxisRaw("Horizontal");
        zMov = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && zMov >= 0.01)
        {
            isSprinting = true;
            shootingBlock = true;
        }
        else
        {
            isSprinting = false;
            shootingBlock = false;
        }
        movHorizontal = transform.right * xMov;
        movVertical = transform.forward * (zMov);



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

    public bool getShootingBlock()
    {
        return this.shootingBlock;
    }
}