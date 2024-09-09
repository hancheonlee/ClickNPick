using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowShopInfo()
    {
        anim.SetTrigger("Active");
        anim.SetBool("IsActive", true);
    }

    public void HideShopInfo()
    {
        anim.SetTrigger("Back");
        anim.SetBool("IsActive" , false);
    }

    public void ShopAnimation()
    {
        if (anim.GetBool("IsActive"))
        {
            HideShopInfo(); // Close shop if it's active
        }
        else
        {
            ShowShopInfo(); // Open shop if it's inactive
        }
    }
}
