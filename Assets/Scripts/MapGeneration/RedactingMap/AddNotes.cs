using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class AddNotes : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mapTexture;
    [SerializeField]
    private GameObject notePoint;
    private GameObject[] notePoints;
    private int currentPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        notePoints = new GameObject[100];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Texture2D texture = (Texture2D)mapTexture.materials[0].mainTexture;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;
                Vector3 click = Input.mousePosition;
                Vector3 position = Camera.main.ScreenToWorldPoint(click)/*new Vector3(pixelUV.x, pixelUV.y)*/;
                position.z = -1;

                Debug.Log("Add note: " + position.x + " / " + position.y);
                PlaceNotePoint(position);
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete)) 
        {
            DestroyPoints();
        }
    }

    private void PlaceNotePoint(Vector3 clickCoords)
    {
        notePoints[currentPoint] = Instantiate(notePoint, clickCoords, new Quaternion());
        currentPoint = CountPoints();
    }

    private int CountPoints()
    {
        int counter = 0;
        while (notePoints[counter] != null) counter += 1;
        return counter;
    }

    private void DestroyPoints()
    {
        int counter = 0;
        while (notePoints[counter] != null)
        {
            Destroy(notePoints[counter].gameObject);
            counter += 1;
        }
        notePoints = new GameObject[100];
        currentPoint = 0;
    }
}
