using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public TMP_InputField textInputChat; 

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0;

    [HideInInspector]
    public bool canMove = true;
    public bool canRotate = false;

    public Camera playerCamera; 
    public Camera firstPersonCamera; 

    [Header("Camera for Bobbing")]
    public Camera bobbingCamera1;
    public Camera bobbingCamera2; 

    private Alteruna.Avatar _avatar;

    public Button lockCursorButton;
    public GameObject[] settingsPanels;

    public float bobbingAmount = 0.05f;
    public float bobbingSpeed = 14.0f;
    private float bobbingTimer = 0.0f;
    private Vector3 initialBobbingCameraPosition1;
    private Vector3 initialBobbingCameraPosition2; 
    private float currentBobbingOffset1 = 0.0f;
    private float currentBobbingOffset2 = 0.0f;
    private float bobbingLerpSpeed = 5.0f;

    [Header("Camera Rotation")]
    [SerializeField]
    private Transform centralObject;
    [SerializeField]
    private float rotationSpeed = 3.0f;

    [Header("Visual Rotation")]
    public GameObject visualPlayer;
    public float visualRotationSpeed = 5f;

    private Vector3 lastMoveDirection;

    private bool isFirstPerson = false; 
    public GameObject firstPersonUI; 

    
    [Header("Mouse Sensitivity")]
    public float firstPersonMouseSensitivity = 2.0f; 
    public float firstPersonCameraSmoothness = 0; 

    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();

        if (!_avatar.IsMe)
            return;

        characterController = GetComponent<CharacterController>();

        if (bobbingCamera1 == null)
        {
            bobbingCamera1 = Camera.main;
        }

        if (bobbingCamera2 == null)
        {
            
            bobbingCamera2 = Camera.main;
        }

        initialBobbingCameraPosition1 = bobbingCamera1.transform.localPosition;
        initialBobbingCameraPosition2 = bobbingCamera2.transform.localPosition; 

        UnlockCursor();

        if (lockCursorButton != null)
        {
            lockCursorButton.onClick.AddListener(UnlockCursor);
        }

        lastMoveDirection = Vector3.zero;

       
        if (firstPersonCamera != null)
        {
            firstPersonCamera.enabled = false;
        }

        if (playerCamera != null)
        {
            playerCamera.enabled = true;
        }

        
        if (firstPersonUI != null)
        {
            firstPersonUI.SetActive(false);
        }
    }


    void Update()
    {
        if (!_avatar.IsMe)
            return;

       
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleCamera();
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        bool anyPanelActive = false;
        foreach (GameObject panel in settingsPanels)
        {
            if (panel != null && panel.activeSelf)
            {
                anyPanelActive = true;
                break;
            }
        }

        if (anyPanelActive)
        {
            UnlockCursor();
        }
        else if (Input.GetMouseButton(1) && !isFirstPerson)
        {
            LockCursor();
        }
        else if (!isFirstPerson) 
        {
            UnlockCursor();
        }

        HandleMovement(isRunning);

        if (canRotate)
        {
            if (isFirstPerson)
            {
                HandleFirstPersonCameraRotation();
            }
            else if (Input.GetMouseButton(1))
            {
                HandleCameraRotation();
            }
        }

        TrackMovementDirection();
        HandleVisualRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
    }

    private void HandleMovement(bool isRunning)
    {
        if (!canMove)
            return;

        Vector3 forward = centralObject.forward;
        Vector3 right = centralObject.right;

        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

       
        if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            currentBobbingOffset1 = Mathf.Sin(bobbingTimer) * bobbingAmount;
            currentBobbingOffset2 = Mathf.Sin(bobbingTimer) * bobbingAmount; 
        }
        else
        {
            currentBobbingOffset1 = Mathf.Lerp(currentBobbingOffset1, 0.0f, Time.deltaTime * bobbingLerpSpeed);
            currentBobbingOffset2 = Mathf.Lerp(currentBobbingOffset2, 0.0f, Time.deltaTime * bobbingLerpSpeed); 
        }

       
        Vector3 bobbingPosition1 = bobbingCamera1.transform.localPosition;
        bobbingPosition1.y = initialBobbingCameraPosition1.y + currentBobbingOffset1;
        bobbingCamera1.transform.localPosition = bobbingPosition1;

        Vector3 bobbingPosition2 = bobbingCamera2.transform.localPosition;
        bobbingPosition2.y = initialBobbingCameraPosition2.y + currentBobbingOffset2;
        bobbingCamera2.transform.localPosition = bobbingPosition2;
    }

    private void HandleCameraRotation()
    {
        if (playerCamera == null || centralObject == null)
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseX * rotationSpeed;
        rotationX -= mouseY * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        centralObject.rotation = Quaternion.Euler(rotationX, rotationY, 0); 
    }

    private void HandleFirstPersonCameraRotation()
    {
        if (firstPersonCamera == null || centralObject == null)
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

       
        rotationY += mouseX * firstPersonMouseSensitivity;
        rotationX -= mouseY * firstPersonMouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

     
        firstPersonCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

       
        centralObject.rotation = Quaternion.Euler(rotationX, rotationY, 0); // FIXED
    }


    private void TrackMovementDirection()
    {
        Quaternion centralRotation = Quaternion.Euler(0, centralObject.transform.eulerAngles.y, 0);
        Vector3 localMoveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 worldMoveDirection = centralRotation * localMoveDirection;

        if (worldMoveDirection.magnitude > 0.1f)
        {
            lastMoveDirection = worldMoveDirection.normalized;
        }
    }

    private void HandleVisualRotation()
    {
        if (visualPlayer == null || lastMoveDirection == Vector3.zero) return;
        if (isFirstPerson)
            {
              
                Quaternion targetRotation = Quaternion.Euler(0, centralObject.transform.eulerAngles.y + 180f, 0);
                visualPlayer.transform.rotation = Quaternion.Slerp(visualPlayer.transform.rotation, targetRotation, Time.deltaTime * visualRotationSpeed);
            }
            else
            {
                // Rotate based on movement direction in third-person mode
                Quaternion targetRotation = Quaternion.LookRotation(-lastMoveDirection); // Flipped rotation
                visualPlayer.transform.rotation = Quaternion.Slerp(visualPlayer.transform.rotation, targetRotation, Time.deltaTime * visualRotationSpeed);
            }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canMove = true;
        canRotate = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canMove = true;
        canRotate = false;
    }

    private void ToggleCamera()
    {
        if (playerCamera == null || firstPersonCamera == null) return;

        isFirstPerson = !isFirstPerson;

        if (isFirstPerson)
        {
           
            rotationX = centralObject.localEulerAngles.x;
            if (rotationX > 180f) rotationX -= 360f;
            rotationY = centralObject.localEulerAngles.y;

            // Enable first-person
            playerCamera.enabled = false;
            firstPersonCamera.enabled = true;

            // Apply rotation properly
            firstPersonCamera.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

            firstPersonUI.SetActive(true);
            LockCursor();
        }
        else
        {
            // Enable third-person
            playerCamera.enabled = true;
            firstPersonCamera.enabled = false;

            // DO NOT reset centralObject’s rotation
            firstPersonUI.SetActive(false);
        }
    }

    public Quaternion CameraRotation // all added kinda badly (can del later)
    {
        get
        {
            return isFirstPerson ? firstPersonCamera.transform.rotation : playerCamera.transform.rotation;
        }
    }

    public float CameraDistance
    {
        get
        {
            if (centralObject == null) return 0f;

            Camera currentCamera = isFirstPerson ? firstPersonCamera : playerCamera;
            return Vector3.Distance(currentCamera.transform.position, centralObject.position);
        }
    }
}