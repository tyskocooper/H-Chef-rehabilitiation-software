using UnityEngine;

public class DropArea : MonoBehaviour
{
    public BoxCollider2D areaCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (areaCollider == null)
        {areaCollider = GetComponent<BoxCollider2D>();
        }
        areaCollider.isTrigger = true;
    }

}
