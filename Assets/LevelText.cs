using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private Animator anim;

    public string level0;
    public string level1;
    public string level2;

    private void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
    }

    public void UpdateText(int lvl)
    {
        if (lvl == 0)
        {
            levelText.text = level0;
        }
        else if (lvl == 1)
        {
            levelText.text = level1;
        }
        else if (lvl == 2)
        {
            levelText.text = level2;
        }
        anim.Play("LevelText");
        
    }
}
