using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFountainSystem : MonoBehaviour
{
    public Animator waterFountainUI;
    public GameObject cameraTransition;
    public GameObject waterFalls;

    public void OpenUI()
    {
        waterFountainUI.SetTrigger("Toggle");
        CameraSystem.free = false;
        ObjectSelector.inDialogue = true;
        CursorManager.enableCursor = false;
        AudioManager.Instance.PlaySFX("Click");
        cameraTransition.SetActive(true);
    }

    public void CloseUI()
    {
        waterFountainUI.SetTrigger("Close");
        CameraSystem.free = true;
        ObjectSelector.inDialogue = false;
        CursorManager.enableCursor = true;
        AudioManager.Instance.PlaySFX("Click");
        cameraTransition.SetActive(false);
    }

    public void FixWaterFall()
    {
        waterFalls.SetActive(true);
    }

}
