using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void CharacterSelet()
    {
        string type = gameObject.name;
        switch (type)
        {
            case "Ganman":
                Character.Instance.SetCharacterType(Character.CharacterType.Gunman);
                break;

            case "Sheriff":
                Character.Instance.SetCharacterType(Character.CharacterType.Sheriff);

                break;

            case "Outlaw":
                Character.Instance.SetCharacterType(Character.CharacterType.Outlaw);
                break;
        }
        Debug.Log($"Seleted : {type}");
        SceneManager.LoadScene("MainScene");
    }
}