using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong {
    class Line : GameObject{
        public Line(int gameWidth, int gameHeight) : base(1, gameHeight){
            X = gameWidth / 2;
            Y = 0;
            for (int i = 0; i < Shape.GetLength(1); i += 2) {
                Shape[0, i] = '█';
            }
        }
    }
}
