using System;
using System.Reflection;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace AutocadPluginTemplate.Utils
{
    public static class AutocadPluginUtils
    {
        static readonly string NewLine = Environment.NewLine;
        static readonly string InfoMessageDelimiter = $"#####{NewLine}";

        public static Editor GetCurrentEditor()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            return editor;
        }

        public static void ThrowAutocadException(string message, ErrorStatus status = ErrorStatus.OK)
        {
            Exception ex = new Exception(status, message);
            throw ex;
        }


        public static Point3d GetPointFormPrompt(string message)
        {
            Editor editor = GetCurrentEditor();
            PromptPointOptions baseOptions = new PromptPointOptions($"{NewLine}{message}");
            PromptPointResult ppr = editor.GetPoint(baseOptions);
            if (ppr.Status != PromptStatus.OK)
                ThrowAutocadException("Error getting point from prompt", ErrorStatus.InvalidInput);
            Point3d point = ppr.Value;
            return point;
        }

        public static Double GetDoubleFormPrompt(string message)
        {
            Editor editor = GetCurrentEditor();
            PromptDoubleOptions baseOptions = new PromptDoubleOptions($"{NewLine}{message}");
            PromptDoubleResult ppr = editor.GetDouble(baseOptions);
            if (ppr.Status != PromptStatus.OK)
                ThrowAutocadException("Error getting point from prompt", ErrorStatus.InvalidInput);
            Double point = ppr.Value;
            return point;
        }

        public static Database GetDatabase()
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            var database = document.Database;
            return database;
        }

        public static string Dump(DBObject obj)
        {
            string message = $"Properties of the {obj.GetType().Name} with handle {obj.Handle}:{NewLine}";

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object value = null;
                try
                {
                    value = property.GetValue(obj, null);
                }
                catch (System.Exception ex)
                {
                    if (ex.InnerException is Exception &&
                        ((Exception)ex.InnerException).ErrorStatus == ErrorStatus.NotApplicable)
                        continue;
                }

                message += $"{property.Name}: {value}{NewLine}";
            }

            return message;
        }

        public static string GetSelectedElementsInfo()
        {
            Editor editor = GetCurrentEditor();
            Database database = GetDatabase();
            PromptSelectionResult selectionResult = editor.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) return "";
            SelectionSet selectionSet = selectionResult.Value;
            string selectedElementsInfo = "";
            using (var tr = database.TransactionManager.StartTransaction())
            {
                int i = 0;
                foreach (SelectedObject selObj in selectionSet)
                {
                    i++;
                    if (selObj == null) continue;
                    ObjectId objId = selObj.ObjectId;
                    Entity entity = tr.GetObject(objId, OpenMode.ForRead) as Entity;

                    if (entity == null) continue;
                    string objectInfo = $"{InfoMessageDelimiter}";

                    objectInfo += $"Describe Entity : {i}{NewLine}";
                    objectInfo += $"Entity Type: {entity.GetType().Name}{NewLine}";
                    objectInfo += Dump(entity);

                    objectInfo += $"{InfoMessageDelimiter}{NewLine}";
                    selectedElementsInfo += objectInfo;
                }

                tr.Commit();
            }

            return selectedElementsInfo;
        }
    }
}