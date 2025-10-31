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
            var database = new DatabaseIfc(ReleaseVersion.IFC4X3_ADD2);
            var project = new IfcProject(database, string.IsNullOrWhiteSpace(projectName) ? "Sewage network" : projectName);

            var site = new IfcSite(database, project.Name + " Site");
            new IfcRelAggregates(project, site);

            string resolvedSystemName = string.IsNullOrWhiteSpace(systemName) ? project.Name : systemName;
            var system = new IfcDistributionSystem(site, resolvedSystemName, IfcDistributionSystemEnum.SEWAGE);

            foreach (SewagePipeSegment segment in segmentList)
            {
                CreatePipeSegment(site, system, segment, database);
            }

            return database;
        }

        private const double DefaultOuterDiameter = 0.1;
        private const double RadiusTolerance = 1e-6;

        private static void CreatePipeSegment(IfcSite site, IfcDistributionSystem system, SewagePipeSegment segment, DatabaseIfc database)
        {
            var startPoint = new IfcCartesianPoint(database, segment.Start.Easting, segment.Start.Northing, segment.Start.Elevation);
            var endPoint = new IfcCartesianPoint(database, segment.End.Easting, segment.End.Northing, segment.End.Elevation);

            if (startPoint == null || endPoint == null)
            {
                return;
            }

            var directrix = new IfcPolyline(startPoint, endPoint);

            double resolvedOuterDiameter = segment.OuterDiameter.HasValue && segment.OuterDiameter.Value > RadiusTolerance
                ? segment.OuterDiameter.Value
                : DefaultOuterDiameter;

            double outerRadius = resolvedOuterDiameter / 2.0;

            double? innerRadius = null;
            if (segment.InnerDiameter.HasValue && segment.InnerDiameter.Value > RadiusTolerance)
            {
                double candidate = segment.InnerDiameter.Value / 2.0;
                if (candidate < outerRadius - RadiusTolerance)
                {
                    innerRadius = candidate;
                }
            }

            IfcSweptDiskSolid sweptDisk = innerRadius.HasValue
                ? new IfcSweptDiskSolid(directrix, outerRadius, innerRadius.Value)
                : new IfcSweptDiskSolid(directrix, outerRadius);

            var bodyRepresentation = new IfcShapeRepresentation(sweptDisk, ShapeRepresentationType.SweptSolid);
            var shape = new IfcProductDefinitionShape(bodyRepresentation);

            var placement = new IfcLocalPlacement(site.ObjectPlacement, new IfcAxis2Placement3D(startPoint));

            var pipeSegment = new IfcPipeSegment(site, placement, shape, system);
            pipeSegment.Name = segment.Identifier;
            pipeSegment.PredefinedType = IfcPipeSegmentTypeEnum.NOTDEFINED;

            var subContext = database.Factory.SubContext(IfcGeometricRepresentationSubContext.SubContextIdentifier.Axis);
            var axisRepresentation = new IfcShapeRepresentation(subContext, directrix, ShapeRepresentationType.Curve3D);
            shape.Representations.Add(axisRepresentation);
        }
    }
}
