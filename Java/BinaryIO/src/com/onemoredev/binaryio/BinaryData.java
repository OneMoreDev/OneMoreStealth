package com.onemoredev.binaryio;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;

/**
 * A class to hold binary data.
 * @author Adam Gaskins (Entity)
 */
public class BinaryData
{
	private ArrayList<BinaryObject> objects; 
	
	public BinaryData()
	{
		
	}
	
	public void addObject(BinaryObject obj)
	{
		objects.add(obj);
	}
	
	public ArrayList<BinaryObject> getObjects()
	{
		return objects;
	}
	
	/**
	 * Parses and returns the BinaryData from a stream.
	 * @param inStream The stream to parse.
	 * @return Returns the data from the stream in a BinaryData object.
	 */
	public static BinaryData createFromStream(InputStream iStream)
	{
	    CustomInputStream cis = new CustomInputStream(iStream);
	    BinaryData binaryData = new BinaryData();
		try
		{
			while(true)
			{
				int b = cis.readByte();
				if(b == -1) break; // end of stream
							
				if(b != C.BSTART) throw new BinaryException("Invalid binary format.");
				
				BinaryObject binObj = new BinaryObject(cis.readShort(C.IS_LITTLE_ENDIAN)); // read ID
				
				// TODO: Talk to Kroltan about the specifics of the binary specification.
				// TODO: Finish parsing
			}
		}
		catch (IOException e)
		{
			e.printStackTrace();
		}
		return binaryData;
	}
	
	/**
	 * Writes the data contained in this object to a stream.
	 * @param outStream
	 */
	public void writeToStream(OutputStream outStream)
	{
		// TODO: Save data to stream
	}
}
