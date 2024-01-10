using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    List<bool> chamber = new();
    [SerializeField]
    int cylinder = 6;
    [SerializeField]
    int bullet = 1;

    public static event Action<bool> OnFire;

    // Start is called before the first frame update
    void Start()
    {
        LoadCylinder(cylinder,bullet);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadCylinder(int cylinder, int bullet)
    {
        chamber.Clear();
        for (int i = 0; i < cylinder; i++)
        {
            chamber.Add(false);
        }
        for (int i = 0; i < bullet; i++)
        {
            int loadBulletPosition = UnityEngine.Random.Range(0, cylinder);
            if (chamber[loadBulletPosition])
            {
                Debug.Log($"Bullet Conflicted{i}...Reloaded");
                i--;
            }
            chamber[loadBulletPosition] = true;
        }
        Debug.Log("Bullet Loaded");

        Debug.Log(string.Join(", ", chamber));
        

    }

    public void Fire()
    {
        if(chamber.Count == 0)
        {
            Debug.Log("Reload");
            return;
        }

        OnFire?.Invoke(chamber[0]);

        Debug.Log($"Fire:{chamber[0]}");
        chamber.RemoveAt(0);
    }
    public void Reload()
    {
        LoadCylinder(cylinder, bullet);
    }
}
