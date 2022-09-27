using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    #region Variables
    public GameObject Ground;
    public GameObject SpawnRoot;
    public GameObject UnitPrefab;

    [Range(1, 6)]
    public float GroundSize = 1;

    [Range(2, 30)]
    public int UnitsCount = 2;

    private Vector3 _startScale;
    #endregion

    #region UnityMethods
    private void OnValidate()
    {
        if (_startScale == Vector3.zero)
            _startScale = Ground.transform.localScale;

        Ground.transform.localScale = _startScale * GroundSize;

        foreach (GameObject go in SpawnRoot.transform)
        {
            if (go != SpawnRoot.gameObject)
                Destroy(go);
        }

        for (int i = 0; i < UnitsCount; i++)
        {
            Instantiate(UnitPrefab, transform.position, Quaternion.identity, SpawnRoot.transform);
        }
    }
    #endregion
}
