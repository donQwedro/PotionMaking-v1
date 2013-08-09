using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Fixxbook.fixxbookPlus.Code.Bundles
{
    public class StyleImagePathBundle : Bundle
    {
        public StyleImagePathBundle(string virtualPath)
            : base(virtualPath)
        {
            // In that order
            base.Transforms.Add(new LessTransform());
            base.Transforms.Add(new StyleRelativePathTransform());
            base.Transforms.Add(new CssMinify());
        }
    }
}