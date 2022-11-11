using amgclHandle = System.IntPtr;

namespace AmgclSharp;

public class Amg
{
    private NativeMethods.conv_info _convInfo;

    public int Iterations => _convInfo.iterations;
    public double Residual => _convInfo.residual;

    private amgclHandle? _params;
    private amgclHandle? _precond;
    private amgclHandle? _solver;

    /// <summary>
    /// Create parameter list.
    /// </summary>
    public void ParamsCreate() => _params = NativeMethods.amgcl_params_create();

    /// <summary>
    /// Set integer parameter in a parameter list.
    /// </summary>
    /// <param name="name">parameter name</param>
    /// <param name="value">integer value</param>
    /// <exception cref="NullReferenceException"></exception>
    public void ParamsSetInt(string name, int value)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        NativeMethods.amgcl_params_seti((amgclHandle)_params, name, value);
    }

    /// <summary>
    /// Set floating point parameter in a parameter list.
    /// </summary>
    /// <param name="name">parameter name</param>
    /// <param name="value">float value</param>
    /// <exception cref="NullReferenceException"></exception>
    public void ParamsSetFloat(string name, float value)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        NativeMethods.amgcl_params_setf((amgclHandle)_params, name, value);
    }

    /// <summary>
    /// Set string parameter in a parameter list.
    /// </summary>
    /// <param name="name">parameter name</param>
    /// <param name="value">string value</param>
    /// <exception cref="NullReferenceException"></exception>
    public void ParamsSetString(string name, string value)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        NativeMethods.amgcl_params_sets((amgclHandle)_params, name, value);
    }

    /// <summary>
    /// Read parameters from a JSON file
    /// </summary>
    /// <param name="fname">JSON file name</param>
    /// <exception cref="NullReferenceException"></exception>
    public void ParamsReadJson(string fname)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        NativeMethods.amgcl_params_read_json((amgclHandle)_params, fname);
    }

    /// <summary>
    /// Destroy parameter list.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void ParamsDestroy()
    {
        if (_params is null) throw new NullReferenceException("params is null");
        NativeMethods.amgcl_params_destroy((amgclHandle)_params);
        _params = null;
    }

    /// <summary>
    /// Create AMG preconditioner.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="ptr"></param>
    /// <param name="col"></param>
    /// <param name="val"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void PrecondCreate(int n, int[] ptr, int[] col, double[] val)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        _precond = NativeMethods.amgcl_precond_create(n, ptr, col, val, (amgclHandle)_params);
    }

    /// <summary>
    /// Apply AMG preconditioner (x = M^(-1) * rhs).
    /// </summary>
    /// <param name="rhs"></param>
    /// <param name="x"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void PrecondApply(double[] rhs, double[] x)
    {
        if (_precond is null) throw new NullReferenceException("precond handle is null");
        NativeMethods.amgcl_precond_apply((amgclHandle)_precond, rhs, x);
    }

    /// <summary>
    /// Printout preconditioner structure
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void PrecondReport()
    {
        if (_precond is null) throw new NullReferenceException("precond handle is null");
        NativeMethods.amgcl_precond_report((amgclHandle)_precond);
    }

    /// <summary>
    /// Destroy AMG preconditioner
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void PrecondDestroy()
    {
        if (_precond is null) throw new NullReferenceException("precond handle is null");
        NativeMethods.amgcl_precond_destroy((amgclHandle)_precond);
        _precond = null;
    }

    /// <summary>
    /// Create iterative solver preconditioned by AMG.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="ptr"></param>
    /// <param name="col"></param>
    /// <param name="val"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void SolverCreate(int n, int[] ptr, int[] col, double[] val)
    {
        if (_params is null) throw new NullReferenceException("params is null");
        _solver = NativeMethods.amgcl_solver_create(n, ptr, col, val, (amgclHandle)_params);
    }

    /// <summary>
    /// Solve the problem for the given right-hand side.
    /// </summary>
    /// <param name="rhs"></param>
    /// <param name="x"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void SolverSolve(double[] rhs, double[] x)
    {
        if (_solver is null) throw new NullReferenceException("solver handle is null");
        _convInfo = NativeMethods.amgcl_solver_solve((amgclHandle)_solver, rhs, x);
    }

    /// <summary>
    /// Solve the problem for the given matrix and the right-hand side.
    /// </summary>
    /// <param name="A_ptr"></param>
    /// <param name="A_col"></param>
    /// <param name="A_val"></param>
    /// <param name="rhs"></param>
    /// <param name="x"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void SolverSolveMtx(int[] A_ptr, int[] A_col, double[] A_val, double[] rhs, double[] x)
    {
        if (_solver is null) throw new NullReferenceException("solver handle is null");
        _convInfo = NativeMethods.amgcl_solver_solve_mtx((amgclHandle)_solver, A_ptr, A_col, A_val, rhs, x);
    }

    /// <summary>
    /// Printout solver structure
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void SolverReport()
    {
        if (_solver is null) throw new NullReferenceException("solver handle is null");
        NativeMethods.amgcl_solver_report((amgclHandle)_solver);
    }

    /// <summary>
    /// Destroy iterative solver.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public void SolverDestroy()
    {
        if (_solver is null) throw new NullReferenceException("solver handle is null");
        NativeMethods.amgcl_solver_destroy((amgclHandle)_solver);
        _solver = null;
    }
}
