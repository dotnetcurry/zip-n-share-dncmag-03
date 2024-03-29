﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using System.Collections.Generic;
using System.IO;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(MyToolWindow))]
    [ProvideOptionPage(typeof(ZipNShareTools), "Zip N Share", "General", 101, 106, true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndNotBuildingAndNotDebugging_string)]
    [Guid(GuidList.guidZipNSharePkgString)]
    public sealed class ZipNSharePackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ZipNSharePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidZipNShareCmdSet, (int)PkgCmdIDList.cmdidZipNShareOptions);
                MenuCommand menuItem = new MenuCommand(OptionsMenuItemCallback, menuCommandID);

                CommandID menuCommandRunID = new CommandID(menuItem.CommandID.Guid, (int)PkgCmdIDList.cmdidZipNShareRun);
                MenuCommand menuItemRun = new MenuCommand(RunMenuItemCallback, menuCommandRunID);

                mcs.AddCommand(menuItemRun);
                mcs.AddCommand(menuItem);

                // Create the command for the tool window
                CommandID toolwndCommandID = new CommandID(GuidList.guidZipNShareCmdSet, (int)PkgCmdIDList.cmdidZipAndShareToolWindow);
                MenuCommand menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
                mcs.AddCommand(menuToolWin);
            }
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void RunMenuItemCallback(object sender, EventArgs e)
        {
            ArchiveFiles();
        }

        private void ArchiveFiles()
        {
            ZipNShareTools toolsPage = (ZipNShareTools)GetDialogPage(typeof(ZipNShareTools));
            string exclusions = string.Empty;
            exclusions = string.Concat(toolsPage.Exceptions);
            string folderName = toolsPage.OutputFolder;

            DTE devenv = GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;

            int numberOfFiles = 0;
            if (devenv.Solution.Count > 0)
            {
                Solution soln = devenv.Solution;
                string fullName = soln.FullName;
                string solutionFolder = Path.GetDirectoryName(soln.FullName);
                string solutionName = Path.GetFileNameWithoutExtension(soln.FullName);
                numberOfFiles = Archiving.ZipSolution.CreateArchive(solutionFolder,
                    toolsPage.Exceptions, 
                    toolsPage.OutputFileName.Replace("%SOLUTION_NAME%", solutionName), 
                    toolsPage.OutputFolder, 
                    toolsPage.OverwriteZipFileIfExists);
            }
            
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
                       0,
                       ref clsid,
                       "ZipNShare",
                       string.Format(CultureInfo.CurrentCulture, 
                       "FolderName: {0} {1}Exclusions: {2} {1}Number of Files Zipped: {3}", 
                       folderName, Environment.NewLine, exclusions, numberOfFiles),
                       string.Empty,
                       0,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                       OLEMSGICON.OLEMSGICON_INFO,
                       0,        // false
                       out numberOfFiles));
        }


        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void OptionsMenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            ZipNShareTools toolsPage = (ZipNShareTools)GetDialogPage(typeof(ZipNShareTools));
            ZipNShareConfigDialog dialog = new ZipNShareConfigDialog(toolsPage);
            bool dialogResult = (bool)dialog.ShowDialog();
            if (dialogResult && dialog.TriggerRun)
            {
                ArchiveFiles();
            }
        }
    }
}
