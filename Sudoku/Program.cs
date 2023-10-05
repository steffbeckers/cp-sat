using Google.OrTools.Sat;

CpModel model = new CpModel();

int size = 9; // Size of the Sudoku grid (9x9 in this case)
int sqrtSize = 3; // Size of a subgrid (3x3 in this case)
// Create a 2D array of integer variables to represent the Sudoku grid
IntVar[][] grid = new IntVar[size][];

for (int i = 0; i < size; i++)
{
    grid[i] = new IntVar[size];

    for (int j = 0; j < size; j++)
    {
        grid[i][j] = model.NewIntVar(1, size, $"cell_{i}_{j}");
    }
}

// Add constraints to ensure rows and columns have distinct values
for (int i = 0; i < size; i++)
{
    model.AddAllDifferent(grid[i]);
    model.AddAllDifferent(Enumerable.Range(0, size).Select(j => grid[j][i]).ToArray());
}

// Add constraints to ensure 3x3 subgrids have distinct values
for (int i = 0; i < sqrtSize; i++)
{
    for (int j = 0; j < sqrtSize; j++)
    {
        IntVar[] subgrid = new IntVar[size];

        for (int x = 0; x < sqrtSize; x++)
        {
            for (int y = 0; y < sqrtSize; y++)
            {
                subgrid[x * sqrtSize + y] = grid[i * sqrtSize + x][j * sqrtSize + y];
            }
        }

        model.AddAllDifferent(subgrid);
    }
}

// Define the initial Sudoku puzzle configuration
// Sudoku 1: https://en.wikipedia.org/wiki/Sudoku
// int[][] initialGrid = new int[][] {
//     new int[] { 5, 3, 0,  0, 7, 0,  0, 0, 0 },
//     new int[] { 6, 0, 0,  1, 9, 5,  0, 0, 0 },
//     new int[] { 0, 9, 8,  0, 0, 0,  0, 6, 0 },

//     new int[] { 8, 0, 0,  0, 6, 0,  0, 0, 3 },
//     new int[] { 4, 0, 0,  8, 0, 3,  0, 0, 1 },
//     new int[] { 7, 0, 0,  0, 2, 0,  0, 0, 6 },

//     new int[] { 0, 6, 0,  0, 0, 0,  2, 8, 0 },
//     new int[] { 0, 0, 0,  4, 1, 9,  0, 0, 5 },
//     new int[] { 0, 0, 0,  0, 8, 0,  0, 7, 9 }
// };

// Sudoku 2: https://sudoku.com/evil/
int[][] initialGrid = new int[][] {
    new int[] { 9, 0, 0,  0, 1, 0,  0, 0, 6 },
    new int[] { 2, 0, 0,  5, 0, 6,  3, 0, 0 },
    new int[] { 0, 5, 0,  0, 4, 0,  0, 0, 0 },

    new int[] { 0, 2, 0,  7, 0, 9,  0, 0, 4 },
    new int[] { 3, 0, 0,  0, 0, 0,  0, 8, 0 },
    new int[] { 0, 0, 0,  0, 5, 0,  0, 0, 0 },

    new int[] { 0, 0, 0,  0, 0, 1,  0, 0, 0 },
    new int[] { 0, 7, 0,  6, 0, 2,  0, 0, 9 },
    new int[] { 0, 0, 9,  0, 0, 0,  4, 0, 0 }
};

// Add constraints to enforce the initial configuration
for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        model.Add(grid[i][j] > 0);

        if (initialGrid[i][j] != 0)
        {
            model.Add(grid[i][j] == initialGrid[i][j]);
        }
    }
}

// Create a solver
CpSolver solver = new CpSolver();

// Solve the Sudoku puzzle
CpSolverStatus status = solver.Solve(model);

// Print the solution
if (status == CpSolverStatus.Feasible || status == CpSolverStatus.Optimal)
{
    Console.WriteLine($"{status} solution found:");

    for (int i = 0; i < size; i++)
    {
        Console.WriteLine(string.Join(" ", grid[i].Select(cell => solver.Value(cell))));
    }
}
else
{
    Console.WriteLine("No solution found.");
}

// Sudoku 1
// Output:
// Optimal solution found:
// 5 3 4  6 7 8  9 1 2
// 6 7 2  1 9 5  3 4 8
// 1 9 8  3 4 2  5 6 7

// 8 5 9  7 6 1  4 2 3
// 4 2 6  8 5 3  7 9 1
// 7 1 3  9 2 4  8 5 6

// 9 6 1  5 3 7  2 8 4
// 2 8 7  4 1 9  6 3 5
// 3 4 5  2 8 6  1 7 9

// Sudoku 2
// Output:
// Optimal solution found:
// 9 8 3  2 1 7  5 4 6
// 2 4 7  5 8 6  3 9 1
// 6 5 1  9 4 3  2 7 8

// 5 2 8  7 6 9  1 3 4
// 3 9 6  1 2 4  7 8 5
// 7 1 4  3 5 8  9 6 2

// 8 3 2  4 9 1  6 5 7
// 4 7 5  6 3 2  8 1 9
// 1 6 9  8 7 5  4 2 3
