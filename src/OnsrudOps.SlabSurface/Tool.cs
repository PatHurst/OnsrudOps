using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnsrudOps.SlabSurface;

/// <summary>
/// Definition for a CNC router tool
/// </summary>
/// <param name="Number"></param>
/// <param name="Diameter"></param>
/// <param name="FeedRate"></param>
public record Tool(int Number, double Diameter, double RPM, double FeedRate)
{
    public override string ToString()
    {
        return $"#{Number} Diameter:{Diameter} RPM:{RPM} Feed:{FeedRate}";
    }
}
