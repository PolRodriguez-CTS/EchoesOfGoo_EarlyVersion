using UnityEngine;
using Unity.Cinemachine; // ¡Importante para manejar la cámara!

public class ControladorPrisionero : MonoBehaviour 
{
    public int cadenasRestantes = 3;
    public Animator anim;

    [Header("Configuración de Cámara")]
    public CinemachineCamera camaraEnemigoLibre; // La nueva cámara
    public GameObject scriptMovimientoJugador; // Para bloquear al jugador durante la animación

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
        // 1. Disparar animación
        anim.SetTrigger("Unchained"); 

        // 2. Cambiar la cámara de forma permanente
        if (camaraEnemigoLibre != null)
        {
            // Le damos una prioridad alta (ej. 20)
            // Como no tenemos código que la baje, se quedará ahí para siempre
            camaraEnemigoLibre.Priority = 20; 
        }

        // 3. Opcional: Bloquear al jugador si no quieres que se mueva más
        if (scriptMovimientoJugador != null)
        {
            scriptMovimientoJugador.SetActive(false);
        }

        Debug.Log("¡El enemigo es libre y la cámara ha cambiado!");
    }
}