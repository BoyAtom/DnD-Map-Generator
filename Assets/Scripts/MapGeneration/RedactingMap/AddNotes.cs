using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddNotes : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mapTexture;
    [SerializeField]
    private GameObject notePoint;
    private GameObject descriptionField;
    private TMP_InputField inputField;

    public List<GameObject> notePoints;
    //private GameObject[] notePoints;
    public List<string> notesText;
    //private string[] notesText;

    bool isRedacting = false;
    public static int currentPoint = 0;
    public static int numberOfClickedButton = 0;

    public void SetClickedButton(int number)
    {
        Debug.Log(number);
        numberOfClickedButton = number;
    }

    /*
    public class Note
    {
        public GameObject point { get; private set; }
        public string description;

        public Note()
        {
            description = "exe3228";
        }

        public void SetPoint(GameObject dot)
        {
            point = dot;
        }
    }
    */

    // Start is called before the first frame update

    void Start()
    {
        initNotes();
        descriptionField = GameObject.Find("DescriptionField");
        inputField = descriptionField.gameObject.GetComponent<TMP_InputField>();
    }

    public void initNotes()
    {
        notePoints = new List<GameObject>();
        notesText = new List<string>();
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

    int timer = -1;
    private void FixedUpdate()
    {
        if (transform.gameObject.tag != "Button" & timer >= 10 & !isRedacting)
        {
            try
            {
                inputField.text = notesText[numberOfClickedButton];
            } catch
            {
                inputField.text = "";
            }
        }
        if (timer >= 10) timer = -1;
        timer += 1;
    }

    public void ShowHideDescriptionField()
    {
        Debug.Log(transform.gameObject.name);
        numberOfClickedButton = int.Parse(transform.gameObject.name);
        //Надеюсь это никто не увидит...
        transform.gameObject.GetComponentInParent<AddNotes>().SetClickedButton(numberOfClickedButton);
    }

    public void OnStartRedaction()
    {
        isRedacting = !isRedacting;
    }

    public void SetDescriptionIntoNote()
    {
        if (inputField.text != "")
        {
            string desc = inputField.text;
            notesText[numberOfClickedButton] = desc;
            Debug.Log("" + notesText[numberOfClickedButton]);
        }
    }

    private void PlaceNotePoint(Vector3 clickCoords)
    {
        GameObject obj = Instantiate(notePoint, clickCoords, new Quaternion());
        obj.transform.parent = transform;
        obj.name = "" + currentPoint;
        notePoints.Add(obj);
        notesText.Add("Text");
        currentPoint = CountPoints();
        Debug.Log(CountPoints());
    }

    private int CountPoints()
    {
        return notePoints.Count;
    }

    private void DestroyPoints()
    {
        foreach (GameObject obj in notePoints)
        {
            Destroy(obj);
        }
        initNotes();
        currentPoint = 0;
        numberOfClickedButton = 0;
    }
}
