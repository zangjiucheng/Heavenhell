using UnityEngine;
using UnityEngine.EventSystems;

public class Persona : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Canvas ReportToShow;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI Image clicked!");
        
        // Show the report with animation
        if (ReportToShow != null)
        {
            ReportToShow.enabled = true;
        }
    }

    void Start()
    {
        ReportToShow.enabled = false;
    }

    void Update()
    {
        // Close report when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ReportToShow != null && ReportToShow.enabled)
            {
                ReportToShow.enabled = false;
            }
        }
    }
}
