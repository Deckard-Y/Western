using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public static event Action<string,Button> OnCardSelected; // �C�x���g��`

    [SerializeField] private Button button; // �{�^���̎Q��
    [SerializeField] private string cardInfo; // ���̃J�[�h�̏��

    private void Start()
    {
        button.onClick.AddListener(() => OnCardSelected?.Invoke(cardInfo,button));
    }

    public void Destroy()
    {
        Destroy(this);
    }
}
