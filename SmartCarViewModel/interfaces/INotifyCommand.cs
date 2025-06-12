namespace SmartCarViewModel.interfaces
{
    public interface INotifyCommand
    {
        void InvalidateRequerySuggested();
        void Register(EventHandler value);
        void Unregister(EventHandler value);
        void Dispatcher(Action action);
    }
}