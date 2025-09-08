using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoIA : MonoBehaviour
{
    [Header("Configuraci�n")]
    [SerializeField] float velocidad = 5f;

    [Header("Referencia al jugador")]
    [SerializeField] Transform jugador;

    private Rigidbody2D miRigidbody2D;
    private Vector2 direccion;

    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (jugador == null) return; // Evita errores si no est� asignado el jugador

        // Calculamos direcci�n en 2D
        direccion = (jugador.position - transform.position).normalized;

        // Convertimos a Vector2 para evitar problemas de tipos
        Vector2 nuevaPosicion = miRigidbody2D.position + direccion * (velocidad * Time.fixedDeltaTime);

        miRigidbody2D.MovePosition(nuevaPosicion);
    }
}

