/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FSDataInputStream;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.RecordReader;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;
import utils.*;

/**
 *
 * @author tdquang
 */
public class TriangleRecordReader extends RecordReader<Text, NodeInfo>{
    FileSplit _split;
    FSDataInputStream _in;
    Text _key;
    NodeInfo _value;
    long _fileSize;
    
    public TriangleRecordReader(Configuration conf, FileSplit split) throws IOException {
        _split = split;
        _key = new Text("");
        _value = new NodeInfo();
    }

    // Things to consider: inputsplit, why don't we read from inputsplit but 
    // from the FileSplit?
    @Override
    public void initialize(InputSplit is, TaskAttemptContext tac) throws IOException, InterruptedException {
        // Maybe need to get total number of nodes for progress???
        
        Path path = _split.getPath();
        Configuration conf = tac.getConfiguration(); 
        FileSystem fs = path.getFileSystem(conf);
        _fileSize = fs.getFileStatus(path).getLen();
        _in = fs.open(path);
    }

    @Override
    public boolean nextKeyValue() throws IOException, InterruptedException {
        if (_in.getPos() < _fileSize) // Still can read
        {
            final String COLON = ": ";
            final String BLANK = " ";
            String DATE_PATTERN = "d/m/yyyy";
            
            String ID = StringHelper.GetValue2(_in.readLine(), COLON);
            String Name = StringHelper.GetValue2(_in.readLine(), COLON);
            String Email = StringHelper.GetValue2(_in.readLine(), COLON);

            //* Get the birthday
            Date Birthday;
            
            SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
            try
            {
                Birthday = sdf.parse(StringHelper.GetValue2(_in.readLine(), COLON));
            }
            catch (ParseException ex)
            {                
            }
            
            //* Get the list of friends
            String value = StringHelper.GetValue1(_in.readLine(), BLANK);
            int friendsCount = Integer.parseInt(value);
            String line = _in.readLine();
            String[] parts = line.split(BLANK);
            List<String> friends = new ArrayList();
            
            for(int i = 0; i < parts.length; i++)
            {
                friends.add(parts[i]);
            }
            
            
        }
        
        return true;
    }

    @Override
    public Text getCurrentKey() throws IOException, InterruptedException {
        return _key;
    }

    @Override
    public NodeInfo getCurrentValue() throws IOException, InterruptedException {
        return _value;
    }

    @Override
    public float getProgress() throws IOException, InterruptedException {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public void close() throws IOException {
        _in.close();
    }
}
