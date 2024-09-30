using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance;

    [Header("Assign Cameras")]
    public GameObject playerCam;
    public GameObject zoomCam;

    [Header("Camera Settings")]
    public float dragSpeed = 1.5f;
    public float zoomSpeed = 2.5f; 
    public float minZoom = 5.0f;   
    public float maxZoom = 9.0f;

    [Header("Camera Zoom Offset")]
    public Vector2 zoomOffset;
    public float zoomSize = 3.5f;

    [Header("Camera Borders")]
    [SerializeField] private Levels currentLevel;
    public Collider2D[] colliderLevel;

    private Vector3 dragOrigin;
    private Camera mainCamera;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineVirtualCamera zoomCamera;
    private CinemachineConfiner2D confiner;
    private Collider2D confinerCollider;
    public static bool free = true;
    private float scrollInput;

    public enum Levels
    {
        Level1, Level2, Level3
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        virtualCamera = playerCam.GetComponent<CinemachineVirtualCamera>();
        zoomCamera = zoomCam.GetComponent<CinemachineVirtualCamera>();
        confiner = playerCam.GetComponent<CinemachineConfiner2D>();
        StartCoroutine(HandleCameraMovement());
        StartCoroutine(HandleCameraZoom());
        LevelSwitcher(Levels.Level1);
    }

    private IEnumerator HandleCameraMovement()
    {
        while (true)
        {
            if (free)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    StartCoroutine(DragCamera());
                }
            }
            yield return null;
        }
    }

    private IEnumerator DragCamera()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            playerCam.transform.position += difference * dragSpeed;
            ClampCameraPosition();
            yield return null; // Continue while the input is being held
        }
    }

    private IEnumerator HandleCameraZoom()
    {
        while (true)
        {
            if (free)
            {
                scrollInput = Input.GetAxis("Mouse ScrollWheel");
                if (scrollInput != 0)
                {
                    virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize - scrollInput * zoomSpeed, minZoom, maxZoom);
                }
            }
            yield return null;
        }
    }

    private void ClampCameraPosition()
    {
        Vector3 cameraPos = playerCam.transform.position;
        Bounds bounds = confinerCollider.bounds;

        // Clamp the camera's position within the bounds of the confiner
        float clampedX = Mathf.Clamp(cameraPos.x, bounds.min.x, bounds.max.x);
        float clampedY = Mathf.Clamp(cameraPos.y, bounds.min.y, bounds.max.y);

        // Apply the clamped position to the camera's transform
        playerCam.transform.position = new Vector3(clampedX, clampedY, cameraPos.z);
    }

    public void LevelSwitcher(Levels level)
    {
        currentLevel = level;

        switch (currentLevel)
        {
            case Levels.Level1:
                confinerCollider = colliderLevel[0];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                break;

            case Levels.Level2:
                confinerCollider = colliderLevel[1];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                break;

            case Levels.Level3:
                confinerCollider = colliderLevel[2];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                break;

            default:
                Debug.LogWarning("Unknown level selected!");
                break;
        }
    }

    public void ZoomInToObject(GameObject g)
    {
        zoomCamera.m_Lens.OrthographicSize = zoomSize;
        zoomCam.SetActive(true);
        zoomCam.transform.position = new Vector3(g.transform.position.x + zoomOffset.x, 
            g.transform.position.y + zoomOffset.y, zoomCam.transform.position.z);
        playerCam.SetActive(false);
    }

    public void ZoomOut()
    {
        playerCam.SetActive(true);
        zoomCam.SetActive(false);
    }
}
