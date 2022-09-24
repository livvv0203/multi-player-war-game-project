using System;
namespace WarGameProject.DataModels
{
    public class Card : IComparable<Card>, IEquatable<Card>
    {
        /// <summary>
        /// Aces are high, Suits are ignored
        /// [2, 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A]
        /// </summary>
        public const ushort MIN_CARD = 2;

        public const ushort MAX_CARD = 14;

        public const ushort TOTAL_SUITS = 4;

        protected Card() { }

        public int Number { get; protected set; }

        public static Card Create(int number)
        {
            if (number < MIN_CARD || number > MAX_CARD)
            {
                Console.WriteLine($"Attempted to CREATE a Card with number {number}, but failed, number is invalid.");
                return null;
            }

            return new Card()
            {
                Number = number
            };
        }

        public override string ToString()
        {
            if (this.Number < MIN_CARD || this.Number > MAX_CARD)
            {
                return $"Invalid Card [{this.Number}]";
            }

            switch (Number)
            {
                case 11: return "J";
                case 12: return "Q";
                case 13: return "K";
                case 14: return "A";

                default:
                    return Number.ToString(); // "2" -> "10"
            }
        }

        public bool Equals(Card other)
        {
            if (other == null)
                return false;
            return this.Number == other.Number;
        }

        #region IComparable

        public int CompareTo(Card other)
        {
            // Compare reference
            if (ReferenceEquals(this, other)) return 0;
            // If other is not not a valid obj reference, this instance is greater
            if (ReferenceEquals(null, other)) return 1; 

            return Number.CompareTo(other.Number);
        }

        public static bool operator <(Card left, Card right)
        {
            return left.Number < right.Number;
        }

        public static bool operator >(Card left, Card right)
        {
            return left.Number > right.Number;
        }

        public static bool operator <=(Card left, Card right)
        {
            return left.Number <= right.Number;
        }

        public static bool operator >=(Card left, Card right)
        {
            return left.Number >= right.Number;
        }


        #endregion

        public Card DeepCopy()
        {
            return new Card()
            {
                Number = this.Number,
            };
        }
    }
}

