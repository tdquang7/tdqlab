/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package pagerank;

import java.io.IOException;
import java.util.List;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FSDataInputStream;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapred.JobConf;
import org.apache.hadoop.mapreduce.InputSplit;
import org.apache.hadoop.mapreduce.RecordReader;
import org.apache.hadoop.mapreduce.TaskAttemptContext;
import org.apache.hadoop.mapreduce.lib.input.FileSplit;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;
import Utils.*;
import java.util.ArrayList;

/**
 *
 * @author tdquang
 */
public class GraphNodeRecordReader extends RecordReader<Text, GraphNodeInfo>{
    private int count = 0; 
    private FileSystem fs = null;
    private long fileSize;
    private FSDataInputStream input;
    private Boolean isWarc = false; // TODO, suwar laij thanh false
    
    private Text currentKey = new Text("Initial");
    private GraphNodeInfo currentValue = null;
            
    public GraphNodeRecordReader(Configuration conf, FileSplit split) throws IOException {
        this.fs = FileSystem.get(conf);
        
        Path path = split.getPath();
        fileSize = fs.getFileStatus(path).getLen();
        input = fs.open(path);
                
        isWarc = path.getName().endsWith("warc");
                    
        // Skip some header information until meet the record
        // Currently deleted to simplyfy
//            Boolean stillInHeader = true;
//            do{
//                String line = input.readLine();                
//                if (line.contains("WARC-Type") && line.contains("response"))
//                    stillInHeader = false;
//            }
//            while(stillInHeader);
            
    }

    @Override
    public boolean nextKeyValue() throws IOException, InterruptedException {
        final String SEPARATOR = ": ";
        final String CONTENT_LENGTH = "Content-Length";
        final String URL = "WARC-Target-URI";
        
        currentValue = new GraphNodeInfo(false);
       
        Boolean foundRecord = false;
        
        if(isWarc){
            Boolean meetContentLength = false;
            
            while(!meetContentLength && 
                 (input.getPos() < fileSize))
            {
                String line = input.readLine();
                
                if(line.contains(CONTENT_LENGTH)){
                    meetContentLength = true; // Doesn't need to find out length here                   
                }
                else if (line.contains(URL)){ // Get the key - the link
                    String[] parts = line.split(SEPARATOR); // Should be "url: http"

                    currentKey = new Text(parts[1].trim());
                }                    
            }            
            
            if (meetContentLength) { // Already get the key from previous paragraph
                meetContentLength = false; // Reset for the following paragraph 
                
                // Get the length of the HTML below            
                currentValue = new GraphNodeInfo(GraphNodeInfo.GRAPH_STRUCTURE);
            
                while(!meetContentLength &&
                        (input.getPos() < fileSize))
                {                
                    String line = input.readLine();
                    int length;

                    if(line.contains(CONTENT_LENGTH)){
                        meetContentLength = true; 

                        String[] parts = line.split(SEPARATOR);
                        length = Integer.parseInt(parts[1]);

                        input.readLine(); // Eat the empty line

                        // Read the HTML below
                        byte[] buffer = new byte[length];
                        input.read(buffer, 0, length);                    
                        String html = new String(buffer);

                        Document doc = Jsoup.parse(html); 

                        Elements links = doc.getElementsByTag("a"); // Get all the a href link

                        // Add all link to output value
                        String folder = StringHelper.getFolder(currentKey.toString());
                        
                        for(Element e: links){
                            String link = e.attr("href");
                            link = preProcessLink(link); // Remove weird input link
                            
                            if (link.length() != 0){                                
                                currentValue.getOutUrls().add(StringHelper.combineFolderAndLink(folder, link));
                            }
                        }
                        
                        foundRecord = true;
                    }
                }            
            }
            // TODO: Not sure if we skip 2 LF or just let the iteration read through lines again!!
        } // end if
        else{// if (fileSize > 0){ // Ensure not read the file _SUCCESS with 0 byte            
            if (input.getPos() < fileSize){            
                currentKey = new Text(input.readLine());
                
                // Get the old pagerank
                float rank = Float.parseFloat(input.readLine());
                currentValue.setRank(rank);

                // Get all the outlinks
                int size = Integer.parseInt(input.readLine()); // number of outlinks

                for(int i = 0; i < size; i++){
                    currentValue.getOutUrls().add(input.readLine());
                }            
                
                foundRecord = true; 
                
                // Eat the two empty lines that seperates two records
                if (input.getPos() < fileSize) input.readLine();
                if (input.getPos() < fileSize) input.readLine();                             
            } // end if
        } // end else

        return foundRecord;
    }

    private String preProcessLink(String link){
        String newLink = "";
        if(!StringHelper.isEmail(link) && 
           !StringHelper.isJavascript(link) &&
           !StringHelper.isLocalBookmark(link) &&
           !StringHelper.isSkypeContact(link) &&
           StringHelper.hasEnoughLength(link))
        {
            newLink = StringHelper.removeParametersFromLink(link);
            
        }
        
        return newLink;
    }
    
    @Override
    public Text getCurrentKey() throws IOException, InterruptedException {
        return currentKey;
    }

    @Override
    public GraphNodeInfo getCurrentValue() throws IOException, InterruptedException {
        return currentValue;
    }
    
    @Override
    public float getProgress() throws IOException {
        return count;
    }

    @Override
    public void initialize(InputSplit is, TaskAttemptContext tac) throws IOException, InterruptedException {
        
    }
    
    @Override
    public void close() throws IOException {
        
    }
}
