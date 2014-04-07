package com.onemoredev.binaryio;

/**
 * A class that represents a field of a BinaryObject (e.g. color) 
 * @author Adam
 */
public class BinaryField
{	
	private int type;
	
	private String string_value;
	private int int_value;
	private float float_value;
	private int[] reflist_value;
	
	public BinaryField(String value)
	{
		this.string_value = value;
		this.type = C.TYPE_STRING;
	}
	
	public BinaryField(int value)
	{
		this.int_value = value;
		this.type = C.TYPE_INT;
	}
	
	public BinaryField(float value)
	{
		this.float_value = value;
		this.type = C.TYPE_FLOAT;
	}
	
	public BinaryField(int[] references)
	{
		this.reflist_value = references;
		this.type = C.TYPE_REFLIST;
	}
	
	public int getFieldType() { return type; }
	
	public String getStringValue()
	{
		checkValue(C.TYPE_STRING);
		return string_value;
	}

	public int getIntValue()
	{
		checkValue(C.TYPE_INT);
		return int_value;
	}

	public float getFloatValue()
	{
		checkValue(C.TYPE_FLOAT);
		return float_value;
	}

	public int[] getReflistValue()
	{
		checkValue(C.TYPE_REFLIST);
		return reflist_value;
	}
	
	private void checkValue(int checktype)
	{
		if(this.type != checktype)
			throw new RuntimeException("ERROR: Tried to get value of a " + this.type + " type object as type " + checktype + ".");
	}
}
