using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BaseLibrary.Database;
using Rehm.Structure.Core;
using Rehm.Structure.IfcExport.Domain;

namespace Rehm.Structure.IfcExport.Readers
{
    /// <summary>
    /// Reads sewage network information from a GraPS data source.
    /// </summary>
    public sealed class GraPsSewageNetworkReader
    {
        private readonly GraPSObjectReader _objectReader;

        public GraPsSewageNetworkReader(GraPSObjectReader objectReader)
        {
            if (objectReader == null)
            {
                throw new ArgumentNullException("objectReader");
            }

            _objectReader = objectReader;
        }

        public static GraPsSewageNetworkReader FromDatabaseConnection(DatabaseConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            return new GraPsSewageNetworkReader(new GraPSObjectReader(connection));
        }

        public IReadOnlyCollection<SewagePipeSegment> ReadSegments()
        {
            IList<GraPSHaltung> haltungen = _objectReader.ReadHaltungen();
            var segments = new List<SewagePipeSegment>(haltungen.Count);

            foreach (GraPSHaltung haltung in haltungen)
            {
                if (haltung == null)
                {
                    continue;
                }

                string identifier = string.IsNullOrWhiteSpace(haltung.Bezeichnung) ? string.Empty : haltung.Bezeichnung;

                SewerCoordinate start = SewerCoordinate.From(haltung.RechtswertSohleOben, haltung.HochwertSohleOben, haltung.HoeheOben);
                SewerCoordinate end = SewerCoordinate.From(haltung.RechtswertSohleUnten, haltung.HochwertSohleUnten, haltung.HoeheUnten);

                segments.Add(new SewagePipeSegment(identifier, start, end));
            }

            return new ReadOnlyCollection<SewagePipeSegment>(segments);
        }
namespace Rehm.Structure.IfcExport.Readers;

/// <summary>
/// Reads sewage network information from a GraPS data source.
/// </summary>
public sealed class GraPsSewageNetworkReader
{
    private readonly GraPSObjectReader _objectReader;

    public GraPsSewageNetworkReader(GraPSObjectReader objectReader)
    {
        _objectReader = objectReader ?? throw new ArgumentNullException(nameof(objectReader));
    }

    public static GraPsSewageNetworkReader FromDatabaseConnection(DatabaseConnection connection)
    {
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        return new GraPsSewageNetworkReader(new GraPSObjectReader(connection));
    }

    public IReadOnlyCollection<SewagePipeSegment> ReadSegments()
    {
        var haltungen = _objectReader.ReadHaltungen();
        var segments = new List<SewagePipeSegment>(haltungen.Count);

        foreach (var haltung in haltungen)
        {
            if (haltung == null)
            {
                continue;
            }

            var identifier = string.IsNullOrWhiteSpace(haltung.Bezeichnung) ? string.Empty : haltung.Bezeichnung;

            var start = SewerCoordinate.From(haltung.RechtswertSohleOben, haltung.HochwertSohleOben, haltung.HoeheOben);
            var end = SewerCoordinate.From(haltung.RechtswertSohleUnten, haltung.HochwertSohleUnten, haltung.HoeheUnten);

            segments.Add(new SewagePipeSegment(identifier, start, end));
        }

        return new ReadOnlyCollection<SewagePipeSegment>(segments);
    }
}
