using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighLightManager : MonoBehaviour
{
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask selectableLayer;

    private Outline highlightOutline;
    private RaycastHit hit;

    

    void Update()
    {
        HoverHighLight();
    }

    public void HoverHighLight()
    {
        if (highlightOutline != null)
        {
            if (selectedObj == null || highlightOutline != selectedObj.GetComponent<Outline>())
            {
                highlightOutline.enabled = false;
            }
        } 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform;

            if (highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();
                highlightOutline.enabled = true;
            }
        }
    }

    public void SelectedHighlight()
    {
        if (highlightedObj.CompareTag("Enemy"))
        {
            if (selectedObj != null)
            {
               selectedObj.GetComponent<Outline>().enabled = false;
            }
            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;

            highlightOutline.enabled = true;
        }
    }

    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectedObj = null;
        }
    }
}
