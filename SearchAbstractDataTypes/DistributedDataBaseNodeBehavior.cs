using ActionSet;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace SearchAbstractDataTypes
{
    internal class DistributedDataBaseNodeBehavior: NodeBehavior, ISearchNodeBehavior
    {
        public override Stack<CompoundAction> buildMethodStack(SearchNode expansionLimit)
        {
            Stack<CompoundAction> methodStack = new Stack<CompoundAction>();
            if (base.caller.parent == null)
                return methodStack;

            if (matches(expansionLimit))
            {
                methodStack.Push(expansionLimit.methodUsedToDeriveNodeFromParent);
                return methodStack;
            }
            methodStack.Push(base.caller.methodUsedToDeriveNodeFromParent);
            SearchNode nextNode = base.caller.parent;
            while (nextNode.parent != null)
            {
                methodStack.Push(nextNode.methodUsedToDeriveNodeFromParent);
                nextNode = nextNode.parent;
            }
            methodStack.Push(expansionLimit.methodUsedToDeriveNodeFromParent);
            return methodStack;
        }

        public override void increaseNodeDepth()
        {
            base.caller.searchSpaceInformation.depth += 1;
        }

        public override bool matches(SearchNode m)
        {
            double nposx = base.caller.perceptionState_Body.location.x;
            double mposx = m.perceptionState_Body.location.x;

            double nposy = base.caller.perceptionState_Body.location.y;
            double mposy = m.perceptionState_Body.location.y;

            if (!NumberMethods.sameSign(nposx, mposx))
                return false;

            if (!NumberMethods.sameSign(nposy, mposy))
                return false;

            this.caller.absoluteOrSetToZeroBothNumbersIfEitherIsNAN(ref nposx, ref mposx);
            this.caller.absoluteOrSetToZeroBothNumbersIfEitherIsNAN(ref nposy, ref mposy);

            if (NumberMethods.doublesRepresentTheSameNumber(nposx, mposx)
                && NumberMethods.doublesRepresentTheSameNumber(nposy, mposy))
                return true;
            else
                return false;
            //TODO : revise matching criteria. Currently, there is no way to ignore
            //orientations because they automatically change to unit length, screwing up
            //their DOUBLE_NULL values.
        }

        public override string solutionExpand(SearchNode expansionLimit)
        {
            Stack<CompoundAction> solutionStack = buildMethodStack(expansionLimit);

            Stack<string> solutionAsStackOfStrings = new Stack<string>();
            foreach (CompoundAction a in solutionStack)
            {
                string aAsString = a.ToString();
                solutionAsStackOfStrings.Push(aAsString);
            }
            
            string solution = StringMethods.buildStringFromStack(solutionAsStackOfStrings);
            return solution;
        }
    }
}
