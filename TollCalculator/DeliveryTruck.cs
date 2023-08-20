using System;

namespace TollCalculator
{
    /// <summary>
    /// Represents a delivery truck class.
    /// </summary>
    public class DeliveryTruck : Vehicle
    {
        private int grossWeightClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryTruck"/> class with
        /// the specified <paramref name="baseToll"/> and <paramref name="grossWeightClass"/>.
        /// </summary>
        /// <param name="baseToll">A baseToll of this <see cref="DeliveryTruck"/> class.</param>
        /// <param name="grossWeightClass">A grossWeightClass of this <see cref="DeliveryTruck"/> class.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="baseToll"/>less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="grossWeightClass"/>less than zero.</exception>
        public DeliveryTruck(decimal baseToll, int grossWeightClass)
            : base(baseToll)
        {
            if (baseToll < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(baseToll), "The toll cannot be less than zero.");
            }

            if (grossWeightClass <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(grossWeightClass), "The gross weight class cannot be less than zero.");
            }

            this.grossWeightClass = grossWeightClass;
        }

        /// <summary>
        /// Gets or sets a gross weight class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>less than zero.</exception>
        public int GrossWeightClass
        {
            get => this.grossWeightClass;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The gross weight class cannot be less than zero.");
                }

                this.grossWeightClass = value;
            }
        }

        /// <summary>
        /// Calculates the base toll that relies only on the delivery truck type.
        /// ----------------------------------------------
        /// Weight class        Extra or discount
        /// ----------------------------------------------
        /// over 5000 lbs       extra $5.00
        /// under 3000 lbs      $2.00 discount.
        /// </summary>
        /// <returns>The base toll of delivery truck.</returns>
        protected override decimal Calculate()
        {
            decimal adjustedToll = this.BaseToll;

            // Determine the gross weight class and adjust toll accordingly
            if (this.GrossWeightClass < 3000)
            {
                adjustedToll -= 2.00m;
            }
            else if (this.GrossWeightClass > 5000)
            {
                adjustedToll += 5.00m;
            }

            return adjustedToll;
        }
    }
}
