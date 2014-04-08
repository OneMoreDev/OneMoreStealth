using System.Collections.Generic;
using System;

namespace OneDescript {
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
		public OValueType type {get; private set;}
		private object _value;
		
		public OValue(OValueType type, object value) {
			this.type = type;
			_value = value;
		}
		
		public object GetValue(OValueType type) {
			if (type == this.type) {
				return _value;
			}
			if (type == OValueType.STRING) {
				return _value.ToString();
			}
			throw new ArgumentException("Types don't match");
		}
	}
}