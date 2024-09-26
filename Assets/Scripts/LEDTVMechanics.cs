using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDTVMechanics : MonoBehaviour
{
    public int keyCount = 0;
    public TVState currentState;
    private SpriteRenderer currentSprite;
    private ProgressBarSystem progressBarSystem;
    [SerializeField] private Sprite level1;
    [SerializeField] private Sprite level2;
    [SerializeField] private Sprite level3;
    [SerializeField] private Sprite level4;
    public GameObject video;
    public enum TVState
    {
        Broken, Level1, Level2, Level3, Level4
    }
    private void Start()
    {
        currentState = TVState.Broken;
        currentSprite = GetComponent<SpriteRenderer>();
        progressBarSystem = FindAnyObjectByType<ProgressBarSystem>();
    }

    private void Update()
    {
        if (keyCount == 0)
        {
            currentState = TVState.Broken;
        }
        if (keyCount == 1)
        {
            currentState = TVState.Level1;
            currentSprite.sprite = level1;
        }
        if (keyCount == 2)
        {
            currentState = TVState.Level2;
            currentSprite.sprite = level2;
        }
        if (keyCount == 3)
        {
            currentState = TVState.Level3;
            currentSprite.sprite = level3;
        }
        if (keyCount == 4)
        {
            currentState = TVState.Level4;
            currentSprite.sprite = level4;
            video.SetActive(true);
            CameraSystem.Instance.LevelSwitcher(CameraSystem.Levels.Level2);
        }


    }

}
