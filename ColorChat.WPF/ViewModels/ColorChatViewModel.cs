using ColorChat.Domain.Models;
using ColorChat.WPF.Commands;
using ColorChat.WPF.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Serilog;

namespace ColorChat.WPF.ViewModels
{
	internal sealed class LanguageViewModel : ViewModelBase
	{
		private bool _isSelected;

		public LanguageViewModel(string name)
		{
			Name = name;
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				
				OnPropertyChanged();
			}
		}

		public string Name { get; }

		public override string ToString()
		{
			return Name;
		}
	}

	internal sealed class LanguageSelectionViewModel : ViewModelBase
	{
		private static readonly string[] Langs = { "En", "Ru", "De" };
		public ObservableCollection<LanguageViewModel> Languages { get; }
		public ICommand SelectLangCommand { get; }
		public LanguageViewModel Selected { get; private set; }

		public LanguageSelectionViewModel()
		{
			Languages = new ObservableCollection<LanguageViewModel>(Langs.Select(x => new LanguageViewModel(x)));

			Selected = Languages.First();

			Selected.IsSelected = true;

			SelectLangCommand = new RelayCommand(Execute);
		}

		private void Execute(object obj)
		{
			if (obj is LanguageViewModel languageViewModel)
			{
				Selected.IsSelected = false;

				Selected = languageViewModel;

				Selected.IsSelected = true;
				
				OnPropertyChanged(nameof(Selected));

			}
		}
	}
	
    internal class ColorChatViewModel : ViewModelBase
    {
        private byte _red;
        public byte Red
        {
            get => _red;
            set
            {
                _red = value;
                OnPropertyChanged(nameof(Red));
            }
        }

        private byte _green;
        public byte Green
        {
            get => _green;
            set
            {
                _green = value;
                OnPropertyChanged(nameof(Green));
            }
        }

        private byte _blue;
        public byte Blue
        {
            get => _blue;
            set
            {
                _blue = value;
                OnPropertyChanged(nameof(Blue));
            }
        }

        private string _errorMessage = string.Empty;
		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
				OnPropertyChanged(nameof(HasErrorMessage));
			}
		}

		public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

		private bool _isConnected;
		public bool IsConnected
		{
			get => _isConnected;
			set
			{
				_isConnected = value;
				OnPropertyChanged(nameof(IsConnected));
			}
		}

		public ObservableCollection<ColorChatColorViewModel> Messages { get; }
		
		public LanguageSelectionViewModel LanguageSelectionViewModel { get; }

        public ICommand SendColorChatColorMessageCommand { get; }

        private readonly ILogger _logger;
        private ColorChatViewModel(SignalRChatService chatService)
        {
	        _logger = Log.Logger;
            SendColorChatColorMessageCommand = new SendColorChatColorMessageCommand(this, chatService);

			Messages = new ObservableCollection<ColorChatColorViewModel>();

			LanguageSelectionViewModel = new LanguageSelectionViewModel();

            chatService.ColorMessageReceived += ChatService_ColorMessageReceived;
		}

        public static ColorChatViewModel CreatedConnectedViewModel(SignalRChatService chatService)
        {
            ColorChatViewModel viewModel = new ColorChatViewModel(chatService);

            chatService.Connect().ContinueWith(task =>
            {
                if(task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to color chat hub";
            
                    Log.Logger.Error(task.Exception, "Unable to connect to color chat hub");
                    
                }
            });

            return viewModel;
        }

        private void ChatService_ColorMessageReceived(ColorChatColor color)
        {
            Messages.Add(new ColorChatColorViewModel(color));
            
            _logger.Debug("ColorMessageReceived: {Color}", color.ToString());
        }
    }
}
