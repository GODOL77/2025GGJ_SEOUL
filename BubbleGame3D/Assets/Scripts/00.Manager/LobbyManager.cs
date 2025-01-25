using GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class LobbyManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Intro");
        }

        public void ExitGame()
        
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit(); // 어플리케이션 종료
            #endif
        }

        public void NextGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}