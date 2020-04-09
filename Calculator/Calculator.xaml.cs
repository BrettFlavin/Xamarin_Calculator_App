using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Calculator
{    
    public partial class CalculatorPage : ContentPage
    {
        string theOperator;         // represents an operation to perform on 2 numbers  
        double firstNum, secondNum; // stores two numbers at a time to be used in the calculation
        int currentState = 1;       // represents change of state after an operator button is pressed

        public CalculatorPage()
        {
            InitializeComponent();
        }

        // called whenever a digit button is pressed to store the digit(s)
        private void OnDigitButton_Clicked(object sender, EventArgs args)
        {
            // appropriate buttons become enabled only after a number button is pressed
            enableButtons();

            // get digit from sender
            Button button = (Button)sender;
            string digit = button.Text;

            // if an operator button is already stored or a zero was pressed
            if (currentState < 0 || this.displayLabel.Text == "0")
            {
                this.displayLabel.Text = " ";
                if (currentState < 0)
                    currentState *= -1;               
            }
            this.displayLabel.Text += digit;
            
            // converts the string representation of the number on the display to a new double 
            // TryParse() returns true or false and 'out' keyword initializes a new variable 
            // note: when using 'out' no need to declare variable first (from C# 7.0 onwards)
            if (double.TryParse(this.displayLabel.Text, out double number))
            {
                // set the first number
                if (currentState == 1)
                {
                    firstNum = number;
                }
                // set the second number
                else
                {
                    secondNum = number;
                }                
            }
        }

        // called whenever an operator button is pressed to set the new operator
        private void OnOperatorButton_Clicked(object sender, EventArgs e)
        {
            // get operator from sender
            Button button = (Button)sender;
            string pressed = button.Text;

            // an operator has already been set so we must perform a calculation first 
            // firstNum stores result of calculation, display result and set the new operator
            if (currentState == 2)
            {
                firstNum = Calculate(firstNum, secondNum, theOperator);
                this.displayLabel.Text = firstNum.ToString();
                currentState = -2;
                theOperator = pressed;                
            }            
            // no operator was previously set so just set a new operator
            else
            {
                currentState = -2;
                theOperator = pressed;
            }
        }

        // called whenever the clear button is pressed to clear all numbers
        private void OnClearButton_Clicked(object sender, EventArgs e)
        {
            // clear all numbers from display and local storage then disable buttons
            if (this.displayLabel.Text != "0")
            {
                firstNum = 0;
                secondNum = 0;
                currentState = 1;
                this.displayLabel.Text = "0";
                disableButtons();
            }
        }

        // called whenever the delete button is pressed to delete last number
        private void OnDeleteButton_Clicked(object sender, EventArgs e)
        {
            // keep buttons enabled only while numbers are still left on the display
            if (displayLabel.Text.Length == 1)
            {
                disableButtons();
                displayLabel.Text = "0";
            }
            else
            {
                displayLabel.Text = displayLabel.Text.Substring(0, displayLabel.Text.Length - 1);
            }           
        }

        // called whenever the equals button is pressed to perform the calculation     
        private void OnEqualsButton_Clicked(object sender, EventArgs e)
        {
            // if 2 numbers are stored then calculate and display result of operation
            if (currentState == 2)
            {
                // call helper function Calculate() to get result
                var result = Calculate(firstNum, secondNum, theOperator);
                this.displayLabel.Text = result.ToString();
                firstNum = result;
                currentState = -1;               
            }
        }

        // called whenever the decimal button is pressed to add a decimal
        private void OnDecimalButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.displayLabel.Text += button.Text;
        }

        // called whenever the negate button is pressed to negate the current number 
        private void OnNegativeButton_Clicked(object sender, EventArgs e)
        {
            // convert the string representation of a number to a double
            double.TryParse(this.displayLabel.Text, out double number);            
            
            // update the number
            if (number == firstNum)
                firstNum *= -1;
            else
                secondNum *= -1;

            // update the display
            number *= -1;
            this.displayLabel.Text = number.ToString();
        }

        // helper function called by OnEqualsButton_Clicked() to get result of calculation
        private static double Calculate(double firstValue, double secondValue, string theOperator)
        {
            double result = 0;
            switch (theOperator)
            {
                case "/":
                    result = firstValue / secondValue;
                    break;
                case "*":
                    result = firstValue * secondValue;
                    break;
                case "+":
                    result = firstValue + secondValue;
                    break;
                case "-":
                    result = firstValue - secondValue;
                    break;
                case "%":
                    result = firstValue % secondValue;
                    break;                    
            }
            return result;
        }

        // enables the appropriate buttons
        private void enableButtons()
        {
            clearButton.IsEnabled = true;
            deleteButton.IsEnabled = true;
            modButton.IsEnabled = true;
            divideButton.IsEnabled = true;
            multiplyButton.IsEnabled = true;
            addButton.IsEnabled = true;
            subtractButton.IsEnabled = true;
            negativeButton.IsEnabled = true;
            equalsButton.IsEnabled = true;            
        }

        // disables the appropriate buttons
        private void disableButtons()
        {
            clearButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
            modButton.IsEnabled = false;
            divideButton.IsEnabled = false;
            multiplyButton.IsEnabled = false;
            addButton.IsEnabled = false;
            subtractButton.IsEnabled = false;
            negativeButton.IsEnabled = false;
            equalsButton.IsEnabled = false;
        }

    }
}
