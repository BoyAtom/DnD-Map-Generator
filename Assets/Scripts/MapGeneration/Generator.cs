using UnityEngine;
using AccidentalNoise;
using System.Threading;
using System;
using System.Collections;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

	// Adjustable variables for Unity Inspector
	public GameObject Planet;
	public int Width = 512;
	public int Height = 512;
	public int TerrainOctaves = 6;
	public double TerrainFrequency = 1.25;
	public float DeepWater = 0.2f;
	public float ShallowWater = 0.4f;
	public float Sand = 0.5f;
	public float Grass = 0.7f;
	public float Forest = 0.8f;
	public float Rock = 0.9f;

	public Button SaveButton;

	// private variables
	ImplicitFractal HeightMap;
	MapData HeightData;
	Tile[,] Tiles;

	// Our texture output gameobject
	MeshRenderer HeightMapRenderer;

	void Start()
	{
		HeightMapRenderer = transform.Find ("HeightTexture").GetComponent<MeshRenderer> ();
	}

	private void Initialize()
	{
        // Initialize the HeightMap Generator
        System.Random rnd = new System.Random();
		HeightMap = new ImplicitFractal (FractalType.MULTI, 
		                               BasisType.SIMPLEX, 
		                               InterpolationType.QUINTIC, 
		                               TerrainOctaves, 
		                               TerrainFrequency, 
		                               rnd.Next(0, int.MaxValue));
	}
	
	// Extract data from a noise module
	private void GetData(ImplicitModuleBase module, ref MapData mapData)
	{
		mapData = new MapData (Width, Height);
 
		for (var x = 0; x < Width; x++) {
			for (var y = 0; y < Height; y++) {
	
				float x1 = 0, x2 = 2;
				float y1 = 0, y2 = 2;               
				float dx = x2 - x1;
				float dy = y2 - y1;
	
				float s = x / (float)Width;
				float t = y / (float)Height;
			
				float nx = x1 + Mathf.Cos (s*2*Mathf.PI) * dx/(2*Mathf.PI);
				float ny = y1 + Mathf.Cos (t*2*Mathf.PI) * dy/(2*Mathf.PI);
				float nz = x1 + Mathf.Sin (s*2*Mathf.PI) * dx/(2*Mathf.PI);
				float nw = y1 + Mathf.Sin (t*2*Mathf.PI) * dy/(2*Mathf.PI);
			
				float heightValue = (float)HeightMap.Get (nx, ny, nz, nw);
				
				if (heightValue > mapData.Max) mapData.Max = heightValue;
				if (heightValue < mapData.Min) mapData.Min = heightValue;
	
				mapData.Data[x,y] = heightValue;
			}
		}
	}

	// Build a Tile array from our data
	private void LoadTiles()
	{
		Tiles = new Tile[Width, Height];
		
		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height; y++)
			{
				Tile t = new Tile();
				t.X = x;
				t.Y = y;
				
				float value = HeightData.Data[x, y];
				value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);
				
				t.HeightValue = value;
				
				//HeightMap Analyze
				if (value < DeepWater)  {
					t.HeightType = HeightType.DeepWater;
				}
				else if (value < ShallowWater)  {
					t.HeightType = HeightType.ShallowWater;
				}
				else if (value < Sand) {
					t.HeightType = HeightType.Sand;
				}
				else if (value < Grass) {
					t.HeightType = HeightType.Grass;
				}
				else if (value < Forest) {
					t.HeightType = HeightType.Forest;
				}
				else if (value < Rock) {
					t.HeightType = HeightType.Rock;
				}
				else  {
					t.HeightType = HeightType.Snow;
				}
				
				Tiles[x,y] = t;
			}
        }
	}

	public void UpdateMap()
	{
        Initialize();
        GetData(HeightMap, ref HeightData);
        LoadTiles();
    }

    IEnumerator SetMap()
	{
		yield return new WaitForSeconds(20);
        Texture2D texture2D = TextureGenerator.GetTexture(Width, Height, Tiles);
        Planet.GetComponent<MeshRenderer>().material.SetTexture("_PlanetTexture", texture2D);
        HeightMapRenderer.materials[0].mainTexture = texture2D;

        SaveButton.enabled = true;
    }

	public void RegenerateMap(){
        SaveButton.enabled = false;
        Thread thrd = new Thread(new ThreadStart(UpdateMap));
		thrd.Start();
		StartCoroutine(SetMap());
    }

	public void SaveMapButton(){
		Texture2D tex2D = (Texture2D)HeightMapRenderer.materials[0].mainTexture;
		SaveImageFromTexture(tex2D);
	}

	private void SaveImageFromTexture(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/RenderOutput";
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }   
        System.IO.File.WriteAllBytes(dirPath + "/R_" + UnityEngine.Random.Range(0, 100000) + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);
    }
}