using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ResourceType {food}
public class ResourceSource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;

    //events
    public UnityEvent onQuantityChange;

    public void GatherResource(int  amount, Player gatheringPLayer)
    {
        quantity -= amount;

        int amountToGive = amount;
        //obezbedjujemo da ne damo vise resursa nego sto postoji
        if(quantity < 0)
        {
            amountToGive = amount + quantity;
        }
        //
        gatheringPLayer.GainResource(type, amount);
        if (quantity <= 0)
            Destroy(gameObject);
        if (onQuantityChange != null)
        {
            onQuantityChange.Invoke();
        }




    }

}
