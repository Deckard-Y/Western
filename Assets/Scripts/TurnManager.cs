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

    [SerializeField] private GameObject cardButtonPrefab; // �J�[�h�{�^���̃v���n�u
    [SerializeField] private Transform cardsParent; // �{�^����z�u����p�l����Transform
    [SerializeField] private List<UnityEngine.UI.Button> cardButtons;
    int initialHandSize = 5; // ������D�̖���
    private void Start()
    {
        SetPhase(Phase.Selection);
        timer = CountTime;

        // ��D�̃J�[�h�ɑΉ�����{�^���𐶐�
        for (int i = 0; i < initialHandSize; i++)
        {
            CreateCardButton();
        }
    }
    private void CreateCardButton()
    {
        GameObject buttonObj = Instantiate(cardButtonPrefab, cardsParent);
        UnityEngine.UI.Button button = buttonObj.GetComponent<UnityEngine.UI.Button>();
        cardButtons.Add(button); // ���X�g�ɒǉ�
                                 // �����Ń{�^���̐ݒ���s��
                                 // ��: button.onClick.AddListener(() => OnCardClicked());
    }


    private void Update()
    {
        switch (currentPhase)
        {
            case Phase.Selection:
                // �^�C�}�[�̏���
                HandleSelectionTimer();
                break;

            case Phase.Action:
                // �A�N�V�����t�F�[�Y�̏���
                HandleActionPhase();
                break;

            case Phase.Processing:
                // �����t�F�[�Y�̏���
                HandleProcessingPhase();
                break;
        }
    }

    private void SetPhase(Phase newPhase)
    {
        currentPhase = newPhase;
        // �t�F�[�Y���Ƃ̏���������

        switch (currentPhase)
        {
            case Phase.Selection:
                timer = CountTime;
                break;
            case Phase.Action:
                // �A�N�V�����t�F�[�Y�̏�����
                break;
            case Phase.Processing:
                // �����t�F�[�Y�̏�����
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
        // �A�N�V�����t�F�[�Y�̃��W�b�N
        // ...
        uiStatusText.text = "Action Phase";
        if(Input.GetKeyDown(KeyCode.Space))
            SetPhase(Phase.Processing); // ���̃t�F�[�Y��
    }

    private void HandleProcessingPhase()
    {
        // �����t�F�[�Y�̃��W�b�N
        // ...
        uiStatusText.text = "Processing Phase";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCardButton();
            SetPhase(Phase.Selection); // ���̃t�F�[�Y��
        }
    }

    private void DisplayRandomTimeOverMessage()
    {
        if (uiStatusText != null)
        {
            // ���X�g���烉���_���ȃ��b�Z�[�W��I��
            int randomIndex = Random.Range(0, timeOverMessages.Count);
            uiTimerText.text = timeOverMessages[randomIndex];
        }
    }

}

