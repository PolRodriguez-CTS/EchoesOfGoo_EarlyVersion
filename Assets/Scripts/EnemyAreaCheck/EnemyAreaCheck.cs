using UnityEngine;
using System.Collections.Generic;

public class EnemyAreaCheck : MonoBehaviour
{
    public Door doorScript; 
    // NUEVO: Arrastra aquí directamente el script de la cinemática desde el Inspector
    public DoorCinematic doorCinematicScript; 

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private bool hasOpened = false;
    private bool enemiesDetectedOnce = false;

    void Update()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count > 0)
        {
            enemiesDetectedOnce = true;
        }

        if (enemiesDetectedOnce && enemiesInRange.Count == 0 && !hasOpened)
        {
            doorScript.OpenAnimation();
            hasOpened = true; 

            // NUEVO: Llamada directa y segura
            if (doorCinematicScript != null)
            {
                doorCinematicScript.PlayCinematic();
            }
            else
            {
                Debug.LogError("¡No has asignado el script DoorCinematic en el Inspector de EnemyAreaCheck!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (!enemiesInRange.Contains(other.gameObject))
            {
                enemiesInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}