using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Selection Visual")]
    public GameObject selectionIndicator;
    public Color selectedColor = Color.green;
    public Color normalColor = Color.white;
    
    private SpriteRenderer spriteRenderer;
    private virus_movement movementScript;
    private bool isSelected = false;

    void Start()
    {
        movementScript = GetComponent<virus_movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Deselect();
        
        if (selectionIndicator == null)
        {
            CreateSelectionIndicator();
        }
    }

    public void Select()
    {
        isSelected = true;
        
        if (selectionIndicator != null)
            selectionIndicator.SetActive(true);
            
        if (spriteRenderer != null)
            spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;
        
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
            
        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;
    }

    public void MoveTo(Vector2 position)
    {
        if (movementScript != null)
        {
            Vector3 targetPos = new Vector3(position.x, position.y, transform.position.z);
            movementScript.MoveToPosition(targetPos);
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public bool IsMoving()
    {
        return movementScript != null && movementScript.IsMoving();
    }

    void CreateSelectionIndicator()
    {
        GameObject indicator = new GameObject("SelectionIndicator");
        indicator.transform.parent = transform;
        indicator.transform.localPosition = Vector3.zero;
        
        SpriteRenderer indicatorRenderer = indicator.AddComponent<SpriteRenderer>();
        
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            indicatorRenderer.sprite = spriteRenderer.sprite;
            indicatorRenderer.color = new Color(selectedColor.r, selectedColor.g, selectedColor.b, 0.3f);
            indicator.transform.localScale = Vector3.one * 1.2f;
            indicatorRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        }
        
        selectionIndicator = indicator;
        selectionIndicator.SetActive(false);
    }
}