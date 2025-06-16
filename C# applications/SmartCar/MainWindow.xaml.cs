using SmartCarViewModel;
using SmartCarViewModel.interfaces;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace SmartCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyCommand
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CarViewModel(this);
        }

        public void InvalidateRequerySuggested() => CommandManager.InvalidateRequerySuggested();
        public void Register(EventHandler value) => CommandManager.RequerySuggested += value;
        public void Unregister(EventHandler value) => CommandManager.RequerySuggested -= value;

        void INotifyCommand.Dispatcher(Action action) 
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if(DataContext is CarViewModel carViewModel)
            //{
            //    IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            //    carViewModel.SmartCarController.SetMainWindowHandle(windowHandle);
            //}
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(DataContext is CarViewModel carViewModel)
            {
                carViewModel.SmartCarController.Dispose();
            }
        }
    }
}