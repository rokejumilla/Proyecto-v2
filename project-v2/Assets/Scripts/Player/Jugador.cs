using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Para usar TextMeshPro

public class Jugador : MonoBehaviour
{
    [Header("Configuraci�n de Vida")]
    [SerializeField] private float vida = 5f;

    [Header("UI Textos (TextMeshProUGUI)")]
    [SerializeField] private TextMeshProUGUI metaText;     // Texto que aparece al ganar
    [SerializeField] private string mensajeMeta = "�GANASTE!";

    [SerializeField] private TextMeshProUGUI derrotaText;  // Texto que aparece al perder
    [SerializeField] private string mensajeDerrota = "GAME OVER";

    [Header("Opcional: scripts a desactivar")]
    [Tooltip("Arrastra aqu� los scripts (ej. PlayerController) que quieras desactivar al congelar el juego.")]
    [SerializeField] private MonoBehaviour[] scriptsToDisableOnFreeze;

    private bool juegoCongelado = false;

    // Guardamos valores anteriores para restaurar si es necesario
    private float previousTimeScale = 1f;
    private float previousFixedDeltaTime = 0.02f;
    private bool previousAudioPause = false;

    private void Awake()
    {
        // Aseguramos que los textos est�n ocultos al inicio
        if (metaText != null) metaText.gameObject.SetActive(false);
        if (derrotaText != null) derrotaText.gameObject.SetActive(false);

        previousFixedDeltaTime = Time.fixedDeltaTime;
        previousTimeScale = Time.timeScale;
    }

    // Modifica la vida del jugador (puede ser da�o o curaci�n)
    public void ModificarVida(float puntos)
    {
        vida += puntos;
        Debug.Log($"Vida actual: {vida}. �Est� vivo? {EstasVivo()}");

        if (!EstasVivo())
        {
            Perder();
        }
    }

    // Devuelve si el jugador sigue vivo
    private bool EstasVivo()
    {
        return vida > 0;
    }

    // Detecta si el jugador entra en un trigger con el tag "Meta"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Meta")) return;

        Ganar();
    }

    // Rutina al llegar a la meta
    private void Ganar()
    {
        if (juegoCongelado) return;

        MostrarTexto(metaText, mensajeMeta);
        FreezeGame();
        Debug.Log("GANASTE");
    }

    // Rutina cuando la vida llega a 0
    private void Perder()
    {
        if (juegoCongelado) return;

        MostrarTexto(derrotaText, mensajeDerrota);
        FreezeGame();
        Debug.Log("GAME OVER");
    }

    // Muestra un texto TMP
    private void MostrarTexto(TextMeshProUGUI textoUI, string mensaje)
    {
        if (textoUI == null)
        {
            Debug.LogWarning("Jugador: falta asignar un TextMeshProUGUI en el Inspector.");
            return;
        }

        textoUI.text = mensaje;
        textoUI.gameObject.SetActive(true);
    }

    // Congela el juego
    private void FreezeGame()
    {
        // Guardar estado actual
        previousTimeScale = Time.timeScale;
        previousFixedDeltaTime = Time.fixedDeltaTime;
        previousAudioPause = AudioListener.pause;

        // Congelar tiempo y f�sica
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        // Pausar audio
        AudioListener.pause = true;

        // Desactivar scripts opcionales
        if (scriptsToDisableOnFreeze != null)
        {
            foreach (var mb in scriptsToDisableOnFreeze)
            {
                if (mb != null) mb.enabled = false;
            }
        }

        juegoCongelado = true;
    }

    // (Opcional) descongelar el juego
    public void UnfreezeGame()
    {
        if (!juegoCongelado) return;

        // Restaurar tiempo y f�sicas
        Time.timeScale = previousTimeScale;
        Time.fixedDeltaTime = previousFixedDeltaTime;
        AudioListener.pause = previousAudioPause;

        // Reactivar scripts
        if (scriptsToDisableOnFreeze != null)
        {
            foreach (var mb in scriptsToDisableOnFreeze)
            {
                if (mb != null) mb.enabled = true;
            }
        }

        // Ocultar textos
        if (metaText != null) metaText.gameObject.SetActive(false);
        if (derrotaText != null) derrotaText.gameObject.SetActive(false);

        juegoCongelado = false;
    }
}
