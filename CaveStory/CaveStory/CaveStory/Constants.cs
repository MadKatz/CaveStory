using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    static class Constants
    {
        public const int TILESIZE = 32;
        public const int BACKGROUNDSIZE = 128; // pixels
        // Walk Motion
        public const float WALKINGACCELERATION = 0.00083007812f;
        public const float MAXSPEEDX = 0.15859375f; // (pixels / ms) / ms or pixels per ms per ms.
        public const float FRICTION = 0.00049804687f; // pixels / ms

        // Fall Motion
        public const float GRAVITY = 0.00078125f; // (pixels / ms) / ms
        public const float MAXSPEEDY = 0.2998046875f; // pixel / ms

        // Jump Motion
        public const float JUMPSPEED = 0.25f; // pixels / ms
        public const float AIRACCELERATION = 0.0003125f;
        public const float JUMPGRAVITY = 0.0003125f;

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
