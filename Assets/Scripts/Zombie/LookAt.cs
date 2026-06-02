using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LookAt : MonoBehaviour
{
    [Header("Head Bone")]
    public Transform head = null;

    [Header("Look Target")]
    public Vector3 lookAtTargetPosition;

    [Header("Timing")]
    public float lookAtCoolTime = 0.2f;
    public float lookAtHeatTime = 0.2f;

    [Header("State")]
    public bool looking = true;

    private Vector3 lookAtPosition;
    private Animator anim;
    private float lookAtWeight = 0.0f;

    private void Reset()
    {
        SetHeadBone();
    }

    private void Start()
    {
        if (!head)
        {
            Debug.LogError("No head transform - LookAt disabled");
            enabled = false;
            return;
        }

        anim = GetComponent<Animator>();

        lookAtTargetPosition = head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!head) return;

        // Mantener la misma altura de la cabeza
        lookAtTargetPosition.y = head.position.y;

        // Peso objetivo
        float targetWeight = looking ? 1.0f : 0.0f;

        // Dirección actual y futura
        Vector3 currentDir = lookAtPosition - head.position;
        Vector3 futureDir = lookAtTargetPosition - head.position;

        // Rotación suave
        currentDir = Vector3.RotateTowards(
            currentDir,
            futureDir,
            6.28f * Time.deltaTime,
            float.PositiveInfinity
        );

        lookAtPosition = head.position + currentDir;

        // Interpolación del peso
        float blendTime = targetWeight > lookAtWeight
            ? lookAtHeatTime
            : lookAtCoolTime;

        lookAtWeight = Mathf.MoveTowards(
            lookAtWeight,
            targetWeight,
            Time.deltaTime / blendTime
        );

        // Configuración IK
        anim.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        anim.SetLookAtPosition(lookAtPosition);
    }

    private void SetHeadBone()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform t in children)
        {
            if (t.name.ToLower().Contains("head"))
            {
                head = t;
                return;
            }
        }
    }
}