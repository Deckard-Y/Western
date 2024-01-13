using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public static event Action<string> OnCardSelected; // �C�x���g��`

    [SerializeField] private Button button; // �{�^���̎Q��
    [SerializeField] private string cardInfo; // ���̃J�[�h�̏��

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
