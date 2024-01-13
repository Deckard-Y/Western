using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    List<bool> chamber = new();
    [SerializeField] private int cylinder = 6;
    [SerializeField] private int bullet = 1;

    public static Character Instance { get; private set; }

    public virtual int Health { get; set; }
    public bool IsBulletExist { get; private set; } = false;
    public CharacterState State { get; private set; } = CharacterState.None;
    public CharacterType SelectedCharacter { get; set; }

    public enum GanmanSkill
    {
        QuickDraw,
        DeadEye,
    }
    public enum SheriffSkill
    {
        LawsShield,
        FistOfJustice,
    }
    public enum OutlawSkill
    {
        WildShot,
        Plunder,
    }
    public enum CharacterState
    {
        None,
        Dodge,
        Fire
    }

    public enum CharacterType
    {
        Gunman,
        Sheriff,
        Outlaw
    }

    public void SetCharacterType(CharacterType type)
    {
        SelectedCharacter = type;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Selected : {SelectedCharacter}");

        LoadCylinder(cylinder, bullet);
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
                i--;

            chamber[loadBulletPosition] = true;
        }

        Debug.Log($"player : {string.Join(", ", chamber)}");

    }

    public virtual void Fire(Character target)
    {
        if (chamber.Count != 0)
        {
            State = CharacterState.Fire;
            IsBulletExist = chamber[0];
            chamber.RemoveAt(0);
            if (IsBulletExist && State != CharacterState.Dodge)
            {
                target.TakeDamage();
            }
        }
        else
        {
            State = CharacterState.None;
        }
    }
    private void Reload()
    {
        LoadCylinder(cylinder, bullet);
    }
    public virtual void Dodge()
    {
        State = CharacterState.Dodge;
    }

    public virtual void Change()
    {
        State = CharacterState.None;
        Reload();
    }
    public virtual void Skill()
    {
        State = CharacterState.None;
    }

    public virtual void TakeDamage()
    {
        Debug.Log($"Health:{Health}");
        Health--;
    }
}
