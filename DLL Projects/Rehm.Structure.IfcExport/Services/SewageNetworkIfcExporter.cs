using System;
using GeometryGym.Ifc;
using Rehm.Structure.IfcExport.Readers;
using Rehm.Structure.IfcExport.Writers;

namespace Rehm.Structure.IfcExport.Services
{
    /// <summary>
    /// Coordinates reading GraPS sewage data and exporting it as IFC.
    /// </summary>
    public sealed class SewageNetworkIfcExporter
    {
        private readonly GraPsSewageNetworkReader _reader;
        private readonly IfcSewageNetworkWriter _writer;

        public SewageNetworkIfcExporter(GraPsSewageNetworkReader reader, IfcSewageNetworkWriter writer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            _reader = reader;
            _writer = writer;
        }

        public DatabaseIfc Export(string projectName, string outputFilePath, string systemName = null)
        {
            var segments = _reader.ReadSegments();
            var database = _writer.BuildModel(segments, projectName, systemName);

            if (!string.IsNullOrWhiteSpace(outputFilePath))
            {
                database.WriteFile(outputFilePath);
            }

            return database;
        }
    }
}
