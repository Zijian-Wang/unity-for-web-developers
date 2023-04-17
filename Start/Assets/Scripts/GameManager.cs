using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI m_gameOverText;
  [FormerlySerializedAs("m_TimerText")] [SerializeField] TextMeshProUGUI m_timerText;
  [SerializeField] Score m_playerScore;
  [SerializeField] Score m_opponentScore;
  [SerializeField] float m_gameTimeInSeconds = 60f;
  float m_timer;
  bool m_isGamePlaying = false;
  public static float RespawnHeight = 0.25f;

  // Start is called before the first frame update
  void Start()
  {
    // set the time scale to 0 to pause the game at the start
    Time.timeScale = 0f;

    // set the timer to default count down time 
    m_timer = m_gameTimeInSeconds;

    // update timer's text
    UpdateTimerText();
  }

  void UpdateTimerText()
  {
    m_timerText.text = string.Format("{0:0}:{1:00}",
                                     Mathf.Floor(m_timer / 60),
                                     Mathf.Floor(m_timer % 60));
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    UpdateTimerText();

    if (!m_isGamePlaying) return;

    m_timer -= Time.fixedDeltaTime;

    if (m_timer <= 0)
    {
      GameOver();
      m_timer = 0;
    }
  }

  void GameOver()
  {
    m_isGamePlaying = false;
    Time.timeScale = 0;

    string gameOverText;

    if (m_playerScore.ScoreValue > m_opponentScore.ScoreValue)
      gameOverText = "You Win!";
    else if (m_playerScore.ScoreValue < m_opponentScore.ScoreValue)
      gameOverText = "You Loose.";
    else
      gameOverText = "It's a tie.";

    m_gameOverText.text = gameOverText + "\n\nPress SPACE to Restart.";

    m_gameOverText.gameObject.SetActive(true);
  }

  public void RestartGame()
  {
    if (!m_isGamePlaying)
    {
      Time.timeScale = 1f;

      m_gameOverText.gameObject.SetActive(false);

      m_timer = m_gameTimeInSeconds;
      m_isGamePlaying = true;
    }
  }
}
