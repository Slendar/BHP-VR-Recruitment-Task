using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarUI_SINGLE : MonoBehaviour
{
    public static ScrollbarUI_SINGLE Instance;

    public event EventHandler<OnScrollbarChangeEventArgs> OnScrollbarChange;
    public class OnScrollbarChangeEventArgs : EventArgs
    {
        public float scrollbarValue;
    }


    private Scrollbar scrollbar;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one ScrollbarUI_SINGLE");
        }
        Instance = this;

        scrollbar = GetComponent<Scrollbar>();
    }

    public void CallOnScrollbarChange()
    {
        OnScrollbarChange?.Invoke(this, new OnScrollbarChangeEventArgs
        {
            scrollbarValue = scrollbar.value
        });
    }
}
