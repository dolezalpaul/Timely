using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Optimization
{
    public interface ITemplateName
    {
        string Transform(string relativePath);
    }
}
