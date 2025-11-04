using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickButton : MonoBehaviour
{
    private bool isSpeedUp = false;

    [Header("Button Setting")]
    [SerializeField] private Image buttonIamge;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite pressedSprite;

    public void ToggleQuick()
    {
        isSpeedUp = !isSpeedUp;

        Time.timeScale = isSpeedUp ? 2f : 1f;
        buttonIamge.sprite = isSpeedUp ? pressedSprite : idleSprite;

    }
}
