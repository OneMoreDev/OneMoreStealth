using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RelativeRect{
	public static Rect GetRelative(float x, float y, float width, float height) {
		Rect rect = new Rect((x / 2 / 100) * Screen.width,
			(y / 100) * Screen.height,
			(width / 100) * Screen.width,
			(height / 100) * Screen.height);
		return rect;
	}
}
