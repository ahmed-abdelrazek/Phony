using Caliburn.Micro;
using Phony.WPF.EventModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class SettingsViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<SettingsEvents>
    {
        SimpleContainer _container;

        IEventAggregator _events;

        public SettingsViewModel(IEventAggregator events, SimpleContainer container)
        {
            _container = container;
            _events = events;
            _events.SubscribeOnBackgroundThread(this);

            Items.Add(new PaletteSelectorViewModel
            {
                DisplayName = "الواجهة"
            });
            Items.Add(new GeneralSettingsViewModel(events)
            {
                DisplayName = "العام"
            });
            Items.Add(new MSSQLMovementViewModel(events)
            {
                DisplayName = "النقل"
            });
            Items.Add(new AboutAppViewModel
            {
                DisplayName = "عن البرنامج"
            });
            ActivateItem(Items.First());
        }

        public async Task HandleAsync(SettingsEvents message, CancellationToken cancellationToken)
        {
            await Task.Delay(5);
            if (message.CloseWindow)
            {
                TryClose();
            }
        }
    }
}