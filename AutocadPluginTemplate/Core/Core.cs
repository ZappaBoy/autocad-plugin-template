using System;
using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using AutocadPluginTemplate.Utils;

namespace AutocadPluginTemplate.Core
{
    public class AutocadPluginCore
    {
        public void DrawCube(Point3d basePoint, double sideLength)
        {
            using (Transaction trans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead) as BlockTable;

                if (bt != null)
                {
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Solid3d cube = new Solid3d();
                    cube.CreateBox(sideLength, sideLength, sideLength);

                    Matrix3d transform = Matrix3d.Displacement(basePoint.GetVectorTo(Point3d.Origin));
                    cube.TransformBy(transform);

                    if (btr != null) btr.AppendEntity(cube);
                    trans.AddNewlyCreatedDBObject(cube, true);
                }
                trans.Commit();
            }
        }
    }
}