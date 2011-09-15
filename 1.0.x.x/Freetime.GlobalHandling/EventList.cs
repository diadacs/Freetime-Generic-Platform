using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freetime.GlobalHandling
{
    internal class EventList : List<GlobalEvent>
    {
        public string Name { get; set; }

        public EventList(string eventName)
        {
            Name = eventName;
        }

        public void Invoke(object sender, EventArgs e)
        {
            foreach (GlobalEvent gevent in this)
                gevent.Invoke(sender, e);
        }
    }
}
