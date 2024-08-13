using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrowdChatMessage : MonoBehaviour
{
    public string[] crowdMessages;

    public bool showing = false;

    public Canvas chatCanvas;
    public TextMeshProUGUI crowdText;

    public Camera cam;

    private void Start()
    {
        chatCanvas.enabled = false;
    }
    private void Update()
    {

    }
    public IEnumerator ShowRandomMessage()
    {
        showing = true;
        chatCanvas.enabled = true;
        if (crowdMessages.Length > 0)
        {
            int randomIndex = Random.Range(0, crowdMessages.Length);
            crowdText.text = crowdMessages[randomIndex];
        }
        yield return new WaitForSeconds(5);
        chatCanvas.enabled = false;
        showing = false;
    }


}
