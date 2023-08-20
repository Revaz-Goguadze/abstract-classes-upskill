using System;

namespace TollCalculator
{
    /// <summary>
    /// Represents a vehicle base class in pricing and toll calculation system.
    /// </summary>
    public abstract class Vehicle
    {
        private decimal baseToll;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        protected Vehicle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class with the specified base toll.
        /// </summary>
        /// <param name="baseToll">A base toll for the <see cref="Vehicle"/> class.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="baseToll"/> is less than zero.</exception>
        protected Vehicle(decimal baseToll)
        {
            if (baseToll < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.baseToll), "Cannot be less than 0");
            }

            this.baseToll = baseToll;
        }

        /// <summary>
        /// Gets or sets a base toll for the vehicle.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than zero.</exception>
        public decimal BaseToll
        {
            get => this.baseToll;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Value cannot be less than 0", nameof(value));
                }

                this.baseToll = value;
            }
        }

        /// <summary>
        /// Calculates the final toll for the vehicle that adjusts for time peaks and traffic direction.
        /// </summary>
        /// <param name="timeOfToll">A time of toll.</param>
        /// <param name="direction">A traffic direction.</param>
        /// <returns>The final toll for the vehicle that adjusts for time peaks and traffic direction.</returns>
        public decimal CalculateToll(DateTime timeOfToll, TrafficDirection direction) => this.Calculate() * PeakTimePremium(timeOfToll, direction);

        /// <summary>
        /// Calculates the base toll that relies only on the vehicle type.
        /// </summary>
        /// <returns>The base toll of vehicle.</returns>
        protected abstract decimal Calculate();

        /// <summary>
        /// Calculates a weighting factor for the base toll, taking into account time peaks and direction of travel.
        /// ----------------------------------------------------------
        /// Day         Time            Direction       Weight factor
        /// ----------------------------------------------------------
        /// Weekday     morning rush    inbound             x 2.00
        /// Weekday     morning rush    outbound            x 1.00
        /// Weekday     daytime         inbound             x 1.50
        /// Weekday     daytime         outbound            x 1.50
        /// Weekday     evening rush    inbound             x 1.00
        /// Weekday     evening rush    outbound            x 2.00
        /// Weekday     overnight       inbound             x 0.75
        /// Weekday     overnight       outbound            x 0.75
        /// Weekend     morning rush    inbound             x 1.00
        /// Weekend     morning rush    outbound            x 1.00
        /// Weekend     daytime         inbound             x 1.00
        /// Weekend     daytime         outbound            x 1.00
        /// Weekend     evening rush    inbound             x 1.00
        /// Weekend     evening rush    outbound            x 1.00
        /// Weekend     overnight       inbound             x 1.00
        /// Weekend     overnight       outbound            x 1.00.
        /// </summary>
        /// <param name="timeOfToll">A time of toll.</param>
        /// <param name="direction">A traffic direction.</param>
        /// <returns>A weight factor that adjusts for time peaks and traffic direction.</returns>
        private static decimal PeakTimePremium(DateTime timeOfToll, TrafficDirection direction)
        {
            var weightFactor = 1.00m;
            if (IsWeekDay(timeOfToll))
            {
                weightFactor = GetTimeBand(timeOfToll) switch
                {
                    TimeBand.MorningRush => direction == TrafficDirection.Inbound ? 2.00m : 1.00m,
                    TimeBand.Daytime => 1.50m,
                    TimeBand.EveningRush => direction == TrafficDirection.Inbound ? 1.00m : 2.00m,
                    TimeBand.Overnight => 0.75m,
                    _ => 1.00m
                };
            }

            return weightFactor;
        }

        /// <summary>
        /// Defines whether a DateTime represents a weekend or a weekday.
        /// </summary>
        /// <param name="timeOfToll">The time when the toll was collected.</param>
        /// <returns>true if <paramref name="timeOfToll"/> is weekday; false otherwise.</returns>
        private static bool IsWeekDay(DateTime timeOfToll)
        {
            return timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };
        }

        /// <summary>
        /// Categorizes the time into the time bands.
        /// </summary>
        /// <returns><see cref="TimeBand"/> instance.</returns>
        private static TimeBand GetTimeBand(DateTime tollTime)
        {
            TimeBand timeBand;

            // Determine the appropriate time band based on the toll time
            if (tollTime.Hour >= 6 && tollTime.Hour <= 10)
            {
                timeBand = TimeBand.MorningRush;
            }
            else if (tollTime.Hour >= 10 && tollTime.Hour <= 16)
            {
                timeBand = TimeBand.Daytime;
            }
            else if (tollTime.Hour >= 16 && tollTime.Hour <= 19)
            {
                timeBand = TimeBand.EveningRush;
            }
            else
            {
                timeBand = TimeBand.Overnight;
            }

            return timeBand;
        }
    }
}
