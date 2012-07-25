/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pagerank;

import java.io.DataOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.Writer;
import java.util.List;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.RecordWriter;
import org.apache.hadoop.mapreduce.TaskAttemptContext;

/**
 *
 * @author tdquang
 */
public class GraphNodeRecordWriter extends RecordWriter<Text, GraphNodeInfo>{
    private Writer writer;
    
    public GraphNodeRecordWriter(DataOutputStream output) throws IOException {
        this.writer = new OutputStreamWriter(output, "UTF-8");    
    }        
    
    @Override
    public synchronized void write(Text key, GraphNodeInfo value) throws IOException { 
        final String LINEBREAK = "\n";
        
        writer.write(key.toString() + LINEBREAK);     
        writer.write(Float.toString(value.getRank()) + LINEBREAK);
        
        List<String> urls = value.getOutUrls();
        writer.write(Integer.toString(urls.size()));
        writer.write(LINEBREAK);
        
        for(String link: urls){
            writer.write(link + LINEBREAK);
        }
        
        writer.write(LINEBREAK);
        writer.write(LINEBREAK);
    }

    @Override
    public void close(TaskAttemptContext tac) throws IOException, InterruptedException {
        writer.flush();
        writer.close();
    }
}
