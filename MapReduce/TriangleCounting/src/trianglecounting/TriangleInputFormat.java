/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.JobContext;
import org.apache.hadoop.mapreduce.RecordReader;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;

/**
 *
 * @author tdquang
 */
public class TriangleInputFormat extends FileInputFormat<Text, NodeInfo>{    
    @Override
    protected boolean isSplitable(JobContext context,Path filename)
    {
        return true;
    }
            
    @Override
    public RecordReader<Text, NodeInfo> createRecordReader(InputSplit is, TaskAttemptContext tac) throws IOException, InterruptedException 
    { 
        return new TriangleRecordReader(tac.getConfiguration(), (FileSplit) is);
    }
}
