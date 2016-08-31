using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPaletteSwap : MonoBehaviour {

	public Texture2D originalPalette;
	public Texture2D newPalette;
	// Use this for initialization
	void Start () {
		int uniqueColors = 0;
		List<Color32> foundColors = new List<Color32>();
		foreach (Color32 found in originalPalette.GetPixels()) {
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
		Texture2D generatedPallete;
		generatedPallete = new Texture2D (uniqueColors, 1, TextureFormat.ARGB32, false, false);
		generatedPallete.filterMode = FilterMode.Point;
		for (int x = 0; x < uniqueColors; x++) {
			generatedPallete.SetPixel (x, 0, foundColors [x]);
		}
		generatedPallete.Apply ();

		SpriteRenderer targetSprite = GetComponent<SpriteRenderer> ();
		targetSprite.material.SetTexture("_ColorPaletteOriginal", generatedPallete);
		targetSprite.material.SetTexture("_ColorPaletteMorph", newPalette);
		targetSprite.material.SetInt ("_textureWidth", uniqueColors +1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
