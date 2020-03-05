using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BallBlaster
{

    public class GameManager : MonoBehaviour
    {
        public bool IsGameStarted { get; private set; }
        public bool IsGameFinished { get; private set; }

        public delegate void OnGameStarted();
        public delegate void OnGameFinished();
        public event OnGameStarted GameStartedEvent;
        public event OnGameFinished GameFinishedEvent;

        public AdsManager adManager;
        public GameObject rewardAdButton;
        public int adProbability = 15; //25 = 25% chance to show ad

        private IPlayerInput _input;

        #region MonoBehaviour

        private void Start()
        {
            if (!gameObject.CompareTag(Tags.GameManager))
                Debug.LogError("GameManager: has to be tagged as GameManager.");

            _input = GameObject.FindWithTag(Tags.Input).GetComponent<IPlayerInput>();
        }

        private void Update()
        {
            if (_input.IsPointerDown() && !IsGameStarted && !IsGameFinished)
            {
                StartGame();
            }
            else if (_input.IsPointerUp() && IsGameStarted && IsGameFinished)
            {
                if (_onceWasUp)
                    LoadLevel();
                else
                    _onceWasUp = true;
            }
        }

        #endregion

        private void StartGame()
        {
            IsGameStarted = true;

            if (GameStartedEvent != null)
                GameStartedEvent();

        }

        public void FinishGame()
        {
            IsGameFinished = true;

            if (GameFinishedEvent != null)
                GameFinishedEvent();

            LightVibration.VibrationManager.VibrateGameOver();



            if (ScoreCounter._currentScore > 0)
            {


                ScoreCounter._currentScore = 0;

                if (Random.Range(0, 100) <= adProbability)

                {
                    adManager.ShowAd();
                    return;
                }

            }



        }

        private bool _onceWasUp;
        private void LoadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

}