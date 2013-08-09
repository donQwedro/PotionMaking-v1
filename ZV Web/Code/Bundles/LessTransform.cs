using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Optimization;
using dotless.Core;
using dotless.Core.Abstractions;
using dotless.Core.Importers;
using dotless.Core.Input;
using dotless.Core.Loggers;
using dotless.Core.Parser;

namespace Fixxbook.fixxbookPlus.Code.Bundles
{
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();

            var lessParser = new Parser();
            ILessEngine lessEngine = CreateLessEngine(lessParser);

            foreach (FileInfo fileInfo in response.Files)
            {
                SetCurrentFilePath(lessParser, fileInfo.FullName);

                var source = File.ReadAllText(fileInfo.FullName);

                var result = lessEngine.TransformToCss(source, fileInfo.FullName);

                // NOTE: HttpContext.Items is used instead of BundleResponse.Content
                // to pass processed files
                context.HttpContext.Items.Add(fileInfo.FullName, result);

                AddFileDependencies(lessParser);
            }
        }

        /// <summary>
        /// Creates an instance of LESS engine.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private ILessEngine CreateLessEngine(Parser lessParser)
        {
            var logger = new AspNetTraceLogger(LogLevel.Debug, new Http());
            return new LessEngine(lessParser, logger, false, false);
        }

        /// <summary>
        /// Adds imported files to the collection of files on which the current response is dependent.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private void AddFileDependencies(Parser lessParser)
        {
            IPathResolver pathResolver = GetPathResolver(lessParser);

            foreach (string importedFilePath in lessParser.Importer.Imports)
            {
                string fullPath = pathResolver.GetFullPath(importedFilePath);
                HttpContext.Current.Response.AddFileDependency(fullPath);
            }

            lessParser.Importer.Imports.Clear();
        }

        /// <summary>
        /// Returns an <see cref="IPathResolver"/> instance used by the specified LESS lessParser.
        /// </summary>
        /// <param name="lessParser">The LESS prser.</param>
        private IPathResolver GetPathResolver(Parser lessParser)
        {
            var importer = lessParser.Importer as Importer;
            if (importer != null)
            {
                var fileReader = importer.FileReader as FileReader;
                if (fileReader != null)
                {
                    return fileReader.PathResolver;
                }
            }

            return null;
        }

        /// <summary>
        /// Informs the LESS parser about the path to the currently processed file. 
        /// This is done by using custom <see cref="IPathResolver"/> implementation.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <param name="currentFilePath">The path to the currently processed file.</param>
        private void SetCurrentFilePath(Parser lessParser, string currentFilePath)
        {
            var importer = lessParser.Importer as Importer;
            if (importer == null)
                throw new InvalidOperationException("Unexpected importer type on dotless parser");

            var fileReader = importer.FileReader as FileReader;
            if (fileReader == null)
                importer.FileReader = fileReader = new FileReader();

            var pathResolver = fileReader.PathResolver as ImportedFilePathResolver;

            if (pathResolver != null)
            {
                pathResolver.CurrentFilePath = currentFilePath;
            }
            else
            {
                fileReader.PathResolver = new ImportedFilePathResolver(currentFilePath);
            }
        }
    }
}