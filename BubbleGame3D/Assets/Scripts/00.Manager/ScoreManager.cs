using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float score;
    public float scoreIncrement = 5f;
    // Start is called before the first frame update
    void Start()
    {
        score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreIncrement;
        int intScore = (int)score;
        scoreText.text = "Score: " + intScore;

        if (score >= 1000)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
