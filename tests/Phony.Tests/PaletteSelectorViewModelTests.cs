using Phony.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Xunit;

namespace Phony.Tests;

public sealed class PaletteSelectorViewModelTests
{
    [Fact]
    public void LoadsPreferencesAndOffersSwatchesAndStyleCommands()
    {
        UiThread.Run(() =>
        {
            var settings = Phony.Properties.Settings.Default;
            // The constructor saves preferences. Route those saves into memory.
            using var scope = new InMemorySettingsScope(settings);
            settings.IsAlternateStyle = true;
            settings.IsDarkTheme = true;
            var vm = new PaletteSelectorViewModel();
            Assert.True(vm.Alternate);
            Assert.True(vm.IsDark);
            Assert.NotEmpty(vm.Swatches);
            vm.ToggleStyleCommand.Execute(false);
            Assert.False(settings.IsAlternateStyle);
            vm.ToggleStyleCommand.Execute(true);
            Assert.True(settings.IsAlternateStyle);
        });
    }

    private sealed class InMemorySettingsScope : IDisposable
    {
        private readonly ApplicationSettingsBase _settings;
        private readonly List<(SettingsProperty Property, SettingsProvider Provider, object Value)> _original = new();

        public InMemorySettingsScope(ApplicationSettingsBase settings)
        {
            _settings = settings;
            var provider = new InMemoryProvider();
            provider.Initialize("PhonyTests", new NameValueCollection());
            foreach (SettingsProperty property in settings.Properties)
            {
                _original.Add((property, property.Provider, settings[property.Name]));
                property.Provider = provider;
            }
        }

        public void Dispose()
        {
            foreach (var entry in _original)
            {
                entry.Property.Provider = entry.Provider;
                _settings[entry.Property.Name] = entry.Value;
            }
        }
    }

    private sealed class InMemoryProvider : SettingsProvider
    {
        public override string ApplicationName { get; set; } = "PhonyTests";

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            var values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in properties)
            {
                values.Add(new SettingsPropertyValue(property) { SerializedValue = property.DefaultValue });
            }

            return values;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values) { }
    }
}
