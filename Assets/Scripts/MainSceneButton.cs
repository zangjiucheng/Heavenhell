using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainSceneButton : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public TextMeshPro textMeshPro;
    public Color normalColor = new Color(1f, 1f, 1f, 0.85f);
    public Color hoverColor = new Color(1f, 1f, 1f, 1f);
    public float hoverScale = 1.02f;
    private Vector3 _originalScale;
    private bool _isHovered;
    private float _hoverT; // 0 = normal, 1 = hover
    public float transitionSpeed = 10f;

    [Header("Scene to load on click")]
    public string sceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if (textMeshPro == null)
            textMeshPro = GetComponentInChildren<TextMeshPro>();
        _originalScale = transform.localScale;
        // SetNormalState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null)
            return;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;
        Collider2D col = GetComponent<Collider2D>();
        bool hoverNow = col != null && col.OverlapPoint(mouseWorld);
        if (hoverNow && !_isHovered)
        {
            if (textMeshPro != null)
                textMeshPro.fontStyle |= FontStyles.Underline;
            _isHovered = true;
        }
        else if (!hoverNow && _isHovered)
        {
            if (textMeshPro != null)
                textMeshPro.fontStyle &= ~FontStyles.Underline;
            _isHovered = false;
        }

        // Lerp transition
        float target = _isHovered ? 1f : 0f;
        _hoverT = Mathf.MoveTowards(_hoverT, target, transitionSpeed * Time.deltaTime);
        // Color lerp
        if (meshRenderer != null)
            meshRenderer.material.color = Color.Lerp(normalColor, hoverColor, _hoverT);
        // Scale lerp
        transform.localScale = Vector3.Lerp(_originalScale, _originalScale * hoverScale, _hoverT);
    }

    void OnMouseUpAsButton()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
