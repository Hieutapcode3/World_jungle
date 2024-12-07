using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CharactorManager : MonoBehaviour
{
    private List<char> currentChars = new List<char>(); 
    private List<Transform> charPositions = new List<Transform>(); 
    private bool isTracking = false; 
    public bool IsTracking => isTracking;
    [SerializeField] private TextMeshPro myWord; 
    public static CharactorManager Instance { get; private set; } 
    private int correctWordAmount = 0;

    [System.Serializable]
    public class KeyWord
    {
        public string _keyWord;
        public List<GameObject> charatorOfKeyWord; 
        public bool isAvailable = false;
    }
    public List<KeyWord> keyWords;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (myWord != null)
        {
            myWord.text = "";
        }
        if(keyWords.Count != 0){
            foreach (KeyWord keyword in keyWords)
            {
                foreach (GameObject charactor in keyword.charatorOfKeyWord)
                {
                    charactor.SetActive(false);
                }
                
            }
        }
    }
    public void StartTracking(char startChar, Transform startPosition)
    {
        isTracking = true;
        currentChars.Clear();
        charPositions.Clear();
        currentChars.Add(startChar);
        charPositions.Add(startPosition);
        UpdateWord();
    }

    public void AddChar(char nextChar, Transform nextPosition)
    {
        if (!isTracking) return;
        if (charPositions.Count > 1 && nextPosition == charPositions[charPositions.Count - 2])
        {
            currentChars.RemoveAt(currentChars.Count - 1);
            charPositions.RemoveAt(charPositions.Count - 1);
        }
        else if (!charPositions.Contains(nextPosition))
        {
            currentChars.Add(nextChar);
            charPositions.Add(nextPosition);
        }

        UpdateWord();
    }

    public void EndTracking()
    {
        isTracking = false;
        CheckAndActivateKeywords();
    }

    private void UpdateWord()
    {
        myWord.text = string.Join("", currentChars);
    }

    private void CheckAndActivateKeywords()
    {
        bool isCorrect = false;
        foreach (KeyWord keyword in keyWords){
            if (myWord.text == keyword._keyWord){
                isCorrect = true;
                StartCoroutine(CorrectEffect(myWord.transform));
                break;
            }
        }
        if(!isCorrect)
            StartCoroutine(UnCorrectEffect(myWord.transform));
        foreach (KeyWord keyword in keyWords)
        {
            if (myWord.text == keyword._keyWord)
            {
                if(!keyword.isAvailable){
                    keyword.isAvailable = true;
                    foreach (GameObject charactor in keyword.charatorOfKeyWord)
                    {
                        charactor.SetActive(true);
                    }
                    correctWordAmount++;
                    if(correctWordAmount == keyWords.Count){
                        GameManager.Instance.Success();
                    }
                }
            }
        }
    }
    private IEnumerator UnCorrectEffect(Transform answer){
        TextMeshPro yourAnswer = answer.GetComponent<TextMeshPro>();
        yourAnswer.color = Color.red;
        Transform originPos = yourAnswer.transform;
        yourAnswer.transform.DOMove(originPos.position + Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.transform.DOMove(originPos.position - Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.transform.DOMove(originPos.position + Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.transform.DOMove(originPos.position - Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.transform.DOMove(originPos.position + Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.transform.DOMove(originPos.position - Vector3.right * 0.2f,0.05f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(.05f);
        yourAnswer.text = "";
        yourAnswer.color = Color.black;

    }
    private IEnumerator CorrectEffect(Transform answer)
    {
    TextMeshPro yourAnswer = answer.GetComponent<TextMeshPro>();
    yourAnswer.color = Color.green;
    Transform originPos = yourAnswer.transform;
    for (int i = 0; i < yourAnswer.text.Length; i++)
    {
        string charToMove = yourAnswer.text[i].ToString();
        GameObject charObj = new GameObject("Character");
        TextMeshPro charText = charObj.AddComponent<TextMeshPro>();
        charText.text = charToMove;
        charText.fontSize = yourAnswer.fontSize;  
        charText.color = yourAnswer.color;        
        charObj.transform.position = originPos.position + new Vector3(i * 0.2f, 0, 0);  
        charText.transform.DOMoveY(originPos.position.y - 1f, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
    }
    yourAnswer.text = "";
    yourAnswer.color = Color.black;
    }

}
