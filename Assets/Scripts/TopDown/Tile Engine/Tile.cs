using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Map.TopDown {
	public abstract class Tile {
		/// <summary>
		/// The  serialized ID of this tile type
		/// </summary>
		public static int ID = -1;
		/// <summary>
		/// The TerrainManager that owns this tile
		/// </summary>
		public TerrainManager manager;
		/// <summary>
		/// The model to be used for this tile
		/// </summary>
		public Mesh model = null;
		/// <summary>
		/// The model's material.
		/// </summary>
		public Material material;
		/// <summary>
		/// Can units pass trough it?
		/// </summary>
		public bool passable = true;
		/// <summary>
		/// The UI actions this tile can perform
		/// </summary>
		public List<TileAction> actions;
		/// <summary>
		/// The UI name of the tile
		/// </summary>
		public string name;
		/// <summary>
		/// The tile's health. -1 if invulnerable
		/// </summary>
		public int health, maxHealth;
		public Tile() {}
		/// <summary>
		/// Called when the tile is added to a terrain
		/// </summary>
		public virtual void created() {}
		/// <summary>
		/// Called every frame before rendering occurs
		/// </summary>
		public virtual void onTick() {}
		/// <summary>
		/// Called every frame after rendering occurs
		/// </summary>
		public virtual void onRender(Vector3 position) {}
		/// <summary>
		/// Called when the tile is removed form a terrain
		/// </summary>
		public virtual void removed() {}
		/// <summary>
		/// Loads a model from the Resources folder
		/// </summary>
		/// <returns>A Mesh representation of the model</returns>
		/// <param name="name">The path to load the model from</param>
		public static Mesh LoadModel(string name) {
			return Resources.Load<GameObject>(name).GetComponent<MeshFilter>().mesh;
		}
		/// <summary>
		/// Loads a material from the Resources folder
		/// </summary>
		/// <returns>A Material instance</returns>
		/// <param name="name">The path to load the material from</param>
		public static Material LoadMaterial(string name) {
			return Resources.Load<Material>("Materials/"+name);
		}
	}
}