using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS.PacMan
{
    class Items
    {
        public static List<Obj> ObjList = new List<Obj>();

        public static Pacman Pacman;
        public static Ghost Blinky;
        public static Ghost Pinky;
        public static Ghost Inky;
        public static Ghost Clyde;

        public static void Initialize()
        {
            //ObjList.Add(Blinky = new Ghost(new Vector2(510, 350), "ghostRed_animationA",0));
            //ObjList.Add(Pinky = new Ghost(new Vector2(440, 350), "ghostPink_animation", 2));
            //ObjList.Add(Inky = new Ghost(new Vector2(440, 390), "ghostBlue_animation", 4));
            //ObjList.Add(Clyde = new Ghost(new Vector2(510, 390), "ghostOrange_animation", 6));                     

            ObjList.Add(Blinky = new Ghost(new Vector2(300, 250), "ghostRed_animationA", 0, "Blinky"));
            ObjList.Add(Pinky = new Ghost(new Vector2(300, 320), "ghostPink_animation", 2, "Pinky"));
            ObjList.Add(Inky = new Ghost(new Vector2(250, 450), "ghostBlue_animation", 4, "Inky"));
            ObjList.Add(Clyde = new Ghost(new Vector2(700, 450), "ghostOrange_animation", 6, "Clyde"));


            ObjList.Add(Pacman = new Pacman(new Vector2(480, 565), "pacman_animation5"));
        }


        //podmienic texture na ducha jadanlegp
        public static void SetGhostsAsEatable()
        {
            foreach (Obj item in ObjList)
            {
                if (item is Ghost)
                {
                    Ghost ghost = (Ghost)item;
                    ghost.isEatable = true;
                    ghost.TextureName = "ghostsAfterSuperDot_animation";
                }
            }
            Game1.reloadContent = true;
        }

        //przywrocic textury domyslne
        public static void SetGhostAsNotEatable()
        {
            foreach (Obj item in ObjList)
            {
                if (item is Ghost)
                {
                    Ghost ghost = (Ghost)item;
                    ghost.isEatable = false;
                    
                    switch (ghost.Name)
                    {
                        case "Blinky":
                            ghost.TextureName = "ghostRed_animationA";
                            if(!ghost.isAlive)
                            ghost.Position = new Vector2(300, 250);
                            break;
                        case "Pinky":
                            ghost.TextureName = "ghostPink_animation";
                            if (!ghost.isAlive)
                            ghost.Position = new Vector2(300, 320);
                            break;
                        case "Inky":
                            ghost.TextureName = "ghostBlue_animation";
                            if (!ghost.isAlive)
                            ghost.Position = new Vector2(250, 450);
                            break;
                        case "Clyde":
                            ghost.TextureName = "ghostOrange_animation";
                            if (!ghost.isAlive)
                            ghost.Position = new Vector2(700, 450);
                            break;
                        default:
                            break;
                    }
                    ghost.isAlive = true;
                }
            }
            Game1.reloadContent = false;
        }
    }
}
