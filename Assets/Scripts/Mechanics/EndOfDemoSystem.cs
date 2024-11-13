using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDemoSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public void ReloadScene()
    {
        SceneManager.LoadScene("Built 0.2");
    }

    // Update is called once per frame
    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
