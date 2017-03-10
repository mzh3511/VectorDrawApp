using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ranplan.iBuilding.TeighaApp.Models;
using Teigha.DatabaseServices;
using Teigha.GraphicsInterface;
using Teigha.GraphicsSystem;
using Teigha.Runtime;

namespace Ranplan.iBuilding.TeighaApp.Forms
{
    public partial class FormDwgReader : Form
    {
        private string _defaultTitle = string.Empty;
        private LayoutHelperDevice _helperDevice = null;
        private TeighaDocument _document = null;
        public FormDwgReader()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            //SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            UpdateStyles();
        }

        private void UpdateFormTitle(string filePath)
        {
            Text = string.IsNullOrEmpty(filePath) ? _defaultTitle : $"{_defaultTitle} [{Path.GetFileName(filePath)}]";
        }

        private void InitializeGraphics(Database database)
        {
            try
            {
                var graphics = Graphics.FromHwnd(grpxView.Handle);
                // load some predefined rendering module (may be also "WinDirectX" or "WinOpenGL")
                using (var gsModule = (GsModule)SystemObjects.DynamicLinker.LoadModule("WinGDI_3.03_9.txv", false, true))
                {
                    // create graphics device
                    using (var graphichsDevice = gsModule.CreateDevice())
                    {
                        // setup device properties
                        using (Dictionary props = graphichsDevice.Properties)
                        {
                            if (props.Contains("WindowHWND")) // Check if property is supported
                                props.AtPut("WindowHWND", new RxVariant((int)grpxView.Handle)); // hWnd necessary for DirectX device
                            if (props.Contains("WindowHDC")) // Check if property is supported
                                props.AtPut("WindowHDC", new RxVariant(graphics.GetHdc())); // hWindowDC necessary for Bitmap device
                            if (props.Contains("DoubleBufferEnabled")) // Check if property is supported
                                props.AtPut("DoubleBufferEnabled", new RxVariant(true));
                            if (props.Contains("EnableSoftwareHLR")) // Check if property is supported
                                props.AtPut("EnableSoftwareHLR", new RxVariant(true));
                            if (props.Contains("DiscardBackFaces")) // Check if property is supported
                                props.AtPut("DiscardBackFaces", new RxVariant(true));
                        }
                        // setup paperspace viewports or tiles
                        ContextForDbDatabase ctx = new ContextForDbDatabase(database) { UseGsModel = true };

                        _helperDevice = LayoutHelperDevice.SetupActiveLayoutViews(graphichsDevice, ctx);
                        //helperDevice.ActiveView.Mode = Teigha.GraphicsSystem.RenderMode.HiddenLine;
                    }
                }
                // set palette
                _helperDevice.SetLogicalPalette(Device.DarkPalette);
                _helperDevice.Model.Invalidate(InvalidationHint.kInvalidateAll);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;
            _document = new TeighaDocument();
            if (!_document.Load(dialog.FileName))
            {
                _document = null;
                MessageBox.Show("加载文件错误");
                return;
            }
            UpdateFormTitle(dialog.FileName);
            InitializeGraphics(_document.Database);
        }

        private void FormDwgReader_Load(object sender, EventArgs e)
        {
            _defaultTitle = Text;
        }

        private void grpxView_Paint(object sender, PaintEventArgs e)
        {
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            if (_helperDevice == null)
            {
                e.Graphics.DrawString("没有合适的LayoutHelperDevice", SystemFonts.DefaultFont, Brushes.Blue, e.ClipRectangle, stringFormat);
                return;
            }
            try
            {
                e.Graphics.DrawString("11111111111115555555555555555555555555555555555", SystemFonts.DefaultFont, Brushes.Blue, e.ClipRectangle, stringFormat);
                //var props = _helperDevice.Properties;
                //if (props.Contains("WindowHDC")) // Check if property is supported
                //    props.AtPut("WindowHDC", new RxVariant(e.Graphics.GetHdc())); // hWindowDC necessary for Bitmap device
                _helperDevice.OnSize(e.ClipRectangle);
                _helperDevice.Update();
                e.Graphics.DrawString("333333333333333333333", SystemFonts.DefaultFont, Brushes.Blue, e.ClipRectangle, stringFormat);
                e.Graphics.ReleaseHdc();
            }
            catch (System.Exception ex)
            {
                e.Graphics.DrawString(ex.ToString(), SystemFonts.DefaultFont, Brushes.Red, e.ClipRectangle);
            }
        }
    }
}
