using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;
using ActionSet;

namespace SearchAbstractDataTypes
{
    [Serializable]
    public class SearchNodeAsString
    {
//Internal fields
        private SearchNode _parentNode = null;
        internal SearchNode parentNode
        {
            get { return this._parentNode; }
            set { this._parentNode = value; }
        }


//Constructors
        internal SearchNodeAsString(SearchNode parentNode)
        {
            this.parentNode = parentNode;
        }



//Internal methods
        /// <summary>
        /// Returns a string that is essentially a sequence of search node information -
        /// this node's parents all list their information in order from start to this node.
        /// </summary>
        /// <returns></returns>
        internal Stack<SearchNodeInformation> deriveSearchTreeUpToThisNodeAsString(SearchNode n)
        {
            Stack<SearchNodeInformation> solutionStack = buildSearchNodeAsStringStack(n);
            
            return solutionStack;
        }

        /// <summary>
        /// Parses the SearchNode's possibly compound action into a list of chars
        /// corresponding to ActionMap's actionChars.
        /// </summary>
        /// <returns></returns>
        internal char[] parseMethodUsedToDeriveNodeToCharArray(SearchNode n)
        {
            List<ElementaryAction> methodSequence = parseMethodUsedToDeriveNodeToElementaryActionList(n);
            char[] actionCharSequence = mapElementaryActionArrayToCharArray(methodSequence.ToArray());

            return actionCharSequence;
        }


//Private methods
        private Stack<CompoundAction> buildCompoundActionStack()
        {
            if (this.parentNode.parent == null)
                return new Stack<CompoundAction>();
            else
                return fillCompoundActionStack();
        }

        //database
        private Stack<CompoundAction> buildMethodStack(SearchNode expansionLimit)
        {
            return this.parentNode.behavior.buildMethodStack(expansionLimit);
        }

        private Stack<SearchNodeInformation> buildSearchNodeAsStringStack(SearchNode n)
        {
            if (n.parent == null)
                return new Stack<SearchNodeInformation>();
            else
                return fillSearchNodeInformationStack(n);
        }

        private Stack<CompoundAction> fillCompoundActionStack()
        {
            Stack<CompoundAction> compoundActionStack = new Stack<CompoundAction>();
            SearchNode nextNode = this.parentNode.parent;
            while (nextNode.parent != null)
            {
                compoundActionStack.Push(nextNode.methodUsedToDeriveNodeFromParent);
                nextNode = nextNode.parent;
            }

            return compoundActionStack;
        }

        private Stack<SearchNodeInformation> fillSearchNodeInformationStack(SearchNode n)
        {
            Stack<SearchNodeInformation> searchNodeInfoStack = startSearchNodeInfoStack(n);
            SearchNode nextNode = n.parent;
            do
            {
                searchNodeInfoStack.Push(new SearchNodeInformation(nextNode));
                nextNode = nextNode.parent;
            }
            while (nextNode != null);
            return searchNodeInfoStack;
        }

        private char[] mapElementaryActionArrayToCharArray(ElementaryAction[] elementaryActionSequence)
        {
            char[] actionCharSequence = new char[elementaryActionSequence.Length];

            int i = 0;
            foreach (ElementaryAction elementaryAction in elementaryActionSequence)
            {
                if (StringMethods.stringsAreTheSame(elementaryAction, ElementaryAction.START))
                {
                    i++;
                    continue;
                }
                try
                {
                    actionCharSequence[i] = ActionMap.methodNameToActionCharMap[elementaryAction];
                }
                catch (Exception)//TODO : figure out what type of exception this should be
                {
                    //catch the possibility that the char is nonsense somehow
                    continue;
                }
                i++;
            }
            return actionCharSequence;
        }

        /// <summary>
        /// Parses the SearchNode's possibly compound action into a list of elementary ones.
        /// </summary>
        /// <returns></returns>
        private List<ElementaryAction> parseMethodUsedToDeriveNodeToElementaryActionList(SearchNode n)
        {
            return n.methodUsedToDeriveNodeFromParent.parseCompoundMethodIntoElementaryActions();
        }

        private Stack<SearchNodeInformation> startSearchNodeInfoStack(SearchNode n)
        {
            Stack<SearchNodeInformation> searchNodeInfoStack = new Stack<SearchNodeInformation>();
            searchNodeInfoStack.Push(new SearchNodeInformation(n));

            return searchNodeInfoStack;
        }
    }
}
