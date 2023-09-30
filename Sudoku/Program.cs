using Google.OrTools.Sat;

CpModel model = new CpModel();
int size = 9;
int sqrtSize = 3;

IntVar[][] grid = new IntVar[size][];
for (int i = 0; i < size; i++)
{
  // TODO
  grid[i] = model.NewIntVarArray(size, 1, size, $"row_{i + 1}");
}

// Add constraints to ensure rows and columns have distinct values.
for (int i = 0; i < size; i++)
{
  model.AddAllDifferent(grid[i]);
  model.AddAllDifferent(Enumerable.Range(0, size).Select(j => grid[j][i]).ToArray());
}

// Add constraints to ensure 3x3 subgrids have distinct values.
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

CpSolver solver = new CpSolver();
CpSolverStatus status = solver.Solve(model);

if (status == CpSolverStatus.Feasible)
{
  Console.WriteLine("Solution found:");

  for (int i = 0; i < size; i++)
  {
    Console.WriteLine(string.Join(" ", grid[i].Select(cell => solver.Value(cell))));
  }
}
else
{
  Console.WriteLine("No solution found.");
}
