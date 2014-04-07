using System.Collections.Generic;
using System;

namespace OneDescript {
	public enum OValueType {
		INT = 0,
		FLOAT = 1,
		STRING = 2,
		REFLIST = 3
	}
	public class OValue {
		private OValueType _type;
		private object _value;
		
		public OValue(object value) {
			Type type = value.GetType();
			if (type == typeof(int)) {
				_type = OValueType.INT;
			} else if (type == typeof(float)) {
				_type = OValueType.FLOAT;
			} else if (type == typeof(string)) {
				_type = OValueType.STRING;
			} else if (type == typeof(List<OValue>)) {
				_type = OValueType.REFLIST;
			}
			_value = value;
		}
		
		public object GetValue(OValueType type) {
			if (type == _type) {
				return _value;
			}
			if (type == OValueType.STRING) {
				return _value.ToString();
			}
			throw new ArgumentException("Types don't match");
		}
	}
}