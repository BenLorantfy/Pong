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

namespace pong {
    class Pong {
        private int width;          // width of game
        private int height;         // height of game
        private Controls controls;  // keyboard controls
        private Line line;          // Line dividing the 2 sides
        private Score p1Score;      // Player 1 score
        private Score p2Score;      // Player 2 score
        private Paddle p1Paddle;    // Player 1 paddle
        private Paddle p2Paddle;    // player 2 paddle
        private Ball ball;          // ball
        private Screen screen;      // Used to draw objects of type GameObject
        private bool run;           // Is still playing flag

        public void Play() {
            //
            // Starts a game
            //
            StartGame();

            //
            // Listen to keyboard for keypresses
            //
            StartControls();

            //
            // Initilizes screen
            // Draws paddles, ball, and scores
            //
            CreateGameObjects();

            //
            // Calculates ball path
            // Bounces ball off paddles and top/bottom
            // Detects goal
            // Updates scores
            //
            PlayGame();

            //
            // Clears Console and stops listening to keyboard
            //
            EndGame();
        }
        private void StartGame() {
            //
            // Set playing flag to true
            // 
            run = true;
        }
        private void CreateGameObjects() {
            //
            // Clears Console
            //
            Console.Clear();

            //
            // Sets game width and height
            //
            width = Console.WindowWidth;
            height = Console.WindowHeight;

            //
            // Creates dividing line
            //
            line = new Line(width, height);

            //
            // Creates scoreboard
            //
            p1Score = new Score(width / 4 - 1, 2);
            p2Score = new Score(3 * width / 4 - 1, 2);

            //
            // Creates player 1 paddle
            //
            p1Paddle = new Paddle(1, height / 2 - 2);
            p2Paddle = new Paddle(width - 2, height / 2 - 2);
            ball = new Ball(width / 2, height / 2);

            //
            // Adds paddles to the screen and update
            //
            screen = new Screen(width, height);
            screen.Add(p1Paddle);
            screen.Add(p2Paddle);
            screen.Add(ball);
            screen.Add(p1Score);
            screen.Add(p2Score);
            screen.Add(line);
            screen.Update();
        }
        private void StartControls() {
            //
            // When player 1 presses w/s move paddle up/down, respectively
            // When player 2 presses i/k move paddle up/down, respectively
            //
            controls = new Controls(ConsoleKey.W, ConsoleKey.S, ConsoleKey.I, ConsoleKey.K, ConsoleKey.Q);
            controls.WhileDown(delegate(ConsoleKey key) {
                if (key == ConsoleKey.W && p1Paddle.Y > 0) {
                    p1Paddle.Y -= 1;
                } else if (key == ConsoleKey.S && p1Paddle.Y < height - 5) {
                    p1Paddle.Y += 1;
                } else if (key == ConsoleKey.I && p2Paddle.Y > 0) {
                    p2Paddle.Y -= 1;
                } else if (key == ConsoleKey.K && p2Paddle.Y < height - 5) {
                    p2Paddle.Y += 1;
                } else if (key == ConsoleKey.Q) {
                    run = false;
                }

                screen.Update();
            });
        }
        private void PlayGame() {
            while (run) {
                //
                // Propel ball
                //
                ball.X += ball.Run;
                ball.Y += ball.Rise;

                //
                // Get ball status
                // 
                bool hitRightSide = ball.X >= p2Paddle.X - 1;
                bool hitLeftSide = ball.X <= p1Paddle.X + 1;
                bool hitSide = hitLeftSide || hitRightSide;
                bool hitP1Paddle = hitLeftSide && ball.Y >= p1Paddle.Y && ball.Y <= p1Paddle.Y + 4;
                bool hitP2Paddle = hitRightSide && ball.Y >= p2Paddle.Y && ball.Y <= p2Paddle.Y + 4;
                bool hitPaddle = hitP1Paddle || hitP2Paddle;
                bool hitTop = ball.Y <= 0;
                bool hitBottom = ball.Y >= height - 1;

                //
                // Contain ball within width and height
                //
                if (hitTop) ball.Y = 0;
                if (hitBottom) ball.Y = height - 1;

                //
                // Bounce ball of top or bottom with same angle it hit with
                //
                if (hitTop || hitBottom) {
                    ball.Rise *= -1;
                }

                //
                // Reset ball if scored
                //
                if (hitSide && !hitPaddle) {
                    ball.Reset();
                }                
                
                //
                // Update scores
                // Update run so that ball moves towards the player who was scored on
                //
                if (hitRightSide && !hitP2Paddle) {
                    p1Score.Number++;
                }

                if (hitLeftSide && !hitP1Paddle) {
                    p2Score.Number++;
                    ball.Run *= -1;
                }

                //
                // Reverse run and
                // Change ball angle depending on where on the paddle the ball hit
                //
                if (hitPaddle) {
                    int relativeY = 0;

                    if (hitP1Paddle) {
                        relativeY = ball.Y - p1Paddle.Y;
                    } else {
                        relativeY = ball.Y - p2Paddle.Y;
                    }

                    switch (relativeY) {
                        case 0:
                            ball.Rise = -2;
                            break;

                        case 1:
                            ball.Rise = -1;
                            break;

                        case 2:
                            ball.Rise = 0;
                            break;

                        case 3:
                            ball.Rise = 1;
                            break;

                        case 4:
                            ball.Rise = 2;
                            break;

                    }

                    ball.Run *= -1;
                }




                Thread.Sleep(40);
                screen.Update();
            }
        }
        private void EndGame(){
            Console.Clear();
            controls.Stop();
        }
    }
}
