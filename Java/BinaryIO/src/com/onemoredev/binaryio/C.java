package com.onemoredev.binaryio;

/**
 * Constants class.
 * @author Adam Gaskins (Entity)
 */
public class C
{
    public static final int BSTART = 0xC3;
    public static final int BEND = 0x3C;
    
    public static final boolean IS_LITTLE_ENDIAN = true;
    public static final boolean IS_BIG_ENDIAN = !IS_LITTLE_ENDIAN;
    
    // field type
    public static final int TYPE_STRING  = 0; 
    public static final int TYPE_INT     = 1; 
    public static final int TYPE_FLOAT   = 2; 
    public static final int TYPE_REFLIST = 3;    
}
