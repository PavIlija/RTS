using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceSourceUI : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI resourceQuantityText;
    public ResourceSource resource;

     void OnMouseEnter()
    {
        popupPanel.SetActive(true);
    }
    private void OnMouseExit()
    {
        popupPanel.SetActive(false);
    }
    public void onResourceQuantityChange()
    {
        resourceQuantityText.text = resource.quantity.ToString();
    }
}
