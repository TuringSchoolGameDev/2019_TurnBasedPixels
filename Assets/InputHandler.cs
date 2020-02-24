using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        OnCastComplete();
    }

    public GameObject selected;
    public GameObject newSelected;
    public void OnCastComplete()
    {
        newSelected = null;

        Vector2 coords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(coords, Vector2.zero, 100f);
        if (raycastHits != null)
        {
            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("GroundTile")))
                {
                    newSelected = raycastHit.collider.gameObject;
                }
                break;
            }
        }

        if (newSelected != null)
        {
            if (newSelected != selected)
            {
                if (selected != null)
                {
                    selected.GetComponent<SpriteRenderer>().color = Color.white;
                    selected = null;
                }
                selected = newSelected;
                selected.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else if (selected != null)
        {
            selected.GetComponent<SpriteRenderer>().color = Color.white;
            selected = null;
        }
    }
}
