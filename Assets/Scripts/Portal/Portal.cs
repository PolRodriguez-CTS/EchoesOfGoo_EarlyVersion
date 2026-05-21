using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    /*
    [SerializeField] private Renderer portalRenderer;
    [SerializeField] private float maxPortal = 10;
    [SerializeField] private float minPortal = 0.6f;
    [SerializeField] private float currentPortal;

    void Start()
    {
        currentPortal = maxPortal;
    }

    void Awake()
    {
        portalRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nilloIsInside && currentPortal >= minPortal)
        {
            currentPortal -= Time.deltaTime * 4;
            PortalGraph();
        }
        if(!nilloIsInside && currentPortal <= maxPortal)
        {
            currentPortal += Time.deltaTime * 4;
            PortalGraph();
        }
    }

    void PortalGraph()
    {
        Material mat = portalRenderer.material;
        mat.SetFloat("_RadialDistortion", currentPortal);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            nilloIsInside = true;
        }
    }
    void OnTriggerExit(Collider colldier)
    {
        if(colldier.gameObject.CompareTag("Player"))
        {
            nilloIsInside = false;
        }
    }
    */
}