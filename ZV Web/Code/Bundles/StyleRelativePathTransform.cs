using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Fixxbook.fixxbookPlus.Code.Bundles
{
    public class StyleRelativePathTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = string.Empty;

            var builder = new StringBuilder();
            Regex pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
            // open each of the files
            foreach (FileInfo fileInfo in response.Files)
            {
                if (fileInfo.Exists)
                {
                    // Get file content processed by LessTransform
                    // from HttpContext.Items or fallback to file read.
                    var contents = context.HttpContext.Items.Contains(fileInfo.FullName)
                        ? context.HttpContext.Items[fileInfo.FullName] as string
                        : File.ReadAllText(fileInfo.FullName);

                    MatchCollection matches = pattern.Matches(contents);
                    // Ignore the file if no match 
                    if (matches.Count > 0)
                    {
                        string cssFilePath = fileInfo.DirectoryName;
                        string cssVirtualPath = context.HttpContext.RelativeFromAbsolutePath(cssFilePath);
                        foreach (Match match in matches)
                        {
                            // this is a path that is relative to the CSS file
                            string relativeToCSS = match.Groups[2].Value;
                            string queryString = "";
                            int queryStart = relativeToCSS.IndexOf('?');
                            if (queryStart > 0)
                            {
                                queryString = relativeToCSS.Substring(queryStart);
                                relativeToCSS = relativeToCSS.Substring(0, queryStart);
                            }

                            // combine the relative path to the cssAbsolute
                            string absoluteToUrl = Path.GetFullPath(Path.Combine(cssFilePath, relativeToCSS)) + queryString;

                            // make this server relative
                            string serverRelativeUrl = context.HttpContext.RelativeFromAbsolutePath(absoluteToUrl);

                            string quote = match.Groups[1].Value;
                            string replace = string.Format("url({0}{1}{0})", quote, serverRelativeUrl);
                            contents = contents.Replace(match.Groups[0].Value, replace);
                        }
                    }

                    // copy the result into the response.
                    builder.AppendLine(contents);
                }
            }

            response.Content = builder.ToString();
            response.ContentType = "text/css";
        }
    }

    static class Ext
    {
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}