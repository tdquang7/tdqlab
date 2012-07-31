/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import java.util.ArrayList;
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
            context.write(key, value); // Write back the structure
            
            for(String friend: value.Friends)
            {
                NodeInfo newValue = new NodeInfo();
                newValue.Type = NodeInfo.NEIGHBORINFO_REQUEST;
                newValue.ID = key.toString();
                newValue.Degree = value.Friends.size(); // Used for heavy edges matching
                
                context.write(new Text(friend), newValue);
            }
	}
    }
    
    public static class ReducePhase1 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {
            // Find the structure to get the degree of key
            int keyDegree = 0;
            
            for(NodeInfo ni: values)
            {
                if (NodeInfo.STRUCTURE == ni.Type)
                {
                    keyDegree = ni.Friends.size();
                    context.write(key, ni); // Release the structure of the graph
                    
                    break;
                }
            }
            
            // Keep the list of lower degree for open triad
            List<String> apex = new ArrayList();
            List<Integer> degrees = new ArrayList();
            
            // Generate candidate from higher degree node
            for(NodeInfo ni: values)
            {
                if (NodeInfo.NEIGHBORINFO_REQUEST == ni.Type)
                {
                    if (ni.Degree > keyDegree)
                    {
                        // Create candidate for closing open triad
                        NodeInfo newValue = new NodeInfo();
                        newValue.Type = NodeInfo.CANDIDATE;
                        context.write(new Text(ni.ID + "," + key), newValue);
                    }
                    else
                    {
                        apex.add(ni.ID);
                        degrees.add(ni.Degree);
                    }
                }
            }
            
            // Sorting descending based on degree
            int length = apex.size();
            for(int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (degrees.get(i) < degrees.get(j))
                    {
                        // Do the permutation
                        String temp = apex.get(i);
                        apex.set(i, apex.get(j)) ;
                        apex.set(j, temp);
                        
                        Integer num = degrees.get(i);
                        degrees.set(i, degrees.get(j));
                        degrees.set(j, num);
                    }
                }
            }
            
            // Create open triads
            for(int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    NodeInfo newValue = new NodeInfo();
                    newValue.Type = NodeInfo.OPENTRIAD;
                    newValue.ID = key.toString();
                            
                    context.write(new Text(apex.get(i) + "," + apex.get(j)), newValue);
                }
            }
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
