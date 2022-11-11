using System.Runtime.InteropServices;
using amgclHandle = System.IntPtr;

namespace AmgclSharp;

internal static class NativeMethods
{
    private const string DllExtern = "amgcl";

    // Create parameter list.
    [DllImport(DllExtern)]
    public static extern amgclHandle amgcl_params_create();

    // Set integer parameter in a parameter list.
    [DllImport(DllExtern)]
    public static extern void amgcl_params_seti(amgclHandle prm, string name, int value);

    // Set floating point parameter in a parameter list.
    [DllImport(DllExtern)]
    public static extern void amgcl_params_setf(amgclHandle prm, string name, float value);

    // Set string parameter in a parameter list.
    [DllImport(DllExtern)]
    public static extern void amgcl_params_sets(amgclHandle prm, string name, string value);

    // Read parameters from a JSON file
    [DllImport(DllExtern)]
    public static extern void amgcl_params_read_json(amgclHandle prm, string fname);

    // Destroy parameter list.
    [DllImport(DllExtern)]
    public static extern void amgcl_params_destroy(amgclHandle prm);

    // Create AMG preconditioner.
    [DllImport(DllExtern)]
    public static extern amgclHandle amgcl_precond_create(
        int n,
        int[] ptr,
        int[] col,
        double[] val,
        amgclHandle parameters
        );

    // Apply AMG preconditioner (x = M^(-1) * rhs).
    [DllImport(DllExtern)]
    public static extern void amgcl_precond_apply(amgclHandle amg, double[] rhs, double[] x);

    // Printout preconditioner structure
    [DllImport(DllExtern)]
    public static extern void amgcl_precond_report(amgclHandle amg);

    // Destroy AMG preconditioner
    [DllImport(DllExtern)]
    public static extern void amgcl_precond_destroy(amgclHandle amg);

    // Create iterative solver preconditioned by AMG.
    [DllImport(DllExtern)]
    public static extern amgclHandle amgcl_solver_create(
        int n,
        int[] ptr,
        int[] col,
        double[] val,
        amgclHandle parameters
        );

    // Convergence info
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct conv_info
    {
        public int iterations;
        public double residual;
    };

    // Solve the problem for the given right-hand side.
    [DllImport(DllExtern)]
    public static extern conv_info amgcl_solver_solve(
        amgclHandle solver,
        double[] rhs,
        double[] x
        );


    // Solve the problem for the given matrix and the right-hand side.
    [DllImport(DllExtern)]
    public static extern conv_info amgcl_solver_solve_mtx(
        amgclHandle solver,
        int[] A_ptr,
        int[] A_col,
        double[] A_val,
        double[] rhs,
        double[] x
        );


    // Printout solver structure
    [DllImport(DllExtern)]
    public static extern void amgcl_solver_report(amgclHandle solver);

    // Destroy iterative solver.
    [DllImport(DllExtern)]
    public static extern void amgcl_solver_destroy(amgclHandle solver);
}