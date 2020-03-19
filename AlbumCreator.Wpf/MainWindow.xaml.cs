using System.Windows;
using Microsoft.Extensions.DependencyInjection;

using AlbumCreator.Common.Interfaces;
using AlbumCreator.WpfElements.CustomTreeView;
using AlbumCreator.ViewModel;
using AlbumCreator.Services;

namespace AlbumCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            var provider = CompositionRoot.Create();

            var uiService = provider.GetService<IUiService>() as UiService;
            uiService.Attach(this);

            _viewModel = new MainViewModel(uiService, provider.GetService<IAlbumManager>());
            AlbumItem.SelectedItemChanged += _viewModel.TreeViewSelectedItemChanged;

            DataContext = _viewModel;
        }
    }
}
