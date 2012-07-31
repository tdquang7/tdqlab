/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import java.util.List;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.mapreduce.Reducer;

/**
 *
 * @author tdquang
 */
public class TriangleCounting {
    public static class MapPhase1 extends Mapper<Text, NodeInfo, Text, NodeInfo>
    {       
        @Override
	protected void map (Text key, NodeInfo value, Context context) throws IOException, InterruptedException {
            
	}
    }
    
    public static class ReducePhase1 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {
            
	}
    }    
       
    //--------------------------------------------------------------------------
    // In phase two, try to 
    public static class MapPhase2 extends Mapper<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void map (Text key, NodeInfo value, Context context) throws IOException, InterruptedException {
            
        }
    }
    
    public static class ReducePhase2 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {
            
        }
    }
    
    //--------------------------------------------------------------------------

    public static void main(String[] args) {
        // TODO code application logic here
    }
}
