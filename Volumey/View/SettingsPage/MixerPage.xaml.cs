#nullable enable
using Volumey.ViewModel.Pages;
using System.Windows;

namespace Volumey.View.SettingsPage;

public sealed partial class MixerPage
{
	internal MixerPage(MixerPageViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
		//read the screens again when the page opens
		Loaded += (_, _) => vm.Window.LoadedCommand.Execute(null);
	}

	private void PresetEdited(object sender, RoutedEventArgs e)
	{
		if(DataContext is MixerPageViewModel vm)
			vm.SavePresets();
	}
}
