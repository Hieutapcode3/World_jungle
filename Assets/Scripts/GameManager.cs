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
    [SerializeField] private List<Button> _musicButtons;
    [SerializeField] private GameObject sucessPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winGamePanel;
    public static GameManager Instance;

    
    private void Awake() {
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
        if(_musicButtons.Count != 0){
            foreach (Button btn in _musicButtons)
            {
                btn.image.sprite = isMuted ? _muteImg : _musicImg;
                btn.onClick.AddListener(OnMusicButtonClick);
            }
        }
    }
    public void OnMusicButtonClick()
    {
        // AudioManager.Instance.PlayBackgroundMusic();
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        // AudioManager.Instance.audioBackground.mute = isMuted;
        foreach (Button btn in _musicButtons)
        {
            btn.image.sprite = isMuted ? _muteImg : _musicImg;
        }
    }
    public void GoToPraticeScene(){
        SceneManager.LoadScene("PracticeScene");
    }
    public void PlayGame()
    {
        // AudioManager.Instance.PlayBackgroundMusic();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneIndex);
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
    public IEnumerator ActiveSuccessPanel(){
        // AudioManager.Instance.PlayAudioSuccess();
        yield return new WaitForSeconds(3f);
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
