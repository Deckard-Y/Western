using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public static event Action<string> OnCardSelected; // イベント定義

    [SerializeField] private Button button; // ボタンの参照
    [SerializeField] private string cardInfo; // このカードの情報

    private void Start()
    {
        cardInfo = button.name.Substring(0, button.name.Length - 7);
        button.onClick.AddListener(() => OnCardSelected?.Invoke(cardInfo));
    }

    public void Destroy()
    {
        Destroy(this);
    }
}
