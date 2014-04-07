package com.onemoredev.timezones;

import java.awt.BorderLayout;
import java.awt.Container;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.TimeZone;

import javax.swing.JButton;
import javax.swing.JCheckBox;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTable;
import javax.swing.JTextField;

public class TimezoneConverter extends JFrame implements Runnable, ActionListener
{
    // This is just to quiet the Eclipse warning
    private static final long serialVersionUID = -817960053444207213L;

    private ArrayList<Person> timezones;
    
    // Elements
    private JPanel controlElements = new JPanel();
    private JLabel inputCurrentTimezone = new JLabel("You: ");
    private JComboBox currentTimezone;
    private JLabel inputTimeLabel = new JLabel("Time: ");
    private JTextField inputTime = new JTextField(10);
    private JCheckBox inputTimeNow = new JCheckBox("Now", true);
    private JButton calculate = new JButton("Convert");
    
    private JTable results = new JTable();
    
    public TimezoneConverter(ArrayList<Person> timezones)
    {
        setTitle("OneMoreDev Timezone Converter");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setSize(500, 300);
        
        this.timezones = timezones;
        
        String[] names = new String[timezones.size()];
        for(int i = 0; i < timezones.size(); i++)
        {
            names[i] = timezones.get(i).getName();
        }
        currentTimezone = new JComboBox(names);
        
        
        inputTime.setText("1:00 am");
        calculate.addActionListener(this);
        
        
        Container content = this.getContentPane();
        
        controlElements.add(inputCurrentTimezone);
        controlElements.add(currentTimezone);
        controlElements.add(inputTimeLabel);
        controlElements.add(inputTime);
        controlElements.add(inputTimeNow);
        controlElements.add(calculate);
               
        content.setLayout(new BorderLayout());
        content.add(controlElements, BorderLayout.PAGE_START);
        content.add(results, BorderLayout.CENTER);        
        
        setVisible(true);
        
        new Thread(this).start();
    }
    
    private void _recalculateTimes()
    {
        ArrayList<ConvertedPerson> convertedTimes = new ArrayList<ConvertedPerson>();
        long time = getInputTime();
        
        if(time == -1) 
        {
            JOptionPane.showMessageDialog(this, "Error: Invalid time format: \""+inputTime.getText()+"\".");
            return;
        }
        
        for(int i = 0; i < timezones.size(); i++)
        {
            Person timeZone = timezones.get(i);            
            DateFormat out = new SimpleDateFormat("hh:mm a");
            out.setTimeZone(TimeZone.getTimeZone(timeZone.getTimeZone()));
            
/*            int hour     = convertedTime.get(Calendar.HOUR);
            int minute   = convertedTime.get(Calendar.MINUTE);
            int second   = convertedTime.get(Calendar.SECOND);
            String ampm  = convertedTime.get(Calendar.AM_PM) == Calendar.AM ? "am" : "pm";*/

            String dateString = out.format(new Date(time));
            //String dateString = new SimpleDateFormat("hh:mm:ss a").format(Date);
            //String dateString = hour + ":" + minute + ":" + second + " " + ampm;
            
            convertedTimes.add(new ConvertedPerson(timeZone, dateString));
        }
        
        results.setModel(new PersonTableModel(convertedTimes));
    }
    
    private Date parseDateString(TimeZone timeZone, String inputString, String[] dateStrings) throws Exception
    {
        for(int i = 0; i < dateStrings.length; i++)
        {
            try
            {
                SimpleDateFormat sdf = new SimpleDateFormat(dateStrings[i]);
                sdf.setTimeZone(timeZone);
                Date date = sdf.parse(inputString);
                // success
                return date;
            }
            catch(Exception e)
            {
                // fail so try the next one
            }
        }
        throw new Exception("Invalid input format.");
    }
    
    private long getInputTime()
    {        
        String inputString = inputTime.getText();
     
        TimeZone currentTimeZone = TimeZone.getTimeZone(timezones.get(currentTimezone.getSelectedIndex()).getTimeZone());
        Date date = null;
        try
        {
            date = parseDateString(currentTimeZone, inputString, new String[] {"h:m:s a", "h:m a", "H:m:s", "H:m"});
        }
        catch(Exception e)
        {
            return -1;
        }
    
        return date.getTime();
    }
    
    public void run()
    {
        while(true)
        {
            if(inputTimeNow.isSelected())
            {
                String now = new SimpleDateFormat("hh:mm:ss a").format(new Date());
                inputTime.setText(now);
            }
            
            try {
                Thread.sleep(1000);
            } catch (Exception e) {}
        }
    }
    
    public static void main(String[] args) throws IOException
    {
        ArrayList<Person> timezones = new ArrayList<Person>();
        
        File f = new File("timezones.dat");
        if(!f.exists())
        {
            JOptionPane.showMessageDialog(null, "No timezones.dat file found.");            
            return;            
        }
        
        BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(f)));
        while(true)
        {
            String line = br.readLine();
            if(line == null) break;
            if(line.startsWith("//")) continue;
            String parts[] = line.split(":", 2);
            
            if(parts.length > 1)            
                timezones.add(new Person(parts[0], parts[1]));
        }
        
        Collections.sort(timezones, new PersonComparator());
        
        new TimezoneConverter(timezones);
    }

    @Override
    public void actionPerformed(ActionEvent arg0)
    {
        if(arg0.getSource() == calculate)
        {
            _recalculateTimes();
        }
    }
}
