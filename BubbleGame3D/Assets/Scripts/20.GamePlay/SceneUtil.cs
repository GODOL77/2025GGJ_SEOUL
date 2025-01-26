using UnityEngine.SceneManagement;

namespace GamePlay
{
    public static class SceneUtil
    {
        public static readonly string Game = "Game";
        public static readonly string Lobby = "Lobby";
        public static readonly string Die = "Die";
        public static readonly string Ending = "End";

        public static void GoLobbyScene() => SceneManager.LoadScene(Lobby);
        public static void GoGameScene() => SceneManager.LoadScene(Game);
        public static void GoEnding() => SceneManager.LoadScene(Ending);
        public static void AddDieScene() => SceneManager.LoadScene(Die, LoadSceneMode.Additive);
    }
}