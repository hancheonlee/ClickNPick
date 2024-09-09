using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public Shop[] shops;
    public Animator anim;
    public TextMeshProUGUI shopTitleText;
    public TextMeshProUGUI shopInfoText;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
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

    public void UpdateShopUI(int shopIndex)
    {
        if (shopIndex >= 0 && shopIndex < shops.Length)
        {
            shopTitleText.text = shops[shopIndex].shopTitle;  // Update title text
            shopInfoText.text = shops[shopIndex].shopInfo;    // Update info text
        }
    }
}
