package com.onemoredev.timezones;

public class ConvertedPerson
{
    private Person person;
    private String convertedTime;
    
    public ConvertedPerson(Person person, String convertedTime)
    {
        this.person = person;
        this.convertedTime = convertedTime;
    }
    
    public Person getPerson() { return person; }
    public String getConvertedTime() { return convertedTime; }
}
