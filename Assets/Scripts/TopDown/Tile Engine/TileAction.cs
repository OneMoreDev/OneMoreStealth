using UnityEngine;
using System.Collections;

namespace Map.TopDown {
	public abstract class TileAction {
		public string name;
		public Texture image;
		public TileAction() {
			created();
		}
		public abstract void act(Tile tile);
		public abstract void created();
		
		public static Texture LoadImage(string name) {
			return Resources.Load<Texture>("Images/"+name);
		}
	}
}
