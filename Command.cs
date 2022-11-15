using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    public abstract class Command
    {
        public string Name { get; set; }

        public string SecondWord { get; set; }

        protected Command()
        {
            this.Name = "";
            this.SecondWord = null;
        }

        protected bool HasSecondWord()
        {
            return this.SecondWord != null;
        }

        public abstract bool Execute(Player player);
    }
}
