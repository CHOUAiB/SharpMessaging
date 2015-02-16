using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpMessaging.Frames;
using SharpMessaging.Server;

namespace SharpMessaging
{
    public class SharpMessagingQueue
    {

    }

    public interface IMessagingService
    {
        
    }
    public class SharpMessagingServiceBroker
    {
        private SharpMessagingServer _server;
        ConcurrentDictionary<string, IMessagingService> _items = new ConcurrentDictionary<string, IMessagingService>();
        public SharpMessagingServiceBroker()
        {
            _server = new SharpMessagingServer();
            _server.FrameReceived += OnFrame;
            
        }

        public bool AllowDynamicQueues { get; set; }
        public bool AllowDynamicTopics { get; set; }

        private void OnFrame(ServerClient arg1, MessageFrame arg2)
        {
            var destination = arg2.Destination;
            IMessagingService service;
            if (!_items.TryGetValue(destination, out service))
            {
                if (destination.StartsWith("topic/"))
                {
                    if (!AllowDynamicTopics)
                    {
                        var errorFrame = new ErrorFrame("Topics must exist before messages are sent. Cannot create " + destination);
                        //arg1.Send(errorFrame);
                    }
                }
        
            }
        }
    }
}
