using System;

namespace Rehm.Structure.IfcExport.Domain
{
    /// <summary>
    /// Represents a single sewage pipe segment derived from GraPS data.
    /// </summary>
    public sealed class SewerCoordinate
    {
        public double Easting { get; private set; }
        public double Northing { get; private set; }
        public double Elevation { get; private set; }

        public SewerCoordinate(double easting, double northing, double elevation)
        {
            Easting = easting;
            Northing = northing;
            Elevation = elevation;
        }

        public static SewerCoordinate From(double easting, double northing, double elevation)
        {
            return new SewerCoordinate(easting, northing, elevation);
        }
    }

    /// <summary>
    /// Immutable domain object describing the start and end points of a sewage pipe segment.
    /// </summary>
    public sealed class SewagePipeSegment
    {
        private const double MinimumDiameterTolerance = 1e-6;

        public string Identifier { get; private set; }
        public SewerCoordinate Start { get; private set; }
        public SewerCoordinate End { get; private set; }
        public double? OuterDiameter { get; private set; }
        public double? InnerDiameter { get; private set; }

        public SewagePipeSegment(string identifier, SewerCoordinate start, SewerCoordinate end, double? outerDiameter = null, double? innerDiameter = null)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            if (outerDiameter.HasValue && outerDiameter.Value <= MinimumDiameterTolerance)
            {
                outerDiameter = null;
            }

            if (innerDiameter.HasValue && innerDiameter.Value <= MinimumDiameterTolerance)
            {
                innerDiameter = null;
            }

            if (outerDiameter.HasValue && innerDiameter.HasValue && innerDiameter.Value >= outerDiameter.Value)
            {
                innerDiameter = Math.Max(MinimumDiameterTolerance, outerDiameter.Value - MinimumDiameterTolerance);
            }

            Identifier = identifier ?? string.Empty;
            Start = start;
            End = end;
            OuterDiameter = outerDiameter;
            InnerDiameter = innerDiameter;
        }
    }
}
