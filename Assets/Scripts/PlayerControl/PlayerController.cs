using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    public Animator animator;

    private PlayerInput playerInput;

    private CharacterController controller;
    private Transform cameraMain;
    private Transform child;
    private Vector3 playerVelocity;
    //private bool groundedPlayer;

    public bool isMoving;

    public float playerSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 0f;
    //[SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
        child = transform.GetChild(0).transform;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        /*groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }*/

        Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
        move.y = 0f;
        // Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);

        if (controller.enabled)
            controller.Move(move * Time.deltaTime * playerSpeed);

        /*
        if (move.x > 0 || move.z > 0)
            isMoving = true;
        else
            isMoving = false;
        */

        if (move != Vector3.zero)
        {
            isMoving = true;
            animator.SetBool("IsMoving", true);
            gameObject.transform.forward = move;
        }
        else
        {
            isMoving = false;
            animator.SetBool("IsMoving", false);
        }

        // Changes the height position of the player..
        /*
        if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }*/

        if (controller.enabled)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        if (movementInput != Vector2.zero)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(child.localEulerAngles.x, cameraMain.localEulerAngles.y, child.eulerAngles.z));
            child.rotation = Quaternion.Lerp(child.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void PlayerCollIgnore(Collider other)
    {
        Physics.IgnoreCollision(controller, other);
    }
}
