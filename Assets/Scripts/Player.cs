using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private int health = 3;
    public override int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, 10); } // 0-10�ɐ���
    }
    public static new Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
