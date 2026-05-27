using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputReaderBehaviour;

    private IPlayerInput inputReader;
    private IInteractable currentInteractable;

    private void Awake()
    {
        inputReader = inputReaderBehaviour as IPlayerInput;

        if (inputReader == null)
        {
            Debug.LogError($"{nameof(inputReaderBehaviour)} debe implementar IPlayerInput", this);
        }
    }

    private void OnEnable()
    {
        if (inputReader != null)
            inputReader.OnInteractEvent += Interact;
    }

    private void OnDisable()
    {
        if (inputReader != null)
            inputReader.OnInteractEvent -= Interact;
    }

    private void Interact()
    {
        Debug.Log("E pulsada / Interact llamado");

        if (currentInteractable == null)
        {
            Debug.Log("No hay interactuable cerca");
            return;
        }

        currentInteractable.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entró en trigger: " + other.name);

        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            Debug.Log("Interactuable detectado");
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) &&
            interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }
}