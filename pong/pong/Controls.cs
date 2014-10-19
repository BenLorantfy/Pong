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
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace pong
{
    class Controls
    {
        [DllImport("user32.dll")]
        static extern ushort GetAsyncKeyState(int key);

        private Action<ConsoleKey> keyDownFn;
        private Action<ConsoleKey> keyUpFn;
        private Action<ConsoleKey> whileDownFn;
        private Thread pollKeyboardThread;
        private Thread whileDownLoopThread;
        List<Pair<ConsoleKey,bool>> controlKeys;
        bool quit;

        #region construction
        public Controls(ConsoleKey key1) {
            this.controlKeys = new List<Pair<ConsoleKey,bool>>();
            controlKeys.Add(new Pair<ConsoleKey, bool>(key1, false));
            StartPollingKeyboard();
        }

        public Controls(ConsoleKey key1, ConsoleKey key2) {
            this.controlKeys = new List<Pair<ConsoleKey, bool>>();
            controlKeys.Add(new Pair<ConsoleKey, bool>(key1, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key2, false));
            StartPollingKeyboard();
        }

        public Controls(ConsoleKey key1, ConsoleKey key2, ConsoleKey key3) {
            this.controlKeys = new List<Pair<ConsoleKey, bool>>();
            controlKeys.Add(new Pair<ConsoleKey, bool>(key1, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key2, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key3, false));
            StartPollingKeyboard();
        }

        public Controls(ConsoleKey key1, ConsoleKey key2, ConsoleKey key3, ConsoleKey key4) {
            this.controlKeys = new List<Pair<ConsoleKey, bool>>();
            controlKeys.Add(new Pair<ConsoleKey, bool>(key1, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key2, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key3, false));
            controlKeys.Add(new Pair<ConsoleKey, bool>(key4, false));
            StartPollingKeyboard();
        }

        public Controls(List<ConsoleKey> controlKeys){
            this.controlKeys = new List<Pair<ConsoleKey, bool>>();
            foreach (ConsoleKey key in controlKeys) {
                this.controlKeys.Add(new Pair<ConsoleKey, bool>(key, false));
            }
            StartPollingKeyboard();
        }

        private void StartPollingKeyboard() {
            quit = false;
            keyDownFn = delegate(ConsoleKey key) { };
            keyUpFn = delegate(ConsoleKey key) { };
            pollKeyboardThread = new Thread(PollKeyboard);
            whileDownLoopThread = new Thread(WhileDownLoop);
            pollKeyboardThread.Start();
            whileDownLoopThread.Start();
        }
        #endregion

        public void KeyDown(Action<ConsoleKey> fn) {
            keyDownFn = fn;
        }

        public void KeyUp(Action<ConsoleKey> fn) {
            keyUpFn = fn;
        }

        public void WhileDown(Action<ConsoleKey> fn) {
            whileDownFn = fn;
        }

        private void WhileDownLoop() {
            while (true) {
                Thread.Sleep(50);  // Repeat delay
                foreach (Pair<ConsoleKey, bool> key in controlKeys) {
                    if (IsDown(key.First)) {
                        whileDownFn(key.First);
                    }             
                }
            }
        }

        private void PollKeyboard(){
            while (!quit) {
                Thread.Sleep(1);    // Be nice to the CPU

                //
                // Loops through each control key
                // Checking for new keyboard events
                //
                foreach (Pair<ConsoleKey, bool> key in controlKeys) {
                    if (IsDown(key.First)) {
                        //
                        // Keydown
                        //
                        if (!key.Second) {
                            key.Second = true;
                            keyDownFn(key.First);
                        }
                    } else {
                        //
                        // Keyup
                        //
                        if (key.Second) {
                            key.Second = false;
                            keyUpFn(key.First);
                        }
                    }

                }
                
            }
        }

        private bool IsDown(ConsoleKey key) {
            return (GetAsyncKeyState((int)key) & 0x8000) != 0;
        }
     
    }
}
