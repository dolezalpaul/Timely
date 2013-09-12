using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace System.Web.Optimization
{
    public class HandlebarsBundle : Bundle
    {
        public HandlebarsBundle(string virtualPath)
            : base(virtualPath)
        {
            this.Transforms.Add(new HandlebarsTransform());
        }
    }
}