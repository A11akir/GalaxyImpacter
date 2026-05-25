using Feature.Hero;
using Feature.UI;
using R3;

namespace Feature.ClassBranchWindow.Script
{
    public class ClassLevelWindowPresenter : System.IDisposable
    {
        private readonly ClassLevelWindowView _view;
        private readonly GameSessionData.GameSessionModel _gameSessionModel;
        private readonly HeroClassColorConfig _colorConfig;
        private readonly CompositeDisposable _disposables = new();

        public ClassLevelWindowPresenter(
            ClassLevelWindowView view,
            GameSessionData.GameSessionModel gameSessionModel,
            HeroClassColorConfig colorConfig)
        {
            _view = view;
            _gameSessionModel = gameSessionModel;
            _colorConfig = colorConfig;
        }

        public void Init()
        {
            foreach (AllHeroClass heroClass in System.Enum.GetValues(typeof(AllHeroClass)))
            {
                var currentClass = heroClass;
                
                _gameSessionModel.PlayerHero.HeroClassLevel
                    .GetLevel(currentClass)
                    .Subscribe(level =>
                    {
                        var color = _colorConfig.GetColor(currentClass);
                        _view.UpdateEntry(currentClass, level, color);
                    })
                    .AddTo(_disposables);
            }
        }

        public void Dispose() => _disposables.Dispose();
    }
}