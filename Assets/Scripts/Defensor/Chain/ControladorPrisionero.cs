using UnityEngine;
using Unity.Cinemachine; // ¡Importante para manejar la cámara!

public class ControladorPrisionero : MonoBehaviour 
{
    public int cadenasRestantes = 3;
    public Animator anim;

    [Header("Configuración de Cámara")]
    public CinemachineCamera camaraEnemigoLibre; // La nueva cámara
    public GameObject scriptMovimientoJugador; // Para bloquear al jugador durante la animación

    [Header("Configuración de Audio")]
    public AudioSource fuenteAudio;     // El reproductor (AudioSource)
    public AudioClip musicaJefe;        // El nuevo sonido o música que quieres poner

    public void CadenaRompida() 
    {
        cadenasRestantes--;

        if (cadenasRestantes <= 0) 
        {
            LiberarEnemigo();
        }
    }

    void LiberarEnemigo() 
    {
        anim.SetTrigger("Unchained"); 

        // 1. Cambiar la cámara
        if (camaraEnemigoLibre != null)
        {
            camaraEnemigoLibre.Priority = 20; 
        }

        // 2. CAMBIAR Y REPRODUCIR EL AUDIO
        if (fuenteAudio != null && musicaJefe != null)
        {
            fuenteAudio.Stop();               // Detiene el sonido actual (si hay alguno sonando)
            fuenteAudio.clip = musicaJefe;    // Intercambia el clip viejo por el nuevo
            fuenteAudio.Play();               // Reproduce el nuevo sonido
            
            // Opcional: Si quieres que el nuevo sonido se repita en bucle (ej: música de combate)
            // fuenteAudio.loop = true; 
        }

        if (scriptMovimientoJugador != null)
        {
            scriptMovimientoJugador.SetActive(false);
        }

        Debug.Log("¡Enemigo libre, cámara cambiada y nuevo audio sonando!");
    }
}