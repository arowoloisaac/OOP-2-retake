using Arowolo_Project_2.Enums;

namespace Arowolo_Project_2
{
    public class Money
    {
        private char sign;
        private uint integerPart;
        private ushort fractionalPart;
        private int result;

        public Currency currency { get; set; }

        //public string _raw;

        //public decimal value { get; set; }

        //AM1
        public char GetSign() { return sign; }
        //AM 2
        public uint GetIntegerPart() { return integerPart; }
        //AM 3
        public ushort GetFractionalPart() { return fractionalPart; }
        //AM 4
        public Currency GetCurrency() { return currency; }


        public void SetSign(char newSign)
        {
            if (newSign != '+' && newSign != '-')
            {
                //throw new ArgumentException("Invalid sign");
            }
            sign = newSign;
        }

        // method 2 (MS2)
        public void SetIntegerPart(uint newIntegerPart)
        {
            if (newIntegerPart < 0)
            {
                throw new ArgumentException("Number less than zero");
            }

            integerPart = newIntegerPart;
        }

        // method 3 (MS3)
        public void SetFractionalPart(ushort newFractionalPart)
        {
            fractionalPart = newFractionalPart;
        }

        // method 4 (MS4)
        public void SetCurrency(Currency currency)
        {
            this.currency = currency;
        }

        public override string ToString()
        {
            return $"{sign}{integerPart}.{fractionalPart} {currency}";
        }


        public void SetWholeNumberFromString(string wholeNumberString, Currency vcon)
        {
            if (string.IsNullOrEmpty(wholeNumberString))
            {
                throw new ArgumentException("Invalid money string format, money can't be empty");
            }

            int currencyIndex = wholeNumberString.Length;

            // Find the currency part (e.g., RUB) and its index
            for (int i = wholeNumberString.Length - 1; i >= 0; i--)
            {
                if (!char.IsLetter(wholeNumberString[i]))
                {
                    break;
                }
                currencyIndex = i;
            }

            // Extract the currency
            currency = vcon;

            // Extract the sign and amount
            /*string signAndAmount = wholeNumberString.Substring(0, currencyIndex);
            if (signAndAmount.Length < 2)
            {
                throw new ArgumentException("Invalid money string format, Money must have a sign ");
            }

            sign = signAndAmount[0];
            string amountString = signAndAmount.Substring(1);*/

            // NB: to make it user friendly we have to stop prompting the user to stop adding the operation sign

            string amountString = wholeNumberString;

            string[] amountParts = amountString.Split('.');
            if (amountParts.Length != 2)
            {
                throw new ArgumentException("Invalid money string format, Money must have it fractionsl part ");
            }

            if (!uint.TryParse(amountParts[0], out integerPart) || !ushort.TryParse(amountParts[1], out fractionalPart))
            {
                throw new ArgumentException("Invalid money string format");
            }

            SetCurrency(currency);
            SetSign(sign);
            SetIntegerPart(integerPart);
            SetFractionalPart(fractionalPart);
        }


        public Money(string moneyString, Currency? curr)
        {
            SetWholeNumberFromString(moneyString, (Currency)curr!);
        }

        public Money(Currency currency, int result)
        {
            this.currency = currency;
            this.result = result;
        }

        public Money Add(Money other)
        {
            /*if (currency != other.currency)
            {
                throw new ArgumentException("Currency mismatch");
            }*/

            char newSign = (sign == other.sign) ? '+' : '-';
            int thisValue = (int)(integerPart * 100 + fractionalPart);
            int otherValue = (other.sign == '+' ? 1 : -1) * (int)(other.integerPart * 100 + other.fractionalPart);

            int resultValue = thisValue + otherValue;

            if (resultValue < 0)
            {
                newSign = '-';
                resultValue = Math.Abs(resultValue);
            }

            //integerPart = (uint)(resultValue / 100);
            //fractionalPart = (ushort)(resultValue % 100);
            uint newIntegerPart = (uint)(resultValue / 100);
            ushort newFractionalPart = (ushort)(resultValue % 100);

            //sign = newSign;
            SetCurrency(currency);
            SetSign(sign);
            SetIntegerPart(newIntegerPart);
            SetFractionalPart(newFractionalPart);

            return this;
            //return result;
        }


        // Method for calculating difference of two money objects (MDif)
        public Money Subtract(Money other, Currency currency)
        {
            if (currency != other.currency)
            {
                throw new ArgumentException("Currency mismatch");
            }

            char newSign = (sign == other.sign) ? '+' : '-';
            int thisValue = (int)(integerPart * 100 + fractionalPart);
            int otherValue = (other.sign == '+' ? 1 : -1) * (int)(other.integerPart * 100 + other.fractionalPart);

            int resultValue = thisValue - otherValue;

            if (resultValue < 0)
            {
                newSign = '-';
                resultValue = Math.Abs(resultValue);
            }

            //integerPart = (uint)(resultValue / 100);
            //fractionalPart = (ushort)(resultValue % 100);
            uint newIntegerPart = (uint)(resultValue / 100);
            ushort newFractionalPart = (ushort)(resultValue % 100);

            //sign = newSign;
            SetCurrency(currency);
            SetSign(sign);
            SetIntegerPart(newIntegerPart);
            SetFractionalPart(newFractionalPart);

            return this;
            //return result;
        }

    }
}
