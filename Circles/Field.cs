using System;

namespace Circles {
    public class Field {
        private Circle[,] circles;
        private int player;

        public Field(int player) {
            this.player = player;
            circles = new Circle[Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT];

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    this[i, j] = new Circle(i, j, player);
                }
            }
        }

        public Circle this [int i, int j]
        {
            get { return circles[i, j]; }
            private set { circles[i, j] = value; }
        }
    }
}
