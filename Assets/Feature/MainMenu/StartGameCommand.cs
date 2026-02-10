using Feature.EntryPoint.Script;
using Feature.UI.Buttons;
using Zenject;

namespace Feature.MainMenu
{
    public class StartGameCommand : AbstractButton, IMenuCommand
    {
        private GameBootstrap _bootstrap;

        [Inject]
        public void Construct(GameBootstrap bootstrap)
        {
            _bootstrap = bootstrap;
        }

        protected override void OnExecute()
        {
            _bootstrap.CheckStartLevel();
        }
    }
}