using NasuTek.DevEnvironment.Extensibility.Workbench;
using System;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility.Project;

namespace NasuTek.DevEnvironment.Extensibility
{
    public interface IDevEnvPackageSvc
    {
        void AddProduct(Product prod);
        void AddUpdate(Update upd);
        /// <summary>
        /// Allows you to attach an AbstractCommand to a command group for extending a command.
        /// </summary>
        /// <param name="commGroup">
        /// Can be BeforeEnvironmentInitialized, BeforeInitialization, AfterInitialization, Finalize, 
        /// or a custom group if an addin provides its own Command group.
        /// </param>
        /// <param name="command">AbstractCommand to attach to the command group</param>
        void AttachCommand(string commGroup, AbstractCommand command);
    }

    public interface IDevEnvLoggingSvc
    {
        void Info(string message);
        void Error(string message);
        void Error(Exception exception);
        void Warning(string message);
        void ShowDialog(string message, MessageType type);
    }

    public interface IDevEnvSolutionSvc
    {
        bool HideSolutionRoot { get; set; }
        ISolution ActiveSolution { get; set; }
        void RemoveObject(TreeNode folderNode, TreeNode deleteNode, IObject objId, IProject project);
        void RemoveFolder(TreeNode folderNode, TreeNode deleteNode, IFolder folder, IProject project);
        void AddObject(TreeNode folderNode, IObject objId, IProject project);
        void AddFolder(TreeNode folderNode, IFolder folder, IProject project);
        void DeleteSelectedNode();
        void OpenDocument(DocumentMetadata meta);
    }

    public interface IDevEnvUISvc
    {
        void AddToolbar(ToolBar toolbar);
        ToolBar GetToolBar(string name);
        void AddRootMenuItem(MenuItem menuItem);
        MenuItem GetRootMenuItem(string id);
        void RegisterPane(DevEnvPane pane);
        DevEnvPane GetPane(string id);
    }

    public enum SettingsReg
    {
        Global,
        User,
    }

    public interface IDevEnvRegSvc
    {
        IDevEnvRegSubKey OpenSubKey(SettingsReg reg, string keyName);
        bool SubKeyExists(SettingsReg reg, string keyName);
        void CreateSubKey(SettingsReg reg, string keyName);
    }

    public interface IDevEnvRegSubKey
    {
        IDevEnvRegSubKey[] GetSubKeys();
        IDevEnvRegSubKey OpenSubKey(string keyName);
        bool SubKeyExists(string keyName);
        void CreateSubKey(string keyName);
        object GetValue(string valueName);
        object GetValue(string valueName, object defaultValue);
        void SetValue(string valueName, object value);
        string Name { get; }
    }
}
