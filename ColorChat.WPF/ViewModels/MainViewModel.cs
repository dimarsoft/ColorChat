namespace ColorChat.WPF.ViewModels
{
    internal class MainViewModel
    {
        public ColorChatViewModel ColorChatViewModel { get; }

        public MainViewModel(ColorChatViewModel chatViewModel)
        {
            ColorChatViewModel = chatViewModel;
        }
    }
}
