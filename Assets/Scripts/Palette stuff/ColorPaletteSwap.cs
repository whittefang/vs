using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPaletteSwap : MonoBehaviour {

	public Texture2D originalPalette;
	public Texture2D newPaletteDefault;
	public int uniqueColors = 0;
	public List<Color32> foundColors = new List<Color32>();
	public List<Color32> foundColorsnew = new List<Color32>();
	public bool fireOnAwake = true;
	// Use this for initialization
	void Awake () {
		if (fireOnAwake) {
			LoadColors (newPaletteDefault);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void LoadColors(Texture2D newPalette){
		// move original into a texture

		uniqueColors = 0;
		foundColors.Clear ();
		foundColorsnew.Clear ();

		foreach (Color32 found in originalPalette.GetPixels()) {
			uniqueColors++;
			foundColors.Add (found);
		}
		Texture2D generatedPallete;
		generatedPallete = new Texture2D (uniqueColors, 1, TextureFormat.ARGB32, false, false);
		generatedPallete.filterMode = FilterMode.Point;
		for (int x = 0; x < uniqueColors; x++) {
			generatedPallete.SetPixel (x, 0, foundColors [x]);
		}
		//Debug.Log (uniqueColors);
		generatedPallete.Apply ();



		uniqueColors = 0;
		foreach (Color32 found in newPalette.GetPixels()) {
			uniqueColors++;
			foundColorsnew.Add (found);
		}
		Texture2D NewPaletteGeneration;
		NewPaletteGeneration = new Texture2D (uniqueColors, 1, TextureFormat.ARGB32, false, false);
		NewPaletteGeneration.filterMode = FilterMode.Point;
		for (int x = 0; x < uniqueColors; x++) {
			NewPaletteGeneration.SetPixel (x, 0, foundColorsnew [x]);
		}
		//Debug.Log (uniqueColors);
		NewPaletteGeneration.Apply ();



		SpriteRenderer targetSprite = GetComponent<SpriteRenderer> ();
		targetSprite.material.SetTexture("_ColorPaletteOriginal", generatedPallete);
		targetSprite.material.SetTexture("_ColorPaletteMorph", NewPaletteGeneration);
		targetSprite.material.SetInt ("_textureWidth", uniqueColors +1 );
		Debug.Log (gameObject.name);
	}
}
