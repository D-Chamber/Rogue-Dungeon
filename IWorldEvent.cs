using System;
using System.Collections.Generic;
using System.Text;

namespace StarterGame
{
     public interface IWorldEvent
     {

          Room Trigger { get; }
          void Execute();
     }
}