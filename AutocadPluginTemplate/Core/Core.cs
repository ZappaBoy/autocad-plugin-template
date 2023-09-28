using System;
using System.IO;
using AutocadPluginTemplate.Utils;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace AutocadPluginTemplate.Core
{
    public class AutocadPluginCore
    {
        static readonly string FileName = "output.txt";
        static readonly string OutputFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static readonly string OutputFilePath = Path.Combine(OutputFolderPath, FileName);

        public void DrawCube(Point3d basePoint, double sideLength)
        {
            using (Transaction trans = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    trans.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead) as
                        BlockTable;

                if (bt != null)
                {
                    BlockTableRecord btr =
                        trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

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

        public void DescribeSelectedElementsInfo()
        {
            Editor editor = AutocadPluginUtils.GetCurrentEditor();
            string selectedElementsInfo = AutocadPluginUtils.GetSelectedElementsInfo();
            editor.WriteMessage(selectedElementsInfo);
        }

        public void DumpSelectedElementsInfo()
        {
            string selectedElementsInfo = AutocadPluginUtils.GetSelectedElementsInfo();
            File.WriteAllText(OutputFilePath, selectedElementsInfo);
        }
    }
}