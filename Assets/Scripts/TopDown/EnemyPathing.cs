using UnityEngine;
using System.Collections;
using SimpleAI;

public class EnemyPathing : MonoBehaviour {
	// Use this for initialization
	Grid grid = new Grid();
	void Start () {
		grid.Awake(new Vector3(-10, 0, 0), 1000, 10, 2f, true);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
