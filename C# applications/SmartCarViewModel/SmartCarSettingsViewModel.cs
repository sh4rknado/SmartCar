namespace SmartCarViewModel
{
    using SharpDX.DirectInput;
    using SmartCarModel;
    using SmartCarProtocol;
    using SmartCarProtocolClient;
    using SmartCarViewModel.interfaces;
    using SmartCarViewModel.utils;
    using System.Collections.ObjectModel;

    public class SmartCarSettingsViewModel : ViewModelBase
    {
        private readonly SmartCarSettingsModel _settings;
        private readonly Collection<Controllers> _controllers;
        private readonly ISaveConfiguration _saveConfiguration;
        private readonly ISetParameters _setParameters;

        public SmartCarSettingsViewModel(SmartCarSettingsModel settings, ISaveConfiguration saveConfig, INotifyCommand? notifyCmd = null) : base(notifyCmd)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(saveConfig);

            _settings = settings;
            _saveConfiguration = saveConfig;
            _controllers = [_settings.SelectedController];
            AvailableControllers = new ReadOnlyCollection<Controllers>(_controllers);
            _setParameters = new ProtocolWriter();
        }

        public string SmartCarIP
        {
            get { return _settings.SmartCarIP; }
            set
            {
                if (_settings.SmartCarIP != value)
                {
                    _settings.SmartCarIP = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(SmartCarIP));
                }
            }
        }

        public int SmartCarPort
        {
            get => _settings.SmartCarPort;
            set
            {
                if (_settings.SmartCarPort != value)
                {
                    _settings.SmartCarPort = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(SmartCarPort));
                }
            }
        }
        
        public string SmartCarCamera
        {
            get { return _settings.SmartCarCamera; }
            set
            {
                if (_settings.SmartCarCamera != value)
                {
                    _settings.SmartCarCamera = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(SmartCarCamera));
                }
            }
        }

        public double CameraXMin
        {
            get { return _settings.CameraXMin; }
            set
            {
                if (_settings.CameraXMin != value)
                {
                    _settings.CameraXMin = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(CameraXMin));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetCameraSettings(_settings.CameraXMin, _settings.CameraXMax, _settings.CameraXMin, _settings.CameraXMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double CameraXMax
        {
            get { return _settings.CameraXMax; }
            set
            {
                if (_settings.CameraXMax != value)
                {
                    _settings.CameraXMax = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(CameraXMax));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetCameraSettings(_settings.CameraXMin, _settings.CameraXMax, _settings.CameraXMin, _settings.CameraXMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double CameraYMin
        {
            get { return _settings.CameraYMin; }
            set
            {
                if (_settings.CameraYMin != value)
                {
                    _settings.CameraYMin = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(CameraYMin));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetCameraSettings(_settings.CameraXMin, _settings.CameraXMax, _settings.CameraXMin, _settings.CameraXMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double CameraYMax
        {
            get { return _settings.CameraYMax; }
            set
            {
                if (_settings.CameraYMax != value)
                {
                    _settings.CameraYMax = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(CameraYMax));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetCameraSettings(_settings.CameraXMin, _settings.CameraXMax, _settings.CameraXMin, _settings.CameraXMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double ServoFreq
        {
            get { return _settings.ServoFreq; }
            set
            {
                if (_settings.ServoFreq != value)
                {
                    _settings.ServoFreq = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(ServoFreq));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetServoSettings(_settings.ServoFreq, _settings.ServoPwmMin, _settings.ServoPwmMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double DirectionMin 
        {
            get => _settings.DirectionMin;
            set
            {
                if (_settings.DirectionMin != value)
                {
                    _settings.DirectionMin = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(DirectionMin));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetDirectionSettings(_settings.DirectionMin, _settings.DirectionMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double DirectionMax
        {
            get => _settings.DirectionMax;
            set
            {
                if (_settings.DirectionMax != value)
                {
                    _settings.DirectionMax = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(DirectionMax));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetDirectionSettings(_settings.DirectionMin, _settings.DirectionMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double ServoPwmMin
        {
            get { return _settings.ServoPwmMin; }
            set
            {
                if (_settings.ServoPwmMin != value)
                {
                    _settings.ServoPwmMin = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(ServoPwmMin));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetServoSettings(_settings.ServoFreq, _settings.ServoPwmMin, _settings.ServoPwmMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public double ServoPwmMax
        {
            get { return _settings.ServoPwmMax; }
            set
            {
                if (_settings.ServoPwmMax != value)
                {
                    _settings.ServoPwmMax = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(ServoPwmMax));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetServoSettings(_settings.ServoFreq, _settings.ServoPwmMin, _settings.ServoPwmMax);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }
        
        public double MotorMin 
        { 
            get => _settings.MotorMin;
            set
            {
                if(value != _settings.MotorMin)
                {
                    _settings.MotorMin = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    _setParameters.SetMotorSettings(_settings.MotorMax, _settings.MotorMin);
                    OnPropertyChanged(nameof(MotorMin));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetMotorSettings(_settings.MotorMax, _settings.MotorMin);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }
        
        public double MotorMax 
        { 
            get => _settings.MotorMax;
            set
            {
                if (value != _settings.MotorMax)
                {
                    _settings.MotorMax = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(MotorMax));

                    Task.Run(() => {
                        ProtocolMessage result = _setParameters.SetMotorSettings(_settings.MotorMax, _settings.MotorMin);
                        _saveConfiguration.SendConfiguration(result);
                    });
                }
            }
        }

        public Controllers SelectedController 
        {
            get => _settings.SelectedController;
            set
            {
                if(_settings.SelectedController != value)
                {
                    _settings.SelectedController = value;
                    _saveConfiguration.SaveConfiguration(_settings);
                    OnPropertyChanged(nameof(SelectedController));
                }
            }
        }

        public ReadOnlyCollection<Controllers> AvailableControllers { get; }

        private IEnumerable<Controllers> GetAvailableControllers()
        {
            using DirectInput directInput = new();
            IList<DeviceInstance> devices = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly); // Trouver tous les périphériques DirectInput
            return devices.Select(x => new Controllers(x.ProductGuid, x.ProductName));
        }

    }
}