using UnityEngine;
using System.Collections;

namespace Map.TopDown {
	[RequireComponent(typeof(TerrainManager))]
	public class Selector : MonoBehaviour {
		public delegate void SelectionChangedEvent(Tile tile);
		public event SelectionChangedEvent Changed;

		public LayerMask layer;
		public TerrainManager manager = null;
		public Tile selection;
		// Use this for initialization
		void Start () {
			manager = gameObject.GetComponent<TerrainManager>();
		}
		
		void Update() {
			if (Input.GetMouseButtonUp(0)) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Physics.Raycast(ray, out hit, 10000, layer.value);
				int[] pos = manager.tilePos(hit.point);
				if (pos != null) {
					selection = manager.getTile(pos[0], pos[1]);
					Changed(selection);
				}
			}
		}
	}
}
