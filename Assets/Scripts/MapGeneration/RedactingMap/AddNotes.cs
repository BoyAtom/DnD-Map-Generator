using TMPro;
using UnityEngine;

public class AddNotes : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mapTexture;
    [SerializeField]
    private GameObject notePoint;
    [SerializeField]
    private GameObject descriptionField;
    //private GameObject[] notePoints;
    private Note[] notePoints;
    private int currentPoint = 0;

    public class Note
    {
        public GameObject point;
        public string description;

        public Note()
        {
            description = string.Empty;
        }

        public void SetPoint(GameObject dot)
        {
            point = dot;
        }

        public void SetDescription(string text)
        {
            description = text;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        notePoints = new Note[100];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && transform.gameObject.tag != "Button")
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
        if (Input.GetKeyDown(KeyCode.Delete) && transform.gameObject.tag != "Button") 
        {
            DestroyPoints();
        }
    }

    public void ShowHideDescriptionField()
    {
        gameObject.SetActive(true);
        TMP_InputField inputField = descriptionField.gameObject.GetComponent<TMP_InputField>();
        inputField.text = "Leeeroy";
    }

    private void PlaceNotePoint(Vector3 clickCoords)
    {
        GameObject obj = Instantiate(notePoint, clickCoords, new Quaternion());
        notePoints[currentPoint] = new Note();
        notePoints[currentPoint].SetPoint(obj);
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
            Destroy(notePoints[counter].point.gameObject);
            counter += 1;
        }
        notePoints = new Note[100];
        currentPoint = 0;
    }
}
