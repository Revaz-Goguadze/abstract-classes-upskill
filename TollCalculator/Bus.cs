using System;

namespace TollCalculator
{
    /// <summary>
    /// Represents a bus class.
    /// </summary>
    public class Bus : Vehicle
    {
        private int capacity;
        private int passengers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bus"/> class with the specified the base toll, capacity and passengers.
        /// </summary>
        /// <param name="basicToll">A toll of this <see cref="Bus"/> class.</param>
        /// <param name="capacity">A capacity of this <see cref="Bus"/> class.</param>
        /// <param name="passengers">A passengers of this <see cref="Bus"/> class.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="basicToll"/>less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/>less than or equals zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="passengers"/>less than zero.</exception>
        public Bus(decimal basicToll, int capacity, int passengers)
            : base(basicToll)
        {
            if (basicToll < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(basicToll), "The toll cannot be less than zero.");
            }

            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity cannot be less than or equals zero.");
            }

            if (passengers < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(passengers), "The passengers cannot be less than zero.");
            }

            this.Capacity = capacity;
            this.Passengers = passengers;
        }

        /// <summary>
        /// Gets or sets the capacity of this <see cref="Bus"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>less than zero.</exception>
        public int Capacity
        {
            get => this.capacity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The capacity cannot be less than zero.");
                }

                this.capacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the passengers of this <see cref="Bus"/> class.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>less than zero.</exception>
        public int Passengers
        {
            get => this.passengers;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The passengers cannot be less than zero.");
                }

                this.passengers = value;
            }
        }

        /// <summary>
        /// Calculates the base toll that relies only on the bus type.
        /// ----------------------------------------------
        /// Passenger filling in %      Extra or discount
        /// ----------------------------------------------
        /// less than 50%               extra $2.00
        /// more than 90%               $1.00 discount.
        /// </summary>
        /// <returns>The base toll of bus.</returns>
        protected override decimal Calculate()
        {
            decimal adjustedToll = this.BaseToll;

            // Calculate the passenger filling percentage
            decimal fillingPercentage = (decimal)this.passengers / this.capacity;

            // Check for adjustments based on passenger filling percentage
            if (fillingPercentage < 0.5m)
            {
                adjustedToll += 2;
            }
            else if (fillingPercentage > 0.9m)
            {
                adjustedToll -= 1;
            }

            return adjustedToll;
        }
    }
}
