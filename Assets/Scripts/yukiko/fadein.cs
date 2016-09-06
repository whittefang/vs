using UnityEngine;
using System.Collections;

public class fadein : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake  () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnEnable(){
		StartCoroutine (SummonEffect ());

	}
	IEnumerator SummonEffect(){	
		spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0);

		for (int i = 0; i < 10;) {
			yield return null;

			i++;
			spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, spriteRenderer.color.a + .1f);


		}

	}
}
