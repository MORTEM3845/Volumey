#nullable enable
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Volumey.DataProvider;
using Volumey.DataProvider.Settings;
using Volumey.Model;

namespace Volumey.ViewModel;

/// <summary>
/// The mixer window's view models: the window's behavior, the devices and their apps (mixer and tray), the theme (tray icon)
/// and the level meters
/// </summary>
public sealed partial class MainViewModel : ObservableObject
{
	public AppBehaviorViewModel Window { get; }
	public DeviceProviderViewModel Devices { get; }
	public ThemeViewModel Theme { get; }
	public LevelMeters Meters { get; }
	public ObservableCollection<VolumePreset> VolumePresets { get; }
	[ObservableProperty]
	public partial VolumePreset? SelectedPreset { get; set; }

	internal MainViewModel(AppBehaviorViewModel window, DeviceProviderViewModel devices, ThemeViewModel theme, LevelMeters meters)
	{
		Window = window;
		Devices = devices;
		Theme = theme;
		Meters = meters;
		VolumePresets = new ObservableCollection<VolumePreset>(SettingsProvider.Settings.General.VolumePresets);
		//without the picker there's no way to leave another device: the mixer goes back to the default one
		window.PropertyChanged += (_, e) =>
		{
			if(e.PropertyName == nameof(AppBehaviorViewModel.ShowDevicePicker) && !window.ShowDevicePicker)
				devices.ShowDefaultDevice();
		};
	}

	[RelayCommand]
	private void ApplyPreset()
	{
		if(SelectedPreset != null && Devices.SelectedDevice?.Master is { } master)
			master.Volume = Math.Clamp(SelectedPreset.Volume, 0, 100);
	}
}
