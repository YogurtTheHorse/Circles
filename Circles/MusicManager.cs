using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace Lines {
    public class MusicManager {
        private Dictionary<string, TimeSpan> songStarts;
        private Song[] songs;
        private int currentSongNumber;

        private Song currentSong {
            get {
                return songs[currentSongNumber];
            }
        }

        public MusicManager() {
            songStarts = new Dictionary<string, TimeSpan>();

            songs = new Song[] {
                LinesGame.instance.Content.Load<Song>("song17")
                //Song.FromUri("song17", new Uri("Content/song17.wav", UriKind.Relative)),
                //Song.FromUri("song18", new Uri("Content/song18.wav", UriKind.Relative))
            };

            currentSongNumber = -1;
        }

        private void PlayNext(GameTime gameTime) {
            currentSongNumber = (++currentSongNumber) % songs.Length;
            songStarts[currentSong.Name] = gameTime.TotalGameTime;

            MediaPlayer.Play(currentSong);
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