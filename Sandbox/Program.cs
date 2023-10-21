using Google.OrTools.Sat;

CpModel model = new CpModel();

int numberOfValues = 100;

IntVar x = model.NewIntVar(0, numberOfValues - 1, "x");
IntVar y = model.NewIntVar(0, numberOfValues - 1, "y");
IntVar z = model.NewIntVar(0, numberOfValues - 1, "z");

model.Add(x > y);
model.Add(x == z - 50);
model.AddAllDifferent(new IntVar[] { x, y, z });

model.Maximize(x);

CpSolver solver = new CpSolver();

CpSolverStatus status = solver.Solve(model);

// Print the solution
if (status == CpSolverStatus.Feasible || status == CpSolverStatus.Optimal)
{
	Console.WriteLine($"{status} solution found:");

	Console.WriteLine($"x: {solver.Value(x)}");
	Console.WriteLine($"y: {solver.Value(y)}");
	Console.WriteLine($"z: {solver.Value(z)}");
}
else
{
	Console.WriteLine("No solution found.");
}