using System;
using System.Collections.Generic;
using System.Linq;
using GeometryGym.Ifc;
using Rehm.Structure.IfcExport.Domain;

namespace Rehm.Structure.IfcExport.Writers
{
    /// <summary>
    /// Creates IFC representations for sewage networks.
    /// </summary>
    public sealed class IfcSewageNetworkWriter
    {
        public DatabaseIfc BuildModel(IEnumerable<SewagePipeSegment> segments, string projectName, string systemName = null)
        {
            if (segments == null)
            {
                throw new ArgumentNullException("segments");
            }

            List<SewagePipeSegment> segmentList = segments.ToList();
            var database = new DatabaseIfc(ModelView.Ifc4X3_RC2);
            var project = new IfcProject(database, string.IsNullOrWhiteSpace(projectName) ? "Sewage network" : projectName);

            var site = new IfcSite(database, project.Name + " Site");
            new IfcRelAggregates(project, site);

            string resolvedSystemName = string.IsNullOrWhiteSpace(systemName) ? project.Name : systemName;
            var system = new IfcDistributionSystem(site, resolvedSystemName, IfcDistributionSystemEnum.WASTEWATERSYSTEM);

            foreach (SewagePipeSegment segment in segmentList)
            {
                CreatePipeSegment(site, system, segment, database);
            }

            return database;
        }

        private static void CreatePipeSegment(IfcSite site, IfcDistributionSystem system, SewagePipeSegment segment, DatabaseIfc database)
        {
            var startPoint = new IfcCartesianPoint(database, segment.Start.Easting, segment.Start.Northing, segment.Start.Elevation);
            var endPoint = new IfcCartesianPoint(database, segment.End.Easting, segment.End.Northing, segment.End.Elevation);

            var polyline = new IfcPolyline(startPoint, endPoint);
            var shape = new IfcProductDefinitionShape(new IfcShapeRepresentation(polyline, ShapeRepresentationType.Curve3D));

            var placement = new IfcLocalPlacement(site.ObjectPlacement, new IfcAxis2Placement3D(startPoint));

            var pipeSegment = new IfcPipeSegment(site, placement, shape, system);
            pipeSegment.Name = segment.Identifier;
            pipeSegment.PredefinedType = IfcPipeSegmentTypeEnum.NOTDEFINED;

            var subContext = database.Factory.SubContext(IfcGeometricRepresentationSubContext.SubContextIdentifier.Axis);
            var axisRepresentation = new IfcShapeRepresentation(subContext, polyline, ShapeRepresentationType.Curve3D);
            shape.Representations.Add(axisRepresentation);
        }
namespace Rehm.Structure.IfcExport.Writers;

/// <summary>
/// Creates IFC representations for sewage networks.
/// </summary>
public sealed class IfcSewageNetworkWriter
{
    public DatabaseIfc BuildModel(IEnumerable<SewagePipeSegment> segments, string projectName, string? systemName = null)
    {
        if (segments == null)
        {
            throw new ArgumentNullException(nameof(segments));
        }

        var segmentList = segments.ToList();
        var database = new DatabaseIfc(ModelView.Ifc4X3_RC2);
        var project = new IfcProject(database, string.IsNullOrWhiteSpace(projectName) ? "Sewage network" : projectName);

        var site = new IfcSite(database, $"{project.Name} Site");
        new IfcRelAggregates(project, site);

        var system = new IfcDistributionSystem(site, string.IsNullOrWhiteSpace(systemName) ? project.Name : systemName, IfcDistributionSystemEnum.WASTEWATERSYSTEM);

        foreach (var segment in segmentList)
        {
            CreatePipeSegment(site, system, segment, database);
        }

        return database;
    }

    private static void CreatePipeSegment(IfcSite site, IfcDistributionSystem system, SewagePipeSegment segment, DatabaseIfc database)
    {
        var startPoint = new IfcCartesianPoint(database, segment.Start.Easting, segment.Start.Northing, segment.Start.Elevation);
        var endPoint = new IfcCartesianPoint(database, segment.End.Easting, segment.End.Northing, segment.End.Elevation);

        var polyline = new IfcPolyline(startPoint, endPoint);
        var shape = new IfcProductDefinitionShape(new IfcShapeRepresentation(polyline, ShapeRepresentationType.Curve3D));

        var placement = new IfcLocalPlacement(site.ObjectPlacement, new IfcAxis2Placement3D(startPoint));

        var pipeSegment = new IfcPipeSegment(site, placement, shape, system)
        {
            Name = segment.Identifier,
            PredefinedType = IfcPipeSegmentTypeEnum.NOTDEFINED
        };

        var axisRepresentation = new IfcShapeRepresentation(database.Factory.SubContext(IfcGeometricRepresentationSubContext.SubContextIdentifier.Axis), polyline, ShapeRepresentationType.Curve3D);
        shape.Representations.Add(axisRepresentation);
    }
}
