using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public TextAsset wordListFile, levelJson;
    public TMPro.TextMeshProUGUI avgWordScoreTextBox, totalScoreTextBox;
    public List<TMPro.TextMeshProUGUI> letterTiles;
    public RectTransform answersRectTransform;
    [SerializeField]
    Button nextWordButton;
    WordsParser wordsParser;
    LettersGenerator lettersTile;
    string userWord = string.Empty;
    int totalScore = 0, currentLevel = 0;
    int MAX_CHAR = 3, MAX_SUPPORTED_LEVEL = 7;  //Max supported level is till 8 and Starts from 0
    public enum GamePlayType
    {
        Endless,
        Level
    }
    public GamePlayType gamePlayType;

    void Awake() => Instance = this;
    void Start()
    {
        if (wordsParser == null)
            wordsParser = new WordsParser(wordListFile.text);
        nextWordButton.interactable = false;
        nextWordButton.onClick.AddListener(() =>
        {
            NextChallenge();
        });
    }
    void NextChallenge()
    {
        bool isCorrect = wordsParser.CheckForCorrectAnswer(userWord);
        if (isCorrect)
        {
            totalScore += userWord.Length;
            avgWordScoreTextBox.text = $"Avg. Word Score : {userWord.Length}";
            totalScoreTextBox.text = $"Total Score : {totalScore}";
            if (gamePlayType == GamePlayType.Endless)
            {
                userWord = string.Empty;
                lettersTile.ResetLetterTiles(letterTiles);
            }
            else
            {
                userWord = string.Empty;
                currentLevel = (currentLevel < MAX_SUPPORTED_LEVEL-1) ? currentLevel + 1 : 0;   //This is added to clamp the levels supported for Levels JSON
                lettersTile.ResetLetterTiles(letterTiles, currentLevel);
            }
            foreach (Transform _transform in answersRectTransform.transform)
                Destroy(_transform.gameObject);
            nextWordButton.interactable = false;
        }
    }
    public void LoadLevel(bool isEndless)
    {
        if (isEndless)
        {
            gamePlayType = GamePlayType.Endless;
            EndlessGameType();
        }
        else
        {
            gamePlayType = GamePlayType.Level;
            LevelGameType();
        }
    }
    void EndlessGameType()
    {
        lettersTile = new LettersGenerator(letterTiles);
    }
    void LevelGameType()
    {
        lettersTile = new LettersGenerator(letterTiles, levelJson.text);
    }
    public bool AddCharaterToWord(char _droppedLetter)
    {
        if (userWord.Length < 6)
        {
            userWord += _droppedLetter;
            if (userWord.Length >= MAX_CHAR)
                nextWordButton.interactable = true;
            return true;
        }
        else
            return false;
    }
    public bool removeCharaterToWord(int _index)
    {
        if (userWord.Length > _index)
        {
            userWord = userWord.Remove(_index, 1);
            return true;
        }
        else
            return false;
    }
}