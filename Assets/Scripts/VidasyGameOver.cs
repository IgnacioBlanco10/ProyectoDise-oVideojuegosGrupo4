using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VidasyGameOver : MonoBehaviour
{
    [SerializeField] private int startingLives=3;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] GameObject GameOver;


    private int currentLives;

    private void Start(){
        currentLives=startingLives;
        UpdateLives();
        if(GameOver!= null){
            GameOver.SetActive(false);
        }
    }

    public void ReduceLives()
    {
        currentLives--;
        UpdateLives();
        if(currentLives<=0)
        {
            ShowGameOver();
        }
    }

    private void UpdateLives()
    {
        if( livesText!=null)
        {
            livesText.text= "Lives: " +currentLives;
        }
    }

    private void ShowGameOver (){
        GameOver.SetActive(true);

        Time.timeScale=0f;
    }

    public void Restart()
    {
        GameOver.SetActive(false);
        ResetLives();
        Time.timeScale=1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetLives(){
        currentLives=startingLives;
        UpdateLives();
    }
}
