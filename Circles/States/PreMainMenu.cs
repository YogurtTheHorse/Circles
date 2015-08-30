using Lines.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lines.States {
    public class PreMainMenu : PreSelectState{
        public PreMainMenu(bool isOpening) :  base(
            Constants.COLORS[Constants.DRAW], 
            LinesGame.GameTitle, 
            LinesGame.LocalGame, 
            LinesGame.MultiplayerGame, 
            OnLocalGame, 
            OnMultiplayerGame, 
            isOpening) { }

        public static void OnLocalGame() {
            LinesGame.CurrentState = new OpeningState(true);
        }

        public static void OnMultiplayerGame() {
            LinesGame.CurrentState = new WaitForServerState();
        }
    }
}
