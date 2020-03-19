using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AlbumCreator.Annotations;

namespace AlbumCreator.WpfElements
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            Commands = new List<RelayCommand>();
        }

        public List<RelayCommand> Commands { get; set; }
        public RelayCommand RegisterCommand(Action action, Func<bool> func)
        {
            var command = new RelayCommand(action, func);
            Commands.Add(command);
            return command;
        }

        public void RefreshCommands()
        {
            foreach (var command in Commands)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
