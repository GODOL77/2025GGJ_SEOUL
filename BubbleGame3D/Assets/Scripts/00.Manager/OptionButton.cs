using UnityEngine;
using UnityEngine.UI;
using Util;

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
        if(isOptionPanelActive)
        {
            HideOptionPanel();
        }
        else
        {
            ShowOptionPanel();
        }
    }
    void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
        isOptionPanelActive = true;
    }
    void HideOptionPanel()
    {
        optionPanel.SetActive(false);
        isOptionPanelActive = false;
    }
}
