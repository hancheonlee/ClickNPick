using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void GrowSprinkler()
    {
        anim.SetTrigger("Grow");
        col.enabled = false;
    }
}
