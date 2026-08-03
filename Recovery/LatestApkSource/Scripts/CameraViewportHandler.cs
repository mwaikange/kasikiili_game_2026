using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraViewportHandler : MonoBehaviour
{
	public enum Constraint
	{
		Landscape,
		Portrait
	}

	public Color wireColor = Color.white;

	public float UnitsSize = 1f;

	public Constraint constraint = Constraint.Portrait;

	public static CameraViewportHandler Instance;

	public Camera camera;

	public bool executeInUpdate;

	private float _width;

	private float _height;

	private Vector3 _bl;

	private Vector3 _bc;

	private Vector3 _br;

	private Vector3 _ml;

	private Vector3 _mc;

	private Vector3 _mr;

	private Vector3 _tl;

	private Vector3 _tc;

	private Vector3 _tr;

	public float Width => _width;

	public float Height => _height;

	public Vector3 BottomLeft => _bl;

	public Vector3 BottomCenter => _bc;

	public Vector3 BottomRight => _br;

	public Vector3 MiddleLeft => _ml;

	public Vector3 MiddleCenter => _mc;

	public Vector3 MiddleRight => _mr;

	public Vector3 TopLeft => _tl;

	public Vector3 TopCenter => _tc;

	public Vector3 TopRight => _tr;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		Instance = this;
		ComputeResolution();
	}

	private void ComputeResolution()
	{
		if (constraint == Constraint.Landscape)
		{
			camera.orthographicSize = 1f / camera.aspect * UnitsSize / 2f;
		}
		else
		{
			camera.orthographicSize = UnitsSize / 2f;
		}
		_height = 2f * camera.orthographicSize;
		_width = _height * camera.aspect;
		float x = camera.transform.position.x;
		float y = camera.transform.position.y;
		float x2 = x - _width / 2f;
		float x3 = x + _width / 2f;
		float y2 = y + _height / 2f;
		float y3 = y - _height / 2f;
		_bl = new Vector3(x2, y3, 0f);
		_bc = new Vector3(x, y3, 0f);
		_br = new Vector3(x3, y3, 0f);
		_ml = new Vector3(x2, y, 0f);
		_mc = new Vector3(x, y, 0f);
		_mr = new Vector3(x3, y, 0f);
		_tl = new Vector3(x2, y2, 0f);
		_tc = new Vector3(x, y2, 0f);
		_tr = new Vector3(x3, y2, 0f);
	}

	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = wireColor;
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
		if (camera.orthographic)
		{
			float z = camera.farClipPlane - camera.nearClipPlane;
			float z2 = (camera.farClipPlane + camera.nearClipPlane) * 0.5f;
			Gizmos.DrawWireCube(new Vector3(0f, 0f, z2), new Vector3(camera.orthographicSize * 2f * camera.aspect, camera.orthographicSize * 2f, z));
		}
		else
		{
			Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
		}
		Gizmos.matrix = matrix;
	}
}
