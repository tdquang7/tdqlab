/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pagerank;

import java.io.IOException;
import java.util.*;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.Path;

import org.apache.hadoop.io.*;
import org.apache.hadoop.mapred.OutputCollector;
import org.apache.hadoop.mapred.Reporter;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.mapreduce.Reducer;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.input.KeyValueTextInputFormat;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;


/**
 *
 * @author tdquang
 */
public class PageRank {    
    public static class MapPhase1 extends Mapper<Text, GraphNodeInfo, Text, GraphNodeInfo>
    {       
        @ Override
	protected void map (Text key, GraphNodeInfo value, Context context) throws IOException, InterruptedException {
            List<String> urls = value.getOutUrls();
            
            for(String link: urls){
                GraphNodeInfo newValue = new GraphNodeInfo(GraphNodeInfo.MESSAGE);
                
                newValue.AddLink(link);                
                context.write(key, newValue);
            }
	}
    }
    
    public static class ReducePhase1 extends Reducer<Text, GraphNodeInfo, Text, GraphNodeInfo>{
        @Override
	protected void reduce (Text key, Iterable <GraphNodeInfo> values, Context context) throws IOException, InterruptedException {
            GraphNodeInfo newValue = new GraphNodeInfo(GraphNodeInfo.GRAPH_STRUCTURE);
            
            for(GraphNodeInfo info: values){
                newValue.AddAllLinks(info);
            }
            
            newValue.setRank(1.0f);
            context.write(key, newValue);
	}
    }    
       
    //--------------------------------------------------------------------------
    
    public static class MapPhase2 extends Mapper<Text, GraphNodeInfo, Text, GraphNodeInfo>{
        @ Override
	protected void map (Text key, GraphNodeInfo value, Context context) throws IOException, InterruptedException {
            List<String> urls = value.getOutUrls();
            
            for(String link: urls){ // for each outlink
                GraphNodeInfo message = new GraphNodeInfo(GraphNodeInfo.MESSAGE);
                message.setRank(value.getRank());
                
                message.AddLink(key.toString());
                message.AddLink(Integer.toString(urls.size())); // Tricky, save number of outlinks
                
                context.write(new Text(link), message);
            }
            
            // Release the graph structure too
            value.setGraphStructure(); // Just to be sure
            context.write(key, value);
        }
    }
    
    public static class ReducePhase2 extends Reducer<Text, GraphNodeInfo, Text, GraphNodeInfo>{
        @Override
	protected void reduce (Text key, Iterable <GraphNodeInfo> values, Context context) throws IOException, InterruptedException {
            GraphNodeInfo value = new GraphNodeInfo(GraphNodeInfo.GRAPH_STRUCTURE); // Save new PR and list of urls. NOT OUT, but IN
            float newPageRank = 0;
            
            for(GraphNodeInfo node : values)
            {                 
                if (node.IsMessage())
                {
                    // Agrregate page rank
                    String link = node.getOutUrls().get(0); // Supported link
                    float contributeRank = node.getRank(); // Old rank
                    float outLinksCount = Float.parseFloat(node.getOutUrls().get(1)); // Tricky

                    newPageRank += contributeRank / outLinksCount;
                }
                else // It is the structure we emit through network  
                {                   
                    value.AddAllLinks(node); // Add back again the graph structure
                }
            }
                        
            value.setRank(value.getRank() + newPageRank);
            context.write(key, value);
        }
    }
    
    //--------------------------------------------------------------------------
    
    public static void main(String[] args) throws Exception{   
        // Phase 1: Parse HTML into records
        Path inPath = new Path(args[0]);
        Path outPath = new Path(args[1]);
        
        Configuration conf = new Configuration();
        Job job = new Job(conf, "PageRank");
        job.setJarByClass(PageRank.class);
        job.setMapperClass(MapPhase1.class);
        //job.setCombinerClass(ReducePhase1.class);
        job.setReducerClass(ReducePhase1.class);
                
        job.setInputFormatClass(GraphNodeInputFormat.class);
        job.setOutputFormatClass(GraphNodeOutputFormat.class);
        
        job.setOutputKeyClass(Text.class);
        job.setOutputValueClass(GraphNodeInfo.class);
        
        FileInputFormat.addInputPath(job, inPath);
        FileOutputFormat.setOutputPath(job, outPath);
                
        job.waitForCompletion(true);
        
        // Second phase, do iterations
        final int COUNT = 10;
        
        for(int i = 0; i < COUNT; i++){
            inPath = outPath;
            outPath = new Path(args[1] + Integer.toString(i));
            
            job = new Job(conf, "PageRank" + i);
            job.setJarByClass(PageRank.class);
            job.setMapperClass(MapPhase2.class);
            //job.setCombinerClass(ReducePhase2.class);
            job.setReducerClass(ReducePhase2.class);
            
            job.setInputFormatClass(GraphNodeInputFormat.class);
            job.setOutputFormatClass(GraphNodeOutputFormat.class);
            
            job.setOutputKeyClass(Text.class);
            job.setOutputValueClass(GraphNodeInfo.class);
        
            FileInputFormat.addInputPath(job, inPath);
            FileOutputFormat.setOutputPath(job, outPath);

            job.waitForCompletion(true);       
        }        
    }   
}