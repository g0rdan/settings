using System;


namespace Acr.Settings
{

    public static class CrossSettings
    {
        static ISettings current;
        public static ISettings Current
        {
            get
            {
#if PCL
                throw new ArgumentException("[Acr.Settings] Platform plugin not found.  Did you reference the nuget package in your platform project?");
#elif NETFULL
                current = current ?? new AppConfigSettingsImpl();
                return current;
#else
                current = current ?? new SettingsImpl();
                return current;
#endif

            }
            set { current = value; }
        }
    }
}
