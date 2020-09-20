using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Parsed list of words from the file.
/// </summary>
public class WordsParser
{
    List<string> _answerWords;
    public WordsParser(string _wordsData) => _answerWords = _wordsData.Split('\n').ToList();
    public bool CheckForCorrectAnswer(string _userCreatedWord) => _answerWords.Contains(_userCreatedWord.ToLower());
    public int IsWordsLoaded() => _answerWords.Count;
}