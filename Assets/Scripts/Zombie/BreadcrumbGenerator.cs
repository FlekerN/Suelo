using UnityEngine;
using UnityEngine.AI;

public class BreadcrumbGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject breadcrumbPrefab;

    [SerializeField]
    private bool showBreadcrumbs = true;

    private NavMeshAgent agent;

    private Transform tempContainer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject container =
            new GameObject("BreadcrumbContainer");

        tempContainer = container.transform;

#if !UNITY_EDITOR
        showBreadcrumbs = false;
#endif
    }

    private void Start()
    {
        InvokeRepeating(
            nameof(CreateBreadcrumb),
            0f,
            0.2f
        );
    }

    private void CreateBreadcrumb()
    {
        if (agent.velocity.magnitude <= 0.1f)
            return;

        GameObject crumb =
            Instantiate(
                breadcrumbPrefab,
                transform.position + Vector3.up,
                Quaternion.identity
            );

        crumb.transform.SetParent(tempContainer);

        if (!showBreadcrumbs)
        {
            Transform gfx =
                crumb.transform.Find("GFX");

            if (gfx != null)
            {
                gfx.gameObject.SetActive(false);
            }
        }
    }
}