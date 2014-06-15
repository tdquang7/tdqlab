using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Net;

namespace ContosoCookbook.Common
{
    class CookbookUriMapper : UriMapperBase
    {
        private static string TargetPageName = "RecipeDetailPage.xaml";
        private static string ProtocolTemplate = "/Protocol?encodedLaunchUri=";
        private static int ProtocolTemplateLength = ProtocolTemplate.Length;
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = uri.ToString();

            if (tempUri.Contains("/Protocol"))
            {
                tempUri = HttpUtility.UrlDecode(tempUri);

                if (tempUri.Contains("recipe:"))
                {
                    return GetMappedUri(tempUri);
                }
            }
            return uri;
        }

        private Uri GetMappedUri(string uri)
        {
            string operation = "";
            string groupUID = "";

            // Extract parameter values from URI.
            if (uri.IndexOf(ProtocolTemplate) > -1)
            {
                int operationLen = uri.IndexOf("?", ProtocolTemplateLength);
                int groupIdLen = uri.IndexOf("=", operationLen + 1);

                operation = uri.Substring(ProtocolTemplateLength, operationLen - ProtocolTemplateLength);
                groupUID = uri.Substring(groupIdLen + 1);
            }

            string NewURI = String.Format("/{0}?ID={1}", TargetPageName, groupUID);

            return new Uri(NewURI, UriKind.Relative);
        }    
    }
}
