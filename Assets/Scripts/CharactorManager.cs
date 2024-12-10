using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactorManager : MonoBehaviour
{
    private List<char> currentChars = new List<char>(); 
    private List<Transform> charPositions = new List<Transform>(); 
    private List<CharactorController> charControllers = new List<CharactorController>();
    private bool isTracking = false; 
    public bool IsTracking => isTracking;
    [SerializeField] private TextMeshPro myWord; 
    private Vector3 originPosMyWord;
    public static CharactorManager Instance { get; private set; } 
    private int correctWordAmount = 0;
    public bool canSelectBox;
    [SerializeField] private List<WordBoxCheck> wordBoxs;

    [System.Serializable]
    public class KeyWord
    {
        public string _keyWord;
        public List<GameObject> charatorOfKeyWord; 
        public bool isAvailable = false;
    }
    public List<KeyWord> keyWords;
    [SerializeField] private RopeEffect ropeEffect;

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
            originPosMyWord = myWord.transform.position;
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
        charControllers.Clear();

        currentChars.Add(startChar);
        charPositions.Add(startPosition);
        charControllers.Add(startPosition.GetComponent<CharactorController>()); 

        UpdateWord();
        UpdateLineRenderer();
    }

    public void AddChar(char nextChar, Transform nextPosition)
    {
        if (!isTracking) return;

        if (charPositions.Count > 1 && nextPosition == charPositions[charPositions.Count - 2])
        {
            int lastIndex = charPositions.Count - 1;
            charControllers[lastIndex].transform.GetChild(0).gameObject.SetActive(false);
            charControllers.RemoveAt(lastIndex);

            currentChars.RemoveAt(lastIndex);
            charPositions.RemoveAt(lastIndex);
        }
        else if (!charPositions.Contains(nextPosition))
        {
            currentChars.Add(nextChar);
            charPositions.Add(nextPosition);
            charControllers.Add(nextPosition.GetComponent<CharactorController>()); // Lấy CharactorController.
            nextPosition.GetChild(0).gameObject.SetActive(true);
        }

        UpdateWord();
        UpdateLineRenderer();
    }

    public void EndTracking()
    {
        isTracking = false;
        foreach (var controller in charControllers)
        {
            controller.transform.GetChild(0).gameObject.SetActive(false);
        }
        ropeEffect.ClearLineRenderer();
        CheckAndActivateKeywords();
    }
    private void UpdateLineRenderer()
    {
        ropeEffect.SetControlPoints(charPositions);
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
                StartCoroutine(CharactorEffectWord(keyword));
                if(!keyword.isAvailable){
                    keyword.isAvailable = true;
                    correctWordAmount++;
                    if(correctWordAmount == keyWords.Count){
                        GameManager.Instance.Success();
                        // LevelManager.Instance.UnlockNextLevel();
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
        answer.position = originPosMyWord;

    }
    private IEnumerator CorrectEffect(Transform answer)
    {
        TextMeshPro yourAnswer = answer.GetComponent<TextMeshPro>();
        yourAnswer.color = Color.green;
        Vector3 originalPosition = answer.position;
        yield return new WaitForSeconds(0.1f);
        answer.DOMove(originalPosition - Vector3.up, 0.25f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(0.25f);
        answer.position = originPosMyWord;
        yourAnswer.text = "";
        yourAnswer.color = Color.black;
    }

    private IEnumerator CharactorEffectWord(KeyWord keyWord){
        foreach (GameObject charactor in keyWord.charatorOfKeyWord){
            charactor.transform.localScale = Vector3.zero;
            charactor.SetActive(true);
            TextMeshPro _color = charactor.GetComponent<TextMeshPro>();
            _color.color = Color.green;
            charactor.transform.DOScale(1.5f,0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.15f);
            charactor.transform.DOScale(1f,0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.15f);
            _color.color = Color.black;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ActivateRandomCharactor()
    {
        List<GameObject> inactiveCharactors = new List<GameObject>();
        foreach (KeyWord keyword in keyWords)
        {
            foreach (GameObject charactor in keyword.charatorOfKeyWord)
            {
                if (!charactor.activeSelf)
                {
                    inactiveCharactors.Add(charactor);
                }
            }
        }
        if (inactiveCharactors.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, inactiveCharactors.Count);
            GameObject selectedCharactor = inactiveCharactors[randomIndex];
            StartCoroutine(CharactorEffect(selectedCharactor));
            CheckWordCompletion();
        }
        GameManager.Instance.DecreaseCoin(100);
        GameManager.Instance.canSelectChar = true;
    }
    public void ActiveSelectEffect(){
        if(wordBoxs.Count !=0){
            foreach(WordBoxCheck box in wordBoxs){
                box.ActiveEfftect();
            }
        }
    }
    public void UnActiveSelectEffect(){
        if(wordBoxs.Count !=0){
            foreach(WordBoxCheck box in wordBoxs){
                box.EndEffect();
            }
        }
    }
    

    public void ActivateSelectedCharactor(GameObject selectedCharactor)
    {
        StartCoroutine(CharactorEffect(selectedCharactor));
        CheckWordCompletion();
    }
    private IEnumerator CharactorEffect(GameObject selectedCharactor){
            selectedCharactor.transform.localScale = Vector3.zero;
            selectedCharactor.SetActive(true);
            TextMeshPro _color = selectedCharactor.GetComponent<TextMeshPro>();
            _color.color = Color.green;
            selectedCharactor.transform.DOScale(1.5f,0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            selectedCharactor.transform.DOScale(1f,0.1f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(0.1f);
            _color.color = Color.black;
    }
    private void CheckWordCompletion()
    {
        foreach (KeyWord keyword in keyWords)
        {
            if (keyword.isAvailable) continue;
            bool isComplete = true;
            foreach (GameObject charactor in keyword.charatorOfKeyWord)
            {
                if (!charactor.activeSelf)
                {
                    isComplete = false;
                    break;
                }
            }
            if (isComplete)
            {
                keyword.isAvailable = true;
                correctWordAmount++;
                StartCoroutine(CharactorEffectWord(keyword));
                if (correctWordAmount == keyWords.Count)
                {
                    GameManager.Instance.Success();
                }
            }
        }
    }
    public void UnActiveWordBox(){
        foreach(WordBoxCheck box in wordBoxs){
            box.gameObject.SetActive(false);
        }
    }


}
