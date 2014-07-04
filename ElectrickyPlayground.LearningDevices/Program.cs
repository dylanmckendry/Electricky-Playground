using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectrickyPlayground.Models;
using OpenZWaveDotNet;

namespace ElectrickyPlayground.LearningDevices
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        private readonly ZWOptions options = new ZWOptions();

        private readonly ZWManager manager = new ZWManager();

        private readonly string port = @"\\.\COM" + SettingsManager.Port;
        private void Start()
        {
            // the directory the config files are copied to in the post build
            options.Create(@"Config", @"", @"");

            // logging options
            options.AddOptionInt("SaveLogLevel", (int)ZWLogLevel.Debug);
            options.AddOptionInt("QueueLogLevel", (int)ZWLogLevel.Debug);
            options.AddOptionInt("DumpTriggerLevel", (int)ZWLogLevel.Error);

            // lock the options
            options.Lock();

            // create the OpenZWave Manager
            manager.Create();
            manager.OnNotification += OnNotification;
            manager.OnControllerStateChanged += state =>
            {
                Debug.WriteLine(state);
            };


            // once the driver is added it takes some time for the device to get ready
            manager.AddDriver(port);

            Console.ReadKey();


            foreach (var node in nodes)
            {
                Console.WriteLine("Node Name: " + node.Name);
            }

            Console.ReadKey();
        }

        private List<Node> nodes = new List<Node>();

        private void OnNotification(ZWNotification notification)
        {
            var notificationType = notification.GetType();

            switch (notificationType)
            {
                case ZWNotification.Type.ValueAdded:
                    Debug.WriteLine("ValueAdded");
                    break;
                case ZWNotification.Type.ValueRemoved:
                    Debug.WriteLine("ValueRemoved");
                    break;
                case ZWNotification.Type.ValueChanged:
                    Debug.WriteLine("ValueChanged");
                    break;
                case ZWNotification.Type.ValueRefreshed:
                    Debug.WriteLine("ValueRefreshed");
                    break;
                case ZWNotification.Type.Group:
                    Debug.WriteLine("Group");
                    break;
                case ZWNotification.Type.NodeNew:
                    {
                        Debug.WriteLine("NodeNew");
                        var node = new Node
                        {
                            Id = notification.GetNodeId(),
                            HomeId = notification.GetHomeId()
                        };
                        nodes.Add(node);
                        break;
                    }
                case ZWNotification.Type.NodeAdded:
                    {
                        Debug.WriteLine("NodeAdded");
                        if (GetNode(notification.GetHomeId(), notification.GetNodeId()) == null)
                        {
                            var node = new Node
                            {
                                Id = notification.GetNodeId(),
                                HomeId = notification.GetHomeId()
                            };
                            nodes.Add(node);
                        }
                        break;
                    }

                case ZWNotification.Type.NodeRemoved:
                    Debug.WriteLine("NodeRemoved");
                    break;
                case ZWNotification.Type.NodeProtocolInfo:
                    Debug.WriteLine("NodeProtocolInfo");
                    break;
                case ZWNotification.Type.NodeNaming:
                    {
                        Debug.WriteLine("NodeNaming");
                        var node = GetNode(notification.GetHomeId(), notification.GetNodeId());
                        if (node != null)
                        {
                            node.Manufacturer = manager.GetNodeManufacturerName(node.HomeId, node.Id);
                            node.Product = manager.GetNodeProductName(node.HomeId, node.Id);
                            node.Location = manager.GetNodeLocation(node.HomeId, node.Id);
                            node.Name = manager.GetNodeName(node.HomeId, node.Id);
                        }
                        break;
                    }
                case ZWNotification.Type.NodeEvent:
                    Debug.WriteLine("NodeEvent");
                    break;
                case ZWNotification.Type.PollingDisabled:
                    Debug.WriteLine("PollingDisabled");
                    break;
                case ZWNotification.Type.PollingEnabled:
                    Debug.WriteLine("PollingEnabled");
                    break;
                case ZWNotification.Type.SceneEvent:
                    Debug.WriteLine("SceneEvent");
                    break;
                case ZWNotification.Type.CreateButton:
                    Debug.WriteLine("CreateButton");
                    break;
                case ZWNotification.Type.DeleteButton:
                    Debug.WriteLine("DeleteButton");
                    break;
                case ZWNotification.Type.ButtonOn:
                    Debug.WriteLine("ButtonOn");
                    break;
                case ZWNotification.Type.ButtonOff:
                    Debug.WriteLine("ButtonOff");
                    break;
                case ZWNotification.Type.DriverReady:
                    Debug.WriteLine("DriverReady");
                    break;
                case ZWNotification.Type.DriverFailed:
                    Debug.WriteLine("DriverFailed");
                    break;
                case ZWNotification.Type.DriverReset:
                    Debug.WriteLine("DriverReset");
                    break;
                case ZWNotification.Type.EssentialNodeQueriesComplete:
                    Debug.WriteLine("EssentialNodeQueriesComplete");
                    break;
                case ZWNotification.Type.NodeQueriesComplete:
                    Debug.WriteLine("NodeQueriesComplete");
                    break;
                case ZWNotification.Type.AwakeNodesQueried:
                    Debug.WriteLine("AwakeNodesQueried");
                    break;
                case ZWNotification.Type.AllNodesQueried:
                    Debug.WriteLine("AllNodesQueried");
                    break;
                case ZWNotification.Type.AllNodesQueriedSomeDead:
                    Debug.WriteLine("AllNodesQueriedSomeDead");
                    break;
                case ZWNotification.Type.Notification:
                    Debug.WriteLine("Notification");
                    break;
            }
        }

        private Node GetNode(uint homeId, byte nodeId)
        {
            return nodes.FirstOrDefault(n => n.Id == nodeId && n.HomeId == homeId);
        }
    }
}
