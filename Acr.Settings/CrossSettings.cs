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
#if NETSTANDARD10
                if (current == null)
                    throw new ArgumentException("[Acr.Settings] Platform plugin not found.  Did you reference the nuget package in your platform project?");
#else
                current = current ?? new DefaultSettings();
#endif
                return current;
            }
            set { current = value; }
        }
    }
}
