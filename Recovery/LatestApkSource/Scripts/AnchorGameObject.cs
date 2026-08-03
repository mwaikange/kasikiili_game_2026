using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class AnchorGameObject : MonoBehaviour
{
	public enum AnchorType
	{
		BottomLeft,
		BottomCenter,
		BottomRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		TopLeft,
		TopCenter,
		TopRight
	}

	public bool executeInUpdate;

	public AnchorType anchorType;

	public Vector3 anchorOffset;

	private IEnumerator updateAnchorRoutine;

	private void Start()
	{
		updateAnchorRoutine = UpdateAnchorAsync();
		StartCoroutine(updateAnchorRoutine);
	}

	private IEnumerator UpdateAnchorAsync()
	{
		uint cameraWaitCycles = 0u;
		while (CameraViewportHandler.Instance == null)
		{
			uint num = cameraWaitCycles + 1;
			cameraWaitCycles = num;
			yield return new WaitForEndOfFrame();
		}
		if (cameraWaitCycles != 0)
		{
			MonoBehaviour.print($"CameraAnchor found CameraFit instance after waiting {cameraWaitCycles} frame(s). You might want to check that CameraFit has an earlie execution order.");
		}
		UpdateAnchor();
		updateAnchorRoutine = null;
	}

	private void UpdateAnchor()
	{
		switch (anchorType)
		{
		case AnchorType.BottomLeft:
			SetAnchor(CameraViewportHandler.Instance.BottomLeft);
			break;
		case AnchorType.BottomCenter:
			SetAnchor(CameraViewportHandler.Instance.BottomCenter);
			break;
		case AnchorType.BottomRight:
			SetAnchor(CameraViewportHandler.Instance.BottomRight);
			break;
		case AnchorType.MiddleLeft:
			SetAnchor(CameraViewportHandler.Instance.MiddleLeft);
			break;
		case AnchorType.MiddleCenter:
			SetAnchor(CameraViewportHandler.Instance.MiddleCenter);
			break;
		case AnchorType.MiddleRight:
			SetAnchor(CameraViewportHandler.Instance.MiddleRight);
			break;
		case AnchorType.TopLeft:
			SetAnchor(CameraViewportHandler.Instance.TopLeft);
			break;
		case AnchorType.TopCenter:
			SetAnchor(CameraViewportHandler.Instance.TopCenter);
			break;
		case AnchorType.TopRight:
			SetAnchor(CameraViewportHandler.Instance.TopRight);
			break;
		}
	}

	private void SetAnchor(Vector3 anchor)
	{
		Vector3 vector = anchor + anchorOffset;
		if (!base.transform.position.Equals(vector))
		{
			base.transform.position = vector;
		}
	}
}
