using ColorChat.Domain.Models;
using ColorChat.WPF.Services;
using ColorChat.WPF.ViewModels;
using System;
using System.Windows.Input;

namespace ColorChat.WPF.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    internal class SendColorChatColorMessageCommand : ICommand
    {
        private readonly ColorChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public SendColorChatColorMessageCommand(ColorChatViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public event EventHandler CanExecuteChanged = (sender, args) => {};

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                await _chatService.SendColorMessage(new ColorChatColor()
                {
                    Red = _viewModel.Red,
                    Green = _viewModel.Green,
                    Blue = _viewModel.Blue,
                });

                _viewModel.ErrorMessage = string.Empty;
            }
            catch (Exception)
            {
                _viewModel.ErrorMessage = "Unable to send color message.";
            }
        }
    }
}
