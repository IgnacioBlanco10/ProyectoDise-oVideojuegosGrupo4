using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VidasyGameOver : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject GameOver;

    [Header("Escudo")]
    [SerializeField] private float shieldCooldown = 5f;

    private int currentLives;
    private bool escudoDisponible = true;

    private void Start()
    {
        currentLives = startingLives;
        UpdateLives();

        if (GameOver != null)
        {
            GameOver.SetActive(false);
        }
    }

    // Daño
    public void ReduceLives()
    {
        if (GameProgress.Instance != null &&
            GameProgress.Instance.nivelLaboratorio >= 3 &&
            escudoDisponible)
        {
            ActivarEscudo();
            return;
        }

        currentLives--;
        UpdateLives();

        if (currentLives <= 0)
        {
            ShowGameOver();
        }
    }

    // Escudo
    private void ActivarEscudo()
    {
        escudoDisponible = false;

        Debug.Log("¡Escudo de energía activado! Daño bloqueado.");

        StartCoroutine(RecargarEscudo());
    }

    private IEnumerator RecargarEscudo()
    {
        yield return new WaitForSeconds(shieldCooldown);

        escudoDisponible = true;

        Debug.Log("Escudo de energía recargado.");
    }

    private void UpdateLives()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    private void ShowGameOver()
    {
        GameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        GameOver.SetActive(false);
        ResetLives();
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ResetLives()
    {
        currentLives = startingLives;
        escudoDisponible = true;
        UpdateLives();
    }
}