﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UHack.Web.Extensions
{
    /// <summary>
    /// https://stackoverflow.com/a/6087294
    /// </summary>
    public partial class DateTimeSurrogate : IDataContractSurrogate
    {

        #region IDataContractSurrogate 成员

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(System.Reflection.MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public Type GetDataContractType(Type type)
        {
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public void GetKnownCustomDataTypes(System.Collections.ObjectModel.Collection<Type> customDataTypes)
        {

        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj.GetType() == typeof(DateTime))
            {
                DateTime dt = (DateTime)obj;
                if (dt == DateTime.MinValue)
                {
                    dt = DateTime.MinValue.ToUniversalTime();
                    return dt;
                }
                return dt;
            }
            if (obj == null)
            {
                return null;
            }
            var q = from p in obj.GetType().GetProperties()
                    where (p.PropertyType == typeof(DateTime)) && (DateTime)p.GetValue(obj, null) == DateTime.MinValue
                    select p;
            q.ToList().ForEach(p =>
            {
                p.SetValue(obj, DateTime.MinValue.ToUniversalTime(), null);
            });
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public System.CodeDom.CodeTypeDeclaration ProcessImportedType(System.CodeDom.CodeTypeDeclaration typeDeclaration, System.CodeDom.CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }

        #endregion
    }
}