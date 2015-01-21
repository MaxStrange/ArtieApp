using Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtieGUI
{
    public class SearchButtonController
    {
//Private fields
        private ISearchButtonParent _parentUI = null;
        private ISearchButtonParent parentUI
        {
            get { return this._parentUI; }
            set { this._parentUI = value; }
        }
        private bool _changeTextFlag = true;
        private bool changeTextFlag
        {
            get { return this._changeTextFlag; }
            set { this._changeTextFlag = value; }
        }
        private SearchSlave _slave = null;
        private SearchSlave slave
        {
            get { return this._slave; }
            set { this._slave = value; }
        }



//Constructors
        public SearchButtonController(ISearchButtonParent parentUI)
        {
            this.parentUI = parentUI;
            this.slave = new SearchSlave(parentUI);
        }



//Internal methods
        internal void searchButtonPushed()
        {
            this.slave.searchButtonPushed();
        }

        internal void nxTextBox_TextChanged(TextBox txtBox)
        {
            if (this.changeTextFlag)
            {
                this.changeTextFlag = false;
                try
                {
                    changeOtherTextBoxText(txtBox);
                }
                catch (System.FormatException)
                {
                    //user input non-double into textbox
                    //do nothing with the exception.
                }
                this.changeTextFlag = true;
            }
        }
        
        internal void nyTextBox_TextChanged(TextBox txtBox)
        {
            if (this.changeTextFlag)
            {
                this.changeTextFlag = false;
                try
                {
                    changeOtherTextBoxText(txtBox);
                }
                catch (System.FormatException)
                {
                    //user input non-double into textbox
                    //do nothing with the exception.
                }
                this.changeTextFlag = true;
            }
        }



//Private methods

        private double calculateOtherComponent(double componentToBeUsedInCalculating)
        {
            return Math.Sqrt(1.0 - (componentToBeUsedInCalculating * componentToBeUsedInCalculating));
        }

        private void changeOtherTextBoxText(TextBox firstTxtBox)
        {
            double component = parseTextBoxValue(firstTxtBox);
            double otherComponent = calculateOtherComponent(component);

            if (firstTxtBox == this.parentUI.NxTextBox)
            {
                this.parentUI.NyTextBox.Text = writeNegOrNotNegTextBoxText(this.parentUI.NyTextBox, otherComponent);
            }
            else if (firstTxtBox == this.parentUI.NyTextBox)
            {
                this.parentUI.NxTextBox.Text = writeNegOrNotNegTextBoxText(this.parentUI.NxTextBox, otherComponent);
            }
        }

        private double parseTextBoxValue(TextBox txtbox)
        {
            return Double.Parse(txtbox.Text);
        }
        
        private string writeComponentTextWithNeg(double component)
        {
            StringBuilder sb = new StringBuilder(writeComponentTextWithoutNeg(component));
            return sb.Insert(0, "-").ToString();
        }

        private string writeComponentTextWithoutNeg(double component)
        {
            StringBuilder sb = new StringBuilder();
            return sb.Append(component).ToString();
        }

        private string writeNegOrNotNegTextBoxText(TextBox txtBox, double component)
        {
            if (txtBox.Text.StartsWith("-"))
            {
                return writeComponentTextWithNeg(component);
            }
            else
            {
                return writeComponentTextWithoutNeg(component);
            }
        }
    }
}
