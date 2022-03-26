﻿namespace SimpleAirVinyl.Helpers
{
    public static class PropertyValueHelpers
    {
        public static bool HasProperty(this object instance, string propertyName)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);
            return (propertyInfo != null);
        }

        public static object GetValue(this object instance, string propertyName)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new Exception("Can't find property with name " + propertyName);
            }
            return propertyInfo!.GetValue(instance, new object[] { })!;
        }
    }
}
