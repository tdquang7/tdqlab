package pagerank;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.apache.hadoop.io.WritableComparable;
import sun.security.x509.IssuerAlternativeNameExtension;

/**
 *
 * @author tdquang
 */
public class GraphNodeInfo implements WritableComparable {
    public static final Boolean MESSAGE = true;
    public static final Boolean GRAPH_STRUCTURE = false;    
    
    private Boolean type; // A message to other node or graph structure
    private float rank;
    private List<String> out_urls;
    
    public GraphNodeInfo(){
        this.type = MESSAGE;
        rank = 0.0f;
        out_urls = new ArrayList<>();
    }
    
    public GraphNodeInfo(Boolean aType){
        this.type = aType;
        rank = 0.0f;
        out_urls = new ArrayList<>();
    }
    
    public Boolean IsMessage(){
        return type;
    }
            
    public void setMessage(){
        type = MESSAGE;
    }
    
    public void setGraphStructure(){
        type = GRAPH_STRUCTURE;
    }
    
    public void setRank(float value){
        rank = value;
    }

    public float getRank(){
        return rank;
    }
    
    public List<String> getOutUrls(){
        return out_urls;
    }    
    
    public void AddLink(String link){
        out_urls.add(link);
    }
    
    public void AddAllLinks(GraphNodeInfo node){
        for(String link: node.getOutUrls()){
            out_urls.add(link);
        }            
    }
       
    
    @Override
    public void write(DataOutput out) throws IOException {
        out.writeBoolean(type);
        out.writeFloat(rank);
        out.writeInt(out_urls.size()); // write out number of links
        
        for(String link: out_urls){
            out.writeUTF(link);
        }
    }

    @Override
    public void readFields(DataInput in) throws IOException {
        type = in.readBoolean();
        rank = in.readFloat();
        int count = in.readInt();
        out_urls = new ArrayList<>();
        
        for(int i = 0; i < count; i++){
            out_urls.add(in.readUTF());
        }
    }

    @Override
    public String toString() {
        String info = "";
        info += Float.toString(rank);
        
        for(String link: out_urls){
            info += ", " + link;
        }    
        
        return info;
    }
    
    @Override
    public int compareTo(Object other) {
        return Float.compare(this.rank, ((GraphNodeInfo) other).rank);
    }
    
    @Override
    public boolean equals(Object o) {
        if (!(o instanceof GraphNodeInfo)) {
            return false;
        }

        GraphNodeInfo other = (GraphNodeInfo)o;
        
    return this.rank == other.rank;
  }

    @Override
  public int hashCode() {
    return Float.floatToIntBits(rank);
  }
}
