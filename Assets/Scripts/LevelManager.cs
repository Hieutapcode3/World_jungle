using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hyb.Utils;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Button> levelBtn;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;
    
    private List<bool> levelUnlocked = new List<bool>();
    public static LevelManager Instance;
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
    void Start()
    {
        LoadLevelStatus();
        levelUnlocked[0] = true;
        UpdateButtonGraphics();
        // ResetLevels();
    }
    private void LoadLevelStatus()
    {
        for (int i = 0; i < levelBtn.Count; i++)
        {
            levelUnlocked.Add(PlayerPrefs.GetInt("Level_" + i, 0) == 1);
        }
    }
    private void UpdateButtonGraphics()
    {
        for (int i = 0; i < levelBtn.Count; i++)
        {
            levelBtn[i].image.sprite = levelUnlocked[i] ? unlockedSprite : lockedSprite;
            int levelIndex = i;  
            levelBtn[i].onClick.AddListener(() => PlayLevel(levelIndex));
        }
    }
    public void UnlockNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex - 2; 
        if (currentLevel + 1 < levelBtn.Count)
        {
            levelUnlocked[currentLevel + 1] = true;
            Debug.Log("Unlock level" + (currentLevel + 1));
            PlayerPrefs.SetInt("Level_" + (currentLevel + 1), 1);
            PlayerPrefs.Save();
        }
    }
    public void PlayLevel(int levelIndex)
    {
        int sceneIndex = levelIndex + 3; 
        if (levelIndex < levelUnlocked.Count && levelUnlocked[levelIndex])
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
    public void ResetLevels()
    {
        for (int i = 0; i < levelBtn.Count; i++)
        {
            PlayerPrefs.DeleteKey("Level_" + i);
        }
        PlayerPrefs.Save();
        levelUnlocked.Clear();
        for (int i = 0; i < levelBtn.Count; i++)
        {
            if (i == 0)
            {
                levelUnlocked.Add(true);
                PlayerPrefs.SetInt("Level_0", 1); 
            }
            else
            {
                levelUnlocked.Add(false); 
            }
        }
        PlayerPrefs.Save();
        UpdateButtonGraphics();
    }

}
