/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import javax.xml.crypto.Data;
import org.apache.hadoop.io.WritableComparable;

/**
 *
 * @author tdquang
 */
public class NodeInfo implements WritableComparable {
    public static final int STRUCTURE = 0;
    public static final int NEIGHBORINFO_REQUEST = 1;
    public static final int CANDIDATE = 2;
    public static final int OPENTRIAD = 4;    
    
    public static final String DATE_PATTERN = "d/m/yyyy";
    
    //--------------------------------------------------
    public int Type; // Type of this message 
    
    // Main structure of graph
    public String ID;
    public String Name;
    public String Email;    
    public Date Birthday;
    
    public List<String> Friends;
            
    //-------------------------------------------------
    public int Degree; // Used in neighbor info request            
    
    @Override
    public void write(DataOutput out) throws IOException {
        out.write(Type);
        
        if(Type == STRUCTURE)
        {
            out.writeUTF(ID);
            out.writeUTF(Name);
            
            SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
            out.writeUTF(sdf.format(Birthday));
                                   
            out.write(Friends.size()); // Write out number of friend
            for(String friend: Friends)
            {
                out.writeUTF(friend);
            }
        } 
        else if (Type == NEIGHBORINFO_REQUEST)
        {
            out.write(Degree);
        }
        else if (Type == CANDIDATE)
        {            
        }
        else if(Type == OPENTRIAD)
        {            
        }
    }

    @Override
    public void readFields(DataInput in) throws IOException {
        Type = in.readInt();
        Email = in.readUTF();
        
        if(Type == STRUCTURE)
        {
            Name = in.readUTF();
                        
            SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
            try
            {
                Birthday = sdf.parse(in.readUTF());
            }
            catch (ParseException ex)
            {
                
            }
            
            int interestsCount = in.readInt();
            for(int i = 0; i < interestsCount; i++)
            {
                Interests.add(in.readUTF());
            }
            
            int friendsCount = in.readInt();
            for(int i = 0; i < friendsCount; i++)
            {
                Friends.add(in.readUTF());
            }            
        } 
        else if (Type == NEIGHBORINFO_REQUEST)
        {
            Degree = in.readInt();
        }
        else if (Type == TWOPATH)
        {            
        }
        else if(Type == CANDIDATE)
        {            
        }
    }

    @Override
    public int compareTo(Object o) {
        NodeInfo other = (NodeInfo) o;
        
        return this.Email.compareTo(other.Email);
    }
    
}
