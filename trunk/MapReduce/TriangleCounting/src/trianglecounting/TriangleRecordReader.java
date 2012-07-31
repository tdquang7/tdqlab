/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
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
            
            
            KeyValueSplitResult result = StringHelper.SplitToKeyValue(_in.readLine(), COLON);
            
            String id = result.Value;
            _key = new Text(result.Value);
            
            if(result.Key.contains("Key"))
            {
                // Read next line to get id
                result = StringHelper.SplitToKeyValue(_in.readLine(), COLON);
                id = result.Value;
            }            
            
            //* Check if contains triangle counting or not  
            int triangles = 0;
            String name = "";
            result = StringHelper.SplitToKeyValue(_in.readLine(), COLON);
            
            if (result.Key.contains("Triangle"))
            {
                triangles = Integer.parseInt(result.Value);
                
                // Read next line to get name
                result = StringHelper.SplitToKeyValue(_in.readLine(), COLON);
            }
            
            name = result.Value;
            
            String email = StringHelper.GetValue2(_in.readLine(), COLON);

            //* Get the birthday
            Date birthday = new Date();
            
            SimpleDateFormat sdf = new SimpleDateFormat(DATE_PATTERN);
            try
            {
                birthday = sdf.parse(StringHelper.GetValue2(_in.readLine(), COLON));
            }
            catch (ParseException ex)
            {                
            }
            
            //* Get the list of friends
            String value = StringHelper.GetValue1(_in.readLine(), BLANK);
            int friendsCount = Integer.parseInt(value); // Not use this number
            
            String line = _in.readLine(); 
            String[] parts = line.split(BLANK);
            List<String> friends = new ArrayList();
            friends.addAll(Arrays.asList(parts));
            
            _in.readLine(); // Eat empty line
            
            //* Turn info into key and value                     
            _value = new NodeInfo(id, triangles, name, email, birthday, friends);            
            
            return true;
        }
        
        return false;
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
