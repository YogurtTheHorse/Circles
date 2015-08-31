using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Lines {
    public class MusicManager {
        private Dictionary<string, TimeSpan> songStarts;
        private SoundEffect[] songs;
        private int currentSongNumber;

        private SoundEffect currentSong {
            get {
                return songs[currentSongNumber];
            }
        }

        public MusicManager() {
            songStarts = new Dictionary<string, TimeSpan>();

            songs = new SoundEffect[] {
                LinesGame.instance.Content.Load<SoundEffect>("song17"),
                LinesGame.instance.Content.Load<SoundEffect>("song18")
                //Song.FromUri("song17", new Uri("Content/song17.wav", UriKind.Relative)),
                //Song.FromUri("song18", new Uri("Content/song18.wav", UriKind.Relative))
            };

            currentSongNumber = -1;
        }

        private void PlayNext(GameTime gameTime) {
            currentSongNumber = (++currentSongNumber) % songs.Length;
            songStarts[currentSong.Name] = gameTime.TotalGameTime;

            currentSong.Play();
        }

        public void Update(GameTime gameTime) {
            if (currentSongNumber < 0) {
                PlayNext(gameTime);
            }

            if (gameTime.TotalGameTime - songStarts[currentSong.Name] > currentSong.Duration) {
                PlayNext(gameTime);
            }
        }
    }
}