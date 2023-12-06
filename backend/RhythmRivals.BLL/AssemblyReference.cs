using System.Reflection;

namespace RhythmRivals.BLL;
public class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
