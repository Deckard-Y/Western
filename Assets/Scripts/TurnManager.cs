using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image uiFill;
    [SerializeField] private TextMeshProUGUI uiTimerText;
    [SerializeField] private TextMeshProUGUI uiStatusText;
    [SerializeField] private TextMeshProUGUI uiSelectedCardText;
    [SerializeField] private TextMeshProUGUI uiEnemySelectedCardText;
    [SerializeField] private float CountTime;
    [SerializeField] private float timer;
    [SerializeField] private string SelectedCard;
    [SerializeField] private string EnemyCard;

    [SerializeField] private GameObject fireButton;
    [SerializeField] private GameObject dodgeButton;
    [SerializeField] private GameObject changeButton;
    [SerializeField] private GameObject skillButton;

    [SerializeField] private Transform cardsParent; // ボタンを配置するパネルのTransform
    [SerializeField] private List<UnityEngine.UI.Button> cardButtons;
    private List<GameObject> cardButtonsArray = new List<GameObject>();

    public Phase currentPhase;
    public enum Phase { Selection, Action, Processing }

    private bool hasPlayerExecuted = false;
    private bool hasEnemyExecuted = false;
    private bool isJudge = false;

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

    private void Start()
    {
        SetPhase(Phase.Selection);
        timer = CountTime;
        
        CreateCardButton(fireButton);
        CreateCardButton(dodgeButton);
        CreateCardButton(changeButton);
        CreateCardButton(skillButton);
    }

    private void OnEnable()
    {
        Card.OnCardSelected += HandleCardClick;
    }

    private void OnDisable()
    {
        Card.OnCardSelected -= HandleCardClick;
    }

    private void HandleCardClick(string cardInfo)
    {
        SelectedCard = cardInfo;
    }
    private void CreateCardButton(GameObject prefab)
    {
        GameObject buttonObj = Instantiate(prefab, cardsParent);
        UnityEngine.UI.Button button = buttonObj.GetComponent<UnityEngine.UI.Button>();
        cardButtons.Add(button);
        cardButtonsArray.Add(buttonObj);
    }

    private void DestroyCardButton()
    {
        cardButtons.Clear();
        foreach (var cardButton in cardButtonsArray)
        {
            Destroy(cardButton);
        }
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
        uiSelectedCardText.text = SelectedCard;

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
        DestroyCardButton();
        if(!hasPlayerExecuted)
        {
            switch (SelectedCard)
            {
                case "Fire":
                    Player.Instance.Fire(Enemy.Instance);
                    break;
                case "Dodge":
                    Player.Instance.Dodge();
                    break;
                case "Change":
                    Player.Instance.Change(); 
                    break;
                case "Skill":
                    Player.Instance.Skill(); 
                    break;
            }
            hasPlayerExecuted = true;
        }

        if(!hasEnemyExecuted)
        {
            int enemyCard = Random.Range(0, 4);
            switch (enemyCard)
            {
                case 0:
                    EnemyCard = "Fire";
                    Enemy.Instance.Fire(Player.Instance);
                    break;
                case 1:
                    EnemyCard = "Dodge";
                    Enemy.Instance.Dodge();
                    break;
                case 2:
                    EnemyCard = "Change";
                    Enemy.Instance.Change();
                    break;
                case 3:
                    EnemyCard = "Skill";
                    Enemy.Instance.Skill();
                    break;
            }
            hasEnemyExecuted = true;
        }

        if (hasPlayerExecuted && hasEnemyExecuted && !isJudge)
        {
            var player = Player.Instance;
            var enemy = Enemy.Instance;

            Debug.Log($"Player:{player.State} Enemy:{enemy.State}");
            Debug.Log($"Player:{player.IsBulletExist} Enemy:{enemy.IsBulletExist}");
            DamageProcesser(player,enemy);
            isJudge = true;
        }

        uiEnemySelectedCardText.text = EnemyCard;
        uiStatusText.text = "Action Phase";
        if(Input.GetKeyDown(KeyCode.Space))
            SetPhase(Phase.Processing); // 次のフェーズへ
    }

    private void DamageProcesser(Character from,Character target)
    {
        if (from.IsBulletExist && target.State != Character.CharacterState.Dodge)
        {
            target.TakeDamage();
        }
    }

    private void HandleProcessingPhase()
    {
        // 処理フェーズのロジック
        // ...
        uiStatusText.text = "Processing Phase";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCardButton(fireButton);
            CreateCardButton(dodgeButton);
            CreateCardButton(changeButton);
            CreateCardButton(skillButton);

            hasEnemyExecuted = false;
            hasPlayerExecuted = false;
            isJudge = false;
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

