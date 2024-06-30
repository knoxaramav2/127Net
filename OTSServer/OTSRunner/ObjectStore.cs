using OTSSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSRunner
{
    public interface IObjectStore
    {
        public bool AddComponent(IOTSComponent component);
        public bool RemoveComponent(Guid componentId);

        public bool AddLink(IOTSOutput output, IOTSInput input);
        public bool RemoveLink(IOTSOutput output, IOTSInput input);
    }

    public class ObjectStore : IObjectStore
    {

        private readonly Dictionary<Guid, IOTSComponent> Components = [];
        private readonly Dictionary<(Guid, Guid, Guid, Guid), IOTSLink> Links = [];

        public bool AddComponent(IOTSComponent component)
        {
            throw new NotImplementedException();
        }

        public bool AddLink(IOTSOutput output, IOTSInput input)
        {
            throw new NotImplementedException();
        }

        public bool RemoveComponent(Guid componentId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveLink(IOTSOutput output, IOTSInput input)
        {
            throw new NotImplementedException();
        }
    }
}
