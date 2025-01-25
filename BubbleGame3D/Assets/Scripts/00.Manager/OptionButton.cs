using UnityEngine;
using UnityEngine.UI;
using Util;
using UnityEngine.SceneManagement;

public class OptionButton : Singleton<OptionButton>
{
    public GameObject optionPanel;
    private  bool isOptionPanelActive;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        isOptionPanelActive = false;
    }
    public void OnPressButton()
    {
        if(!isOptionPanelActive)
        {
            ShowOptionPanel();
            PauseGame();
        }
    }
    void ShowOptionPanel()
    {
        isOptionPanelActive = true;
        optionPanel.SetActive(true);   
    }
    void HideOptionPanel()
    {
        isOptionPanelActive = false;
        optionPanel.SetActive(false);
    }
    
    void PauseGame()
    {
        Time.timeScale = 0f;
    }
    void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        HideOptionPanel();
        ResumeGame();
    }
    public void ReturnLobby()
    {
        SceneManager.LoadScene("Lobby");
        HideOptionPanel();
        ResumeGame();
    }
    public void Back()
    {
        HideOptionPanel();
        ResumeGame();

    }

}
