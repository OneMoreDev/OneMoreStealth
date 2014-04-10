using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory {
	public class InventoryItem {
		#region Constructors
		string _name;
		string _description;
		string _information;
		int _objectCorolation;
		float _weight;
		bool _destructsAfterUse;
		ItemType _type;
		#endregion Constructors
		#region Properties
		public InventoryItem(string name, string descrption, string information, int objectCorolation, float weight, bool destructsAfterUse, ItemType type) {
			_name = name;
			_description = descrption;
			_information = information;
			_objectCorolation = objectCorolation;
			_destructsAfterUse = destructsAfterUse;
			_type = type;
			_weight = weight;
		}
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}
		public string Description {
			get {
				return _description;
			}
			set {
				_description = value;
			}
		}
		public string Information {
			get {
				return _information;
			}
			set {
				_information = value;
			}
		}
		public int ObjectCorolation {
			get {
				return _objectCorolation;
			}
			set {
				_objectCorolation = value;
			}
		}
		public bool DestructsAfterUse {
			get {
				return _destructsAfterUse;
			}
			set {
				_destructsAfterUse = value;
			}
		}
		public float Weight {
			get {
				return _weight;
			}
			set {
				_weight = value;
			}
		}
		public ItemType Type {
			get {
				return _type;
			}
			set {
				_type = value;
			}
		}
		public enum ItemType {
			Nothing = 0,
			Key = 1,
			Book = 2,
			Disc = 4,
			Paper = 5
		}
		#endregion Properties
	}
}
