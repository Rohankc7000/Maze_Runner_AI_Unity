using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject GameWonMenu;

    [Header("For Camera and Rotation")]
    public Transform mainCamera;
    public float mouseSensitivity;
    public float yRotationSpeed, xCameraSpeed;
    private float rotationYVelocity, cameraXVelocity;
    public float wantedYRotation;
    private float currentYRotation;
    public float wantedCameraXRotation;
    private float currentCameraXRotation;
    [SerializeField] private float topAngleView;
    [SerializeField] private float bottomAngleView;

    [Header("For Player Information")]
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody rb;

    public float movementSpeed;
    private float horizontalDirection;
    private float verticalDirection;
    private bool isSprinting;
    [SerializeField] private Vector3 movement;

    [Header("For Sound Information")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip walk;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip jump;
    private AudioClip currnetRunningClip;
    private bool isWalking;
    private bool isRunnig;
    private bool idle;
    [SerializeField] private bool isJumping;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currnetRunningClip = null;
        isJumping = false;

        //When loading from main menu fixing the position
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        getMouseInputs();
        getInputs();
        checkSounds();
    }

    private void getMouseInputs()
    {
        wantedYRotation += Input.GetAxis("Mouse X") * mouseSensitivity;

        wantedCameraXRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        wantedCameraXRotation = Mathf.Clamp(wantedCameraXRotation, bottomAngleView, topAngleView);

    }

    private void checkSounds()
    {
        if (!isJumping)
        {
            if (isWalking)
            {
                if (currnetRunningClip != walk)
                {
                    currnetRunningClip = walk;
                    audioSource.clip = currnetRunningClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else if (isRunnig)
            {
                if (currnetRunningClip != run)
                {
                    currnetRunningClip = run;
                    audioSource.clip = currnetRunningClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else if (idle)
            {
                if (currnetRunningClip != null)
                {
                    currnetRunningClip = null;
                    audioSource.clip = null;
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (currnetRunningClip != jump)
            {
                currnetRunningClip = jump;
                audioSource.clip = currnetRunningClip;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }

    private void getInputs()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        verticalDirection = Input.GetAxisRaw("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPaused)
        {
            movePlayer();
        }
    }

    private void LateUpdate()
    {
        if (!GameManager.Instance.IsPaused)
        {
            applyPlayerAndCameraRotation();
        }
    }

    private void applyPlayerAndCameraRotation()
    {
        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, yRotationSpeed);
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, wantedCameraXRotation, ref cameraXVelocity, xCameraSpeed);
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
        mainCamera.localRotation = Quaternion.Euler(currentCameraXRotation, 0, 0);
    }

    private void movePlayer()
    {
        generateMovementSpeed(); //Yo yeha sarekoo....
        if ((horizontalDirection == 0 && verticalDirection == 0))
        {
            isRunnig = false;
            isWalking = false;
            idle = true;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            movementSpeed = 0;
        }
        else
        {
            // Yeha bata sareko...
            movement = (((transform.right * horizontalDirection) + (transform.forward * verticalDirection))).normalized * movementSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z); // For Jumping
        }
    }

    private void generateMovementSpeed()
    {
        if (isSprinting)
        {
            isRunnig = true;
            isWalking = false;
            idle = false;
            movementSpeed = 2;
        }
        else
        {
            movementSpeed = 1f;
            transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            isWalking = true;
            isRunnig = false;
            idle = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            GameManager.Instance.IsPaused = true;
            GameWonMenu.SetActive(true);
        }
    }
}
