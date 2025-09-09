using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Managers
{
public class PlayerManager
{
    /// <summary>
    /// Creates and returns a list of player entities for registration with EntityManager.
    /// </summary>
    public static List<IEntity> CreatePlayers(Texture2D playerTexture, Core.GameSettings settings, int viewportWidth, int viewportHeight)
    {
        var players = new List<IEntity>
        {
            new Player(playerTexture, new Vector2(viewportWidth/2+viewportWidth*0.05f, viewportHeight/2), settings.InputSettings.Player1, viewportWidth, viewportHeight),
            new Player(playerTexture, new Vector2(viewportWidth/2-viewportWidth*0.05f, viewportHeight/2), settings.InputSettings.Player2, viewportWidth, viewportHeight)
        };
        return players;
    }
}
}
