using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChest : MonoBehaviour
{
    [SerializeField] private string nombreCanvasUI = "UI";
    [SerializeField] private string nombreWinScreen = "WinScreen";
    [SerializeField] private float delayAntesDeVolver = 3f;
    [SerializeField] private int escenaBase = 1;

    private bool activado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activado)
            return;

        if (collision.GetComponent<Movimientos>() != null)
        {
            activado = true;
            StartCoroutine(MostrarVictoria());
        }
    }

    private IEnumerator MostrarVictoria()
    {
        // El cofre se instancia en tiempo de ejecución, así que no puede
        // traer una referencia de escena precargada: busca el panel por nombre.
        // WinScreen empieza inactivo, y GameObject.Find no encuentra objetos
        // inactivos, así que hay que buscarlo a través del Canvas (que sí está activo).
        GameObject canvasUI = GameObject.Find(nombreCanvasUI);

        if (canvasUI != null)
        {
            Transform winScreen = canvasUI.transform.Find(nombreWinScreen);

            if (winScreen != null)
            {
                winScreen.gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(delayAntesDeVolver);

        SceneManager.LoadScene(escenaBase);
    }
}
