package com.onemoredev.timezones;

import java.util.ArrayList;

import javax.swing.table.AbstractTableModel;

public class PersonTableModel extends AbstractTableModel
{
    private ArrayList<ConvertedPerson> persons;
    private String[] columns;
    public PersonTableModel(ArrayList<ConvertedPerson> persons)
    {
        this.persons = persons;
        this.columns = new String[] {"User", "Timezone", "Converted Time"};
    }
    
    @Override
    public int getColumnCount()
    {
        return 3;
    }

    @Override
    public int getRowCount()
    {
        return persons.size();
    }

    @Override
    public boolean isCellEditable(int y, int x)
    {
        return false;
    }
    
    @Override
    public Object getValueAt(int y, int x)
    {
        if(x == 0) return persons.get(y).getPerson().getName();            
        if(x == 1) return persons.get(y).getPerson().getTimeZone();            
        if(x == 2) return persons.get(y).getConvertedTime();
        
        return null;
    }

    @Override
    public String getColumnName(int column)
    {
        if(column < columns.length)
            return columns[column];
        return super.getColumnName(column);
    }
}
