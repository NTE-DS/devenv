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
        
        public IDevEnvRegSubKey OpenSubKey(SettingsReg reg, string keyName)
        {
            switch (reg)
            {
                case SettingsReg.Global:
                    return new DevEnvRegSubKey(keyName, regKeyObj, false);
                case SettingsReg.User:
                    if (!SubKeyExists(reg, keyName))
                        userKeyObj.CreateSubKey(keyName);
                    return new DevEnvRegSubKey(keyName, userKeyObj, true);
                default:
                    return new DevEnvRegSubKey(keyName, regKeyObj, false);
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

        public DevEnvReg(string productId, string version)
        {
            regKeyObj = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version);
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