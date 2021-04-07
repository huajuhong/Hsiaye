using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Hsiaye.Extensions
{
    public static class ExpressionGenericMapper<TIn, TOut>
    {
        private static readonly BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;
        private static readonly Func<TIn, TOut> _func = null;
        static ExpressionGenericMapper()
        {

            Type tin = typeof(TIn);
            Type tout = typeof(TOut);
            MemberExpression property;//要访问的字段或属性
            MemberBinding memberBinding;//创建新对象成员
            ParameterExpression parameterExpression = Expression.Parameter(tin, "p");//参数表达式
            List<MemberBinding> memberBindingList = new List<MemberBinding>();//用于创建新对象的成员（属性或字段）
            PropertyInfo propertyInfo;
            foreach (var outPropertyInfo in tout.GetProperties())
            {
                propertyInfo = tin.GetProperty(outPropertyInfo.Name, flag);
                if (propertyInfo == null)
                    continue;
                if (!propertyInfo.CanWrite)
                    continue;
                //if (item.PropertyType.Name != propertyInfo.PropertyType.Name)
                //{  }
                property = Expression.Property(parameterExpression, propertyInfo);
                try
                {
                    memberBinding = Expression.Bind(outPropertyInfo, property);
                }
                catch (Exception ex)
                {
                    memberBinding = null;
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("字段").Append(propertyInfo.Name).Append("类型不一致：");
                    stringBuilder.Append("输入类型：").Append(propertyInfo.Name);
                    stringBuilder.Append("输出类型：").Append(outPropertyInfo.Name);
                    throw ex;
                }
                memberBindingList.Add(memberBinding);
            }
            FieldInfo fieldInfo;
            foreach (var item in tout.GetFields())
            {
                fieldInfo = tin.GetField(item.Name, flag);
                if (fieldInfo == null)
                    continue;
                property = Expression.Field(parameterExpression, fieldInfo);
                memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(tout), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                parameterExpression
            });
            _func = lambda.Compile();//拼装是一次性的

        }
        public static TOut MapperTo(TIn obj)
        {
            if (obj == null)
                return default;
            return _func(obj);
        }
        public static List<TOut> MapperTo(List<TIn> list)
        {
            if (list == null || list.Count < 1)
                return default;
            List<TOut> temp = new List<TOut>();
            list.ForEach(obj =>
            {
                temp.Add(_func(obj));
            });
            return temp;
        }
    }
}
