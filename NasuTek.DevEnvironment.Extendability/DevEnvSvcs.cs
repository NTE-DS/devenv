using NasuTek.DevEnvironment.Extendability.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NasuTek.DevEnvironment.Extendability
{
    public interface IDevEnvPluginSvc
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
        /// <summary>
        /// Allows changing the DevEnv Settings/Addin Registry to any other method then the Windows
        /// Registry.
        /// 
        /// This command cannot be executed after Environment Initialization. Addins cannot execute
        /// this command as it will cause an exception.
        /// </summary>
        /// <param name="reg">IDevEnvReg object</param>
        void ChangeDevEnvRegistry(IDevEnvReg reg);
    }

    public interface IDevEnvSolutionSvc
    {
        void OpenDocument(DocumentMetadata meta);
    }

    public interface IDevEnvUISvc
    {
        void AddRootMenuItem(MenuItem menuItem);
        MenuItem GetRootMenuItem(string id);
        void RegisterPane(DevEnvPane pane);
        DevEnvPane GetPane(string id);
    }

    public interface IDevEnvReg
    {
        IDevEnvRegSubKey OpenSubKey(string v);
    }

    public interface IDevEnvRegSubKey
    {
        string[] GetSubKeyNames();

        IDevEnvRegSubKey OpenSubKey(string keyName);

        object GetValue(string valueName);
    }
}
