using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson_07
{
    public class Utility
    {
        #region Methods
        /// <summary>
        /// Checks to see if a key has been previously pressed and are currently released.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <param name="prevKeyboardState">The previous keyboard state.</param>
        /// <param name="currKeyboardState">The current keyboard state.</param>
        /// <returns>True if the key has been pressed and released, else returns false.</returns>
        public static bool HasKeyBeenPressed(Keys key, ref KeyboardState prevKeyboardState,
            ref KeyboardState currKeyboardState)
        {
            if ((prevKeyboardState.IsKeyDown(key)) && (currKeyboardState.IsKeyUp(key)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks to see if a list of keys have been previously pressed and are currently released.
        /// </summary>
        /// <param name="keys">The list of keys to check.</param>
        /// <param name="prevKeyboardState">The previous keyboard state.</param>
        /// <param name="currKeyboardState">The current keyboard state.</param>
        /// <returns>True if the list of keys have been pressed and released, else returns false.</returns>
        public static bool HasKeyBeenPressed(List<Keys> keys, ref KeyboardState prevKeyboardState,
            ref KeyboardState currKeyboardState)
        {
            bool result = false;

            // Make sure that the number of keys to check is the same number of previouslyPressedKeys
            // e.g. Look for Alt+Enter but will return false if 3 or more keys was previouslyPressed
            // such as Alt+Enter+Z+R
            if (keys.Count != prevKeyboardState.GetPressedKeys().Length)
                return false;

            // Checked every key in the list against every key previously pressed down
            // if the key matches, and the key is currently up then we have found one matching keypress.
            foreach (Keys k in keys)
            {
                foreach (Keys k2 in prevKeyboardState.GetPressedKeys())
                {
                    if (k == k2) // Our key was previously down, let's see if it is currently up
                    {
                        if (true == currKeyboardState.IsKeyUp(k))
                        {
                            // If here, we have found out that they key was previously pressed down,
                            // and is now currently up, this means we have a valid keypress.
                            // Set the result flag to true
                            result = true;
                            break; //Break from the inner foreach loop because we have found this particular key
                        }
                    }
                    else
                        result = false;
                }
            }

            return result;
        }
        #endregion
    }
}
