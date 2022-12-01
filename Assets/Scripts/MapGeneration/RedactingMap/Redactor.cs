using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Redactor : MonoBehaviour
{
    public void SetHeight()
    {

    }

    public void Testing_SetHeight()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            Debug.Log("You clicked on redactor panel!");
        }
    }
}