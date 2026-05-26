using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private Animator _animator;
    private bool _activado = false;

    [Header("Referencias de Objetos")]
    [SerializeField] private GameObject vfxGameObject; // El objeto del VFX

    [Header("Configuración de Apertura Física")]
    [SerializeField] private float velocidadApertura = 2f;
    private Vector3 _escalaMaxima;
    private Vector3 _currentEscala;

    void Awake()
    {
        _animator = GetComponent<Animator>();

        if (vfxGameObject != null)
        {
            // Guardamos el tamaño real que le hayas puesto al portal en la escena
            _escalaMaxima = vfxGameObject.transform.localScale;
            
            // Lo hacemos invisible al empezar la partida (Escala 0)
            vfxGameObject.transform.localScale = Vector3.zero;
        }
        else
        {
            Debug.LogWarning($"[PortalTrigger] Olvidaste asignar el vfxGameObject en {gameObject.name}.");
        }
        
        _currentEscala = Vector3.zero;
    }

    void Update()
    {
        // En cuanto se activa, el portal crece suavemente hasta su tamaño original
        if (_activado && _currentEscala.x < _escalaMaxima.x)
        {
            // Sumamos escala en todos los ejes a la vez
            _currentEscala += Vector3.one * (Time.deltaTime * velocidadApertura);
            
            // Nos aseguramos de no pasarnos del tamaño máximo que configuraste
            if (_currentEscala.x > _escalaMaxima.x)
            {
                _currentEscala = _escalaMaxima;
            }

            vfxGameObject.transform.localScale = _currentEscala;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detección para tu CharacterController (con el Rigidbody Kinematic en el portal)
        if (other.CompareTag("Player") && !_activado)
        {
            _activado = true; // Candado lógico
            
            if (_animator != null)
            {
                SoundManager.PlaySound(SoundType.PortalBuild, 1);
                _animator.SetTrigger("Build");
            }

            Debug.Log("¡Portal activado! Escalando el VFX de forma segura.");
        }
    }
}