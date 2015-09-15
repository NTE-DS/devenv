using Microsoft.Win32;
using NasuTek.DevEnvironment.Extendability;
using System;

namespace NasuTek.DevEnvironment
{
    public class DevEnvReg : IDevEnvReg
    {
        RegistryKey regKeyObj;

        public IDevEnvRegSubKey OpenSubKey(string v)
        {
            return new DevEnvRegSubKey(v, regKeyObj);
        }

        public DevEnvReg(string productId, string version)
        {
            regKeyObj = Registry.LocalMachine.OpenSubKey("SOFTWARE\\NasuTek Enterprises\\" + productId + "\\" + version);
        }
    }

    public class DevEnvRegSubKey : IDevEnvRegSubKey
    {
        RegistryKey regKeyObj;

        public string[] GetSubKeyNames()
        {
            return regKeyObj.GetSubKeyNames();
        }

        public IDevEnvRegSubKey OpenSubKey(string keyName)
        {
            return new DevEnvRegSubKey(keyName, regKeyObj);
        }

        public object GetValue(string valueName)
        {
            return regKeyObj.GetValue(valueName);
        }

        public DevEnvRegSubKey(string key, RegistryKey root)
        {
            regKeyObj = root.OpenSubKey(key);
        }
    }
}