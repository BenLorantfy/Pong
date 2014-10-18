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
        private Thread pollKeyboardThread;
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
            pollKeyboardThread.Start();
        }
        #endregion

        public void KeyDown(Action<ConsoleKey> fn) {
            keyDownFn = fn;
        }

        public void KeyUp(Action<ConsoleKey> fn) {
            keyUpFn = fn;
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
