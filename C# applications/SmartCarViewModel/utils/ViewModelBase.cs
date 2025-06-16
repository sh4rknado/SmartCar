using SmartCarViewModel.interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartCarViewModel.utils
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly INotifyCommand? _notifyCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ViewModelBase(INotifyCommand? notifyCommand=null)
        {
            _notifyCommand = notifyCommand;
        }

        public INotifyCommand? NotifyCommand => _notifyCommand;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
