using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private NavMeshAgent _agent;

    public bool Freezed;
    #endregion

    #region Methods
    public void Move(Unit target)
    {
        if (_agent.isOnNavMesh)
            _agent.SetDestination(target.transform.position);
    }

    public void SetDestinitionToSelf()
    {
        if (_agent.isOnNavMesh)
            _agent.SetDestination(transform.position);
    }

    public void Freeze()
    {
        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            Freezed = true;
        }
    }

    public void Unfreeze()
    {
        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            Freezed = false;
        }
    }
    #endregion
}
