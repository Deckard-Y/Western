using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireText : MonoBehaviour
{
    public TextMeshProUGUI fireText;

    // Start is called before the first frame update
    void Start()
    {
        Player.OnFire += UpdateFireText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        Player.OnFire -= UpdateFireText; // イベントリスナーを解除
    }

    void UpdateFireText(bool result)
    {
        if(result)
            fireText.text = "Bang!";
        else
            fireText.text = "";

    }
}
