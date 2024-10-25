using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFountainSystem : MonoBehaviour
{
    public Animator waterFountainUI;
    public GameObject cameraTransition;
    public GameObject waterFalls;
    public AudioClip waterFallAmbient;
    public AudioSource sewage;
    public AudioSource waterFallAudio;

    public void OpenUI()
    {
        waterFountainUI.SetTrigger("Toggle");
        CameraSystem.free = false;
        ObjectSelector.inDialogue = true;
        CursorManager.enableCursor = false;
        AudioManager.Instance.PlaySFX("OpenVault");
        cameraTransition.SetActive(true);
    }

    public void CloseUI()
    {
        waterFountainUI.SetTrigger("Close");
        CameraSystem.free = true;
        ObjectSelector.inDialogue = false;
        CursorManager.enableCursor = true;
        AudioManager.Instance.PlaySFX("OpenVault");
        cameraTransition.SetActive(false);
    }

    public void FixWaterFall()
    {
  
        waterFallAudio.clip = waterFallAmbient;
        waterFalls.SetActive(true);
        waterFallAudio.Play();
        AudioManager.Instance.PlaySFX("Pipe");
        sewage.Play();
    }

}
