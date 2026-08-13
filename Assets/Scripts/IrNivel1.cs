using UnityEngine;
using UnityEngine.SceneManagement;

public class IrNivel : MonoBehaviour
{
    private bool jugadorCerca = false;

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("Lvl1");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>() != null)
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>() != null)
        {
            jugadorCerca = false;
        }
    }
}