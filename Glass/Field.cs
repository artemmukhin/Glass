namespace Glass
{
    class Field
    {
        private char[][] cells;
        public char[][] Cells { get { return cells; } }
        private int[] freeCells;
        public int[,] winCells;

        public Field()
        {
            this.cells = new char[10][];
            for (int i = 0; i < 10; i++) this.cells[i] = new char[10];

            this.freeCells = new int[10];
            for (int i = 0; i < 10; i++) freeCells[i] = 9;

            for (int row = 0; row < 10; row++) {
                for (int col = 0; col < 10; col++) {
                    this.cells[row][col] = ' ';
                }
            }
            this.cells[9][4] = '-';
            this.cells[9][5] = '-';

            this.winCells = new int[5,2];
            for (int i = 0; i < 5; i++) {
                this.winCells[i, 0] = -1;
                this.winCells[i, 1] = -1;
            }
        }

        /**
        * Add X or O to coloumn
        *
        * @param  col   coloumn on which the move was made
        * @return false if step is invalid, true if step is valid
        */
        public bool ChangeCell(int col, char xORo)
        {
            if (col < 0 || col > 9) return false;
            if (this.cells[0][col] != ' ') return false;

            char prev, curr;
            int start = ((col == 4 || col == 5) ? 8 : 9);

            prev = this.cells[start][col];
            curr = this.cells[start - 1][col];
            this.cells[start][col] = xORo;
            for (int row = start - 1; row >= 0; row--) {
                curr = this.cells[row][col];
                this.cells[row][col] = prev;
                prev = curr;
            }
            return true;
        }

        /**
        * Remove X or O from coloumn
        * This method is used for rewind steps
        *
        * @param  col   coloumn on which the move was made
        * @see  PrevStep()
        */
        public void DeleteCell(int col)
        {
            if (col == -1) return; // if timeout

            int start = ((col == 4 || col == 5) ? 8 : 9);
            for (int row = start; row > 0; row--) {
                this.cells[row][col] = this.cells[row-1][col];
            }
            this.cells[0][col] = ' ';
        }
    }
}
