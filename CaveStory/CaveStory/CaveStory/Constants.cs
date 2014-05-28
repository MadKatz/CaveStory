using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    static class Constants
    {
        public const int TILESIZE = 32;
        // Walk Motion
        public const float SLOWDOWNFACTOR = 0.8f;
        public const float WALKING_ACCELERATION = 0.0012f; // (pixels / ms) / ms or pixels per ms per ms.
        public const float MAXSPEEDX = 0.325f; // pixels / ms

        // Fall Motion
        public const float MAXSPEEDY = 0.325f; // pixel / ms
        public const float GRAVITY = 0.0012f; // (pixels / ms) / ms

        // Fall Motion
        public const float JUMPSPEED = 0.325f; // pixel / ms
        public const int JUMPTIME = 275; // ms

        // Sprite Frames
        public const string SPRITEFILEPATH = @"Images\MyChar";
        public const int CHARACTERFRAME = 0;

        public const int WALKFRAME = 0;
        public const int STANDFRAME = 0;
        public const int JUMPFRAME = 1;
        public const int FALLFRAME = 2;
        public const int UPFRAMEOFFSET = 3;
        public const int DOWNFRAME = 6;
        public const int BACKFRAME = 7;
        // Walk Animation
        public const int NUMWALKFRAME = 3;
        public const int FPS = 15;
    }
}
