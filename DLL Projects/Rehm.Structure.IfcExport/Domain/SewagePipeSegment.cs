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
        public string Identifier { get; private set; }
        public SewerCoordinate Start { get; private set; }
        public SewerCoordinate End { get; private set; }

        public SewagePipeSegment(string identifier, SewerCoordinate start, SewerCoordinate end)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            Identifier = identifier ?? string.Empty;
            Start = start;
            End = end;
        }
namespace Rehm.Structure.IfcExport.Domain;

/// <summary>
/// Represents a single sewage pipe segment derived from GraPS data.
/// </summary>
public sealed record SewerCoordinate(double Easting, double Northing, double Elevation)
{
    public static SewerCoordinate From(double easting, double northing, double elevation) => new(easting, northing, elevation);
}

/// <summary>
/// Immutable domain object describing the start and end points of a sewage pipe segment.
/// </summary>
public sealed record SewagePipeSegment(string Identifier, SewerCoordinate Start, SewerCoordinate End)
{
    public SewagePipeSegment(string identifier, SewerCoordinate start, SewerCoordinate end)
        : this(identifier ?? string.Empty, start ?? throw new ArgumentNullException(nameof(start)), end ?? throw new ArgumentNullException(nameof(end)))
    {
    }
}
