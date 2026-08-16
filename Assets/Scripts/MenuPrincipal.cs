using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void NuevaPartida()
    {
        // Inicia una nueva partida cargando el primer nivel
        SceneManager.LoadScene("Lvl1");
    }

    public void Continuar()
    {
        // Por ahora carga el primer nivel.
        // Más adelante, si implementan guardado real,
        // aquí se puede cargar el último progreso guardado.
        SceneManager.LoadScene("Lvl1");
    }

    public void Salir()
    {
        // Cierra el juego cuando se ejecuta como aplicación
        Application.Quit();

        // Permite comprobar en la consola de Unity que el botón funciona
        Debug.Log("Saliendo del juego...");
    }
}