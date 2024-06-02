﻿#pragma checksum "..\..\..\ChatPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ED5033C0FFC047B40DC6709D2E3BA52BB766BBC4"
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
    /// ChatPage
    /// </summary>
    public partial class ChatPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReturnUserPageButton;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InviteUserButton;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textblock_chatname;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LeaveChatButton;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox MessageListBox;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\ChatPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox_send;
        
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
            System.Uri resourceLocater = new System.Uri("/ChatBuddyWPF;component/chatpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ChatPage.xaml"
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
            this.ReturnUserPageButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\ChatPage.xaml"
            this.ReturnUserPageButton.Click += new System.Windows.RoutedEventHandler(this.ReturnUserPageButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.InviteUserButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\ChatPage.xaml"
            this.InviteUserButton.Click += new System.Windows.RoutedEventHandler(this.InviteUserButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.textblock_chatname = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.LeaveChatButton = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\ChatPage.xaml"
            this.LeaveChatButton.Click += new System.Windows.RoutedEventHandler(this.LeaveChatButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MessageListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 6:
            this.textbox_send = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 135 "..\..\..\ChatPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SendButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

