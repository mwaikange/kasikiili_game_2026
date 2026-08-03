using UnityEngine;

[ExecuteInEditMode]
public class ScaleToFitScreen : MonoBehaviour
{
	private SpriteRenderer sr;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		float num = Camera.main.orthographicSize * 2f;
		float num2 = num / (float)Screen.height * (float)Screen.width;
		base.transform.localScale = new Vector3(num2 / sr.sprite.bounds.size.x, num / sr.sprite.bounds.size.y, 1f);
	}
}
