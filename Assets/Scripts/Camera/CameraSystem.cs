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
    public GameObject transitionCam0;
    public GameObject transitionCam1;
    public GameObject transitionCam2;

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

    [Header("Camera Transition")]
    public float targetSize = 2f;
    public float duration = 2f;

    private Vector3 dragOrigin;
    private Camera mainCamera;
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineVirtualCamera zoomCamera;
    private CinemachineVirtualCamera transitionCamera;
    private CinemachineConfiner2D confiner;
    private Collider2D confinerCollider;
    public static bool free = true;
    private float scrollInput;
    private GameObject currentTransitionCamera;
    private LevelText levelText;

    public enum Levels
    {
        Level0, Level1, Level2, Level3
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
        cinemachineBrain = GetComponent<CinemachineBrain>();
        virtualCamera = playerCam.GetComponent<CinemachineVirtualCamera>();
        zoomCamera = zoomCam.GetComponent<CinemachineVirtualCamera>();
        confiner = playerCam.GetComponent<CinemachineConfiner2D>();
        levelText = FindAnyObjectByType<LevelText>();
        StartCoroutine(HandleCameraMovement());
        StartCoroutine(HandleCameraZoom()); 
        LevelSwitcher(Levels.Level0);
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
            case Levels.Level0:
                confinerCollider = colliderLevel[0];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                StartCoroutine(LevelTransition());
                break;

            case Levels.Level1:
                confinerCollider = colliderLevel[1];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                StartCoroutine(LevelTransition());
                break;

            case Levels.Level2:
                confinerCollider = colliderLevel[2];
                confiner.m_BoundingShape2D = confinerCollider;
                confiner.InvalidateCache();
                StartCoroutine(LevelTransition());
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
    }

    public void ZoomOut()
    {
        zoomCam.SetActive(false);
    }

    public IEnumerator LevelTransition()
    {
        levelText.gameObject.SetActive(true);
        if (currentLevel == Levels.Level0)
        {
            transitionCamera = transitionCam0.GetComponent<CinemachineVirtualCamera>();
            currentTransitionCamera = transitionCam0;
            levelText.UpdateText(0);
        }
        else if (currentLevel == Levels.Level1)
        {
            transitionCamera = transitionCam1.GetComponent<CinemachineVirtualCamera>();
            currentTransitionCamera = transitionCam1;
            levelText.UpdateText(1);
        }
        else if (currentLevel == Levels.Level2)
        {
            transitionCamera = transitionCam2.GetComponent<CinemachineVirtualCamera>();
            currentTransitionCamera = transitionCam2;
            levelText.UpdateText(2);
        }

        playerCam.SetActive(false);
        currentTransitionCamera.SetActive(true);
        free = false;
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

        float initialSize = transitionCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transitionCamera.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transitionCamera.m_Lens.OrthographicSize = targetSize;

        yield return new WaitForSeconds(0.5f);

        free = true;
        playerCam.SetActive(true);
        currentTransitionCamera.SetActive(false);
        levelText.gameObject.SetActive(false);
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
    }
}
