#nullable enable
using Volumey.ViewModel.Settings;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Volumey.DataProvider;
using Volumey.DataProvider.Settings;
using System.ComponentModel;
using Volumey.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Volumey.ViewModel.Pages;

/// <summary>
/// The Volume mixer settings page: the open-mixer hotkey, the window's behavior, the volume limit and app volume sync
/// </summary>
internal sealed partial class MixerPageViewModel : ObservableObject
{
	/// <summary>For the page's hotkey boxes</summary>
	public IHotkeyService Hotkeys { get; }

	/// <summary>For the hotkey mode note</summary>
	public HotkeyModeViewModel HotkeyMode { get; }

	public AppBehaviorViewModel Window { get; }
	public OpenHotkeyViewModel OpenHotkey { get; }
	public VolumeLimitViewModel VolumeLimit { get; }
	public AppVolumeSyncViewModel AppVolumeSync { get; }
	public ObservableCollection<VolumePreset> VolumePresets { get; }

	internal MixerPageViewModel(IHotkeyService hotkeys, HotkeyModeViewModel hotkeyMode, AppBehaviorViewModel window, OpenHotkeyViewModel openHotkey,
		VolumeLimitViewModel volumeLimit, AppVolumeSyncViewModel appVolumeSync)
	{
		Hotkeys = hotkeys;
		HotkeyMode = hotkeyMode;
		Window = window;
		OpenHotkey = openHotkey;
		VolumeLimit = volumeLimit;
		AppVolumeSync = appVolumeSync;
		VolumePresets = new ObservableCollection<VolumePreset>(SettingsProvider.Settings.General.VolumePresets);
		VolumePresets.CollectionChanged += (_, _) => SavePresets();
		foreach(var preset in VolumePresets)
			Watch(preset);
	}

	[RelayCommand]
	private void AddPreset()
	{
		var preset = new VolumePreset { Name = "Preset", Volume = 50 };
		VolumePresets.Add(preset);
		Watch(preset);
	}

	[RelayCommand]
	private void RemovePreset(VolumePreset? preset)
	{
		if(preset == null)
			return;
		VolumePresets.Remove(preset);
	}

	internal void SavePresets()
	{
		foreach(var preset in VolumePresets)
		{
			preset.Name = preset.Name?.Trim() ?? string.Empty;
			preset.Volume = Math.Clamp(preset.Volume, 0, 100);
		}
		SettingsProvider.Settings.General.VolumePresets = VolumePresets.ToArray();
	}

	private void Watch(VolumePreset preset)
	{
		if(preset is INotifyPropertyChanged notifier)
			notifier.PropertyChanged += (_, _) => SavePresets();
	}
}
