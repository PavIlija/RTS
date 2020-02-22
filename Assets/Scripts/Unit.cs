using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public enum UnitState {
    Idle,
    Move,
    MoveToResource,
    Gather
}

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public UnitState state;

    public int gatherAmount;
    public float gatherRate;
    private float lastGatherTime;
    private ResourceSource curResourceSource;

    [Header("Components")]
    public GameObject selectionVisual;
    private NavMeshAgent navAgent;

    public Player player;
    //events
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    void SetState(UnitState toState)
    {
        state = toState;

        if (onStateChange != null)
            onStateChange.Invoke(state);

        if (toState == UnitState.Idle)
        {
            navAgent.isStopped = true;
            navAgent.ResetPath();
        }
    }

    private void Update()
    {
            switch(state)
        {
            case UnitState.Move:
                {
                    MoveUpdate(); 
                    break;
                }
            case UnitState.Gather:
                {
                    GatherUpdate();
                    break;
                }
            case UnitState.MoveToResource:
                {
                    MoveToResourceUpdate();
                    break;
                }
            
        }
    }

    void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Idle);
    }
    void MoveToResourceUpdate()
    {
        if(curResourceSource==null)
        {
            SetState(UnitState.Idle);
            return;
        }
        if (Vector3.Distance(transform.position, navAgent.destination) == 0.0f)
            SetState(UnitState.Gather);
    }
    void GatherUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }
        LookAt(curResourceSource.transform.position);
        if(Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount, player);
        }

    }
    //postavljanje pozicije gde treba ici
    public void MoveToPosition(Vector3 pos)
    {
        SetState(UnitState.Move);
        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);

        navAgent.isStopped = false;
        navAgent.SetDestination(pos);
    }

    public void ToogleSelectionVisual(bool selected)
    {
        selectionVisual.SetActive(selected);
    }
    //rotira civila ka zadatoj poziciji
    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
