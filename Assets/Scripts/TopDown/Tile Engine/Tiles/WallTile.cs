using UnityEngine;
using System.Collections;
using Map.TopDown;

public class WallTile : Tile {
	new public static int ID = 1;
	
	public override void created() {
		name = "Wall tile";
		model = LoadModel("wall");
		material = LoadMaterial("derp");
	}
}