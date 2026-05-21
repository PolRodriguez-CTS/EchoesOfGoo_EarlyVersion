using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    private Animator _animator;
    private bool _yaSeAbrio = false;

    [Header("Configuración del Animator")]
    [SerializeField] private string openTriggerName = "Open";

    [Header("Configuración del VFX (Instanciado)")]
    [SerializeField] private GameObject vfxPrefab;      // El archivo del VFX desde tu carpeta Assets
    [SerializeField] private Transform vfxSpawnPoint;   // El punto vacío creado dentro del cofre

    void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError($"[ChestTrigger] ¡Mendrugo! El objeto {gameObject.name} no tiene un componente Animator.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_yaSeAbrio)
        {
            _yaSeAbrio = true; // Candado lógico para que solo pase UNA vez
            
            // 1. Disparar Animación
            _animator.SetTrigger(openTriggerName); 

            // 2. Spawnear el VFX si los campos están asignados
            if (vfxPrefab != null && vfxSpawnPoint != null)
            {
                // Crea el prefab en la posición y rotación exactas del Spawn Point
                GameObject nuevoVFX = Instantiate(vfxPrefab, vfxSpawnPoint.position, vfxSpawnPoint.rotation);
                
                // OPCIONAL: Esto destruye el clon del VFX a los 4 segundos para no llenar la escena de basura
                Destroy(nuevoVFX, 4f); 
            }
            else
            {
                Debug.LogWarning($"[ChestTrigger] Revisa el Inspector en {gameObject.name}. Falta asignar el Prefab o el Spawn Point.");
            }

            GameManager.Instance.AddCoins(10);

            Debug.Log($"¡Cofre {gameObject.name} abierto y VFX creado en su sitio!");
        }
    }
}