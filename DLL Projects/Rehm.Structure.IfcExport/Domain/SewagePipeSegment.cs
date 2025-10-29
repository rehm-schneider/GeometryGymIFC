using System;

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
