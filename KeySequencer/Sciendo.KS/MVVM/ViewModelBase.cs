using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Sciendo.KS.MVVM
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Event Handlers

        /// <summary>
        /// Get name of property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }
        /// <summary>
        /// Raise when property value propertychanged or override propertychage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>
            (Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }
        /// <summary>
        /// Raise when property value propertychanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            temp?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
