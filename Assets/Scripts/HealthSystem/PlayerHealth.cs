using UnityEngine;
using System.Collections;

public class PlayerHealth : HealthSystem
{
    public float playerMaxHealth = 4;
    [SerializeField] private Transform spawnPoint;

    [Header("Referencias UI y Animación")]
    private Animator animator;
    [SerializeField] private GameObject canvasMuerte;


    [Header("Tiempos de Espera")]
    [SerializeField] private float tiempoAnimacion = 2f;
    [SerializeField] private float tiempoPantallaMuerte = 1.5f;
    private bool estaMuerto = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        InitialHealth(playerMaxHealth);
        UpdateVisuals();

        if(canvasMuerte != null) canvasMuerte.SetActive(false);
    }

    
    public void Damaged(float damage)
    {
        if (estaMuerto) return;

        SoundManager.PlaySound(SoundType.ArgylDamaged, 0.5f);
        TakeDamage(damage);

        UpdateVisuals();

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        StartCoroutine(SecuenciaMuerte());

        //SoundManager.PlaySound(SoundType.ArgylDeath, 0.75f);
        //Respawn();
    }

    private IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;

        // 1. Desactivar el movimiento del jugador para que no se mueva estando muerto
        // (Asumiendo que tienes un CharacterController o un script de movimiento)
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        
        // Aquí puedes desactivar también tu script de PlayerController personalizado si lo necesitas:
        // GetComponent<TuScriptDeMovimiento>().enabled = false;

        // 2. Activar animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Asegúrate de tener un Trigger llamado "Die" en tu Animator
        }

        // 3. Esperar a que termine la animación
        yield return new WaitForSeconds(tiempoAnimacion);

        // 4. Mostrar el Canvas de muerte
        if (canvasMuerte != null)
        {
            canvasMuerte.SetActive(true);
        }

        // 5. Esperar con la pantalla de muerte visible
        yield return new WaitForSeconds(tiempoPantallaMuerte);

        // 6. Teletransportar al jugador al spawn
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        // 7. Resucitar: Quitar pantalla de muerte, curar vida y reactivar controles
        if (canvasMuerte != null) canvasMuerte.SetActive(false);
        
        FullHeal();
        estaMuerto = false;

        if (cc != null) cc.enabled = true;
        // GetComponent<TuScriptDeMovimiento>().enabled = true;
        
        // 8. Opcional: Volver a la animación de Idle/Suelo
        if (animator != null) animator.Rebind(); 
    }

    private void UpdateVisuals()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateHealthUI(currentHealth);
        }
    }

    public void Respawn()
    {
        currentHealth = playerMaxHealth;
        if(spawnPoint != null)
        {
            CharacterController _playerScript = GetComponent<CharacterController>();
            if(_playerScript != null)
            {
                _playerScript.enabled = false;
            }

            transform.position = spawnPoint.position;

            if(_playerScript != null)
            {
                _playerScript.enabled = true;
            }
        }
        UpdateVisuals();
    }

    public void ReturnToCheckpoint()
    {
        if(spawnPoint != null)
        {
            CharacterController _playerScript = GetComponent<CharacterController>();
            if(_playerScript != null)
            {
                _playerScript.enabled = false;
            }

            transform.position = spawnPoint.position;

            if(_playerScript != null)
            {
                _playerScript.enabled = true;
            }
        }
    }

    public float GetCurrentHealth() { return currentHealth; }

    public void Heal(float amount)
    {
        currentHealth += amount;

        // Si la vida actual supera el máximo, la igualamos al máximo
        if (currentHealth > playerMaxHealth)
        {
            currentHealth = playerMaxHealth;
        }

        UpdateVisuals();
        Debug.Log("Curado. Vida actual: " + currentHealth);
    }

    public void UpdateSpawnPoint(Transform newSpawn)
    {
        spawnPoint = newSpawn;
    }

    public void FullHeal()
    {
        currentHealth = playerMaxHealth;
    }
}