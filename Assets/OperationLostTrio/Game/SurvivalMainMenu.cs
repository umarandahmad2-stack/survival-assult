using UnityEngine;
using UnityEngine.SceneManagement;

namespace OperationLostTrio
{
    /// <summary>Simple runtime menu. Assign menuRoot and gameplayRoot in the Inspector.</summary>
    public class SurvivalMainMenu : MonoBehaviour
    {
        public GameObject menuRoot;
        public GameObject gameplayRoot;
        public string sceneToLoad = "";
        public bool startPaused = false;
        bool started;

        void Start()
        {
            if (startPaused) PauseGame();
            else ShowMenu();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && started)
            {
                if (Time.timeScale > 0f) PauseGame();
                else ResumeGame();
            }
        }

        public void StartMission()
        {
            if (!string.IsNullOrWhiteSpace(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
                return;
            }
            started = true;
            Time.timeScale = 1f;
            if (menuRoot != null) menuRoot.SetActive(false);
            if (gameplayRoot != null) gameplayRoot.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void PauseGame()
        {
            if (menuRoot != null) menuRoot.SetActive(true);
            if (gameplayRoot != null) gameplayRoot.SetActive(false);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResumeGame()
        {
            started = true;
            Time.timeScale = 1f;
            if (menuRoot != null) menuRoot.SetActive(false);
            if (gameplayRoot != null) gameplayRoot.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ReturnToTitle()
        {
            Time.timeScale = 1f;
            started = false;
            ShowMenu();
        }

        void ShowMenu()
        {
            if (menuRoot != null) menuRoot.SetActive(true);
            if (gameplayRoot != null) gameplayRoot.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void QuitGame()
        {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}