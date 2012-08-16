/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package trianglecounting;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.mapreduce.Reducer;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;

/**
 *
 * @author tdquang
 */
public class TriangleCounting {
    final static boolean DEBUG = true;
    
    public static class MapPhase1 extends Mapper<Text, NodeInfo, Text, NodeInfo>{       
        @Override
	protected void map (Text key, NodeInfo value, Context context) throws IOException, InterruptedException {
            value.Type = NodeInfo.STRUCTURE; // To be sure it is structure, seems redundant
            context.write(key, value); // Write back the structure
            
            // Create messages and send
            for(String friend: value.Friends)
            {
                NodeInfo newValue = new NodeInfo();
                newValue.Type = NodeInfo.NEIGHBORINFO_REQUEST;
                newValue.ID = key.toString();
                newValue.Degree = value.Friends.size(); // Used for heavy edges matching               
                
                context.write(new Text(friend), newValue);
            }
	} // void map()
    }
    
    public static class ReducePhase1 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {
            ArrayList<String> cacheID = new ArrayList();
            ArrayList<Integer> cacheDegree = new ArrayList();
            
            // Find the structure to get the degree of key
            int keyDegree = 0;
                    
            for(NodeInfo ni: values)
            {   
                if (NodeInfo.STRUCTURE == ni.Type)
                {
                    keyDegree = ni.Friends.size();
                    context.write(key, ni); // Release the structure of the graph
                } 
                else
                {
                    cacheID.add(ni.ID);
                    cacheDegree.add(ni.Degree);
                }
            }
            
            if (keyDegree > 1)
            {            
                // Keep the list of lower degree for open triad
                List<String> apex = new ArrayList();
                List<Integer> degrees = new ArrayList(); // Corresponding degrees of apex

                // Generate candidate from higher degree node
                for(int i = 0; i < cacheID.size(); i++)
                {
                    if (cacheDegree.get(i) > keyDegree)
                    {
                        // Create candidate for closing open triad
                        NodeInfo newValue = new NodeInfo();
                        newValue.Type = NodeInfo.CANDIDATE;
                        context.write(new Text(cacheID.get(i) + "," + key), newValue);                            
                    } // end if generate candidata

                    // Store apex for creating open triad
                    if (cacheDegree.get(i) < keyDegree)  
                    {
                        apex.add(cacheID.get(i));
                        degrees.add(cacheDegree.get(i));
                    } // end if storing apex for open triad

                    // Special case when equal
                    if (cacheDegree.get(i) == keyDegree)
                    {
                        if (cacheID.get(i).compareTo(key.toString()) > 0)
                        {
                            // Candidate
                            NodeInfo newValue = new NodeInfo();
                            newValue.Type = NodeInfo.CANDIDATE;
                            context.write(new Text(cacheID.get(i) + "," + key), newValue);                            
                        }
                        else 
                        {
                            // Apex for creating open triad
                            apex.add(cacheID.get(i));
                            degrees.add(cacheDegree.get(i));
                        }
                    }
                } // end for            

                // Sorting descending based on degree
                int length = apex.size();

                if (length > 1)
                {
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
                            } // end if 
                            
                            // Special handle when equal
                            if (degrees.get(i) == degrees.get(j))
                            {
                                if (apex.get(i).compareTo(apex.get(j)) < 0)
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
                        } // end for
                    } // end for

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
                    } // end for creating open triad
                } // end if (length of apex less than key > 1)
            } // end if (keyDegree > 1)
	} // end funtion reduce
    }    
       
    //--------------------------------------------------------------------------
    // In phase two, try to find closing edge for open triad
    public static class MapPhase2 extends Mapper<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void map (Text key, NodeInfo value, Context context) throws IOException, InterruptedException {
            // Just do the identity job
            context.write(key, value);
        }
    }
    
    public static class ReducePhase2 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {            
            int count = 0;
            String nodeID = ""; // The node that we should count the ID
            
            for(NodeInfo ni: values)            
            {
                count++;
                
                if (ni.Name.length() != 0) // structure
                    context.write(key, ni);
                
                if (ni.ID.length() != 0) // It is the open triad
                {
                    nodeID = ni.ID; 
                }
            }
            
            if (count >= 2) // Got the candidate and the open triad
            {
                // Found the triangle
                NodeInfo newValue = new NodeInfo();                
                context.write(new Text(nodeID), newValue);
            }
        }
    }
    //---------------------------------------------------------------------------
    
    // Phase three: update graph structure with number of triangles
    public static class MapPhase3 extends Mapper<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void map (Text key, NodeInfo value, Context context) throws IOException, InterruptedException {
            // Just do the identity job
            context.write(key, value);
        }
    }
    
    public static class ReducePhase3 extends Reducer<Text, NodeInfo, Text, NodeInfo>{
        @ Override
	protected void reduce (Text key, Iterable <NodeInfo> values, Context context) throws IOException, InterruptedException {
            NodeInfo structure = null;
            
            int count = 0; // Number of 
            
            for(NodeInfo ni: values)
            {                
                if (ni.ID.length() != 0) // Structure
                {
                    structure = ni;
                }
                else
                    count++;
            }   
            
            structure.TriangleCount = count;
            
            context.write(key, structure);
        }
    }
    //--------------------------------------------------------------------------

    public static void main(String[] args) throws Exception{
        // TODO code application logic here
        Path inPath = new Path(args[0]);
        Path outPath = new Path(args[1]);
        
        Configuration conf = new Configuration();
        Job job = new Job(conf, "Triangle counting phase 1 - Open triads");
        job.setJarByClass(TriangleCounting.class);
        job.setMapperClass(MapPhase1.class);
        job.setReducerClass(ReducePhase1.class);
                
        job.setInputFormatClass(TriangleInputFormat.class);
        job.setOutputFormatClass(TriangleOutputFormat.class);
        
        job.setOutputKeyClass(Text.class);
        job.setOutputValueClass(NodeInfo.class);
        
        FileInputFormat.addInputPath(job, inPath);
        FileOutputFormat.setOutputPath(job, outPath);
                
        job.waitForCompletion(true);
        // -----------------------------------------------------------------
        
        conf = new Configuration();        
        job = new Job(conf, "Triangle counting phase 2 - Find triangles");
        job.setJarByClass(TriangleCounting.class);
        job.setMapperClass(MapPhase2.class);
        job.setReducerClass(ReducePhase2.class);
        
        inPath = outPath;
        outPath = new Path(args[1] + "1");
        
        job.setInputFormatClass(TriangleInputFormat.class);
        job.setOutputFormatClass(TriangleOutputFormat.class);
        
        job.setOutputKeyClass(Text.class);
        job.setOutputValueClass(NodeInfo.class);
        
        FileInputFormat.addInputPath(job, inPath);
        FileOutputFormat.setOutputPath(job, outPath);
        
        job.waitForCompletion(true);        
        // ----------------------------------------------------------------
        
        job = new Job(conf, "Triangle counting phase 3 - Update structure");
        job.setJarByClass(TriangleCounting.class);
        job.setMapperClass(MapPhase3.class);
        job.setReducerClass(ReducePhase3.class);
        
        inPath = outPath;
        outPath = new Path(args[1] + "2");
        
        job.setInputFormatClass(TriangleInputFormat.class);
        job.setOutputFormatClass(TriangleOutputFormat.class);
        
        job.setOutputKeyClass(Text.class);
        job.setOutputValueClass(NodeInfo.class);
        
        FileInputFormat.addInputPath(job, inPath);
        FileOutputFormat.setOutputPath(job, outPath);
        
        job.waitForCompletion(true);        
    }
}
