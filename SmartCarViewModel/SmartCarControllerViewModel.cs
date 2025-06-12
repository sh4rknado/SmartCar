using SharpDX.DirectInput;
using SmartCarModel;
using SmartCarProtocol.data;
using SmartCarProtocolClient;
using SmartCarViewModel.interfaces;
using SmartCarViewModel.utils;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace SmartCarViewModel
{
    public class SmartCarControllerViewModel : ViewModelBase, IDisposable
    {
        private readonly SmartCarSettingsViewModel _config;
        private readonly SmartCarParameters _parameters;
        private readonly ISaveConfiguration _saveParameter;
        private readonly ProtocolWriter _setParameters;
        private DirectInput _directInput;
        private Joystick _steeringWheelDevice;
        private double _wheelPosition;
        private double _motorPosition;
        private double scale;
        private string _streamPath;

        public SmartCarControllerViewModel(SmartCarSettingsViewModel config, ISaveConfiguration parameter, INotifyCommand? notifyCommand = null): base(notifyCommand)
        {
            ArgumentNullException.ThrowIfNull(config);
            ArgumentNullException.ThrowIfNull(parameter);

            _config = config;
            _saveParameter = parameter;
            _parameters = new(_config.CameraXMin, _config.CameraYMin, _config.DirectionMin, _config.MotorMin);
            _setParameters = new ProtocolWriter();
            _directInput = new DirectInput();
            DirectionCommand = new RelayCommandAsync<Direction>(DirectionCommandExecuted, CanDirectionCommandExecuted, notifyCommand);
            StartCommand = new RelayCommand(StartCommandExecuted, CanStartCommandExecuted, notifyCommand);

            Scale = 22.5;
        }

        #region COMMANDS
        public ICommand StartCommand { get; }
        public ICommand DirectionCommand { get; }

        private bool CanDirectionCommandExecuted(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return _parameters.CameraX > _config.CameraXMin;
                case Direction.Right:
                    return _parameters.CameraX < _config.CameraXMax;
                case Direction.Up:
                    return _parameters.CameraY < _config.CameraYMax;
                case Direction.Down:
                    return _parameters.CameraY > _config.CameraYMin;
                default:
                    return false;
            }
        }

        private bool CanStartCommandExecuted(object obj) => _config.SelectedController != null;
        
        #endregion

        public string StreamPath
        {
            get => _streamPath;
            set
            {
                if(_streamPath  != value)
                {
                    _streamPath = value;
                    OnPropertyChanged(nameof(StreamPath));
                }
            }
        }
        
        public double Scale
        {
            get => scale;
            set
            {
                if (value != scale)
                {
                    scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        public double WheelPosition
        {
            get => _wheelPosition;
            set
            {
                if (_wheelPosition != value)
                {
                    _wheelPosition = value;
                    OnPropertyChanged(nameof(WheelPosition));
                }
            }
        }

        public double MotorPosition
        {
            get => _motorPosition;
            set
            {
                if (_motorPosition != value)
                {
                    _motorPosition = value;
                    OnPropertyChanged(nameof(MotorPosition));
                }
            }
        }

        private async Task DirectionCommandExecuted(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (_parameters.CameraX <= _config.CameraXMin)
                        return;
                    _parameters.CameraX -= scale;
                    await _saveParameter.SendConfiguration(_setParameters.SetCameraPosition(_parameters.CameraX, _parameters.CameraY));
                    break;
                case Direction.Right:
                    if (_parameters.CameraX >= _config.CameraXMax)
                        return;
                    _parameters.CameraX += scale;
                    await _saveParameter.SendConfiguration(_setParameters.SetCameraPosition(_parameters.CameraX, _parameters.CameraY));
                    break;
                case Direction.Up:
                    if (_parameters.CameraY >= _config.CameraYMax)
                        return;
                    _parameters.CameraY += scale;
                    await _saveParameter.SendConfiguration(_setParameters.SetCameraPosition(_parameters.CameraX, _parameters.CameraY));
                    break;
                case Direction.Down:
                    if (_parameters.CameraY <= _config.CameraYMin)
                        return;
                    _parameters.CameraY -= scale;
                    await _saveParameter.SendConfiguration(_setParameters.SetCameraPosition(_parameters.CameraX, _parameters.CameraY));
                    break;
            }
        }

        private void StartCommandExecuted(object obj)
        {
            OnPropertyChanged(nameof(StreamPath));
            StreamPath = $"http://{_config.SmartCarCamera}:5000";
            //FindAndInitializeWheel(_config.SelectedController);
            //Task.Run(() =>
            //{
            //    ReadSteeringWheelData();
            //});
        }

        public void FindAndInitializeWheel(Controllers controller)
        {
            // Trouver tous les périphériques DirectInput
            IList<DeviceInstance> devices = _directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly);

            if (devices.FirstOrDefault(x => x.ProductGuid == controller.JoystickGuid) is not DeviceInstance device)
                throw new InvalidOperationException("Aucun joystick/volant DirectInput détecté.");

            Guid joystickGuid = device.InstanceGuid;

            if (joystickGuid == Guid.Empty)
                throw new InvalidOperationException("Aucun volant compatible trouvé.");

            _steeringWheelDevice = new Joystick(_directInput, joystickGuid);
            _steeringWheelDevice.Properties.BufferSize = 128; // Buffer pour les événements
            //_steeringWheelDevice.SetCooperativeLevel(mainWindowHandle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
            _steeringWheelDevice.Acquire(); // Acquérir le contrôle du périphérique
        }

        private void ReadSteeringWheelData()
        {
            if (_steeringWheelDevice == null) return;

            while (true) // La boucle continuera tant que l'application est active
            {
                if (_steeringWheelDevice == null) 
                    return;

                _steeringWheelDevice.Poll(); // Important : Polling the device first
                JoystickState state = _steeringWheelDevice.GetCurrentState();

                double direction = Math.Round(Map(state.X, 0, 65535, 0, 100), 2); //Math.Round((double)state.X / 65535, 2);
                double raw_motor = Math.Round(Map(state.Y, 0, 65535, 0, 100), 2);
                double smoothedMotor = RemoveMotorNoize(raw_motor);

                //Debug.WriteLine(direction);
                Debug.WriteLine(smoothedMotor);

                _setParameters.SetDirection(direction);
                _setParameters.SetMotor(MotorDirection.Forward, smoothedMotor);

                Thread.Sleep(10); // Lire ~100 fois par seconde, ajustable
            }
        }

        private double RemoveMotorNoize(double raw_motor)
        {
            // motor without noize
            double noize = Math.Round(Math.Round(raw_motor - (double)16.75) / 5) * 5;
            return Map(noize, -15, 85, 0, 100);
        }

        // <summary>
        /// Re-map a number from one range to another.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="fromLow">The lower bound of the input range.</param>
        /// <param name="fromHigh">The upper bound of the input range.</param>
        /// <param name="toLow">The lower bound of the output range.</param>
        /// <param name="toHigh">The upper bound of the output range.</param>
        /// <returns>The re-mapped value.</returns>
        private double Map(double value, double fromLow, double fromHigh, double toLow, double toHigh)
        {
            // La formule d'interpolation linéaire est :
            // NouvelleValeur = (ValeurActuelle - MinActuel) * (MaxNouveau - MinNouveau) / (MaxActuel - MinActuel) + MinNouveau
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        public void Dispose()
        {
            if (_steeringWheelDevice != null)
            {
                _steeringWheelDevice.Unacquire();
                _steeringWheelDevice.Dispose();
                _steeringWheelDevice = null;
            }
            if (_directInput != null)
            {
                _directInput.Dispose();
                _directInput = null;
            }
        }

    }
}
