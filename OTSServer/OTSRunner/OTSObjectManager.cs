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

        public bool LinkComponent(IOTSOutput output, IOTSInput input, bool unlinkOnConflict);
        public bool LinkComponent(IOTSOutput output, IOTSComponent component);
        public bool UnlinkComponents(IOTSOutput output, IOTSInput input);

        public IOTSComponent? GetComponent(Guid componentId);

        public int PropgateSignals();
    }

    public class OTSObjectManager : IObjectStore
    {

        private Dictionary<Guid, IOTSComponent> Components = [];
        private List<((Guid, Guid), OTSLink)> Links = [];
        private List<(IOTSComponent, List<IOTSLink>)> LinkGroups = [];

        public bool AddComponent(IOTSComponent component)
        {
            if(Components.ContainsKey(component.ID)) { return false; }
            Components.Add(component.ID, component);
            return true;
        }

        public bool RemoveComponent(Guid componentId)
        {
            throw new NotImplementedException();
        }

        private static (Guid, Guid) PrePostKey(IOTSIONodeDefinition output, IOTSIONodeDefinition input) => (output.ID, input.ID);

        public bool LinkComponent(IOTSOutput output, IOTSInput input, bool unlinkOnConflict=false)
        {
            var key = PrePostKey(output, input);
            if(Links.Any(x => x.Item1 == key))
            {
                return unlinkOnConflict && UnlinkComponents(output, input);
            }

            var link = new OTSLink(output, input);
            Links.Add((key, link));

            return true;
        }

        public bool LinkComponent(IOTSOutput output, IOTSComponent component)
        {
            var view = component.AddViewingInput(output);
            if(view == null) { return false; }
            var link = new OTSLink(output, view);
            var key = PrePostKey(output, view);
            Links.Add((key, link));

            return true;
        }
        
        public bool UnlinkComponents(IOTSOutput output, IOTSInput input)
        {
            var key = PrePostKey(output, input);
            var val = Links.FirstOrDefault(x => x.Item1 == key);
            return Links.Remove(val);
        }

        public IOTSComponent? GetComponent(Guid componentId)
        {
            Components.TryGetValue(componentId, out var component);
            return component;
        }
    
        public int PropgateSignals()
        {
            foreach(var group in LinkGroups)
            {
                foreach(var link in group.Item2)
                {
                    link.Propogate();
                }

                group.Item1.Update();
            }

            return Links.Count;
        }

        public void BuildLinkOrder()
        {
            var networkChains = new Dictionary<IOTSComponent, List<IOTSComponent>>();
            var networkWeights = new List<(IOTSComponent, int)>();
            var components = Components.Values;

            //Collect each actuator and nomad
            var middleNodes = components.Where(x => 
                x.ComponentClass.Value == OTSComponentClass.ACTUATOR ||
                x.ComponentClass.Value == OTSComponentClass.NOMAD
                ).ToList();
            var producerNodes = components.Where(x => 
                x.ComponentClass.Value == OTSComponentClass.PROVIDER ||
                x.ComponentClass.Value == OTSComponentClass.SINGLE
                ).ToList();
            var monitorNodes = components.Where(x => x.ComponentClass.Value == OTSComponentClass.MONITOR);

            //Collect middle circuit links
            var middleLinks = Links.Where(x => 
                middleNodes.Any(y => y.ID == x.Item2.Input.ComponentId) && 
                middleNodes.Any(y => y.ID == x.Item2.Output.ComponentId)
                );

            //Revisit and inflate each node
            foreach(var node in middleNodes)
            {
                List<IOTSComponent> visited = [];
                VisitInputsRecursive(node, middleNodes, visited);
                networkChains[node] = visited;
                networkWeights.Add((node, visited.Count));
            }

            networkWeights.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            //Build network order
            var NetworkSchedule = producerNodes;
            NetworkSchedule.AddRange(networkWeights.Select(x => x.Item1));
            NetworkSchedule.AddRange(monitorNodes);
            
            //Reorder links accordingly
            List<((Guid, Guid), OTSLink)> tmpLinks = [];
            foreach(var node in NetworkSchedule)
            {
                //var links = Links.Where(x => x.Item2.Input.ComponentId == node.ID);
                //tmpLinks.AddRange(links);
                var links = Links.Where(x => x.Item2.Input.ComponentId == node.ID)
                    .Select(x => x.Item2)
                    .Cast<IOTSLink>()
                    .ToList();
                LinkGroups.Add((node, links));
            }
        }

        private IEnumerable<IOTSComponent> GetInputNodes(IOTSComponent node, List<IOTSComponent> middleNodes)
        {
            var subLinks = Links.Where(x => x.Item2.Input.ComponentId == node.ID);
            var inputs = middleNodes.Where(x => subLinks.Any(y => y.Item2.Output.ComponentId == x.ID));
            return inputs;
        }

        private void VisitInputsRecursive(IOTSComponent node, List<IOTSComponent> middleNodes, List<IOTSComponent> visited)
        {
            if(visited.Contains(node)){ return; }

            visited.Add(node);
            var neighbors = GetInputNodes(node, middleNodes).Where(x => !visited.Contains(x));
            
            foreach(var neighbor in neighbors)
            {
                VisitInputsRecursive(neighbor, middleNodes, visited);
            }
        }
    }
}
