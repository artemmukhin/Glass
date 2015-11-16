
namespace Glass
{
    class Field
    {
        private char[][] cells;
        public char[][] Cells { get { return cells; } }
        private int[] freeCells;
        //public int[] FreeCells { get { return freeCells; } }

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
        }

        public bool ChangeCell(int col, char xORo)
        {
            xORo = (xORo == 'X') ? 'x' : 'o'; // lower-case
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

        // для предыдущего хода
        public void DeleteCell(int col)
        {
            // Переделать для удаления последнего хода!
            int start = ((col == 4 || col == 5) ? 8 : 9);

            for (int row = start; row > 0; row--) {
                this.cells[row][col] = this.cells[row-1][col];
            }
            this.cells[0][col] = ' ';
        }
    }
}
