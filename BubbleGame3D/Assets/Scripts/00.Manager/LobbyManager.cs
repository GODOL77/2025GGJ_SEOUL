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
    }
}