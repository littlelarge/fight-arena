using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerWindow : Singleton<WinnerWindow>
{
    #region Variables
    [SerializeField] private TextMeshProUGUI textMeshPro;
    #endregion

    #region Methods
    public void Show()
    {
        gameObject.SetActive(true);

        textMeshPro.text = $"WINNER: {UnitsManager.Instance.Units[0].transform.name}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
