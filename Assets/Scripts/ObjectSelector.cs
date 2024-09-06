using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private InformativeObjectBehaviour objects;

    private AudioManager audioManager;
    private CursorManager cursorManager;
    private CameraMovement cameraMovement;
    private ZoomControl zoomControl;

    public NPCConversation philConversation;
    public NPCConversation lucyConversation;
    public NPCConversation jakeConversation;

    public GameObject wire;

    public Lamppost lamppost;
    public Animator electricBox;
    public GameObject electricBoxCol;

    public GameObject dialogueUI;
    public bool inDialogue = false;

    public LEDTVMechanics LEDTVMechanics;

    public Animator bone;
    public Animator dogMood;
    public GameObject mood;
    public bool boneDropped;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        cameraMovement = FindAnyObjectByType<CameraMovement>();
        cursorManager = FindAnyObjectByType<CursorManager>();
        zoomControl = FindAnyObjectByType<ZoomControl>();
        LEDTVMechanics = FindAnyObjectByType<LEDTVMechanics>();
    }
    private void Update()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0) && !inDialogue)
        {
            HandleInput(Input.mousePosition);
        }

        if (dialogueUI.activeInHierarchy)
        {
            inDialogue = true;
            cameraMovement.enabled = false;
            zoomControl.enabled = false;
        }
        else
        {
            inDialogue = false;
            cameraMovement.enabled = true;
            zoomControl.enabled = true;
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
                audioManager.PlaySFX("Highlight");
            }
            else if (hit.collider.CompareTag("ElectricBox"))
            {

                if (electricBox.GetBool("Open")) //Press button
                {
                    if (lamppost.currentState != Lamppost.lampState.Opened)
                    {
                        audioManager.PlaySFX("Electric");
                    }
                    lamppost.currentState = Lamppost.lampState.Opened;
                    electricBox.SetTrigger("Pressed");
                    electricBoxCol.SetActive(true);
                    audioManager.PlaySFX("Button");

                }
                else //Open box
                {
                    electricBox.SetBool("Open", true);
                    electricBox.SetTrigger("Opened");
                    electricBoxCol.SetActive(true);
                    audioManager.PlaySFX("DoorOpen");
                }
            }
            else if (hit.collider.CompareTag("ElectricBoxDoor"))
            {
                electricBox.SetTrigger("Closed");
                electricBox.SetBool("Open", false);
                electricBoxCol.SetActive(false);
                audioManager.PlaySFX("DoorClose");
            }
            else if (hit.collider.CompareTag("LEDTV"))
            {
                if (LEDTVMechanics.currentState == LEDTVMechanics.TVState.Broken)
                {
                    audioManager.PlaySFX("Electric");
                }
            }
            else if (hit.collider.CompareTag("Key"))
            {
                LEDTVMechanics.keyCount++;
                hit.collider.enabled = false;
                audioManager.PlaySFX("Button");
            }
            else if (hit.collider.CompareTag("Bone"))
            {
                bone.SetTrigger("Fall");
                hit.collider.enabled = false;
                boneDropped = true;
                
            }
            else if (hit.collider.CompareTag("SpecialDog"))
            {
                audioManager.PlaySFX("Bark");
                StartCoroutine(ShowMoodForTwoSeconds());
                if (boneDropped)
                {
                    dogMood.SetTrigger("Happy");

                }
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
                DeselectObject();
            }
            else if (objects.gameObject.name == "Jake")
            {
                ConversationManager.Instance.StartConversation(jakeConversation);
                DeselectObject();
            }
            else if (objects.gameObject.name == "Lucy")
            {
                ConversationManager.Instance.StartConversation(lucyConversation);
                DeselectObject();
            }
        }
    }

    IEnumerator ShowMoodForTwoSeconds()
    {
        mood.SetActive(true);  // Show mood
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        mood.SetActive(false); // Hide mood
    }
}