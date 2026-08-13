 using UnityEngine;

public class EstacionMejoras : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool jugadorCerca = false;

    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(!panel.activeSelf);
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
            panel.SetActive(false);
        }
    }
}