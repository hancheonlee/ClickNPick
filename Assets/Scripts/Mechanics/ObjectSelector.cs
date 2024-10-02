using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [Header("Object Interactions")]
    [SerializeField] private GameObject wire;
    [SerializeField] private Lamppost lamppost;
    [SerializeField] private Animator electricBox;
    [SerializeField] private GameObject electricBoxCol;

    [Header("Dialogue Conversations")]
    [SerializeField] private NPCConversation philConversation;
    [SerializeField] private NPCConversation lucyConversation;
    [SerializeField] private NPCConversation jakeConversation;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject transitionCamera;

    [Header("Mood and Animations")]
    [SerializeField] private Animator bone;
    [SerializeField] private Animator dogMood;
    [SerializeField] private Animator salmon;
    [SerializeField] private Animator catMood;
    [SerializeField] private GameObject mood;
    [SerializeField] private GameObject moodCat;
    public bool boneDropped;
    public bool salmonJumped;

    [Header("Shop and Progress UI")]
    [SerializeField] private ShopUI shop;
    [SerializeField] private ProgressBarSystem progressBarSystem;

    [Header("VFX")]
    [SerializeField] private GameObject clickVFX;
    [SerializeField] private float vfxTime = 0.5f;

    private Shop shops;
    private LEDTVMechanics tvMechanics;
    private InformativeObjectBehaviour objects;
    private AudioManager audioManager;
    private bool inDialogue = false;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        tvMechanics = FindAnyObjectByType<LEDTVMechanics>();
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

        if (dialogueUI.activeInHierarchy)
        {
            inDialogue = true;
            CameraSystem.free = false;
        }
        else
        {
            inDialogue = false;
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
                case "Smoke":
                    HandleSmokeInteraction();
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

    void HandleSmokeInteraction()
    {
        audioManager.PlaySFX("smokepoof");
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
                CameraSystem.Instance.LevelSwitcher(CameraSystem.Levels.Level1);
                CameraSystem.free = false;
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
        if (tvMechanics.currentState == LEDTVMechanics.TVState.Broken)
        {
            audioManager.PlaySFX("Electric");
        }
    }

    void HandleKeyInteraction(Collider2D keyCollider)
    {
        tvMechanics.keyCount++;
        keyCollider.enabled = false;
        audioManager.PlaySFX("Button");
        progressBarSystem.OnClick();

        if (tvMechanics.keyCount == 4)
        {
            tvMechanics.video.SetActive(true);
            CameraSystem.Instance.LevelSwitcher(CameraSystem.Levels.Level2);
            CameraSystem.free = false;
        }
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
