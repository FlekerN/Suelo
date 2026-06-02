using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [SerializeField] private GameObject clickTarget;

    private NavMeshAgent agent;
    private Camera cam;
    private NavMeshHit hit;
    private Transform tempContainer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;

        GameObject container = GameObject.Find("Temp Container");

        if (container != null)
        {
            tempContainer = container.transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit rayHit))
            {
                if (NavMesh.SamplePosition(rayHit.point, out hit, 2f, NavMesh.AllAreas))
                {
                    if (clickTarget != null)
                    {
                        GameObject goClick = Instantiate(
                            clickTarget,
                            hit.position,
                            Quaternion.identity,
                            tempContainer
                        );

                        goClick.name = "Click Target";
                    }

                    agent.SetDestination(hit.position);
                }
            }
        }
    }
}