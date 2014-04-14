using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

namespace Map.TopDown {
	[RequireComponent(typeof(PathManagerComponent))]
	[ExecuteInEditMode]
	public class TerrainManager : PathTerrainComponent {
		
		public string mapFile;
		public int mapSize, debrisTileId;
		public float distanceScale = 1f;
		public GameObject selectCollider = null;
		public Material passableMat, impassableMat;
		public Transform origin;
		public PathManagerComponent pathMan;
		
		private Tile[,] tiles = new Tile[1,1];
		private List<Type> tileTypes = new List<Type>();
		public static Quaternion tileRotation = Quaternion.LookRotation(new Vector3(0, 1, 0));
		private SimpleAI.Navigation.PathGrid grid;
		
		void Awake() {
			grid = new SimpleAI.Navigation.PathGrid();
			grid.Awake(new Vector3(-(mapSize*distanceScale/2f), 0, -(mapSize*distanceScale/2f)), mapSize, mapSize, distanceScale, false);
			
			IEnumerable<Type> tempTileTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => typeof(Tile)
				.IsAssignableFrom(p)&&p.IsClass);
			foreach (Type type in tempTileTypes) {
				tileTypes.Add(type);
			}
		}

		void Start() {
			pathMan = gameObject.GetComponent<PathManagerComponent>();
			selectCollider.transform.localScale = new Vector3(mapSize*distanceScale*2, .1f, mapSize*distanceScale*2);
			tiles = new Tile[mapSize,mapSize];
			for (int y = 0; y < mapSize; y++) {
				for (int x = 0; x < mapSize; x++) {
					int id = 0;
					setTile(x, y, id, false);
				}
			}
			recalculateSolidity();
		}
		
		// Update is called once per frame
		void Update () {
			for (int y = mapSize-1; y >= 0 ; y--) {
				for (int x = mapSize-1; x >= 0; x--) {
					if (tiles[x, y] != null) {
						tiles[x, y].onTick();
						float posX = (mapSize*distanceScale/2f)-(distanceScale/2f)-x*distanceScale;
						float posY = (mapSize*distanceScale/2f)-(distanceScale/2f)-y*distanceScale;
						int tileX = mapSize-1-x;
						int tileY = mapSize-1-y;
						Vector3 position = new Vector3(posX, 0, posY)+origin.position;
						Graphics.DrawMesh(tiles[tileX, tileY].model, position, tileRotation, tiles[tileX, tileY].material, LayerMask.NameToLayer("Terrain"));
						tiles[x, y].onRender(-position);
					}
				}
			}
		}
		
		public void setTile(int x, int y, int tileId) {
			setTile(x, y, tileId, true);
		}
		public void setTile(int x, int y, Tile tile, bool recalculate) {
			Tile oldTile = tiles[x, y];
			tile.manager = this;
			tile.created();
			tiles[x, y] = tile;
			if (recalculate) recalculateSolidity();
			if (oldTile != null) tiles[x, y].removed();
		}
		public void setTile(int x, int y, int tileId, bool recalculate) {
			Tile tile = (Tile)tileTypes.FirstOrDefault(c => (int)c.GetField ("ID").GetValue (c) == tileId).GetConstructor (new Type[] {}).Invoke(new object[]{});
			setTile(x, y, tile, recalculate);
		}
		public void setTile(int x, int y, Tile tile) {
			setTile(x, y, tile, true);
		}
		public Tile getTile(int x, int y) {
			if (inBounds(x, y)) {
				return tiles[mapSize-1-x, mapSize-1-y];
			}
			return null;
		}
		public int getTileId(int x, int y) {
			return (int)getTile(x, y).GetType().GetField("ID").GetValue(null);
		}
		public Type getType(int tileId) {
			return tileTypes.First(t => (int)t.GetField("ID").GetValue(null) == tileId);
		}
		public void damageTile(int x, int y, int damage) {
			Tile tile = tiles[x, y];
			if (tile.maxHealth > -1) {
				tile.health -= damage;
				if(tile.health < 1) {
					setTile(x, y, debrisTileId);
				}
			}
		}
		public Tile tileAt(Vector3 pos) {
			int[] tile = tilePos(pos);
			if (tile != null) return tiles[tile[0], tile[1]];
			return null;
		}
		public int[] tilePos(Vector3 pos) {
			int x = (int)(-pos.x+mapSize/(distanceScale/2f))/2;
			int y = (int)(-pos.z+mapSize/(distanceScale/2f))/2;
			if (inBounds(x, y)) {
				return new int[] {x, y};
			}
			return null;
		}
		public int[] locateTile(Tile tile) {
			for (int y = 0; y < mapSize; y++) {
				for (int x = 0; x < mapSize; x++) {
					if (tiles[x, y] == tile) {
						return new int[]{x, y};
					}
				}
			}
			return null;
		}
		public bool isPassable(int x, int y) {
			return getTile(x, y).passable;
		}
		public bool inBounds(int x, int y) {
			return (x >= 0 && y >= 0 && x < mapSize && y < mapSize);
		}
		public int countTileType(Type tileType) {
			int count = 0;
			for (int y = 0; y < mapSize; y++) {
				for (int x = 0; x < mapSize; x++) {
					if (tiles[x, y] != null && tiles[x, y].GetType() == tileType) {
						count++;
					}
				}
			}
			return count;
		}
		public void recalculateSolidity() {
			for (int y = 0; y < mapSize; y++) {
				for (int x = 0; x < mapSize; x++) {
					if (tiles[x, y] != null) {
						grid.SetSolidity(x*mapSize+y, !tiles[y, x].passable);
					} else {
						grid.SetSolidity(x*mapSize+y, false);
					}
				}
			}
			m_pathTerrain = grid;
		}
		public int[] nearestOfType(Type tileType, Vector3 position) {
			List<Vector3> matches = new List<Vector3>();
			for (int y = 0; y < mapSize; y++) {
				for (int x = 0; x < mapSize; x++) {
					if (tiles[x, y].GetType() == tileType) {
						matches.Add(new Vector3(x, 0, y));
					}
				}
			}
			if (matches.Count > 0) {
				Vector3 nearest = matches.OrderBy(p => Vector3.Distance(position, p)).First();
				return new int[] {(int)nearest.x, (int)nearest.z};
			}
			return null;
		}
		
		public Vector3 positionOf(int[] tileIndices) {
			return new Vector3(	tileIndices[0]*distanceScale - (mapSize*distanceScale/2f), 0, 
			                   tileIndices[1]*distanceScale - (mapSize*distanceScale/2f))+origin.position;
		}
		void OnDrawGizmos() {
			if (Application.isPlaying) {
				for (int i = 0; i < grid.NumberOfCells; i++) {
					Gizmos.color = grid.IsBlocked(i)?impassableMat.color:passableMat.color;
					Gizmos.DrawCube(grid.GetCellCenter(i), new Vector3(distanceScale, 1, distanceScale));
				}
			}
		}
	}
}
