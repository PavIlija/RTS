using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public RectTransform selectionBox;
    public LayerMask unitLayerMask;//za selektovanja civila samo

    private List<Unit> selectedUnits = new List<Unit>();
    private Vector2 startPos;

    //komponente
    private Camera cam;
    private Player player;
    void Awake()
    {
        cam = Camera.main;// kesiramo kameru u promenljivu i to samo jedanput
        player = GetComponent<Player>();
    }
    void Update()
    {
        //klik misa
        if (Input.GetMouseButtonDown(0))//da li je kliknuto levim tasterom
        {
            ToogleSelectionVisual(false);
            selectedUnits = new List<Unit>();
            TrySelect(Input.mousePosition);
            startPos = Input.mousePosition;
        }
        //mis gore
        if (Input.GetMouseButtonUp(0))
        {
            RelaseSelectionBox();
        }
        //mis na dole
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);

        }



    }

    private void RelaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);//ukidamo selekcioni okvir 

        //uzmimamo min i maksimalnu poziciju, donju levu tacku i gornju desnu tacku 
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        //prolazimo kroz svaki nas civil
        foreach (Unit unit in player.units)
        {
            //konvertujemo unit poziciju u screen poziciju
            Vector3 screenPos = cam.WorldToScreenPoint(unit.transform.position);

            //da li je nasa pozicija unita unutar selektovanog okvira  
            if(screenPos.x> min.x && screenPos.x<max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.ToogleSelectionVisual(true);
            }
        }
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
            
        }
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    void TrySelect(Vector2 screenPos)//vektor 2 jer imamo samo 2 koordinate
    {
        //uz pomoc lasera pucamo na dole ono sto zelimo da selektujemo
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit,100, unitLayerMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToogleSelectionVisual(true);
            }
        }

    }

    void ToogleSelectionVisual(bool selected)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.ToogleSelectionVisual(selected);
        }
    }

    //da li ima selektovanih units-a ili ne 
    public bool HasUnitsSelected()
    {
        return selectedUnits.Count>0 ? true: false;
    }
    //vraca selektovane unite u niz
    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }



}
