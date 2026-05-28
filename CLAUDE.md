Обзор проекта
GalaxyImpacter — карточная игра в стиле Hearthstone Battlegrounds / Slay the Spire на Unity.
Два игрока (человек vs AI) выбирают героев, строят деки и сражаются в пошаговом режиме.
Репозиторий: https://github.com/A11akir/GalaxyImpacter
Unity версия: 2022+ (URP)
DI: Zenject
Реактивность: R3 (ReactiveProperty, ReadOnlyReactiveProperty)
Анимации: DOTween
Инспектор: Odin Inspector
Таблицы конфигов: Google Sheets → ScriptableObject через кастомный парсер
Архитектура
Паттерн MVP
Model  — данные (GameSessionPlayerData, CardAndHealthEntityOwnerData)
View   — MonoBehaviour, только отображение и UI-события
Presenter — чистый C# класс, связывает Model и View
System — бизнес-логика, чистый C# класс
Правила:

Presenter никогда не является MonoBehaviour
View не содержит логики — только события (Action) наружу
Init() вызывается из TurnCycleGameSessionSystem.StartGameSession() или Bootstrap
Подписки на ReactiveProperty через .AddTo(_disposables) и CompositeDisposable
DI (Zenject)
Все зависимости регистрируются в BootstrapInstaller.cs:
csharp// Пример регистрации
Container.Bind<SomeSystem>().AsSingle();
Container.Bind<SomeView>().FromComponentInHierarchy().AsSingle();
Container.Bind<SomePresenter>().AsSingle();
Группировка в installer:

BindCore() — AI, Update, Combat
BindUI() — все Presenter/View пары
BindGameSession() — игровые системы
BindCards() — карты и рука
BindBattlefield() — поле боя
BindHero() — герои и силы героя
BindEconomy() — магазин, валюта, инвентарь
BindStages() — фазы игры
BindTimer() — таймер
BindGameSessionFSM() — конечный автомат
BindConfig() — ScriptableObject данные
Assets/Feature/
├── AI/                     — AISystem, AIActionExecutor, AITargetFilter
├── Battlefield/            — BattlefieldSystem, BoardManager, BattlefieldViewManager
├── Card/                   — CardStatsData, SpellCardData, MinionCardData, HandCardView
├── Chakra/                 — ChakraManagerSystem
├── ClassBranchWindow/      — ClassLevelWindowView, ClassLevelEntryView
├── CombatSystem/           — урон, смерти
├── Data/                   — GameData (ScriptableObject)
├── EndGameSession/         — GameOverSystem
├── EntryPoint/             — BootstrapInstaller, GameBootstrap
├── Entity/                 — EntityPresenter, EntityDeathSystem
├── GameSessionData/        — GameSessionModel, GameSessionPlayerData, CardAndHealthEntityOwnerData
├── GameSessionFSM/         — конечный автомат сессии
├── GoogleSheets/           — парсеры, конфиги, импортер
├── HandLogic/              — HandViewSwitcher, HandCardsPositionSystem
├── Health/                 — IHealthView
├── Hero/                   — HeroStatsData, HeroPowerSystem, HeroPowerPresenter, CreateOwnerCardAndHealthEntitySystem
├── Items/                  — ItemData, ItemShopView, InventoryView
├── ShopGamePlay/           — магазин карт и предметов
├── StagesGameLogic/        — TurnCycleGameSessionSystem, фазы подготовки/боя
├── Timer/                  — TimerSystem
└── UI/                     — HeroView, GameSessionView, GameSessionPresenter

GameSessionModel
Центральная модель игры. Хранит ссылки на PlayerHero и EnemyHero (тип GameSessionPlayerData).
GameSessionPlayerData
Данные одного игрока. Содержит:

CardAndHealthEntityOwners — список существ (индекс 0 = главный герой)
CardsInBoard — ReactiveProperty<List<MinionCardData>>
HeroPowers — List<SpellCardData> (2 силы героя)
HeroClassData — купленные классы героя
HeroClassLevel — уровни классов (кол-во карт в BaseDeck)
HeroClassPurchaseCount — кол-во купленных карт по редкостям
HeroPowerUsage — HeroPowerUsageTracker (использована ли сила героя)
Inventory — PlayerInventory
Currency — ReactiveProperty<int>

CardAndHealthEntityOwnerData
Данные одной сущности (герой или существо). Содержит:

CardsInDeck, CardsInHand — ReactiveProperty<List<CardStatsData>>
BaseDeck — IReadOnlyList<CardStatsData> (базовая дека для рефила)
HealthValue, Chakra — ReactiveProperty через обёртки
SpellsList — карты существа (если существо)

TurnCycleGameSessionSystem
Координатор игровой сессии. Вызывает:

StartGameSession() — инициализация всего
CycleStartPrepareTurn() — начало фазы подготовки
CycleStartFightTurn() — начало фазы боя


Игровой процесс
FSM (конечный автомат)
StartState → BanState → PickState → PrepareState ⟷ FightState

BanState — AI и игрок банят героев
PickState — AI и игрок выбирают героев
PrepareState — фаза подготовки (покупки, расстановка существ)
FightState — фаза боя

Фазы

Prepare phase: TargetingSystem.IsPreparePhase = true — можно таргетить только союзников
Fight phase: IsPreparePhase = false — можно таргетить всех

Ресурсы

Чакра игрока: старт 2, +2 за ход, макс 8
Чакра существа: фиксированная из конфига, обновляется каждый ход
Валюта: +15 золота за ход, +стоимость убитого существа врага


Карты
Типы карт

SpellCardData — заклинание (Description, Values, TargetType, MinionNameOwner)
MinionCardData : SpellCardData — существо (Health, Chakra, HandCardCount, SpellsList)

CardRarity
csharpCommon = 0, Hidden = 1, Anomalous = 2, Primordial = 3, None = 4
Конвертация строки → enum через CardRarityConverter.FromString(string)
TargetType
csharpAll = 0, BoardPlace = 1, AnyTarget = 2, Enemy = 3, Ally = 4, OtherTarget = 5
Дека
При старте: 3 базовых карты (All) + 3 карты класса героя Common редкости.
При рефиле: RefillDeckFromBase() — инстанцирует копии из _baseDeck с новыми GUID.
При покупке карты: добавляется в CardsInDeck И в BaseDeck.

Система классов
HeroClass (единый enum)
csharp// Base (1-10)
Alchemist=1, Assassin=2, EarthMage=3, Explorer=4, FireMage=5,
Monster=6, Warrior=7, WaterMage=8, WindMage=9, All=10

// Combo (11-22)
LightningMage=11, MetalMage=12, AbyssLord=13, TimeMage=14,
Witcher=15, Dragonborn=16, GravityMage=17, SupremeAlchemist=18,
InvincibleWanderer=19, AbsolutePredator=20, DeathKing=21, Avatar=22
csharppublic static bool IsBase(this HeroClass c) => (int)c <= 10;
public static bool IsCombo(this HeroClass c) => (int)c >= 11;
HeroClassData
Хранит купленные классы. Заполняется только при покупке карты (BuyCardShopSystem).
Влияет на: шанс появления карт в магазине.
HeroClassLevel
Считает карты в BaseDeck по классам. Пересчитывается реактивно при изменении CardsInDeck.
Влияет на: отображение сфер классов в UI.
HeroClassPurchaseCount
Считает купленные карты по (класс, редкость).
Влияет на: шанс появления редких карт в магазине.

Магазин
Система вероятностей карт
Выбор класса — взвешенный рандом:
Базовый вес = 10 + totalPurchases (≈ +1% за купленную карту)
Выбор редкости — взвешенная рулетка:
csharpwCommon     = 100 (фиксированный)
wHidden     = CalculateRarityWeight(commonPurchases)   // макс 55
wAnomalous  = CalculateRarityWeight(hiddenPurchases)   // макс 55
wPrimordial = CalculateRarityWeight(anomalousPurchases) // макс 55

// Геометрическое убывание: 10+9+8+...+1 = 55 за 10 карт
Fallback: если нет карт нужной редкости → откат на более низкую.
Покупка карты
BuyCardShopSystem.BuyCard(card)
  → SetBaseDeck (обновить до AddCardToDeck!)
  → AddCardToDeck
  → AddClassFromCard → HeroClassData.AddClass
  → HeroClassPurchaseCount.AddPurchase(class, rarity)

Google Sheets импорт
Парсеры

MinionParser — существа → MinionCardData
SpellParser — заклинания → SpellCardData (+ добавляет в SpellsList миньона через MinionNameOwner)
StatsMinionParser — герои → HeroStatsData
ItemParser — предметы → ItemData

Порядок применения
HeroStats → MinionStats → SpellStats → Items
SpellStats применяется ПОСЛЕ MinionStats чтобы найти миньонов и добавить спеллы в SpellsList.
Важные правила парсеров

Пустые строки (Name='') пропускаются через return + _minionStatsConfig = null
Дубликаты имён пропускаются через HashSet<string> processedNames
AssetDatabase.StartAssetEditing() / StopAssetEditing() в try/finally
Поиск SO через freshList.FirstOrDefault(x => x.name == cfg.Name) (не через _targetSO)

Структура таблицы SpellStats
Name | Cost | Rarity | Value1 | Value2 | Value3 | Description | 
Specialization1..4 | Type | MinionNameOwner | InCollection
InCollection

true → карта попадает в списки классов GameData
false + Specialization="All" → попадает в baseCards
false + Specialization!=All → токен/спелл существа, никуда не добавляется


UI Компоненты
HeroView
Отображение героя. Содержит:

_heroPowerPreviewViews — List<HeroPowerPreview> для окна выбора
_heroPowerGameplayViews — List<HeroPowerGameplayView> для игры
SetViewData() — заполняет превью
SetGameplayMode(true) — заполняет игровые вью

HeroPowerViewBase (абстракция)
HeroPowerPreview    ← только отображение (окно выбора героя)
HeroPowerGameplayView ← + SetCanCastView, SetUsedThisTurnView (игра)
HandCardView
Отображение карты в руке. SetDataView() сбрасывает все окна перед установкой:
csharp_heroCardWindow.SetActive(false);
_spellCardWindow.SetActive(false);
_healthContainer.SetActive(false);
CardBuyShopView
Обёртка для покупки карты в магазине. Содержит [SerializeField] HandCardView _handCardView.
Иерархия: CardBuyShopView (клик/анимация) → HandCardView (визуал карты).
DOTween анимации
При повторных кликах — сбрасывать через DOKill():
csharpiconTransform.DOKill();
iconTransform.localScale = originalScale; // запомнить ДО DOKill!
iconTransform.localRotation = Quaternion.identity;
// ... анимация
sequence.OnComplete(() => {
    iconTransform.localScale = originalScale;
    iconTransform.localRotation = Quaternion.identity;
});

AI система
Поток выполнения
ExecutePreparePhase/ExecuteFightPhase
  → CollectAvailableActions (карты + силы героя)
  → PickRandomAction
  → PickRandomValidTarget (FilterValidTargets)
  → AIActionExecutor.ExecuteDelayed (0.5-2 сек задержка)
  → action.Execute(target)
  → рекурсия или EndTurn
Фильтрация целей

DealsDamage() → только враги
IsPreparePhase → только союзники
Стек-оверфлоу защита: CollectAvailableActions предфильтрует через HasValidTargets

AIActionExecutor
Единый класс для задержек AI:

ExecuteDelayed(Action) — простая задержка
SelectAndExecute<T>(List<T>, Action<T>) — выбор из списка с задержкой


Распространённые проблемы
ReactiveProperty и порядок операций
При покупке карты — сначала SetBaseDeck, потом AddCardToDeck:
csharphero.SetBaseDeck(updatedBaseDeck); // СНАЧАЛА
hero.AddCardToDeck(cardCopy);      // ПОТОМ (триггерит Subscribe)
GameSessionPlayerData.AddCardToBoard
Список всегда должен быть размера CardsInBoardMax:
csharpwhile (newList.Count <= index)
    newList.Add(null);
RemoveCardFromBoard заменяет на null, не удаляет элемент.
Бесконечная компиляция в Unity
Причины: дубликаты имён в Google Sheets, невалидные символы в именах файлов,
StartAssetEditing без StopAssetEditing.
DOTween scale не возвращается
Запоминать originalScale ДО DOKill(), возвращать в OnComplete().
#if UNITY_EDITOR
Все парсеры должны быть обёрнуты в #if UNITY_EDITOR снаружи namespace:
csharp#if UNITY_EDITOR
namespace Feature.GoogleSheets { ... }
#endif

Конвенции кода
Именование

aiPlayer вместо enemy (AI может быть любой стороной)
entityOwner вместо owner
availableActions / chosenAction (явное назначение)
Приватные поля: _camelCase
Публичные свойства: PascalCase

Регионы в больших классах
csharp#region Action Collection
#region Target Selection
#region Validation Helpers
#region Utilities
Реактивность
csharp// Подписка с автоматическим dispose
someReactiveProperty
    .Subscribe(value => DoSomething(value))
    .AddTo(_disposables);

// В конструкторе/Init
private readonly CompositeDisposable _disposables = new();
public void Dispose() => _disposables.Dispose();
Паттерн инициализации
csharp// System — вызывается из TurnCycleGameSessionSystem.StartGameSession()
public void Init() { ... }

// Presenter — подписки в конструкторе, данные в Init()
public PresenterName(View view, Model model)
{
    _view = view;
    _model = model;
    _view.OnSomeEvent += HandleEvent; // подписки в конструкторе
}
public void Init() => SubscribeToModel(); // данные при старте сессии