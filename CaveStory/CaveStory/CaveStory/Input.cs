using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace CaveStory
{
    class Input
    {
        private Dictionary<Keys, bool> heldKeys;
        private Dictionary<Keys, bool> pressedKeys;
        private Dictionary<Keys, bool> releasedKeys;

        public Input()
        {
            heldKeys = new Dictionary<Keys, bool>();
            pressedKeys = new Dictionary<Keys, bool>();
            releasedKeys = new Dictionary<Keys, bool>();
        }

        public void BeginNewFrame()
        {
            pressedKeys.Clear();
            releasedKeys.Clear();
        }

        public void KeyDownEvent(Keys key)
        {
            pressedKeys.Add(key, true);
            heldKeys.Add(key, true);
        }

        public void KeyUpEvent(Keys key)
        {
            releasedKeys.Add(key, true);
            heldKeys.Remove(key);
        }

        public bool WasKeyPressed(Keys key)
        {
            return pressedKeys.ContainsKey(key);
        }

        public bool WasKeyReleased(Keys key)
        {
            return releasedKeys.ContainsKey(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return heldKeys.ContainsKey(key);
        }
    }
}
