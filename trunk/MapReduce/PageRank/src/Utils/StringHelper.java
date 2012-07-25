/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Utils;

/**
 *
 * @author tdquang
 */
public class StringHelper 
{   
    public static Boolean isEmail(String s){
        return s.contains("@");
    }
    
    // Check the length of string is larger than 6
    public static Boolean hasEnoughLength(String s){
        return s.length() > 6;
    }
    
    public static Boolean isJavascript(String s)
    {
        return s.contains("javascript");
    }
    
    public static Boolean isLocalBookmark(String s)
    {
        return s.contains("#");
    }
    
    // Remove input type: "abc.html?abc=123&def=567
    public static String removeParametersFromLink(String link)
    {
        String newLink = "";
        
        int pos = link.indexOf("?");
        
        if(pos >= 0)
        {
            newLink = link.substring(0, pos);
        }
        else
            newLink = link;
        
        return newLink;
    }
    
    public static Boolean isSkypeContact(String s){
        return s.contains("skype:");
    }
    
    public static Boolean isFullLink(String link)
    {
        return link.startsWith("http://");
    }
    
    // Objective: If got http://abc.com/Test/link.html, remove /link.html
    public static String getFolder(String link)
    {
        int slashPos = link.lastIndexOf("/");
        int dotPos = link.lastIndexOf(".");
        
        // Normally we got last dot behind last slash
        if (dotPos > slashPos)
            return link.substring(0, slashPos);
        else {// It is already a folder
            if (slashPos == (link.length() - 1)) // Remove the end slash if any
            {
                return link.substring(0, slashPos);
            }
            else
                return link; // as is
        }
            
    }
    
    public static String combineFolderAndLink(String folder, String link)
    {
        if (link.startsWith("http"))
            return link;        
        
        String newLink = folder;
        
        if(link.startsWith("/")) 
        {
            int pos = folder.indexOf("//");
            
            if (pos >= 0){
                int slashPos = folder.indexOf("/", pos + 2);

                if (slashPos >= 0) http://www.abc.com/Test
                {
                    // Only keep the root
                    newLink = folder.substring(0, slashPos);
                }
            }
        }
        else
        {
            newLink += "/";
        }
        
        newLink += link;
        
        return newLink;
    }
}
