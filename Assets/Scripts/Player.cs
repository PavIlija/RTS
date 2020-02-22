using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Units")]
    public List<Unit> units = new List<Unit>();
    [Header("Resources")]
    public int food;
    [Header("Components")]
    public GameObject unitPrefab;
    public Transform unitSpawnPos;
    public readonly int unitCost = 50;

    //events
     [System.Serializable]
    public class UnitCreatedEvent: UnityEvent<Unit> { }
    public UnitCreatedEvent onUnitCreated;

    
    private void Start()
    {
        
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateFoodCountText(food);
        CameraController.instance.FocusOnPosition(unitSpawnPos.position);
        food += unitCost;
        CreateNewUnit();
    }
   

    public void GainResource(ResourceType resourceType,int amount)
    {
         switch (resourceType)
        {
            case ResourceType.food:
            {
                    food = food + amount;
                    GameUI.instance.UpdateFoodCountText(food);
                break;
            }
        }
    }

    public void CreateNewUnit()
    {
        
        if (food - unitCost < 0)
            return;
        GameObject unitObj = Instantiate(unitPrefab, unitSpawnPos.position, Quaternion.identity,transform);
        
        Unit unit = unitObj.GetComponent<Unit>();
        units.Add(unit);
        unit.player = this;

        food -= unitCost;

        if (onUnitCreated != null)
            onUnitCreated.Invoke(unit);

        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateFoodCountText(food);


    }

    //provera da li je ovo moj civil
    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
    }
}
