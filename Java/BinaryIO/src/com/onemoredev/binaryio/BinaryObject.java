package com.onemoredev.binaryio;

import java.util.ArrayList;

/**
 * A class to hold the data for a single object. (e.g. chair)
 * @author Adam Gaskins (Entity)
 */
public class BinaryObject
{
	private int id;
	private ArrayList<BinaryField> fields;
	
	public BinaryObject(int id)
	{
		this.id = id;
		fields = new ArrayList<BinaryField>();
	}
	
	public void addField(BinaryField field)
	{
		fields.add(field);
	}
	
	public ArrayList<BinaryField> getFields()
	{
		return fields;
	}
	
	public int getId()
	{
		return id;
	}
}
