using Utils;
using Zenject;

namespace Lobby
{
    public class AlertDisplayController : UIController
    {
        [Inject] private Alert _settingsAlert;
    
        public override void HandleListener()
            => _settingsAlert.ToggleAlert();
    }
}
