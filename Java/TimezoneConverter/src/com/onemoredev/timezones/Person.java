package com.onemoredev.timezones;

public class Person
{
    private String name;
    private String timeZone;

    public Person(String name, String timeZone)
    {
        this.name = name;
        this.timeZone = timeZone;
    }
    
    public String getName() { return name; }
    public String getTimeZone() { return timeZone; }
}
