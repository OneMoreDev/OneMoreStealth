using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapGeneration.Runner {
	public class Platform : IList<Entity> {
		public float width;
		public float height;
		public bool floating;
		private List<Entity> entities;

		public Platform() {
			entities = new List<Entity>();
		}

		public int IndexOf (Entity item) {
			return entities.IndexOf(item);
		}
		public void Insert(int index, Entity item) {
			entities.Insert(index, item);
		}
		public void RemoveAt (int index) {
			entities.RemoveAt(index);
		}
		public Entity this [int index] {
			get {
				return entities[index];
			}
			set {
				value.parent = this;
				entities[index] = value;
			}
		}
		public void Add (Entity item) {
			entities.Add(item);
		}
		public void Clear () {
			entities.Clear();
		}
		public bool Contains (Entity item) {
			return entities.Contains(item);
		}
		public void CopyTo (Entity[] array, int arrayIndex) {
			entities.CopyTo(array, arrayIndex);
		}
		public bool Remove (Entity item) {
			return entities.Remove(item);
		}
		public int Count {
			get {
				return entities.Count;
			}
		}
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		public IEnumerator<Entity> GetEnumerator () {
			return entities.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator () {
			return entities.GetEnumerator();
		}
	}
}
