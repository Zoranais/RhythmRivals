using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RhythmRivals.BLL;
public class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
