/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.Converters.Abstraction;
using System;

namespace Plexdata.Converters.Extensions
{
    /// <summary>
    /// The extensions methods of interface <see cref="IBitCharger"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All of the provided extension methods are more or less intended 
    /// as convenient wrapper for class <see cref="BitConverter"/>.
    /// </para>
    /// <para>
    /// But be aware, method <c>GetBytes()</c> of class <see cref="BitConverter"/> 
    /// should be used directly in case of charging bits for types <see cref="Single"/>,
    /// <see cref="Double"/>, <see cref="Boolean"/> or <see cref="Char"/>.
    /// </para>
    /// </remarks>
    public static class BitChargerExtension
    {
        #region SByte Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, SByte value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, SByte value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = new Byte[] { (Byte)value };

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="SByte"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="SByte"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="SByte"/> representing all applied 
        /// bits.
        /// </returns>
        public static SByte ToSByte(this IBitCharger charger)
        {
            return charger.ToSByte(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="SByte"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="SByte"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="SByte"/> representing all applied 
        /// bits.
        /// </returns>
        public static SByte ToSByte(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(SByte));

            return (SByte)array[0];
        }

        #endregion

        #region Byte Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Byte value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Byte value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = new Byte[] { value };

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Byte"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Byte"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="Byte"/> representing all applied 
        /// bits.
        /// </returns>
        public static Byte ToByte(this IBitCharger charger)
        {
            return charger.ToByte(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Byte"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Byte"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="Byte"/> representing all applied 
        /// bits.
        /// </returns>
        public static Byte ToByte(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(Byte));

            return array[0];
        }

        #endregion

        #region Int16 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int16 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int16 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int16"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int16"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int16"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int16 ToInt16(this IBitCharger charger)
        {
            return charger.ToInt16(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int16"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int16"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int16"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int16 ToInt16(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(Int16));

            return BitConverter.ToInt16(array, 0);
        }

        #endregion

        #region UInt16 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt16 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt16 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt16"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt16"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt16"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt16 ToUInt16(this IBitCharger charger)
        {
            return charger.ToUInt16(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt16"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt16"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt16"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt16 ToUInt16(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(UInt16));

            return BitConverter.ToUInt16(array, 0);
        }

        #endregion

        #region Int32 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int32 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int32 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int32"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int32"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int32"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int32 ToInt32(this IBitCharger charger)
        {
            return charger.ToInt32(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int32"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int32"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int32"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int32 ToInt32(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(Int32));

            return BitConverter.ToInt32(array, 0);
        }

        #endregion

        #region UInt32 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt32 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt32 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt32"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt32"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt32"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt32 ToUInt32(this IBitCharger charger)
        {
            return charger.ToUInt32(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt32"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt32"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt32"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt32 ToUInt32(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(UInt32));

            return BitConverter.ToUInt32(array, 0);
        }

        #endregion

        #region Int64 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int64 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, Int64 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int64"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int64"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int64"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int64 ToInt64(this IBitCharger charger)
        {
            return charger.ToInt64(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="Int64"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="Int64"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="Int64"/> representing all applied 
        /// bits.
        /// </returns>
        public static Int64 ToInt64(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(Int64));

            return BitConverter.ToInt64(array, 0);
        }

        #endregion

        #region UInt64 Methods

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt64 value)
        {
            charger.SetBytes(value, 0);
        }

        /// <summary>
        /// Sets the bits of <paramref name="value"/> into the 
        /// <paramref name="charger"/> at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of <paramref name="value"/> 
        /// into the <paramref name="charger"/> at offset zero.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// into.
        /// </param>
        /// <param name="value">
        /// The bits to set.
        /// </param>
        /// <param name="offset">
        /// The byte index to start with changing the bits.
        /// </param>
        public static void SetBytes(this IBitCharger charger, UInt64 value, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = BitConverter.GetBytes(value);

            charger.SetBytes(array, offset);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt64"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt64"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt64"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt64 ToUInt64(this IBitCharger charger)
        {
            return charger.ToUInt64(0);
        }

        /// <summary>
        /// Gets all bytes from the charger and converts them into 
        /// <see cref="UInt64"/>.
        /// </summary>
        /// <remarks>
        /// This method gets all bytes from the charger and converts 
        /// them into <see cref="UInt64"/>.
        /// </remarks>
        /// <param name="charger">
        /// An instance of <see cref="IBitCharger"/> to set the bits 
        /// </param>
        /// <param name="offset">
        /// The byte index to get all affected bits from.
        /// </param>
        /// <returns>
        /// A value of type <see cref="UInt64"/> representing all applied 
        /// bits.
        /// </returns>
        public static UInt64 ToUInt64(this IBitCharger charger, Int32 offset)
        {
            charger.ValidateInstance();

            Byte[] array = charger.GetBytes(offset, sizeof(UInt64));

            return BitConverter.ToUInt64(array, 0);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Validates the instance of <see cref="IBitCharger"/>.
        /// </summary>
        /// <remarks>
        /// This method validates the instance of <see cref="IBitCharger"/>.
        /// </remarks>
        /// <param name="charger">
        /// The instance of <see cref="IBitCharger"/> to be validated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of <paramref name="charger"/> 
        /// is null.
        /// </exception>
        private static void ValidateInstance(this IBitCharger charger)
        {
            if (charger is null)
            {
                throw new ArgumentNullException(nameof(charger));
            }
        }

        #endregion
    }
}
