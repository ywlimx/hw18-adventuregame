﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.AdventureGame;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine.Experimental.UIElements;

namespace UnityEngine.AdventureGame
{
    public static class StoryEventConditionNode
    {
        public static IEnumerator Execute(GameLogicData.GameLogicGraphNode currentNode)
        {
            //if true return first return value if false return second
	        yield return currentNode.GetReturnValue(PersistentDataManager.Instance.IsStoryEventFinished(currentNode.m_typeData) ? 0 : 1);
        }

#if UNITY_EDITOR
        public static Node CreateNode(string typeData)
        {
            Node node = new Node();
            node.title = "StoryEventCondition";

	        node.mainContainer.style.backgroundColor = Color.yellow;

			node.capabilities |= Capabilities.Movable;

	        AddInputPort(node);
			AddTrueOutputPort(node);
	        AddFalseOutputPort(node);
	        AddDropDown(node, typeData);
			
			return node;
        }

	    private static void AddInputPort(Node node)
	    {
		    Port inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
		    inputPort.portName = "";
		    inputPort.userData = null;
		    node.inputContainer.Add(inputPort);

	    }

		private static void AddTrueOutputPort(Node node)
		{
			Port outputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
			outputPort1.portName = "true";
			outputPort1.userData = null;
			node.outputContainer.Add(outputPort1);

		}

	    private static void AddFalseOutputPort(Node node)
	    {
			Port outputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
		    outputPort2.portName = "false";
		    outputPort2.userData = null;
		    node.outputContainer.Add(outputPort2);
		}

	    private static void AddDropDown(Node node, string typeData)
	    {
			List<string> storyEvents = StoryEventsDatabase.StoryEventDatabase != null ? StoryEventsDatabase.StoryEventDatabase.events
											: new List<string>();

		    var storyEventsDropdown =
			    new PopupField<string>(storyEvents,
										string.IsNullOrEmpty(typeData) || !storyEvents.Exists((x) => string.Equals(x, typeData)) ? 0
											: storyEvents.FindIndex((x) => string.Equals(x, typeData)));
		    node.mainContainer.Insert(1, storyEventsDropdown);

		}

		public static string ExtractExtraData(Node node)
        {
            foreach (VisualElement ele in node.mainContainer)
            {
                if (ele is PopupField<string>)
                {
                    return (ele as PopupField<string>).value;
                }
            }

            return null;
        }
#endif
    }
}