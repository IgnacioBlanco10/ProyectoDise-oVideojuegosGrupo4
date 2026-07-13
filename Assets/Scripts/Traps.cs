using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    // Start is called before the first frame updat
    [SerializeField] private Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Movimientos>())
        {
            collision.gameObject.transform.position= playerSpawnPoint.position;
            FindFirstObjectByType<VidasyGameOver>().ReduceLives();
        }
    }
}
