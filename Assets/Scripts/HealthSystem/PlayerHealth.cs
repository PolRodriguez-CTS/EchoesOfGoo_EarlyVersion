using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private float playerMaxHealth = 4;
    
    [Header("Settings de Respawn")]
    [SerializeField] private Transform spawnPoint;
    
    [Header("Referencias UI y Animación")]
    [SerializeField] private Animator animator;

    void Start()
    {
        InitialHealth(playerMaxHealth);
        // Actualiza la UI al iniciar el juego
        GameManager.Instance.UpdateHealthUI(currentHealth);
    }

    public void Damaged(float damage)
    {
        TakeDamage(damage);
        
        // Actualiza la UI al recibir daño
        GameManager.Instance.UpdateHealthUI(currentHealth);

        if(currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        InstantRespawn();
        FullHeal(); // FullHeal ya se encarga de avisar a la UI abajo

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    private void InstantRespawn()
    {
        if (spawnPoint != null)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }
    }

    public void UpdateSpawnPoint(Transform newSpawn)
    {
        spawnPoint = newSpawn;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > playerMaxHealth)
        {
            currentHealth = playerMaxHealth;
        }

        // Actualiza la UI cuando la poción te cura
        GameManager.Instance.UpdateHealthUI(currentHealth);

        Debug.Log("Jugador curado. Vida actual: " + currentHealth);
    }

    public void FullHeal()
    {
        currentHealth = playerMaxHealth;
        
        // Actualiza la UI al revivir para rellenar todos los corazones
        GameManager.Instance.UpdateHealthUI(currentHealth);
    }

    // AÑADE ESTO DENTRO DE PLAYERHEALTH.CS
    public void ReturnToCheckpoint()
    {
        if (spawnPoint != null)
        {
            // Desactivamos el CharacterController para evitar que bloquee el movimiento
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Movemos al jugador al checkpoint/spawnpoint actual
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            if (cc != null) cc.enabled = true;
            
            Debug.Log("Jugador devuelto al checkpoint de forma manual.");
        }
        else
        {
            Debug.LogWarning("¡No hay un SpawnPoint asignado para regresar!");
        }
    }
}