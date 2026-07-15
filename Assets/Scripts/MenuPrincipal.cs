using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void NuevaPartida()
    {
        SceneManager.LoadScene("Lvl1");
    }

    public void Salir()
    {
        Application.Quit();
    }
}