using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite _musicImg;
    [SerializeField] private Sprite _muteImg;
    public bool isMuted;
    public bool isStart = false;
    [SerializeField] private Text _bgIndexTxt;
    private int _indexBgTxt = 1;
    [SerializeField] private List<Button> _musicButtons; 
    [SerializeField] private List<Button> _bgButtons; 
    [SerializeField] private List<Sprite> BackgroundSprites; 
    [SerializeField] private List<Image> backgroundPanel; 
    [SerializeField] private GameObject sucessPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winGamePanel;
    private const string SelectedButtonKey = "SelectedButtonIndex"; 
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        if (_musicButtons.Count != 0)
        {
            foreach (Button btn in _musicButtons)
            {
                btn.image.sprite = isMuted ? _muteImg : _musicImg;
                btn.onClick.AddListener(OnMusicButtonClick);
            }
        }
        if(backgroundPanel.Count!=0){
            int selectedButtonIndex = PlayerPrefs.GetInt(SelectedButtonKey, -1);
            if(_bgIndexTxt!=null){
                ChangeIndexBG(selectedButtonIndex);
            }
            ChangeBackground(selectedButtonIndex); 
        }

        if (_bgButtons.Count != 0)
        {
            for (int i = 0; i < _bgButtons.Count; i++)
            {
                int index = i; 
                _bgButtons[i].onClick.AddListener(() => OnBackgroundButtonClick(index));
                // _bgButtons[i].onClick.AddListener(() => ChangeIndexBG(index));
            }
        }
    }

    public void OnMusicButtonClick()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        foreach (Button btn in _musicButtons)
        {
            btn.image.sprite = isMuted ? _muteImg : _musicImg;
        }
    }

    private void OnBackgroundButtonClick(int buttonIndex)
    {
        PlayerPrefs.SetInt(SelectedButtonKey, buttonIndex); 
        PlayerPrefs.Save();
        PlayGame(); 
    }

    private void ChangeBackground(int buttonIndex)
    {
        foreach(Image bg in backgroundPanel){
            bg.sprite = BackgroundSprites[buttonIndex];
        }
    }
    private void ChangeIndexBG(int index){
        _bgIndexTxt.text = "Jungle " + (index+1).ToString();
        if(index + 1==1 ||index +1 == 6){
            _bgIndexTxt.color = new Color(0/255,255/255,87/255,255/255);
        }else
            _bgIndexTxt.color = Color.black;
    }
    public void NextIndexBg(){
        _indexBgTxt += 1;
        if(_indexBgTxt < 1)
            _indexBgTxt = BackgroundSprites.Count;
        else if(_indexBgTxt > 7)
            _indexBgTxt = 1;
        _bgIndexTxt.text = "Jungle " + _indexBgTxt.ToString();
    }
    public void PreviousIndexBg(){
        _indexBgTxt += -1;
        if(_indexBgTxt < 1)
            _indexBgTxt = BackgroundSprites.Count;
        else if(_indexBgTxt > 7)
            _indexBgTxt = 1;
        _bgIndexTxt.text = "Jungle " + _indexBgTxt.ToString();
    }

    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void BackScene()
    {
        // AudioManager.Instance.PlayBackgroundMusic();
        int previousScence = SceneManager.GetActiveScene().buildIndex - 1;
        if (previousScence >= 0)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(previousScence);
        }
    }
    public void ReloadScene(){
        Time.timeScale = 1;
        // AudioManager.Instance.PlayBackgroundMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame(){
        Application.Quit();
    }

    public void BackToStartGame()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public void Success(){
        StartCoroutine(ActiveSuccessPanel());
    }
    public IEnumerator ActiveSuccessPanel(){
        // AudioManager.Instance.PlayAudioSuccess();
        yield return new WaitForSeconds(1.5f);
        sucessPanel.SetActive(true);
        Time.timeScale =0;
    }
    public IEnumerator ActiveWinPanel(){
        // AudioManager.Instance.TurnOffAudioSource();
        yield return new WaitForSeconds(0.5f);
        // AudioManager.Instance.PlaySFXWin();
        yield return new WaitForSeconds(2.5f);
        winGamePanel.SetActive(true);
        // Time.timeScale =0;
    }

    public IEnumerator ActiveLosePanel(){
        yield return new WaitForSeconds(2f);
        // AudioManager.Instance.TurnOffAudioSource();
        // AudioManager.Instance.PlaySFXLose();
        losePanel.SetActive(true);
        // Time.timeScale =0;
    }
}
