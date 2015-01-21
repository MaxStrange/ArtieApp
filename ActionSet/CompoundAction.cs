using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace ActionSet
{
    [Serializable]
    public class CompoundAction
    {
        private List<ElementaryAction> _actionsThisIsComposedOf = new List<ElementaryAction>();
        private List<ElementaryAction> actionsThisIsComposedOf
        {
            get { return this._actionsThisIsComposedOf; }
            set { this._actionsThisIsComposedOf = value; }
        }


//Overridden operators
        public static implicit operator string(CompoundAction action)
        {
            if (action == null)
                return "";
            else
                return action.ToString();
        }

        public static implicit operator CompoundAction(ElementaryAction action)
        {
            List<ElementaryAction> actionList = new List<ElementaryAction>();
            actionList.Add(action);
            return new CompoundAction(actionList);
        }



//Constructors
        public CompoundAction(List<ElementaryAction> actionsToMakeACompoundActionOutOf)
        {
            this.actionsThisIsComposedOf = actionsToMakeACompoundActionOutOf;
        }

        public CompoundAction(List<CompoundAction> actionsToMakeACompoundActionOutOf)
        {
            foreach (CompoundAction compoundAction in actionsToMakeACompoundActionOutOf)
            {
                this.actionsThisIsComposedOf.AddRange(compoundAction.parseCompoundMethodIntoElementaryActions());
            }
        }

        /// <summary>
        /// If actionName is a list of elementary actions separated by underscores, this
        /// will create a new CompoundAction out of it. Otherwise, it will throw an
        /// InvalidCastException.
        /// </summary>
        /// <param name="actionName"></param>
        public CompoundAction(string actionName)
        {
            this.actionsThisIsComposedOf = parseCompoundActionIntoElementaryActions(actionName);
        }


//Public methods
        public List<ElementaryAction> parseCompoundMethodIntoElementaryActions()
        {
            return parseCompoundActionIntoElementaryActions(this.ToString());
        }

        public override string ToString()
        {
            if (this.actionsThisIsComposedOf.Count == 0)
                return "No Action";

            StringBuilder sb = new StringBuilder();
            foreach (ElementaryAction a in this.actionsThisIsComposedOf)
            {
                sb.Append(a.ToString()).Append("_");
            }
            sb.Remove(sb.Length - 1, 1);//remove final underscore
            return sb.ToString();
        }


//Private methods
        private List<ElementaryAction> composeListFromCompoundAction(string compoundName)
        {
            List<ElementaryAction> actionList = new List<ElementaryAction>();
            char underscore = '_';

            while (compoundName.Contains(underscore))
            {
                string justBeforeUnderscore;
                string afterUnderscore;
                StringMethods.parseStringIntoTwoSubStringsFromBeforeAndAfterUnderscore(out justBeforeUnderscore, out afterUnderscore, compoundName);

                actionList.Add(justBeforeUnderscore);
                compoundName = afterUnderscore;
            }
            //add the remainder
            actionList.Add(compoundName);

            return actionList;
        }

        private List<ElementaryAction> parseCompoundActionIntoElementaryActions(string compoundName)
        {
            List<ElementaryAction> actionList;
            char underscore = '_';

            if (compoundName.Contains(underscore))
            {
                actionList = composeListFromCompoundAction(compoundName);
            }
            else
            {
                actionList = new List<ElementaryAction>();
                actionList.Add(compoundName);
            }
            return actionList;
        }
    }
}