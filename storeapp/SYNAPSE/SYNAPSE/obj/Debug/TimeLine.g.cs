﻿

#pragma checksum "C:\Users\tomoki\documents\visual studio 2013\Projects\SYNAPSE\SYNAPSE\TimeLine.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "98673BB770096A72D424B18B34BED0B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SYNAPSE
{
    partial class TimeLine : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 50 "..\..\TimeLine.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyUp += this.TweetDone;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 51 "..\..\TimeLine.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.TweetButton_clik;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


