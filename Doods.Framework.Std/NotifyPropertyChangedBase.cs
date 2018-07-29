using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Doods.Framework.Std
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Handler sur le changement d'une propriété
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Méthode appelé lors du changement d'une propriété
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        ///     Affecte une valeur pour la propriété property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <param name="name"></param>
        /// <param name="raise"></param>
        protected bool SetProperty<TProperty>(ref TProperty currentValue, TProperty newValue,
            [CallerMemberName] string name = "", bool raise = false)
        {
            if (EqualityComparer<TProperty>.Default.Equals(currentValue, newValue) && !raise) return false;
            currentValue = newValue;
            OnPropertyChanged(name);
            return true;
        }
    }
}
