using Lines.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Server {
    public class Program {
        public static void Main(string[] args) {
            LinesServer server = new LinesServer();
            server.Start(log);
        }

        public static void log(string s) {
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss") + "# " + s);
        }
    }
}
