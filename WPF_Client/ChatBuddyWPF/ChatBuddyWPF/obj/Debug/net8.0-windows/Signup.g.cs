﻿#pragma checksum "..\..\..\Signup.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "687706810E0C4DE23EFFD00A38F7CB37A3E51A0B"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using ChatBuddyWPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ChatBuddyWPF {
    
    
    /// <summary>
    /// Signup
    /// </summary>
    public partial class Signup : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 44 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_username;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_email;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordbox_password;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_password;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkbox_showpassword;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\Signup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SignUpBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ChatBuddyWPF;component/signup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Signup.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.textbox_username = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.textbox_email = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.passwordbox_password = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 4:
            this.textbox_password = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.checkbox_showpassword = ((System.Windows.Controls.CheckBox)(target));
            
            #line 76 "..\..\..\Signup.xaml"
            this.checkbox_showpassword.Checked += new System.Windows.RoutedEventHandler(this.checkbox_showpassword_Checked);
            
            #line default
            #line hidden
            
            #line 76 "..\..\..\Signup.xaml"
            this.checkbox_showpassword.Unchecked += new System.Windows.RoutedEventHandler(this.checkbox_showpassword_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SignUpBtn = ((System.Windows.Controls.Button)(target));
            
            #line 82 "..\..\..\Signup.xaml"
            this.SignUpBtn.Click += new System.Windows.RoutedEventHandler(this.SignUpBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 112 "..\..\..\Signup.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.textbox_login_Pressed);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

