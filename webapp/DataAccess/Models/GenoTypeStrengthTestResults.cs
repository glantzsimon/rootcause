using K9.DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.DataAccessLayer.Models
{
    public class GenoTypeStrengthTestResults
    {
        public List<EGenoType> GenoTypes { get; set; }

        public List<GenoTypeStrengthTestResult> Results { get; set; }

        public List<GenoTypeStrengthTestResult> FilteredResults =>
            Results?.Where(e => GenoTypes.Contains(e.GenoType)).ToList().Concat(
            Results?.Where(e => e.Strength == EStrength.VeryStrong).ToList()).Distinct().ToList();

        public double GetMaxValue() => Results.Max(e => e.Max);

        public double GetResultAsPercentage(GenoTypeStrengthTestResult result)
        {
            return Math.Ceiling((result.Count / GetMaxValue()) * 100);
        }

        public double GetGaugaeValue(GenoTypeStrengthTestResult result)
        {
            return Math.Ceiling(100 / GetMaxValue() * result.Count);
        }

    }
}