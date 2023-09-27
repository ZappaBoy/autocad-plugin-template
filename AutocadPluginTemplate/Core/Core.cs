using AutocadPluginTemplate.Utils;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace AutocadPluginTemplate.Core
{
    public class AutocadPluginCore
    {
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

        public void GetSelectedElementsInfo()
        {
            var editor = AutocadPluginUtils.GetCurrentEditor();
            var database = AutocadPluginUtils.GetDatabase();
            var selectionResult = editor.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) return;
            var selectionSet = selectionResult.Value;
            using (var tr = database.TransactionManager.StartTransaction())
            {
                var i = 0;
                foreach (SelectedObject selObj in selectionSet)
                {
                    i++;
                    if (selObj == null) continue;
                    var objId = selObj.ObjectId;
                    var entity = tr.GetObject(objId, OpenMode.ForRead) as Entity;

                    if (entity == null) continue;
                    editor.WriteMessage($"Describe Entity : {i}\n");
                    editor.WriteMessage($"Entity Type: {entity.GetType().Name}\n");
                    editor.WriteMessage($"Entity Handle Value: {entity.Handle.Value.ToString()}\n");
                }

                tr.Commit();
            }
        }
    }
}