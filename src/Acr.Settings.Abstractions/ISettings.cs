using System;
using System.ComponentModel;


namespace Acr.Settings
{

    public interface ISettings
    {
        /// <summary>
        /// WARNING: setting this will prefix ALL get/set setting values
        /// </summary>
        string Environment { get; set; }


        /// <summary>
        /// Gets an object from settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T Get<T>(string key, T defaultValue = default(T));


        /// <summary>
        /// Loosely typed version of Get
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        object GetValue(Type type, string key, object defaultValue = null);


        /// <summary>
        /// Enforces that the key is set and returns value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetRequired<T>(string key);


        /// <summary>
        /// Send any value/object to the settings store
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set<T>(string key, T value);


        /// <summary>
        /// Loosely typed version of Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetValue(string key, object value);


        /// <summary>
        /// This will only set the value if the setting is not currently set.  Will not fire Changed event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool SetDefault<T>(string key, T value);


        /// <summary>
        /// Remove the setting by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);


        /// <summary>
        /// Checks if the setting key is set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);


        /// <summary>
        /// Will create a bound object set FROM the settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Bind<T>() where T : INotifyPropertyChanged, new();


        /// <summary>
        /// Bind to existing instance
        /// </summary>
        /// <param name="obj"></param>
        void Bind(INotifyPropertyChanged obj);


        /// <summary>
        /// Unbinds an object from monitoring
        /// </summary>
        /// <param name="obj"></param>
        void UnBind(INotifyPropertyChanged obj);
    }
}
