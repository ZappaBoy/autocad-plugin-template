using System;
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
        public static Editor GetCurrentEditor()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            return editor;
        }

        public static void ThrowAutocadException(String message, ErrorStatus status = ErrorStatus.OK)
        {
            Exception ex = new Exception(status, message);
            throw ex;
        }


        public static Point3d GetPointFormPrompt(String message)
        {
            Editor editor = GetCurrentEditor();
            PromptPointOptions baseOptions = new PromptPointOptions($"\n{message}");
            PromptPointResult ppr = editor.GetPoint(baseOptions);
            if (ppr.Status != PromptStatus.OK)
                ThrowAutocadException("Error getting point from prompt", ErrorStatus.InvalidInput);
            Point3d point = ppr.Value;
            return point;
        }

        public static Double GetDoubleFormPrompt(String message)
        {
            Editor editor = GetCurrentEditor();
            PromptDoubleOptions baseOptions = new PromptDoubleOptions($"\n{message}");
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
    }
}