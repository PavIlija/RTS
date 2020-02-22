using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    //izracunava unit formaciju za zadatu destinaciju
    public static Vector3[] GetUnitGroupDestination(Vector3 moveToPos, int numUnits, float unitGap)
    {
        //vektor3 niz za finalnu destinaciju
        Vector3[] destinations = new Vector3[numUnits];

        //daje nam broj redova i kolona u odnosu na broj unita
        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);

        //trenutni red i kolona za koje racunamo
        int curRow = 0;
        int curCol = 0;

        float width = ((float)rows - 1) * unitGap;
        float lenght = ((float)cols - 1) * unitGap;

        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + (new Vector3(curRow, 0, curCol) * unitGap)- new Vector3(lenght/2,0,width/2);
            curCol++;

            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }




            
        }
        return destinations;
    }
    public static Vector3[] GetUnitGroupDestinationsAroundResource(Vector3 resourcePos, int unitsNum)
    {
        Vector3[] destinations = new Vector3[unitsNum];
        float unitDistanceGap = 360.0f / (float)unitsNum;

        for( int x=0; x<unitsNum; x++)
        {
            float angle = unitDistanceGap * x;
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
            destinations[x] = resourcePos + dir;



        }
        return destinations;
    }

    public static Vector3 GetUnitDestinationAroundResource(Vector3 pos)
    {
        float angle = Random.Range(0, 360);
        Vector3 dir= new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

        return pos + dir;
    }
}
