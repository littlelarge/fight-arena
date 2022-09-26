using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private NavMeshAgent _agent;
    #endregion

    #region Methods
    public void Move(Unit target)
    {
        _agent.SetDestination(target.transform.position);
    }
    #endregion
}
