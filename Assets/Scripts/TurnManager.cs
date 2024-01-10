using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image uiFill;
    [SerializeField] private TextMeshProUGUI uiTimerText;
    [SerializeField] private float CountTime;
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI uiStatusText;

    private List<string> timeOverMessages = new List<string>
    {
        "Draw!",
        "High Noon...",
        "Time's Up, Partner!",
        "Out of Time, Outlaw!",
        "The Moment's Passed...",
        "Sunset for You...",
        "Your Clock's Run Out...",
        "End of the Line!",
        "No More Time to Linger...",
        "This Town Ain't Big Enough...",
        "Let's Rock!"
    };

    public enum Phase { Selection, Action, Processing }
    public Phase currentPhase;

    [SerializeField] private GameObject cardButtonPrefab; // カードボタンのプレハブ
    [SerializeField] private Transform cardsParent; // ボタンを配置するパネルのTransform
    [SerializeField] private List<UnityEngine.UI.Button> cardButtons;
    int initialHandSize = 5; // 初期手札の枚数
    private void Start()
    {
        SetPhase(Phase.Selection);
        timer = CountTime;

        // 手札のカードに対応するボタンを生成
        for (int i = 0; i < initialHandSize; i++)
        {
            CreateCardButton();
        }
    }
    private void CreateCardButton()
    {
        GameObject buttonObj = Instantiate(cardButtonPrefab, cardsParent);
        UnityEngine.UI.Button button = buttonObj.GetComponent<UnityEngine.UI.Button>();
        cardButtons.Add(button); // リストに追加
                                 // ここでボタンの設定を行う
                                 // 例: button.onClick.AddListener(() => OnCardClicked());
    }


    private void Update()
    {
        switch (currentPhase)
        {
            case Phase.Selection:
                // タイマーの処理
                HandleSelectionTimer();
                break;

            case Phase.Action:
                // アクションフェーズの処理
                HandleActionPhase();
                break;

            case Phase.Processing:
                // 処理フェーズの処理
                HandleProcessingPhase();
                break;
        }
    }

    private void SetPhase(Phase newPhase)
    {
        currentPhase = newPhase;
        // フェーズごとの初期化処理

        switch (currentPhase)
        {
            case Phase.Selection:
                timer = CountTime;
                break;
            case Phase.Action:
                // アクションフェーズの初期化
                break;
            case Phase.Processing:
                // 処理フェーズの初期化
                break;
        }
    }

    private void HandleSelectionTimer()
    {
        uiStatusText.text = "Selection Phase";
        timer -= Time.deltaTime;
        uiFill.fillAmount = timer / CountTime;
        uiTimerText.text = timer.ToString("F2");

        if (timer <= 0)
        {
            DisplayRandomTimeOverMessage();
            SetPhase(Phase.Action);
        }
    }

    private void HandleActionPhase()
    {
        // アクションフェーズのロジック
        // ...
        uiStatusText.text = "Action Phase";
        if(Input.GetKeyDown(KeyCode.Space))
            SetPhase(Phase.Processing); // 次のフェーズへ
    }

    private void HandleProcessingPhase()
    {
        // 処理フェーズのロジック
        // ...
        uiStatusText.text = "Processing Phase";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCardButton();
            SetPhase(Phase.Selection); // 次のフェーズへ
        }
    }

    private void DisplayRandomTimeOverMessage()
    {
        if (uiStatusText != null)
        {
            // リストからランダムなメッセージを選択
            int randomIndex = Random.Range(0, timeOverMessages.Count);
            uiTimerText.text = timeOverMessages[randomIndex];
        }
    }

}

