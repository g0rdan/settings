using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Acr.Settings
{

    public abstract class AbstractSettings : ISettings
    {
        public string Environment { get; set; }
        public abstract bool Contains(string key);
        protected abstract object NativeGet(Type type, string key);
        protected abstract void NativeSet(Type type, string key, object value);
        protected abstract void NativeRemove(string[] keys);
        protected abstract IDictionary<string, string> NativeValues();


        public virtual object GetValue(Type type, string key, object defaultValue = null)
        {
            try
            {
                if (!this.Contains(key))
                    return defaultValue;

                var actualType = this.UnwrapType(type);
                var value = this.NativeGet(actualType, key);
                return value;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error getting key: {key}", ex);
            }
        }


        public virtual T Get<T>(string key, T defaultValue = default(T))
        {
            try
            {
                if (!this.Contains(key))
                    return defaultValue;

                var type = this.UnwrapType(typeof(T));
                var value = this.NativeGet(type, key);
                return (T)value;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error getting key: {key}", ex);
            }
        }


        public virtual T GetRequired<T>(string key)
        {
            if (!this.Contains(key))
                throw new ArgumentException($"Settings key '{key}' is not set");

            return this.Get<T>(key);
        }


        public virtual void SetValue(string key, object value)
        {
            try
            {
                if (value == null)
                {
                    this.Remove(key);
                }
                else
                {
                    var type = this.UnwrapType(value.GetType());
                    this.NativeSet(type, key, value);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error setting key {key} with value {value}", ex);
            }
        }


        public virtual void Set<T>(string key, T value)
        {
            try
            {
                if (EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    this.Remove(key);
                }
                else
                {
                    var type = this.UnwrapType(typeof(T));
                    this.NativeSet(type, key, value);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error setting key {key} with value {value}", ex);
            }
        }


        public virtual bool SetDefault<T>(string key, T value)
        {
            if (this.Contains(key))
                return false;

            var type = this.UnwrapType(typeof(T));
            this.NativeSet(type, key, value);
            return true;
        }


        public virtual bool Remove(string key)
        {
            if (!this.Contains(key))
                return false;

            this.NativeRemove(new[] { key });
            return true;
        }


        protected virtual string Serialize(Type type, object value)
        {
            if (type == typeof(string))
                return (string)value;

            if (this.IsStringifyType(type))
            {
                var format = value as IFormattable;
                return format == null
                    ? value.ToString()
                    : format.ToString(null, CultureInfo.InvariantCulture);
            }
            return "TODO"; // TODO
        }


        protected virtual object Deserialize(Type type, string value)
        {
            if (type == typeof(string))
                return value;

            if (this.IsStringifyType(type))
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            return "TODO"; // TODO
        }


        protected virtual Type UnwrapType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            return type;
        }


        protected virtual bool IsStringifyType(Type t)
        {
            return (
                t == typeof(DateTime) ||
                t == typeof(DateTimeOffset) ||
                t == typeof(bool) ||
                t == typeof(short) ||
                t == typeof(int) ||
                t == typeof(long) ||
                t == typeof(double) ||
                t == typeof(float) ||
                t == typeof(decimal)
            );
        }


        public virtual T Bind<T>() where T : INotifyPropertyChanged, new()
        {
            var obj = new T();
            this.Bind(obj);
            return obj;
        }


        public virtual void Bind(INotifyPropertyChanged obj)
        {
            var type = obj.GetType();
            var prefix = type.Name + ".";
            var props = this.GetTypeProperties(type);

            foreach (var prop in props)
            {
                var key = prefix + prop.Name;
                if (this.Contains(key))
                {
                    var value = this.GetValue(prop.PropertyType, key);
                    prop.SetValue(obj, value);
                }
            }
            obj.PropertyChanged += this.OnPropertyChanged;
        }


        public virtual void UnBind(INotifyPropertyChanged obj)
        {
            obj.PropertyChanged -= this.OnPropertyChanged;
        }


        protected virtual IEnumerable<PropertyInfo> GetTypeProperties(Type type)
        {
            return type
                .GetTypeInfo()
                .DeclaredProperties
                .Where(x => x.CanRead && x.CanWrite);
        }


        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var prop = this
                .GetTypeProperties(sender.GetType())
                .FirstOrDefault(x => x.Name.Equals(args.PropertyName));

            if (prop != null)
            {
                var key = $"{sender.GetType().Name}.{prop.Name}";
                var value = prop.GetValue(sender);
                this.SetValue(key, value);
            }
        }
    }
}