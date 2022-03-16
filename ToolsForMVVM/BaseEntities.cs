using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace MVVM
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {

        }

        // на это событие будут обновляться выражения привязки
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {

        }
    }    

    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canexecute;


        //конструктор с двумя параметрами
        public RelayCommand(Action<object> execute, Predicate<object> canexecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }
            _execute = execute;
            _canexecute = canexecute;
        }

        // Ensures WPF commanding infrastructure asks all RelayCommand objects whether their
        // associated views should be enabled whenever a command is invoked 
        // Гарантирует, что командная инфраструктура WPF запрашивает все объекты RelayCommand,
        // должны ли их связанные представления быть включены всякий раз, когда вызывается команда
        public event EventHandler CanExecuteChanged
        {
            add
            {

                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested += value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canexecute == null || _canexecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
