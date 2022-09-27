using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitsManager : Singleton<UnitsManager>
{
    #region Variables
    [HideInInspector] public List<Unit> Units = new List<Unit>();
    #endregion

    #region UnityMethods
    public void Update()
    {
        if (SceneSettings.Instance.UnitsSpawned && Units.Count == 0)
        {
            Units = FindObjectsOfType<Unit>().ToList();
        }
    }
    #endregion
}
