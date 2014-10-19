using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong {
    class Score : GameObject{
        private int number;
        public int Number {
            get {
                return number;
            }
            set {
                if (value >= 0 && value <= 9) {
                    SetShape(value);
                    number = value;
                }
            }
        }

        public Score(int x, int y) : base(3,7) {
            this.X = x;
            this.Y = y;
            number = 0;
            SetShape(0);
        }

        #region drawNumber
        private void SetShape(int num){
            for(int i = 0; i < Shape.GetLength(0); i++){
                for(int j = 0; j < Shape.GetLength(1); j++){
                    Shape[i,j] = ' ';
                }
            }

            switch (num) {
                case 0:
                    SetTop(0, 1, 2);
                    SetLeft(1, 2, 3);
                    SetRight(1, 2, 3);
                    SetBottom(0, 1, 2);
                    break;

                case 1:
                    SetRight(0,1,2,3,4);
                    break;

                case 2:
                    SetTop(0, 1, 2);
                    SetRight(1);
                    SetMiddle(0, 1, 2);
                    SetLeft(3);
                    SetBottom(0, 1, 2);
                    break;

                case 3:
                    SetTop(0, 1, 2);
                    SetRight(1, 3);
                    SetMiddle(0, 1, 2);
                    SetBottom(0, 1, 2);
                    break;

                case 4:
                    SetTop(0, 2);
                    SetMiddle(0, 1, 2);
                    SetLeft(1);
                    SetRight(1, 3, 4);
                    break;

                case 5:
                    SetTop(0, 1, 2);
                    SetLeft(1);
                    SetRight(3);
                    SetMiddle(0, 1, 2);
                    SetBottom(0, 1, 2);
                    
                    break;

                case 6:
                    SetTop(0, 1, 2);
                    SetMiddle(0, 1, 2);
                    SetBottom(0, 1, 2);
                    SetLeft(1, 3);
                    SetRight(3);
                    break;

                case 7:
                    SetTop(0, 1, 2);
                    SetRight(1, 2, 3, 4);
                    break;

                case 8:
                    SetTop(0, 1, 2);
                    SetMiddle(0, 1, 2);
                    SetBottom(0, 1, 2);
                    SetLeft(1, 3);
                    SetRight(1, 3);
                    break;

                case 9:
                    SetTop(0, 1, 2);
                    SetMiddle(0, 1, 2);
                    SetBottom(2);
                    SetLeft(1);
                    SetRight(1, 3);
                    break;

            }
        }
        private void SetLeft(params int[] ys) {
            foreach (int y in ys) {
                Shape[0, y] = '█';
            }
        }
        private void SetRight(params int[] ys) {
            foreach (int y in ys) {
                Shape[2, y] = '█';
            }
        }
        private void SetTop(params int[] xs) {
            foreach (int x in xs) {
                Shape[x, 0] = '█';
            }
        }
        private void SetMiddle(params int[] xs) {
            foreach (int x in xs) {
                Shape[x, 2] = '█';
            }
        }
        private void SetBottom(params int[] xs) {
            foreach (int x in xs) {
                Shape[x, 4] = '█';
            }
        }
        #endregion
    }
}
