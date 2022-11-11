using AmgclSharp;

namespace AmgclTest;
internal class Program
{
    private static void Main()
    {
        List<int> ptr = new();
        List<int> col = new();
        List<double> val = new();
        List<double> rhs = new();

        int n = sample_problem(64, val, col, ptr, rhs);

        Amg amg = new();

        amg.ParamsCreate();

        amg.ParamsSetInt("precond.coarse_enough", 1000);
        amg.ParamsSetString("precond.coarsening.type", "smoothed_aggregation");
        amg.ParamsSetFloat("precond.coarsening.aggr.eps_strong", 1e-3f);
        amg.ParamsSetString("precond.relax.type", "spai0");

        amg.ParamsSetString("solver.type", "bicgstabl");
        amg.ParamsSetInt("solver.L", 1);
        amg.ParamsSetInt("solver.maxiter", 100);

        amg.SolverCreate(n, ptr.ToArray(), col.ToArray(), val.ToArray());

        amg.ParamsDestroy();

        double[] x = new double[n];
        amg.SolverSolve(rhs.ToArray(), x);

        Console.WriteLine($"Iterations: {amg.Iterations}");
        Console.WriteLine($"Error: {amg.Residual}");

        amg.SolverDestroy();
    }

    static int sample_problem(
        int n,
        List<double>  val,
        List<int>  col,
        List<int>  ptr,
        List<double>    rhs,
        double anisotropy = 1.0
        )
    {
        int n3 = n * n * n;

        ptr.Clear();
        col.Clear();
        val.Clear();
        rhs.Clear();

        double hx = 1;
        double hy = hx * anisotropy;
        double hz = hy * anisotropy;

        ptr.Add(0);
        for (int k = 0, idx = 0; k < n; ++k)
        {
            for (int j = 0; j < n; ++j)
            {
                for (int i = 0; i < n; ++i, ++idx)
                {
                    if (k > 0)
                    {
                        col.Add(idx - n * n);
                        val.Add(-1.0 / (hz * hz));
                    }

                    if (j > 0)
                    {
                        col.Add(idx - n);
                        val.Add(-1.0 / (hy * hy));
                    }

                    if (i > 0)
                    {
                        col.Add(idx - 1);
                        val.Add(-1.0 / (hx * hx));
                    }

                    col.Add(idx);
                    val.Add((2 / (hx * hx) + 2 / (hy * hy) + 2 / (hz * hz)));

                    if (i + 1 < n)
                    {
                        col.Add(idx + 1);
                        val.Add(-1.0 / (hx * hx));
                    }

                    if (j + 1 < n)
                    {
                        col.Add(idx + n);
                        val.Add(-1.0 / (hy * hy));
                    }

                    if (k + 1 < n)
                    {
                        col.Add(idx + n * n);
                        val.Add(-1.0 / (hz * hz));
                    }

                    rhs.Add(1.0);
                    ptr.Add(col.Count());
                }
            }
        }

        return n3;
    }
}