using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    private bool isTalking;
    public GameObject speech;
    public void Talks()
    {
        AudioManager.Instance.PlaySFX("KotakorenTalk");
        if (!isTalking)
        {
            StartCoroutine(showBubble());
        }
    }

    private IEnumerator showBubble()
    {
        speech.SetActive(true);
        isTalking = true;
        yield return new WaitForSeconds(2);
        speech.SetActive(false);
        isTalking = false;
    }

    public TextMeshProUGUI hintText;
    private Animator anim;

    public string level0;
    public string level1;
    public string level2;

    public void UpdateHintText(int lvl)
    {
        if (lvl == 0)
        {
            hintText.text = level0;
        }
        else if (lvl == 1)
        {
            hintText.text = level1;
        }
        else if (lvl == 2)
        {
            hintText.text = level2;
        }
    }
}
