using UnityEngine;
using System.Collections;
using Unity.Cinemachine; // ¡No olvides este using!

public class DoorCinematic : MonoBehaviour
{
    public CinemachineCamera cinematicCam; // Arrastra la cámara de la puerta aquí
    public float cinematicDuration = 3.0f;        // Cuánto dura el plano
    public GameObject playerMovementScript;       // El script de movimiento de tu jugador para pausarlo

    public void PlayCinematic()
    {
        StartCoroutine(CinematicRoutine());
    }

    private IEnumerator CinematicRoutine()
    {
        Debug.Log("¡Iniciando cinemática de la puerta!");

        // 1. Bloqueamos al jugador
        if (playerMovementScript != null) playerMovementScript.SetActive(false);

        // 2. Encendemos el objeto de la cámara cinematográfica y le damos prioridad alta
        cinematicCam.gameObject.SetActive(true);
        cinematicCam.Priority = 40; 

        // 3. Esperamos el tiempo que dura el plano de la puerta
        yield return new WaitForSeconds(cinematicDuration);

        // 4. Bajamos la prioridad y APAGAMOS la cámara para forzar el regreso
        cinematicCam.Priority = 0;
        cinematicCam.gameObject.SetActive(false); // <--- ESTO OBLIGA A CINEMACHINE A CAMBIAR

        // 5. Esperamos un segundo a que la cámara del juego termine de acomodarse
        yield return new WaitForSeconds(1.0f); 
        
        // 6. Devolvemos el control al jugador
        if (playerMovementScript != null) playerMovementScript.SetActive(true);
    }
}