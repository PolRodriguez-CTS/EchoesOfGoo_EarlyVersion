using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI; // IMPRESCINDIBLE para controlar la RawImage
using System.Collections;

public class CinematicaInicioNivel2 : MonoBehaviour
{
    [Header("Componentes de la Cinemática")]
    [Tooltip("El VideoPlayer que reproducirá la cinemática.")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Tooltip("La RawImage del Canvas donde se está mostrando el vídeo.")]
    [SerializeField] private RawImage imagenDelVideo;

    [Header("Componentes del Juego")]
    [Tooltip("El objeto del jugador (Player) para quitarle el control al inicio.")]
    [SerializeField] private GameObject personaje;

    [Header("Configuración del Fade Out")]
    [Tooltip("Duración en segundos del desvanecimiento suave al acabar el vídeo.")]
    [SerializeField] private float duracionFadeOut = 0.8f;

    [Header("Configuración de Carga Autónoma")]
    [Tooltip("El nombre EXACTO del objeto de la pantalla de carga en la escena Core.")]
    [SerializeField] private string nombreDelObjetoCarga = "Canvas_PantallaCarga";

    private GameObject panelPantallaDeCarga;
    private bool cinematicaTerminada = false;

    private void Start()
    {
        // 1. Bloqueamos al jugador inmediatamente para que no actúe a ciegas
        BloquearPersonaje(true);

        // 2. Aseguramos que el vídeo empiece totalmente opaco (visible)
        if (imagenDelVideo != null)
        {
            Color c = imagenDelVideo.color;
            c.a = 1f;
            imagenDelVideo.color = c;
        }

        // 3. Buscamos la pantalla de carga en Core
        GameObject[] todosLosObjetos = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in todosLosObjetos)
        {
            if (obj.name == nombreDelObjetoCarga)
            {
                panelPantallaDeCarga = obj;
                break;
            }
        }

        StartCoroutine(FlujoCinematica());
    }

    private IEnumerator FlujoCinematica()
    {
        // Esperamos a que la pantalla de carga de la escena Core se apague del todo
        if (panelPantallaDeCarga != null)
        {
            while (panelPantallaDeCarga.activeInHierarchy)
            {
                yield return null;
            }
        }

        // 4. Arrancamos el vídeo
        if (videoPlayer != null && imagenDelVideo != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += AlTerminarVideoPorSiSolo;
        }
        else
        {
            Debug.LogError("[CINEMÁTICA] Falta asignar el VideoPlayer o la RawImage en el Inspector.");
            StartCoroutine(ProcesoFinalConFadePropio());
            yield break;
        }

        // 5. Bucle de escucha para el Skip (Espacio)
        while (!cinematicaTerminada)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[CINEMÁTICA] Vídeo saltado por el jugador.");
                videoPlayer.loopPointReached -= AlTerminarVideoPorSiSolo; // Desenganchamos el evento automático
                StartCoroutine(ProcesoFinalConFadePropio());
                yield break;
            }
            yield return null;
        }
    }

    private void AlTerminarVideoPorSiSolo(VideoPlayer vp)
    {
        Debug.Log("[CINEMÁTICA] El vídeo terminó su reproducción natural.");
        videoPlayer.loopPointReached -= AlTerminarVideoPorSiSolo;
        StartCoroutine(ProcesoFinalConFadePropio());
    }

    // 🌟 EL FADE OUT MATEMÁTICO DIRECTO DESDE ESTE SCRIPT
    private IEnumerator ProcesoFinalConFadePropio()
    {
        cinematicaTerminada = true;

        if (imagenDelVideo != null)
        {
            float tiempoPasado = 0f;
            Color colorActual = imagenDelVideo.color;

            // Vamos bajando el canal Alfa (opacidad) poco a poco frame a frame
            while (tiempoPasado < duracionFadeOut)
            {
                tiempoPasado += Time.unscaledDeltaTime;
                colorActual.a = Mathf.Lerp(1f, 0f, tiempoPasado / duracionFadeOut);
                imagenDelVideo.color = colorActual;
                yield return null;
            }

            colorActual.a = 0f;
            imagenDelVideo.color = colorActual;
        }

        // 🌟 ¡EL MANOTAZO SOBRE LA MESA!: 
        // Justo aquí, antes de soltar al jugador, obligamos al UIManager a limpiar
        // cualquier panel temporal (como WASD) que se haya encendido de fondo.
        if (UIManager.Instance != null)
        {
            UIManager.Instance.DespausarYQuitarLore(); 
            // Nota: DespausarYQuitarLore apaga 'panelLorePausado', pero si tu panel WASD 
            // se maneja con 'MostrarPanelTemporal', usaremos la siguiente línea de seguridad:
            
            // Buscamos directamente el objeto de controles en la jerarquía y lo fulminamos por si acaso
            GameObject panelWASDFantasma = GameObject.Find("Canvas_Controles"); // o como se llame el objeto padre de tus controles WASD en la escena
            if (panelWASDFantasma != null)
            {
                panelWASDFantasma.SetActive(false);
            }
        }

        // Una vez que el vídeo es 100% invisible y la pantalla está limpia, lo paramos
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
        }

        // Devolvemos el control al personaje con el mapa limpio
        BloquearPersonaje(false);
        //Debug.Log("[CINEMÁTICA] ¡Pantalla purgada y control devuelto al jugador!");
    }
    private void BloquearPersonaje(bool bloquear)
    {
        if (personaje == null) return;

        MonoBehaviour[] scripts = personaje.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) 
            {
                script.enabled = !bloquear;
            }
        }
    }
}