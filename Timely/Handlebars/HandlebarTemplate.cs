using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace System.Web.Optimization
{
    public class HandlebarTemplate
    {
        public string Name { get; private set; }
        public string Content { get; private set; }
        public string Hash { get; private set; }

        public HandlebarTemplate(string name, string content)
        {
            Name = name;
            Content = content;
            Hash = ComputeHash();
        }

        private string ComputeHash()
        {
            string result;
            using (SHA256 sHA = new SHA256Managed())
            {
                byte[] input2 = sHA.ComputeHash(Encoding.Unicode.GetBytes(Name + Content));
                result = Convert.ToBase64String(input2);
            }
            return result;
        }
    }
}