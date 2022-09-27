using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : Singleton<SceneSettings>
{
    #region Variables
    public GameObject Ground;
    public GameObject SpawnRoot;
    public GameObject UnitPrefab;

    [Range(1, 6)]
    public float GroundSize = 1;

    [Range(2, 100)]
    public int UnitsCount = 2;

    public int RandomOffset = 1;

    [Header("Units Settings")]
    public int MaxHealth = 10;
    public int MaxDamage = 10;

    public bool UnitsSpawned;

    private Vector3 _startScale;
    private bool _started;
    #endregion

    #region UnityMethods
    private void Start()
    {
        _started = true;

        OnValidate();
    }

    private void OnValidate()
    {
        if (_started)
        {
            if (_startScale == Vector3.zero)
                _startScale = Ground.transform.localScale;

            Ground.transform.localScale = _startScale * GroundSize;

            UnitsSpawned = false;

            foreach (Transform child in SpawnRoot.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < UnitsCount; i++)
            {
                Unit unit = Instantiate(UnitPrefab, transform.position, Quaternion.identity, SpawnRoot.transform).GetComponent<Unit>();

                unit.transform.position = new Vector3(Random.Range(-RandomOffset, RandomOffset), transform.position.y, Random.Range(-RandomOffset, RandomOffset));

                unit.Health = Random.Range(1, MaxHealth);
                unit.Damage = Random.Range(1, MaxDamage);
            }

            UnitsSpawned = true;
        }
    }
    #endregion
}
