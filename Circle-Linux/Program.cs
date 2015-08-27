using Lines;
using System;

namespace Lines.Linux {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            using (var game = new LinesGame(false))
                game.Run();
        }
    }
#endif
}
