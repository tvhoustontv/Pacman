using System;
using System.IO;
using SFML.Audio;
using SFML.Graphics;

namespace Pacman.AssetManager
{
    class MainAssetManager
    {
        public Sound LoadSound(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            SoundBuffer soundBuffer = new SoundBuffer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathManager.SoundsPath, fileName));

            return new Sound(soundBuffer);
        }
        public Font LoadFont(string fileName)
        {
            return new Font(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PathManager.FontsPath, fileName));
        }
    }
}
