using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PalMarket.Common.Helpers
{
    /// <summary>
    /// Helper class for application configuration
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// Gets the documents folder path
        /// </summary>
        /// <returns>Folder path</returns>
        public static string GetDocumentsFolderPath()
        {
            string path = ConfigurationManager.AppSettings["documentsFolderPath"];
            if (System.IO.Path.IsPathRooted(path))
            {
                return path;
            }
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
