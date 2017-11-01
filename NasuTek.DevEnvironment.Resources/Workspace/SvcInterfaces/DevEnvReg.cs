using Microsoft.Win32;
using NasuTek.DevEnvironment.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NasuTek.DevEnvironment.Svcs
{
    public class DevEnvReg : IDevEnvRegSvc
    {
        RegistryKey regKeyObj;
        RegistryKey userKeyObj;
        private bool installMode;

        public IDevEnvRegSubKey OpenSubKey(SettingsReg reg, string keyName)
        {
            switch (reg)
            {
                case SettingsReg.Global:
                    return new DevEnvRegSubKey(keyName, regKeyObj, installMode);
                case SettingsReg.User:
                    if (!SubKeyExists(reg, keyName))
                        userKeyObj.CreateSubKey(keyName);
                    return new DevEnvRegSubKey(keyName, userKeyObj, true);
                default:
                    return new DevEnvRegSubKey(keyName, regKeyObj, installMode);
            }
        }

        public bool SubKeyExists(SettingsReg reg, string keyName)
        {
            switch (reg)
            {
                case SettingsReg.Global:
                    return regKeyObj.GetSubKeyNames().Contains(keyName);
                case SettingsReg.User:
                    return userKeyObj.GetSubKeyNames().Contains(keyName);
                default:
                    return regKeyObj.GetSubKeyNames().Contains(keyName);
            }
        }

        public void CreateSubKey(SettingsReg reg, string keyName)
        {
            switch (reg)
            {
                case SettingsReg.Global:
                    regKeyObj.CreateSubKey(keyName);
                    break;
                case SettingsReg.User:
                    userKeyObj.CreateSubKey(keyName);
                    break;
                default:
                    regKeyObj.CreateSubKey(keyName);
                    break;
            }
        }

        public DevEnvReg(string productId, string version) : this(productId, version, false)
        {
        }

        public DevEnvReg(string productId, string version, bool install)
        {
            installMode = install;

            if (installMode)
                Registry.LocalMachine.CreateSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version);

            regKeyObj = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version, install);
            userKeyObj = Registry.CurrentUser.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version, true);
            if (userKeyObj == null)
                userKeyObj = Registry.CurrentUser.CreateSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version);
        }
    }

    public class DevEnvRegSubKey : IDevEnvRegSubKey
    {
        RegistryKey regKeyObj;
        bool isWritable;
        public string Name
        {
            get
            {
                var regS = regKeyObj.Name.Split('\\');
                return regS[regS.Length - 1];
            }
        }
        
        public IDevEnvRegSubKey OpenSubKey(string keyName)
        {
            return new DevEnvRegSubKey(keyName, regKeyObj, isWritable);
        }

        public void CreateSubKey(string keyName)
        {
            regKeyObj.CreateSubKey(keyName);
        }

        public object GetValue(string valueName)
        {
            return regKeyObj.GetValue(valueName);
        }

        public IDevEnvRegSubKey[] GetSubKeys()
        {
            var retval = new List<IDevEnvRegSubKey>();
            foreach(var i in regKeyObj.GetSubKeyNames())
            {
                retval.Add(new DevEnvRegSubKey(i, regKeyObj, isWritable));
            }
            return retval.ToArray();
        }

        public void SetValue(string valueName, object value)
        {
            regKeyObj.SetValue(valueName, value);
        }

        public bool SubKeyExists(string keyName)
        {
            return regKeyObj.GetSubKeyNames().Contains(keyName);
        }

        public object GetValue(string valueName, object defaultValue)
        {
            return regKeyObj.GetValue(valueName, defaultValue);
        }

        public DevEnvRegSubKey(string key, RegistryKey root, bool writable)
        {
            isWritable = writable;
            regKeyObj = root.OpenSubKey(key, isWritable);
        }
    }
}