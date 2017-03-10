
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VectorDraw.Actions;
using VectorDraw.Geometry;

namespace VectorDrawApp.Commands
{
    public class PrintCommand : VectorDrawCommand
    {
        public override string CommandName { get; } = "Auto Print Rect to EMF";
        public override object Execute(vdControls.vdFramedControl vdFramedControl)
        {
            var commandLine = vdFramedControl.CommandLine;
            while (true)
            {
                var box = GetBoundingBox(vdFramedControl);
                if (box == null)
                {
                    commandLine.History.AppendText("\r\n");
                    commandLine.History.AppendText("Quit");
                    break;
                }
                ExecuteLoop(vdFramedControl, box);
            }
            return null;
        }

        private void ExecuteLoop(vdControls.vdFramedControl vdFramedControl, Box box)
        {
            var commandLine = vdFramedControl.CommandLine;
            var document = vdFramedControl.BaseControl.ActiveDocument;
            var layout = document.ActiveLayOut;

            commandLine.History.AppendText("\r\n");
            commandLine.History.AppendText($"BoundingBox = {box}");
            commandLine.History.AppendText("\r\n");

            var emfWidth = 500; //fixed width
            var emfHeight = (int)(emfWidth * box.Height / box.Width); // keep propotions

            var fileDir = Path.Combine(Application.StartupPath, "Print Results");
            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);
            var filePath = Path.Combine(fileDir, DateTime.Now.ToFileTime() + ".wmf");
            commandLine.History.AppendText($"File Path = {filePath}");
            commandLine.History.AppendText("\r\n");

            //导出EMF
            layout.Printer.InitializeProperties();
            layout.Printer.PrinterName = filePath;
            layout.Printer.Resolution = 96; //Screen DPI
            layout.Printer.paperSize = new Rectangle(0, 0,
                emfWidth * 100 / layout.Printer.Resolution,
                emfHeight * 100 / layout.Printer.Resolution);
            layout.Printer.PrintWindow = box;
            layout.Printer.PrintOut();
            commandLine.History.AppendText($"File Exist = {File.Exists(filePath)}");
            commandLine.History.AppendText("\r\n");
        }


        private Box GetBoundingBox(vdControls.vdFramedControl vdFramedControl)
        {
            var document = vdFramedControl.BaseControl.ActiveDocument;
            gPoint gp1;
            if (document.ActionUtility.getUserPoint(out gp1) != StatusCode.Success)
                return null;
            Box box;
            if (document.ActionUtility.getUserRectViewCS(gp1, out box) != StatusCode.Success)
                return null;
            return box;
        }
    }
}