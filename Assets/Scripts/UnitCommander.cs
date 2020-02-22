using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public GameObject selectionMarkPrefab;
    public LayerMask layerMask;

    //komponente
    private UnitSelection unitSelection;
    private Camera cam;

    private void Awake()
    {
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    private void Update()
    {   //pokretanje civila
        if (Input.GetMouseButtonDown(1) && unitSelection.HasUnitsSelected())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //svi koji su selektovani
            Unit[] selectedUnits = unitSelection.GetSelectedUnits();

            if(Physics.Raycast(ray, out hit, 100, layerMask))
            {
                if (hit.collider.CompareTag("Ground"))//da li je ray udario u ground layer
                {
                    UnitsMoveToPosition(hit.point, selectedUnits);
                    CreateSelectionMarker(hit.point,false);
                }
                else if (hit.collider.CompareTag("Resource"))//da li je ray udario u resurs layer
                {
                    
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);
                    CreateSelectionMarker(hit.point, true);
                }
            }

        }
    }
    //kreiranje markera na zemlji podignutog za 0.1 po y osi
    public void CreateSelectionMarker(Vector3 pos,bool large)
    {
        GameObject marker = Instantiate(selectionMarkPrefab,  new Vector3(pos.x, 0.1f, pos.z ),Quaternion.identity);
        if (large)
            marker.transform.localScale = Vector3.one * 3;
    }
    //pozivanje funkcije koja pomera civile
    void UnitsMoveToPosition(Vector3 movePos,Unit[] units)
    {
        Vector3[] destinations = UnitMover.GetUnitGroupDestination(movePos, units.Length, 2);

        for (int x= 0; x<units.Length; x++)
        {
            units[x].MoveToPosition(destinations[x]);
          
        }

    }
    //pozivanje funkcije koja prikuplja resurse
    void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].GatherResource(resource, UnitMover.GetUnitDestinationAroundResource(resource.transform.position));
        }
        else
        {
            Vector3[] destinations = UnitMover.GetUnitGroupDestinationsAroundResource(resource.transform.position, units.Length);
            for (int x=0; x<units.Length;x++)
            {
                units[x].GatherResource(resource, destinations[x]);
            }
        }
    }
}
