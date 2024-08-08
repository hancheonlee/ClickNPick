using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class ObjectSelector : MonoBehaviour
{
    public GameObject Tower1_Panel; //Damian
    public Text infoText;  //Damian
    public string selectedObjectInfo;  //Damian

    public string selectableTag = "InformativeObject";

    private InformativeObjectBehaviour objects;
    
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    
        if (Tower1_Panel != null)  //Damian
        {
           Tower1_Panel.SetActive(false);
        }
        else
        {
           Debug.Log("Info Panel is not assigned in the Inspector");
        }

    }
    private void Update()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition);
        }
    }

    void HandleInput(Vector2 inputPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag(selectableTag))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (objects != null && objects.gameObject == hitObject)
            {
                OnSelectedObjectAction();
            }
            else
            {
                SelectObject(hitObject);
            }
        }
        else
        {
            DeselectObject();
        }
    }

    void SelectObject(GameObject selectedObject)
    {
        if (objects != null)
        {
            objects.selected = false;
        }

        objects = selectedObject.GetComponent<InformativeObjectBehaviour>();
        objects.selected = true;
        audioManager.PlaySFX("Highlight");
    }

    void DeselectObject()
    {
        if (objects != null)
        {
            objects.selected = false;
            objects = null;
        }
    }

    void OnSelectedObjectAction()
    {
        if (objects.selected)
        {
            audioManager.PlaySFX("Select");
            ShowInfoPanel(selectedObjectInfo); //Damian
            Tower1_Panel.SetActive(true); //Damian
        }
    }

    void ShowInfoPanel(string info)
    {
        infoText.text = info; //Damian
        Tower1_Panel.SetActive(true); //Damian
    }
}
