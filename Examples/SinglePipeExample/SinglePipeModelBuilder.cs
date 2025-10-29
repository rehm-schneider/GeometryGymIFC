using System;
using System.IO;
using GeometryGym.Ifc;

namespace GeometryGym.Examples;

/// <summary>
/// Helpers for constructing a minimal IFC project containing a single pipe segment.
/// </summary>
public static class SinglePipeModelBuilder
{
    /// <summary>
    /// Builds an IFC database with a straight pipe segment referenced to a basic spatial structure.
    /// </summary>
    /// <param name="length">Length of the pipe extrusion in metres.</param>
    /// <param name="outerDiameter">Outside diameter of the pipe in metres.</param>
    /// <param name="wallThickness">Wall thickness of the pipe in metres.</param>
    public static DatabaseIfc CreatePipeDatabase(double length = 3.0, double outerDiameter = 0.15, double wallThickness = 0.005)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be greater than zero.");
        }

        if (outerDiameter <= wallThickness * 2)
        {
            throw new ArgumentOutOfRangeException(nameof(outerDiameter), outerDiameter, "Outside diameter must exceed twice the wall thickness.");
        }

        var database = new DatabaseIfc(ModelView.Ifc4X3_RC2);

        var project = new IfcProject(database, "Pipe sample");
        project.UnitsInContext = new IfcUnitAssignment(database, IfcUnitAssignment.Length.Metre);

        var site = new IfcSite(database, "Demo site");
        project.AddAggregated(site);

        var building = new IfcBuilding(database, "Demo building");
        site.AddAggregated(building);

        var storey = new IfcBuildingStorey(building, "Ground floor", 0.0);

        var placement = new IfcLocalPlacement(storey.ObjectPlacement, database.Factory.XYPlanePlacement);

        var pipeProfile = new IfcCircleHollowProfileDef(database, "DN150", outerDiameter / 2.0, wallThickness);
        var pipeSolid = new IfcExtrudedAreaSolid(pipeProfile, depth: length);
        var pipeShape = new IfcProductDefinitionShape(new IfcShapeRepresentation(pipeSolid));

        _ = new IfcPipeSegment(storey, placement, pipeShape, system: null)
        {
            Name = "Sample pipe run",
            PredefinedType = IfcPipeSegmentTypeEnum.RIGIDSEGMENT
        };

        return database;
    }

    /// <summary>
    /// Creates the sample database and writes it to disk. Directories are generated as required.
    /// </summary>
    public static void WriteStepFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("A file path must be supplied.", nameof(filePath));
        }

        var fullPath = Path.GetFullPath(filePath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var database = CreatePipeDatabase();
        database.WriteFile(fullPath);
    }
}
