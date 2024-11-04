using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [Header("Object Interactions")]
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
    [SerializeField] private SpriteRenderer dogMood;
    [SerializeField] private Animator salmon;
    [SerializeField] private SpriteRenderer catMood;
    [SerializeField] private Sprite happyMood;
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
    private CrowdManager crowdManager;
    public static bool inDialogue = false;
    private Gate gate;

    public GameObject endGameText;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        crowdManager = FindAnyObjectByType<CrowdManager>();
        tvMechanics = FindAnyObjectByType<LEDTVMechanics>();
        shop = FindAnyObjectByType<ShopUI>();
        progressBarSystem = FindAnyObjectByType<ProgressBarSystem>();
        gate = FindAnyObjectByType<Gate>(); 
    }

    private void Update()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0) && !inDialogue && CameraSystem.free)
        {
            HandleInput(Input.mousePosition);
        }

        if (dialogueUI.activeInHierarchy)
        {
            inDialogue = true;
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
                    HandleInteractable(hit.collider.gameObject);
                    AudioManager.Instance.PlaySFX("Shops");
                    break;
                case "InteractableCharacter":
                    HandleCharacters(hit.collider.gameObject);
                    break;
                case "Cats":
                    HandleCatInteraction();
                    break;
                case "Dogs":
                    HandleDogInteraction();
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
                case "Gate":
                    HandleGateInteraction(hit.collider.gameObject);
                    break;
                case "WaterFountain":
                    HandleWaterFountainInteraction(hit.collider.gameObject);
                    break;
                case "Stone": 
                    AudioManager.Instance.PlaySFX("Stone");
                    break;
                case "GreenBush":
                    AudioManager.Instance.PlaySFX("GreenBush");
                    break;
                case "Leaves":
                    AudioManager.Instance.PlaySFX("BrownBush");
                    break;
                case "TrashCan":
                    AudioManager.Instance.PlaySFX("Dustbin");
                    break;
                case "Mushroom":
                    AudioManager.Instance.PlaySFX("Mushroom");
                    break;
                case "Miniflower":
                    AudioManager.Instance.PlaySFX("Miniflower");
                    break;
                case "Grass":
                    AudioManager.Instance.PlaySFX("Grass");
                    break;
                case "DontTouchMe":
                    AudioManager.Instance.PlaySFX("DontTouchMe");
                    break;
                case "Bench":
                    AudioManager.Instance.PlaySFX("Bench");
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

    void HandleCharacters(GameObject hitObject)
    {
        audioManager.PlaySFX("Select");

        switch (hitObject.gameObject.name)
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

    void HandleElectricBoxInteraction()
    {
        if (electricBox.GetBool("Open"))
        {
            if (lamppost.currentState != Lamppost.lampState.Opened)
            {
                StartCoroutine(LevelCompleteSFX());
                progressBarSystem.OnClick();
                CameraSystem.Instance.LevelSwitcher(CameraSystem.Levels.Level1);
                CameraSystem.free = false;
                crowdManager.StartCoroutine(crowdManager.SpawnCrowd());
                gate.OpenGate();
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
            StartCoroutine(LevelCompleteSFX());
            CameraSystem.Instance.LevelSwitcher(CameraSystem.Levels.Level2);
            CameraSystem.free = false;
            endGameText.SetActive(true);
        }
    }

    public IEnumerator LevelCompleteSFX()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlaySFX("Win");
    }

    void HandleBoneInteraction(Collider2D boneCollider)
    {
        bone.SetTrigger("Fall");
        boneCollider.enabled = false;
        boneDropped = true;
        dogMood.sprite = happyMood;
        progressBarSystem.OnClick();
    }

    void HandleSpecialDogInteraction()
    {
        audioManager.PlaySFX("Bark");
    }

    void HandleWaterPuddleInteraction(Collider2D puddleCollider)
    {
        salmon.SetTrigger("JumpOutWater");
        puddleCollider.enabled = false;
        salmonJumped = true;
        catMood.sprite = happyMood;
        progressBarSystem.OnClick();
    }

    void HandleFishCatInteraction()
    {
        audioManager.PlaySFX("Meow");
    }

    void HandleShopInteraction(GameObject shopGameObject)
    {
        shops = shopGameObject.GetComponent<Shop>();
        shop.UpdateShopUI(shops.shopTitle, shops.shopInfo);
        shop.ShopAnimation();
    }

    void HandleGateInteraction(GameObject hitObject)
    {
        if (hitObject == null)
        {
            DeselectObject();
        }
        else
        {
            SelectObject(hitObject);
        }
    }

    void HandleWaterFountainInteraction(GameObject hitObject)
    {
        WaterFountainSystem WaterFountain = hitObject.GetComponent<WaterFountainSystem>();
        WaterFountain.OpenUI();
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

            DeselectObject();
        }
    }

    void SpawnVFX()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since it's a 2D game
        GameObject vfx = Instantiate(clickVFX, mousePosition, Quaternion.identity);
        Destroy(vfx, vfxTime); // Destroy the VFX after a specified time
    }
}
