using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchToWin() {
        SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
    }

    public void switchToGame()
    {
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
    }

    public void switchToMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void switchToLose() {
        SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
    }
}
