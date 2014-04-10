using UnityEngine;
using System.Collections;
using Map.TopDown;

public class SimpleTile : Tile {
	new public static int ID = 0;

	public override void created() {
		name = "Derp tile";
		model = LoadModel("example");
		material = LoadMaterial("derp");
	}
}
