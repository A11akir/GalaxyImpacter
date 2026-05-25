// HeroClassData.cs
using System.Collections.Generic;

namespace Feature.Hero
{
    public class HeroClassData
    {
        public AllHeroClass MainClass { get; private set; }

        private readonly List<AllHeroClass> _classes = new();
        public IReadOnlyList<AllHeroClass> Classes => _classes;

        public void SetMainClass(AllHeroClass heroClass)
        {
            MainClass = heroClass;
            AddClass(heroClass);
        }

        public void AddClass(AllHeroClass heroClass)
        {
            if (!_classes.Contains(heroClass))
                _classes.Add(heroClass);
        }

        public bool HasClass(AllHeroClass heroClass) => _classes.Contains(heroClass);
    }
    
    //Получается чем больше карт определенного класса
    //чем шанс чаще встретить карты этого класса.
    //изначально у игрока 3 карты своего класса, и
    //получается надо сделать формулу которая расчитывает
    //шанс выпадения той или иной карты. каждая карта
    //допустим прибавляет 1 процент к шансу встречи этого класса, 
    
}