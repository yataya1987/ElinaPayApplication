using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BnetApplication.Hubs
{
    
    public class counterHub:Hub
    {
        static long counter = 0;//count online users 
        public override System.Threading.Tasks.Task OnConnected()
        {
            counter = counter + 1;//add one when user connected
            Clients.All.UpdateCount(counter);//call client method to update interface
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            if (!(counter <= 0))
            {
                counter = counter - 1;// subtract 1 when user Disconnected
            }
            Clients.All.UpdateCount(counter);//call client method to update interface
            return base.OnDisconnected(stopCalled);
        }
    }
}