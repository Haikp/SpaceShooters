using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    // Start is called before the first frame update
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private GameObject _pausePanel;
    // private Animator _animator;
    [SerializeField]
    private Text _bestScoreText;
    private int bestScore = -1;
    // private int hiScore = 0;

    void Start()
    {
        _scoreText.text = "Score: 0";
        bestScore = PlayerPrefs.GetInt("hiScore");
        _bestScoreText.text = "Score: " + bestScore;

        // _animator = gameObject.GetComponent<Animator>();

        // _animator = GameObject.Find("Pause_Menu").GetComponent<Animator>();
        // if (_animator == null)
        // {
        //     Debug.LogError("Animator null in UIMananger.");
        // }

        // _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_pausePanel.activeSelf)
            {
                _pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                _pausePanel.SetActive(false);
                Time.timeScale = 1;
            }

            // if (_animator.GetBool("isPaused"))
            // {
            //     _pausePanel.SetActive(false);
            //     Time.timeScale = 1;
            //     _animator.SetBool("isPaused", false);
            // }
            // else
            // {
            //     _pausePanel.SetActive(true);
            //     Time.timeScale = 0;
            //     _animator.SetBool("isPaused", true);
            // }
        }
    }

    // Update is called once per frame
    public void UpdateScore(int CurrentScore)
    {
        _scoreText.text = "Score: " + CurrentScore;
        CheckForBestScore(CurrentScore);
    }

    public void CheckForBestScore(int Score)
    {
        if (Score > bestScore)
        {
            bestScore = Score;
            PlayerPrefs.SetInt("hiScore", bestScore);
        }

        _bestScoreText.text = "Score: " + bestScore;
    }

    public void UpdateLives(int currentLives)
    {
        currentLives = Mathf.Clamp(currentLives, 0, 3);
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _restartText.gameObject.SetActive(true);
        StartCoroutine(TextFlicker());
    }

    IEnumerator TextFlicker()
    {
        float flickerSpeed = .5f;
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(flickerSpeed);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
    public void ResumeGameButton()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0); //Scene 0 is the Main Menu
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
