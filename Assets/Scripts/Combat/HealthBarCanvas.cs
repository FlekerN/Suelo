using UnityEngine;
using UnityEngine.UI;
public class HealthBarCanvas : MonoBehaviour
{
    public RectTransform HealthBar;

    public RectTransform Panel;

    private IDamageable _IDamageableObject;

    private float anchoOriginal;

    private void Awake()
    {
        _IDamageableObject = GetComponentInParent<IDamageable>();
        if (_IDamageableObject == null) 
        {
            Debug.LogWarning("Idamageable NOT FOUND");
            enabled = false;
            return;
        }
    }
    private void Start()
    {
        anchoOriginal = HealthBar.sizeDelta.x; 
    }
    private void OnEnable()
    {
        if (_IDamageableObject != null) _IDamageableObject.OnHealthChanged += HandleHealthChanged;
    }
    private void OnDisable()
    {
        if (_IDamageableObject != null) _IDamageableObject.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float max, float current) 
    {
        Panel.gameObject.SetActive(current < max);

        if (HealthBar != null) 
        {
            float nuevoAncho = (current / max) * anchoOriginal;
            HealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,nuevoAncho);
        }
        if (current <= 0) 
        {
            Panel.gameObject.SetActive(false);
            Destroy(this);
        }
    }

}
