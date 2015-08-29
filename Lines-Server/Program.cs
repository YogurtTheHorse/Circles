using Lines.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Server {
    public class Program {
        static void Main(string[] args) {
            LinesServer server = new LinesServer();
            server.Start((s) => { Console.WriteLine(s); });
        }
    }
}
