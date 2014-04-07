package com.onemoredev.binaryio;

import java.io.IOException;
import java.io.InputStream;

/**
 * A class to act as a convenient wrapper around an input stream.
 * @author Adam Gaskins (Entity)
 */
public class CustomInputStream
{
    private InputStream stream;
    public CustomInputStream(InputStream stream)
    {
        this.stream = stream;
    }
    
    /**
     * Reads a byte of data from the stream.
     * @return
     * @throws IOException 
     */
    public int readByte() throws IOException
    {
        return stream.read();
    }
    
    /**
     * Reads a signed 16 bit short integer from the stream.
     * @param littleEndian If true, little endian. If false, big endian.
     * @return A 16 bit short integer from the stream.
     * @throws IOException 
     */
    public int readShort(boolean littleEndian) throws IOException
    {
        if(littleEndian)
            return (stream.read()) | (stream.read() << 8) | (stream.read() << 16) | (stream.read() << 24);
        else
            return (stream.read() << 24) | (stream.read() << 16) | (stream.read() << 8) | (stream.read());
    }
}
