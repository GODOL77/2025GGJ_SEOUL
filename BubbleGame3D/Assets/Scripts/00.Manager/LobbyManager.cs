using GamePlay;
using UnityEngine;

namespace Manager
{
    public class LobbyManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneUtil.GoGameScene();
        }

        public void ExitGame()
        
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit(); // 어플리케이션 종료
            #endif
        }
    }
}