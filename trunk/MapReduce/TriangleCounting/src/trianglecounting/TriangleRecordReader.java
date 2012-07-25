/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FSDataInputStream;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.RecordReader;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;

/**
 *
 * @author tdquang
 */
public class TriangleRecordReader extends RecordReader<Text, NodeInfo>{
    FileSplit _split;
    FSDataInputStream _in;
    Text _key;
    NodeInfo _value;
    
    public TriangleRecordReader(Configuration conf, FileSplit split) throws IOException {
        _split = split;
        _key = new Text("");
        _value = new NodeInfo();
    }

    // Things to consider: inputsplit, why don't we read from inputsplit but from the file?
    @Override
    public void initialize(InputSplit is, TaskAttemptContext tac) throws IOException, InterruptedException {
        Path path = _split.getPath();
        Configuration conf = tac.getConfiguration(); 
        FileSystem fs = path.getFileSystem(conf);
        _in = fs.open(path);
    }

    @Override
    public boolean nextKeyValue() throws IOException, InterruptedException {
        
        
        return true;
    }

    @Override
    public Text getCurrentKey() throws IOException, InterruptedException {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public NodeInfo getCurrentValue() throws IOException, InterruptedException {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public float getProgress() throws IOException, InterruptedException {
        throw new UnsupportedOperationException("Not supported yet.");
    }

    @Override
    public void close() throws IOException {
        throw new UnsupportedOperationException("Not supported yet.");
    }
    
    
}
