using UnityEngine.SceneManagement;

namespace GamePlay
{
    public static class SceneUtil
    {
        public static readonly string Game = "Game";
        public static readonly string Lobby = "Lobby";

        public static void GoLobbyScene() => SceneManager.LoadScene(Lobby);
        public static void GoGameScene() => SceneManager.LoadScene(Game);
    }
}