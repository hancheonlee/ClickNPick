using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private InformativeObjectBehaviour objects;

    private AudioManager audioManager;

    public NPCConversation philConversation;
    public NPCConversation lucyConversation;
    public NPCConversation jakeConversation;

    public GameObject wire;

    public Lamppost lamppost;
    public Animator electricBox;
    public GameObject electricBoxCol;

    public GameObject dialogueUI;
    public GameObject settingUI;
    public bool inDialogue = false;

    public LEDTVMechanics LEDTVMechanics;

    public Animator bone;
    public Animator dogMood;
    public Animator salmon;
    public Animator catMood;
    public GameObject mood;
    public GameObject moodCat;
    public bool boneDropped;
    public bool salmonJumped;

    public ShopUI shop;
    private Shop shops;

    private ProgressBarSystem progressBarSystem;

    public GameObject clickVFX;
    public float vfxTime = 0.5f;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        LEDTVMechanics = FindAnyObjectByType<LEDTVMechanics>();
        shop = FindAnyObjectByType<ShopUI>();
        progressBarSystem = FindAnyObjectByType<ProgressBarSystem>();
    }

    private void Update()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0) && !inDialogue)
        {
            HandleInput(Input.mousePosition);
        }

        if (dialogueUI.activeInHierarchy || settingUI.activeInHierarchy)
        {
            inDialogue = true;
            CameraSystem.free = false;
        }
        else
        {
            inDialogue = false;
            CameraSystem.free = true;
        }
    }

    void HandleInput(Vector2 inputPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            SpawnVFX();
            switch (hit.collider.tag)
            {
                case "InteractableObject":
                case "InteractableCharacter":
                    HandleInteractable(hit.collider.gameObject);
                    break;
                case "Cats":
                    HandleCatInteraction();
                    break;
                case "Dogs":
                    HandleDogInteraction();
                    break;
                case "Lamp":
                    HandleLampInteraction();
                    break;
                case "ElectricBox":
                    HandleElectricBoxInteraction();
                    break;
                case "ElectricBoxDoor":
                    HandleElectricBoxDoorInteraction();
                    break;
                case "LEDTV":
                    HandleLEDTVInteraction();
                    break;
                case "Key":
                    HandleKeyInteraction(hit.collider);
                    break;
                case "Bone":
                    HandleBoneInteraction(hit.collider);
                    break;
                case "SpecialDog":
                    HandleSpecialDogInteraction();
                    break;
                case "WaterPuddle":
                    HandleWaterPuddleInteraction(hit.collider);
                    break;
                case "FishCat":
                    HandleFishCatInteraction();
                    break;
                default:
                    DeselectObject();
                    CameraSystem.Instance.ZoomOut();
                    shop.HideShopInfo();
                    break;
            }
        }
        else
        {
            DeselectObject();
            CameraSystem.Instance.ZoomOut();
            shop.HideShopInfo();
        }
    }

    #region Interaction Methods

    void HandleInteractable(GameObject hitObject)
    {
        if (objects != null && objects.gameObject == hitObject)
        {
            OnSelectedObjectAction();
        }
        else
        {
            SelectObject(hitObject);
            CameraSystem.Instance.ZoomInToObject(hitObject);
        }
    }

    void HandleCatInteraction()
    {
        audioManager.PlaySFX("Meow");
    }

    void HandleDogInteraction()
    {
        audioManager.PlaySFX("Bark");
    }

    void HandleLampInteraction()
    {
        if (objects != null)
        {
            objects.selected = false;
        }

        objects = wire.GetComponent<InformativeObjectBehaviour>();
        objects.selected = true;
        audioManager.PlaySFX("Highlight");
    }

    void HandleElectricBoxInteraction()
    {
        if (electricBox.GetBool("Open"))
        {
            if (lamppost.currentState != Lamppost.lampState.Opened)
            {
                audioManager.PlaySFX("Electric");
                progressBarSystem.OnClick();
            }
            lamppost.currentState = Lamppost.lampState.Opened;
            electricBox.SetTrigger("Pressed");
            electricBoxCol.SetActive(true);
            audioManager.PlaySFX("Button");
        }
        else
        {
            electricBox.SetBool("Open", true);
            electricBox.SetTrigger("Opened");
            electricBoxCol.SetActive(true);
            audioManager.PlaySFX("DoorOpen");
        }
    }

    void HandleElectricBoxDoorInteraction()
    {
        electricBox.SetTrigger("Closed");
        electricBox.SetBool("Open", false);
        electricBoxCol.SetActive(false);
        audioManager.PlaySFX("DoorClose");
    }

    void HandleLEDTVInteraction()
    {
        if (LEDTVMechanics.currentState == LEDTVMechanics.TVState.Broken)
        {
            audioManager.PlaySFX("Electric");
        }
    }

    void HandleKeyInteraction(Collider2D keyCollider)
    {
        LEDTVMechanics.keyCount++;
        keyCollider.enabled = false;
        audioManager.PlaySFX("Button");
        progressBarSystem.OnClick();
    }

    void HandleBoneInteraction(Collider2D boneCollider)
    {
        bone.SetTrigger("Fall");
        boneCollider.enabled = false;
        boneDropped = true;
        progressBarSystem.OnClick();
    }

    void HandleSpecialDogInteraction()
    {
        audioManager.PlaySFX("Bark");
        StartCoroutine(ShowMoodForTwoSeconds());

        if (boneDropped)
        {
            dogMood.SetTrigger("Happy");
        }
    }

    void HandleWaterPuddleInteraction(Collider2D puddleCollider)
    {
        salmon.SetTrigger("JumpOutWater");
        puddleCollider.enabled = false;
        salmonJumped = true;
        progressBarSystem.OnClick();
    }

    void HandleFishCatInteraction()
    {
        audioManager.PlaySFX("Meow");
        StartCoroutine(ShowMoodForTwoSeconds());

        if (salmonJumped)
        {
            catMood.SetTrigger("Happy");
        }
    }

    void HandleShopInteraction(GameObject shopGameObject)
    {
        shops = shopGameObject.GetComponent<Shop>();
        shop.UpdateShopUI(shops.shopTitle, shops.shopInfo);
        shop.ShopAnimation();
    }

    #endregion

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

            if (objects.gameObject.CompareTag("InteractableObject"))
            {
                HandleShopInteraction(objects.gameObject);
            }

            switch (objects.gameObject.name)
            {
                case "Phil":
                    ConversationManager.Instance.StartConversation(philConversation);
                    break;
                case "Jake":
                    ConversationManager.Instance.StartConversation(jakeConversation);
                    break;
                case "Lucy":
                    ConversationManager.Instance.StartConversation(lucyConversation);
                    break;
            }

            DeselectObject();
        }
    }

    IEnumerator ShowMoodForTwoSeconds()
    {
        mood.SetActive(true);  // Show mood
        moodCat.SetActive(true); // Show Cat's Mood
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        mood.SetActive(false); // Hide mood
        moodCat.SetActive(false); // Hide Cat's Mood
    }

    void SpawnVFX()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since it's a 2D game
        GameObject vfx = Instantiate(clickVFX, mousePosition, Quaternion.identity);
        Destroy(vfx, vfxTime); // Destroy the VFX after a specified time
    }
}
