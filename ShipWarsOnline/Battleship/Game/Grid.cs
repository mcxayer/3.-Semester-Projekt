using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Battleship.Game
{
    class Grid
    {

        protected Player Gamer1;
        protected Player Gamer2;

        public Grid(Player Player1, Player Player2)
        {
            Gamer1 = Player1;
            Gamer2 = Player2;
        }

        public bool Clicked(SeaSquare square)
            {
                if (square.Type != SquareType.Unknown)
                {
                    MessageBox.Show("Please choose a new square");
                    return false;
                }

                Gamer1.TakeTurn(square.Row, square.Col, Gamer2);
            

            if (Gamer2.NoShipsSadFace())
            {
                MessageBox.Show("You win!");
                return true;
            }
            else
            {
                Gamer2.TakeTurn(square.Row, square.Col, Gamer1);
                if (Gamer1.NoShipsSadFace())
                {
                    MessageBox.Show("You lose :(");
                    return true;
                }
            }

            return false;
        }
    
        public List<List<SeaSquare>> MyGrid
        {    
                get { return Gamer1.MyGrid; }
                
                
            }
        public List<List<SeaSquare>> enermyGrid
        {
            get { return Gamer1.EnemyGrid; }


        }
    }
}

