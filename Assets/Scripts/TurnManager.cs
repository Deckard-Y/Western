using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image uiFill;
    [SerializeField] private TextMeshProUGUI uiTimerText;
    [SerializeField] private TextMeshProUGUI uiStatusText;
    [SerializeField] private TextMeshProUGUI uiSelectedCardText;
    [SerializeField] private TextMeshProUGUI uiEnemySelectedCardText;
    [SerializeField] private TextMeshProUGUI uiPlayerHealth;
    [SerializeField] private TextMeshProUGUI uiEnemyHealth;
    [SerializeField] private float CountTime;
    [SerializeField] private float timer;
    [SerializeField] private string SelectedCard;
    [SerializeField] private string EnemyCard;
    private Button selectedButton;

    [SerializeField] private GameObject fireButton;
    [SerializeField] private GameObject dodgeButton;
    [SerializeField] private GameObject changeButton;
    [SerializeField] private GameObject skillButton;

    [SerializeField] private Transform cardsParent; // ボタンを配置するパネルのTransform
    [SerializeField] private List<Button> cardButtons;
    private List<GameObject> cardButtonsArray = new List<GameObject>();

    public Phase currentPhase;

    public enum Phase
    { Selection, Action, Processing }

    private bool hasPlayerExecuted = false;
    private bool hasEnemyExecuted = false;
    private bool isJudge = false;
    private bool isSetButtons = false;

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
        Player.Instance.Health = 3;
        Enemy.Instance.Health = 3;
    }

    private void OnEnable()
    {
        Card.OnCardSelected += HandleCardClick;
    }

    private void OnDisable()
    {
        Card.OnCardSelected -= HandleCardClick;
    }

    private void HandleCardClick(string cardInfo, Button button)
    {
        SelectedCard = cardInfo;
        selectedButton = button;
    }

    private void CreateCardButton(GameObject prefab)
    {
        GameObject buttonObj = Instantiate(prefab, cardsParent);
        Button button = buttonObj.GetComponent<Button>();
        cardButtons.Add(button);
        cardButtonsArray.Add(buttonObj);
    }

    private void DestroyCardButton(Button button)
    {
        Button[] buttons = cardsParent.GetComponentsInChildren<Button>();
        foreach (Button cardButton in buttons)
        {
            if (cardButton != button)
                Destroy(cardButton.gameObject);
        }
        cardButtons.Clear();
    }
    private void DestroyAllCardButton()
    {
        Button[] buttons = cardsParent.GetComponentsInChildren<Button>();
        foreach (Button cardButton in buttons)
        {
                Destroy(cardButton.gameObject);
        }
        cardButtons.Clear();
    }

    private void Update()
    {
        uiPlayerHealth.text = "Player Health : " + Player.Instance.Health.ToString();
        uiEnemyHealth.text = "Enemy Health : " + Enemy.Instance.Health.ToString();

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
                if (!isSetButtons)
                {
                    CreateCardButton(fireButton);
                    CreateCardButton(dodgeButton);
                    CreateCardButton(changeButton);
                    CreateCardButton(skillButton);
                    isSetButtons = true;
                }
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

        if (uiSelectedCardText.text.Length > 1)
        {
            DestroyCardButton(selectedButton);
        }

        if (timer <= 0)
        {
            DisplayRandomTimeOverMessage();
            SetPhase(Phase.Action);
        }
    }

    private void HandleActionPhase()
    {
        // アクションフェーズのロジック
        if (!hasPlayerExecuted)
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

        if (!hasEnemyExecuted)
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

        uiEnemySelectedCardText.text = EnemyCard;
        uiStatusText.text = "Action Phase";
        if (Input.GetKey(KeyCode.Space))
        {
            SetPhase(Phase.Processing); // 次のフェーズへ
        }
    }

    private void DamageProcesser(Character player, Character enemy)
    {
        if (player.IsBulletExist &&
            player.State == Character.CharacterState.Fire &&
            enemy.State != Character.CharacterState.Dodge)
        {
            enemy.TakeDamage();
        }
        if (enemy.IsBulletExist &&
            enemy.State == Character.CharacterState.Fire &&
            player.State != Character.CharacterState.Dodge)
        {
            player.TakeDamage();
        }

        if(player.Health <= 0)
        {
            uiStatusText.text = "Player Lose";
            if (Input.GetKey(KeyCode.Space))
                SceneManager.LoadScene("TitleScene");
        }
        else if (enemy.Health <= 0)
        {
            uiStatusText.text = "Player Win";
            if (Input.GetKey(KeyCode.Space))
                SceneManager.LoadScene("TitleScene");

        }
    }

    private void HandleProcessingPhase()
    {
        // 処理フェーズのロジック
        // ...
        uiStatusText.text = "Processing Phase";

        if (!isJudge)
        {
            var player = Player.Instance;
            var enemy = Enemy.Instance;

            Debug.Log($"Player:{player.State}({player.IsBulletExist}) " +
                        $"Enemy:{enemy.State}({enemy.IsBulletExist})");
            DamageProcesser(player, enemy);
            isJudge = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasEnemyExecuted = false;
            hasPlayerExecuted = false;
            isJudge = false;
            isSetButtons = false;
            cardButtons.Clear();
            cardButtonsArray.Clear();

            DestroyAllCardButton();
            SelectedCard = "";
            EnemyCard = "";
            uiSelectedCardText.text = "";

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