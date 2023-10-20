using Google.OrTools.Sat;

CpModel model = new CpModel();

int numberCount = 10;

IntVar x = model.NewIntVar(0, numberCount, "x");
IntVar y = model.NewIntVar(0, numberCount, "y");

model.AddAllDifferent(new IntVar[] { x, y });
model.Add(x == y * 2);
model.Add(x > 5);

CpSolver solver = new CpSolver();

CpSolverStatus status = solver.Solve(model);

// Print the solution
if (status == CpSolverStatus.Feasible || status == CpSolverStatus.Optimal)
{
	Console.WriteLine($"{status} solution found:");

	Console.WriteLine($"x: {solver.Value(x)}");
	Console.WriteLine($"y: {solver.Value(y)}");
}
else
{
	Console.WriteLine("No solution found.");
}