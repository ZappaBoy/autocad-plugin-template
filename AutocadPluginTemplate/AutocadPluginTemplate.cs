using System;
using AutocadPluginTemplate.Core;
using AutocadPluginTemplate.Utils;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

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

        [CommandMethod("DescribeSelection")]
        public void DescribeSelectionCommand()
        {
            _core.GetSelectedElementsInfo();
        }
    }
}