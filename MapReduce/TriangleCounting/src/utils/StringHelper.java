/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package utils;

/**
 *
 * @author tdquang
 */
public class StringHelper {
    // Get value from format "Value: SomeDescriptions" or any seperator other than :
    public static String GetValue1(String line, String seperator)
    {
        String value = "";
        
        String[] parts = line.split(seperator);
        value = parts[0];        
        
        return value;
    }
    
    // Get value from format "Key: Value" or any seperator other than :
    public static String GetValue2(String line, String seperator)
    {
        String value = "";
        
        String[] parts = line.split(seperator);
        
        if (parts.length > 1) 
            value = parts[1];        
        
        return value;
    }
    
    public static KeyValueSplitResult SplitToKeyValue(String line, String seperator)
    {
        KeyValueSplitResult result = new KeyValueSplitResult();
        
        String[] parts = line.split(seperator);
        
        if (parts.length == 2)
        {            
            result.Key = parts[0];
            result.Value = parts[1];
        }
        
        return result;        
    }
}
