using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// App
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class App : System.Windows.Application
    {

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {


#line 4 "..\..\..\App.xaml"
            this.StartupUri = new System.Uri("View_Crtl/MainWindow.xaml", System.UriKind.RelativeOrAbsolute);

#line default
#line hidden
        }

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static void Main()
        {
            Madera_MMB.View_Crtl.App app = new Madera_MMB.View_Crtl.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
