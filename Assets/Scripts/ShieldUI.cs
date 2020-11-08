using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUI : MonoBehaviour
{
    private Image[] _childrenImages;

    void Start()
    {
        _childrenImages = GetComponentsInChildren<Image>();
        _childrenImages[0].enabled = true;
        _childrenImages[1].enabled = false;
    }

    public void TurnOnShield()
    {
        _childrenImages[0].enabled = false;
        _childrenImages[1].enabled = true;
    }

    public void TurnOffShield()
    {
        _childrenImages[0].enabled = true;
        _childrenImages[1].enabled = false;
    }
}
