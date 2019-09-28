using System;
using SFML.Graphics;
using SFML.Window;

namespace Pacman.GameStates.World
{
    public interface IWorld
    {
        event EventHandler<WorldState> WorldStateChanged;
        void KeyPressed(object sender, KeyEventArgs e);

        WorldState _worldState { get; set; }
        void Initialize(RenderWindow target);
        void DrawAllLayers(RenderWindow target);
        void Update(RenderWindow target, float time);
    }
}
