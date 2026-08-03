using UnityEngine;

public class Loading : MonoBehaviour
{
	private Transform icon;

	private int ticks;

	private bool isAnimate;

	private void Awake()
	{
		icon = base.transform.GetChild(0);
		Disable();
	}

	private void FixedUpdate()
	{
		Animate();
	}

	internal void Enable()
	{
		icon.localRotation = Quaternion.identity;
		base.gameObject.SetActive(value: true);
		ticks = 0;
		isAnimate = true;
	}

	internal void Disable()
	{
		isAnimate = false;
		base.gameObject.SetActive(value: false);
	}

	private void Animate()
	{
		if (isAnimate)
		{
			if (ticks == 25)
			{
				ticks = 0;
				icon.localRotation = Quaternion.identity;
				return;
			}
			ticks++;
			ticks = ((ticks > 25) ? 25 : ticks);
			float num = (float)ticks * 0.04f;
			num = 3f * num * num - 2f * num * num * num;
			icon.localRotation = Quaternion.Euler(0f, 0f, (0f - num) * 360f);
		}
	}
}
