using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pong {
    class Program {

        static void Main(string[] args) {
            Controls controls = new Controls(ConsoleKey.W, ConsoleKey.S, ConsoleKey.A, ConsoleKey.D);

            controls.KeyDown(delegate(ConsoleKey key) {
                Console.WriteLine(key.ToString() + " down");
            });

            controls.KeyUp(delegate(ConsoleKey key) {
                Console.WriteLine(key.ToString() + " up");
            });
        }

    }
}
