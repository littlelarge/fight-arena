using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsManager : Singleton<WindowsManager>
{
    #region Variables
    [SerializeField] private WindowsPreset _windows;

    public WindowsPreset Windows => _windows;
    #endregion

    #region Structs
    [System.Serializable]
    public struct WindowsPreset
    {
        public WinnerWindow WinnerWindow;
    }
    #endregion
}