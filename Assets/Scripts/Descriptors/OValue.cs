using System.Collections.Generic;
using System;

namespace OneDescript {
	/// <summary>
	/// Represents the type of the value of a <see cref="OValue"/>.
	/// </summary>
	public class OValueType {
		public static readonly OValueType INT = new OValueType(0);
		public static readonly OValueType FLOAT = new OValueType(1);
		public static readonly OValueType STRING = new OValueType(2);
		public static readonly OValueType REFLIST = new OValueType(3);

		public byte typeByte {get; private set;}

		private OValueType() {}
		private OValueType(byte type) {
			this.typeByte = type;
		}

		/// <summary>
		/// Returns the corresponding type instance for a given type byte.
		/// </summary>
		/// <returns>The byte.</returns>
		/// <param name="b">The blue component.</param>
		public static OValueType FromByte(byte b) {
			switch (b) {
				case 0: return INT;
				case 1: return FLOAT;
				case 2: return STRING;
				case 3: return REFLIST;
				default: throw new ArgumentException("Invalid type "+b);
			}
		}

		public bool Equals(OValueType other) {
			return typeByte == other.typeByte;
		}
	}

	public class OValue {
		/// <summary>
		/// The type of the stored value.
		/// </summary>
		/// <value>The type.</value>
		public OValueType type {get; private set;}
		/// <summary>
		/// The raw value of this instance.
		/// </summary>
		private object _value;
		
		public OValue(OValueType type, object value) {
			this.type = type;
			_value = value;
		}

		/// <summary>
		/// Gets the value of this instance, coerced to the given type.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="type">Type to coerce to.</param>
		public object GetValue(OValueType type) {
			if (type == this.type) {
				return _value;
			}
			if (type == OValueType.STRING) {
				return _value.ToString();
			}
			throw new ArgumentException("Types don't match");
		}


		/// <returns>The value of this instance.</returns>
		public object GetValue() {
			return GetValue(type);
		}
	}
}