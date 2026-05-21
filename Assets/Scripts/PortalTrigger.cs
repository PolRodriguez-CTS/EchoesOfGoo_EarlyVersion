using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private bool _activado = false;

    // Olvídate de Awake por ahora. Lo buscamos al chocar.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_activado)
        {
            _activado = true;
            
            // Buscamos el Animator justo en el momento del choque
            Animator animator = GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Build");
                Debug.Log("¡ENTRÓ! Físicas funcionando y Animator encontrado.");
            }
            else
            {
                Debug.LogError("La física funciona, pero este objeto NO tiene un Animator arriba.");
            }
        }
    }
}