using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private int health = 2;
    public override int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, 10); } // 0-10Ç…êßå¿
    }
    public static new Enemy Instance { get; private set; }

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
