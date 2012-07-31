/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.DataOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.Writer;
import java.text.SimpleDateFormat;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.RecordWriter;
import org.apache.hadoop.mapreduce.TaskAttemptContext;

/**
 *
 * @author tdquang
 */
public class TriangleRecordWriter extends RecordWriter<Text, NodeInfo>
{
    private Writer _writer;
    
    public TriangleRecordWriter(DataOutputStream output) throws IOException {
        this._writer = new OutputStreamWriter(output, "UTF-8");    
    }  
    
    @Override
    public void write(Text key, NodeInfo node) throws IOException, InterruptedException {
        final String LINEBREAK = "\n";
        final String DATE_PATTERN = "d/m/yyyy";
        final String BLANK = " ";
        
        _writer.write("Key: " + key + LINEBREAK);
        _writer.write("ID: " + node.ID + LINEBREAK);
        _writer.write("Triangles count: " + node.TriangleCount + LINEBREAK);
        _writer.write("Name: " + node.Name + LINEBREAK);
        _writer.write("Email: " + node.Email + LINEBREAK) ;
        
        SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
        _writer.write("Birthday: " + sdf.format(node.Birthday) + LINEBREAK);
        
        _writer.write(node.Friends.size() + " friends" + LINEBREAK);
        for(String friendID: node.Friends)
        {
            _writer.write(friendID + BLANK);
        }
        
        _writer.write(LINEBREAK + LINEBREAK);
    }

    @Override
    public void close(TaskAttemptContext tac) throws IOException, InterruptedException {
        _writer.close();
    }
    
}
