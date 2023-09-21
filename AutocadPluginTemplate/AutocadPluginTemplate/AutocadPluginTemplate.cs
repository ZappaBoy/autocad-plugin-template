using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace AutocadPluginTemplate
{
    public class AutocadPluginTemplate
    {
        [CommandMethod("hello")]
        public void HelloCommand()
        {
            Document document = Application.DocumentManager.MdiActiveDocument;
            Editor editor = document.Editor;
            editor.WriteMessage("Hello World!");
            Application.ShowAlertDialog("Hello World!");
        }
    }   
}
