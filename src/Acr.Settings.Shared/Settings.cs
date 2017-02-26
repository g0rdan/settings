using System;


namespace Acr.Settings
{
    public static class Settings
    {


        static ISettings current;
        public static ISettings Current
        {
            get
            {
#if PORTABLE
                if (current == null)
                    throw new ArgumentException("Platform plugin not found.  Did you reference the nuget package in your platform project?");

                return current;
#else
                current = current ?? new DefaultSettings();
                return current;
#endif
            }
            set { current = value; }
        }
    }
}
