using Gameplay.Boosters;
using Gameplay.Character;
using Gameplay.Rain;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameStarter _gameStarter;
        [SerializeField] private CharacterFacade _characterFacade;
    
        public override void InstallBindings()
        {
            Container.Bind<GameStarter>().FromInstance(_gameStarter).AsSingle();
            
            Container.Bind<CharacterFacade>().FromInstance(_characterFacade).AsSingle();

            Container.BindInterfacesAndSelfTo<BoostersService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<RainService>().AsSingle();
            
            Container.Bind<GameService>().AsSingle();
        }
    }
}
