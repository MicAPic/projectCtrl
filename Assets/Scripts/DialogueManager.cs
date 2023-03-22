using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UI;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Parameters")] 
    [SerializeField]
    private float textSpeed = 0.04f;
    [SerializeField]
    private float skipCooldownTime = 0.5f;
    
    [Header("UI")]
    [SerializeField] 
    private TMP_Text nameText;
    [SerializeField] 
    private TMP_Text dialogueText;
    [SerializeField] 
    private GameObject continueIcon;
    [SerializeField] 
    private GameObject canvas;
    [SerializeField] 
    private GameObject finText;
    
    [Header("Ink")]
    [SerializeField] 
    private TextAsset inkScript;
    private Story _story;
    private const string SpeakerTag = "speaker";
    
    private bool _isPlaying;
    private int _currentLineLength;
    private bool _canContinue;
    private bool _canSkip;
    private Coroutine _displayLineCoroutine;
    private Coroutine _skipCooldownCoroutine;
    
    void Awake() 
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue()
    {
        canvas.SetActive(true);

        PlayerController.Instance._rigidbody.velocity = Vector2.zero;
        PlayerController.Instance.canMove = false;
        
        _story = new Story(inkScript.text);
        _story.BindExternalFunction("finishLevel", (string mode) =>
        {
            dialogueText.transform.parent.parent.gameObject.SetActive(false);
            Transition.Instance.duration = 3.0f;
            StartCoroutine(Transition.Instance.Fade(mode[0]));
            StartCoroutine(ShowFinText());
        });
        _isPlaying = true;

        ContinueStory();
    }

    private IEnumerator ShowFinText()
    {
        yield return new WaitForSeconds(2.0f);
        if (finText != null) finText.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }

    void Update()
    {
        if (!_isPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_canContinue)
            {
                ContinueStory();   
            }
            else if (_canSkip)
            {
                StopCoroutine(_displayLineCoroutine);
                dialogueText.maxVisibleCharacters = _currentLineLength;
                FinishDisplayingLine();
            }
        }
    }

    // public void SelectChoice(int id)
    // {
    //     if (_canContinue)
    //     {
    //         _story.ChooseChoiceIndex(id);
    //     }
    // }

    private void ContinueStory()
    {
        if (_story.canContinue)
        {
            // ClearChoices();
            if (_skipCooldownCoroutine != null)
            {
                StopCoroutine(_skipCooldownCoroutine);
            }
            _displayLineCoroutine = StartCoroutine(DisplayLine(_story.Continue()));
            HandleTags(_story.currentTags);
        }
        else
        {
            _isPlaying = false;
            StartCoroutine(WaitBeforeGivingControl());
        }
    }

    private IEnumerator WaitBeforeGivingControl()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController.Instance.canMove = true;
        Destroy(gameObject);
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = line;
        _currentLineLength = line.Length;

        continueIcon.SetActive(false);
        _canContinue = false;
        _canSkip = false;

        _skipCooldownCoroutine = StartCoroutine(SkipCooldown());
        for (var _ = 0; _ < _currentLineLength; _++)
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }

        FinishDisplayingLine();
    }

    private IEnumerator SkipCooldown()
    {
        yield return new WaitForSeconds(skipCooldownTime);
        _canSkip = true;
    }

    private void FinishDisplayingLine()
    {
        // DisplayChoices();
        continueIcon.SetActive(true);
        _canContinue = true;
        _canSkip = true;
    }

    // private void DisplayChoices()
    // {
    //     List<Choice> currentChoices = _story.currentChoices;
    //
    //     for (int i = 0; i < currentChoices.Count; i++)
    //     {
    //         var button = Instantiate(choiceButtonPrefab, choiceContainer, false);
    //         button.GetComponentInChildren<TMP_Text>().text = currentChoices[i].text;
    //         
    //         var choiceIndex = i;
    //         button.onClick.AddListener(() => SelectChoice(choiceIndex));
    //     }
    // }
    //
    // private void ClearChoices()
    // {
    //     foreach (Transform child in choiceContainer)
    //     {
    //         Destroy(child.gameObject);
    //     }
    // }

    private void HandleTags(List<string> currentTags)
    {
        foreach (var tag in currentTags)
        {
            string[] pair = tag.Split(':');
            if (pair.Length != 2)
            {
                Debug.LogError("Tag couldn't be parsed:" + tag);
            }

            string key = pair[0];
            string value = pair[1];

            switch (key)
            {
                case SpeakerTag:
                    nameText.text = value;
                    break;
                default:
                    Debug.LogWarning("Given tag is not implemented:" + key);
                    break;
            }
        }
    }
}
