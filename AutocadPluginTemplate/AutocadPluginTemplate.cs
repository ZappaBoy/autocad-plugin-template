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
using AutocadPluginTemplate.Core;

namespace AutocadPluginTemplate
{
    
    public class AutocadPluginTemplate
    {
        private readonly AutocadPluginCore _core = new AutocadPluginCore();
        [CommandMethod("Hello")]
        public void HelloCommand()
        {
            Editor editor = AutocadPluginUtils.GetCurrentEditor();
            editor.WriteMessage("Hello World!");
            Application.ShowAlertDialog("Hello World!");
        }

        [CommandMethod("CreateCube")]
        public void CreateCubeCommand()
        {
            Point3d basePoint = AutocadPluginUtils.GetPointFormPrompt("Select the base point of the cube: ");
            Double sideLength = AutocadPluginUtils.GetDoubleFormPrompt("Enter the side length: ");
            _core.DrawCube(basePoint, sideLength);
        }
        
    }   
}
