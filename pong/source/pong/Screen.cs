/*

Copyright (c) 2014, Benjamin Lorantfy
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS
BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong {
    class Screen {
        int width;
        int height;
        static private char[,] consoleCharachters;
        static private List<GameObject> gameObjects;
        static object updateLock;

        public Screen(int width, int height) {
            this.width = width;
            this.height = height;
            updateLock = new object();
            gameObjects = new List<GameObject>();
            consoleCharachters = new char[width, height];
        }

        public void Add(GameObject gameObject) {
            gameObjects.Add(gameObject);
        }

        public void Update(){
            lock (updateLock) {
                char[,] newConsoleCharachters = new char[width, height];

                //
                // Loop through each block
                //
                foreach (GameObject gameObject in gameObjects) {

                    //
                    // Loop through each charachter in block
                    //
                    char[,] shape = gameObject.Shape;
                    for (int i = 0; i < shape.GetLength(0); i++) {
                        for (int j = 0; j < shape.GetLength(1); j++) {
                            //
                            // If not NULL or space, set charachter in newConsoleCharachters
                            //
                            if (shape[i, j] != 0 && shape[i, j] != ' ') {
                                newConsoleCharachters[gameObject.X + i, gameObject.Y + j] = shape[i, j];
                            }
                        }
                    }
                }

                for (int i = 0; i < consoleCharachters.GetLength(0); i++) {
                    for (int j = 0; j < consoleCharachters.GetLength(1); j++) {
                        //
                        // If consoleCharachters charachter is the same as newConsoleCharachters charachter, don't redraw
                        // Otherwise, draw newConsoleCharachters charachter
                        //
                        if (consoleCharachters[i, j] == 0) consoleCharachters[i, j] = ' ';
                        if (newConsoleCharachters[i, j] == 0) newConsoleCharachters[i, j] = ' ';

                        if (consoleCharachters[i, j] != newConsoleCharachters[i, j]) {
                            consoleCharachters[i, j] = newConsoleCharachters[i, j];
                            Console.SetCursorPosition(i, j);
                            Console.Write(consoleCharachters[i, j]);
                        }
                    }
                }

                //
                // Moves blinky thing to bottom left corner so its slightly less annoying
                //
                Console.SetCursorPosition(width - 1, height - 1);
            }
        }
    }
}
