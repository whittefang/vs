using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class TextureWriter : MonoBehaviour {
	Texture2D newPalette;
	public string fileName;
	public Texture2D[] originalSpritesArray;
	public string SpritesArrayFolderPath = "Ryu";
	public SpriteRenderer targetSprite;
	public MeshRenderer generatedPalletePreview;
	public MeshRenderer newPalettePreview;
	public ColorPicker colorSelector;

	Color lastColor;
	int lastSpot = 0;
	Texture2D generatedPallete;

	// Use this for initialization
	void Start () {
		//SR = GetComponent<SpriteRenderer> ();
		GeneratePalette();

		//SaveTexture ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			SaveTexture ();
		}
		if (Input.GetKeyDown (KeyCode.F2)) {
			for (int i = 0; i < 64; i++) {

				SetPixel (i, new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1));
			}
		}
		if (Input.GetKeyDown (KeyCode.F3)) {
			originalSpritesArray = Resources.LoadAll<Texture2D>(SpritesArrayFolderPath);
			GeneratePalette ();
		}

	}

	public void SetPixel(int x, Color newCol){
		newPalette.SetPixel (x, 0, newCol);
		lastColor = newCol;
		newPalette.Apply();

	}
	public void SaveTexture(){
		// Encode texture into PNG
		byte[] bytes = newPalette.EncodeToPNG();

		File.WriteAllBytes(Application.dataPath + "/shaderPalettes/" + fileName + ".png", bytes);
	}
	public void ResetColor (int x){
		newPalette.SetPixel (x, 0, generatedPallete.GetPixel (x, 0));
		lastColor = generatedPallete.GetPixel (x, 0);
		newPalette.Apply();
	}
	public void HideSelectionColor (int x){
		newPalette.SetPixel (x, 0, lastColor);
		newPalette.Apply();
	}
	public void ShowPixels(int x){
		newPalette.SetPixel (lastSpot, 0, lastColor);
		lastColor = newPalette.GetPixel (x, 0);
		lastSpot = x;
		colorSelector.SetColor (new Color(lastColor.r, lastColor.g, lastColor.b, 1));
		newPalette.SetPixel (x, 0, new Color(1,.1f,1,1));
		newPalette.Apply();
	}
	public void SetPreviewColor(int x, Color newCol){
		newPalette.SetPixel (x, 0, newCol);
		newPalette.Apply();
	}
	public void GeneratePalette(){
		
		List<Color32> foundColors = new List<Color32>();
		int uniqueColors = 0;
		Debug.Log ("Started scanning sprites, if there is a large Volume it may take some time");
		foreach (Texture2D tex in originalSpritesArray) {
			foreach (Color32 found in tex.GetPixels()) {
				bool match = false;
				//Debug.Log (found);

				foreach (Color32 existing in foundColors) {
					if (found.r == existing.r && found.g == existing.g && found.b == existing.b) {// && found.a == existing.a) {
						match = true;
						break;
					}

				}
				if (!match) {
					uniqueColors++;
					foundColors.Add (found);
					//Debug.Log (found);
				}
			}
		}
		generatedPallete = new Texture2D (uniqueColors, 1, TextureFormat.ARGB32, false, false);
		generatedPallete.filterMode = FilterMode.Point;
		for (int x = 0; x < uniqueColors; x++) {
			generatedPallete.SetPixel (x, 0, foundColors [x]);
		}
		// put pixel data into preview texture
		generatedPallete.Apply ();
		generatedPalletePreview.material.mainTexture = generatedPallete;

		// copy pixels into a new palette and hook into shader
		newPalette = new Texture2D (uniqueColors, 1, TextureFormat.ARGB32, false, false);
		newPalette.filterMode = FilterMode.Point;
		newPalette.SetPixels (generatedPallete.GetPixels ());
		newPalette.Apply();
		targetSprite.material.SetTexture("_ColorPaletteOriginal", generatedPallete);
		targetSprite.material.SetTexture("_ColorPaletteMorph", newPalette);
		targetSprite.material.SetInt ("_textureWidth", uniqueColors +1);
		newPalettePreview.material.mainTexture = newPalette;
		colorSelector.textureWidth = uniqueColors;
		lastColor = newPalette.GetPixel (0, 0);
		Debug.Log (uniqueColors);
	}
}
