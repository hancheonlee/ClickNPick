using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private InformativeObjectBehaviour objects;

    private AudioManager audioManager;

    private CameraMovement cameraMovement;

    public NPCConversation philConversation;
    public NPCConversation lucyConversation;
    public NPCConversation jakeConversation;

    public GameObject wire;

    public Lamppost lamppost;
    public Animator buttonAnim;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        cameraMovement = FindAnyObjectByType<CameraMovement>();
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
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("InteractableObject") || hit.collider.CompareTag("InteractableCharacter"))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (objects != null && objects.gameObject == hitObject)
                {
                    OnSelectedObjectAction();
                }
                else
                {
                    SelectObject(hitObject);
                    cameraMovement.FocusMode(hitObject.transform.position);    // Focus on object
                }
            }
            else if (hit.collider.CompareTag("Cats"))
            {
                audioManager.PlaySFX("Meow");
            }
            else if (hit.collider.CompareTag("Dogs"))
            {
                audioManager.PlaySFX("Bark");
            }
            else if (hit.collider.CompareTag("Lamp"))
            {
                if (objects != null)
                {
                    objects.selected = false;
                }

                objects = wire.GetComponent<InformativeObjectBehaviour>();
                objects.selected = true;
            }
            else if (hit.collider.CompareTag("Button"))
            {
                lamppost.currentState = Lamppost.lampState.Opened;
                buttonAnim.SetTrigger("ButtonPress");
            }
        }
        else
        {
            DeselectObject();
            cameraMovement.isFocusing = false;
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

            if (objects.gameObject.name == "Phil")
            {
                ConversationManager.Instance.StartConversation(philConversation);
            }
            else if (objects.gameObject.name == "Jake")
            {
                ConversationManager.Instance.StartConversation(jakeConversation);
            }
            else if (objects.gameObject.name == "Lucy")
            {
                ConversationManager.Instance.StartConversation(lucyConversation);
            }
        }
    }
}