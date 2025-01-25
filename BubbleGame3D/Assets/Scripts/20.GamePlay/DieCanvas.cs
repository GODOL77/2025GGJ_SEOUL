using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class DieCanvas : MonoBehaviour
    {
        public TMP_Text scoreText;
        public TMP_Text timeText;

        public Button exitButton;
        public Button lobbyButton;
        public Button replayButton;

        private void Awake()
        {
            exitButton.onClick.AddListener(Application.Quit);
            lobbyButton.onClick.AddListener(SceneUtil.GoLobbyScene);
            replayButton.onClick.AddListener(SceneUtil.GoGameScene);
        }

        public void Start()
        {
            var gameManager = FindObjectOfType<GameManager>();
            var scoreManager = FindObjectOfType<ScoreManager>();
            
            SetTime(gameManager.playTime);
            SetScore((int)scoreManager.score);
        }

        public void SetTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60); // 분 계산
            int seconds = Mathf.FloorToInt(time % 60); // 초 계산

            // "mm:ss" 형식으로 문자열 반환
            var timeStr = $"{minutes:00}:{seconds:00}";
            timeText.text = timeStr;
        }

        public void SetScore(int score)
        {
            scoreText.text = $"{score}";
        }
    }
}