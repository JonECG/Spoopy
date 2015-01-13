using UnityEngine;
using System.Collections;

public class Debouncer{

    public static readonly int DEBOUNCE_STEPS = 4; 
    public class DebouncerResults
    {
        internal int counter;
        internal bool wasDown;
        internal bool isDown;

        public bool IsPressed()
        {
            return isDown && !wasDown;
        }

        public bool IsDown()
        {
            return isDown;
        }

        public bool IsReleased()
        {
            return !isDown && wasDown;
        }
    }

    public static DebouncerResults Debounce(string button, DebouncerResults previous)
    {
        if (previous == null)
        {
            previous = new DebouncerResults();
            previous.counter = 0;
            previous.isDown = false;
            previous.wasDown = false;
        }

        previous.wasDown = previous.isDown;

        if (Input.GetButton(button))
        {
            previous.counter = 0;
            previous.isDown = true;
        }
        else
        {
            if (previous.isDown == true)
            {
                previous.counter++;
                if (previous.counter > DEBOUNCE_STEPS)
                {
                    previous.counter = 0;
                    previous.isDown = false;
                }
            }
        }

        return previous;
    }
}
