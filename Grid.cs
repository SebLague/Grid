using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {
	public int width = 16;
	public int height = 9;
	[Range (0, 0.2f)]
	public float cellBorder = 0.05f;
	public Color colour = new Color (0.2f, 0.2f, 0.2f);
	public Color highlightColour = new Color (1, 0.4f, 0.4f);

	Mesh cellMesh;
	Material material;
	MaterialPropertyBlock defaultCellMaterialProperties;
	MaterialPropertyBlock highlightedCellMaterialProperties;
	bool settingsChangedSinceInit;

	void Update () {
		Init ();

		Vector2Int highlightedCell = GetHighlightedCell ();
		Draw (highlightedCell);
	}

	void Draw (Vector2Int highlightedCell) {
		float gridHalfWidth = (width - 1) / 2f;
		float gridHalfHeight = (height - 1) / 2f;

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				bool highlight = x == highlightedCell.x && y == highlightedCell.y;
				MaterialPropertyBlock currentMaterialProperties = (highlight) ? highlightedCellMaterialProperties : defaultCellMaterialProperties;
				Vector3 pos = new Vector3 (x - gridHalfWidth, y - gridHalfHeight);
				Graphics.DrawMesh (cellMesh, pos, Quaternion.identity, material, 0, null, 0, currentMaterialProperties);
			}
		}
	}

	Vector2Int GetHighlightedCell () {
		Vector2Int highlightedCell = new Vector2Int (-1, -1);

		if (Application.isPlaying) {
			float gridHalfWidth = (width - 1) / 2f;
			float gridHalfHeight = (height - 1) / 2f;

			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			highlightedCell.x = (int) (mousePos.x + gridHalfWidth + .5);
			highlightedCell.y = (int) (mousePos.y + gridHalfHeight + .5);
		}
		return highlightedCell;
	}

	void Init () {
		if (settingsChangedSinceInit) {
			settingsChangedSinceInit = false;
			InitColours ();
			InitMesh ();
		}
	}

	void InitColours () {
		if (material == null) {
			material = new Material (Shader.Find ("Unlit/Color"));
		}

		defaultCellMaterialProperties = new MaterialPropertyBlock ();
		highlightedCellMaterialProperties = new MaterialPropertyBlock ();
		defaultCellMaterialProperties.SetColor ("_Color", colour);
		highlightedCellMaterialProperties.SetColor ("_Color", highlightColour);
	}

	void InitMesh () {

		if (cellMesh == null) {
			cellMesh = new Mesh ();
		}
		cellMesh.Clear ();
		float cellHalfSize = 0.5f - cellBorder;
		// Top left, top right, bottom left, bottom right
		Vector3[] vertices = {
			new Vector3 (-1, 1) * cellHalfSize,
			new Vector3 (1, 1) * cellHalfSize,
			new Vector3 (-1, -1) * cellHalfSize,
			new Vector3 (1, -1) * cellHalfSize
		};
		cellMesh.vertices = vertices;
		cellMesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
		cellMesh.RecalculateBounds ();
	}

	void OnValidate () {
		settingsChangedSinceInit = true;
	}

	void Reset () {
		Debug.Log("Test");
		settingsChangedSinceInit = true;
	}
}