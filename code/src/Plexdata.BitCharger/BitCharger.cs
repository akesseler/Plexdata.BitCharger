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
using Plexdata.Converters.Internals;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Plexdata.Converters
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IBitCharger"/>.
    /// </summary>
    /// <remarks>
    /// The bit charger is designed to be used as some kind of repository 
    /// for sets of boolean values.
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("Length: {Length}; Bytes: {Bytes}; Content: {Content}")]
    public class BitCharger : IBitCharger
    {
        #region Private constants

        /// <summary>
        /// Delimiter used for string representation grouping.
        /// </summary>
        /// <remarks>
        /// This field represents the delimiter used for grouping string 
        /// contents. The value of this field is set to one single space.
        /// </remarks>
        private const String Delimiter = " ";

        /// <summary>
        /// Count of bits.
        /// </summary>
        /// <remarks>
        /// This field simply represents the number of bits, which is indeed 
        /// eight.
        /// </remarks>
        private const Int32 BitCount = 8;

        /// <summary>
        /// The enabled bit.
        /// </summary>
        /// <remarks>
        /// This field simply represents an enabled bit.
        /// </remarks>
        private const Char Enabled = '1';

        /// <summary>
        /// The disabled bit.
        /// </summary>
        /// <remarks>
        /// This field simply represents a disabled bit.
        /// </remarks>
        private const Char Disabled = '0';

        /// <summary>
        /// Default capacity of internal buffer.
        /// </summary>
        /// <remarks>
        /// The default capacity is only relevant for increasing the internal 
        /// buffer. Therefore, using such a default capacity only means some 
        /// kind of optimization of memory allocation.
        /// </remarks>
        private const Int32 DefaultCapacity = 2 * sizeof(UInt64) * BitCharger.BitCount;

        #endregion

        #region Private fields

        /// <summary>
        /// The instance of <see cref="IByteOrder"/> to determine the actual 
        /// byte order.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of <see cref="IByteOrder"/> to be used 
        /// to determine the actual byte order at runtime.
        /// </remarks>
        private readonly IByteOrder helper = null;

        /// <summary>
        /// The internal buffer.
        /// </summary>
        /// <remarks>
        /// Why is a String-Builder used instead of for example a Char-Array?
        /// Possibly, a String-Builder provides a faster access to insert items 
        /// at its front.
        /// </remarks>
        private readonly StringBuilder buffer = null;

        #endregion

        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// The standard constructor initialize a new instance with default 
        /// settings.
        /// </remarks>
        /// <seealso cref="BitCharger(Int32)"/>
        /// <seealso cref="Plexdata.Converters.Factories.BitChargerFactory.Create()"/>
        public BitCharger()
            : this(new ByteOrder())
        {
        }

        /// <summary>
        /// The constructor with <paramref name="length"/> initialization.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance and uses provided 
        /// <paramref name="length"/> as initial value.
        /// </remarks>
        /// <param name="length">
        /// The number of bit to be initially set as disabled.
        /// </param>
        /// <seealso cref="Plexdata.Converters.Factories.BitChargerFactory.Create(Int32)"/>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if <paramref name="length"/> is less 
        /// than zero.
        /// </exception>
        public BitCharger(Int32 length)
            : this(new ByteOrder())
        {
            this.ValidateLowerBoundary(length, 0, nameof(length));

            this.EnsureLength(length);
        }

        /// <summary>
        /// The internally visible constructor.
        /// </summary>
        /// <remarks>
        /// Actually, this constructor is only for testing purposes.
        /// </remarks>
        /// <param name="order">
        /// An instance of <see cref="IByteOrder"/> to be used to determine 
        /// actual byte order.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The exception is thrown if parameter <paramref name="order"/> 
        /// is null.
        /// </exception>
        internal BitCharger(IByteOrder order)
            : base()
        {
            this.helper = order ?? throw new ArgumentNullException(nameof(order));

            this.buffer = new StringBuilder(BitCharger.DefaultCapacity);
        }

        /// <summary>
        /// The serialization constructor.
        /// </summary>
        /// <remarks>
        /// This constructor is called by the framework during class 
        /// deserialization.
        /// </remarks>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> to populate with data.
        /// </param>
        /// <param name="context">
        /// The destination (see <see cref="StreamingContext"/>) for this 
        /// serialization.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of <paramref name="info"/> is 
        /// null.
        /// </exception>
        /// <exception cref="SerializationException">
        /// This exception is thrown whenever the deserialization has failed.
        /// </exception>
        protected BitCharger(SerializationInfo info, StreamingContext context)
            : this() // Ensure instance of IByteOrder...
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            this.buffer.Capacity = info.GetInt32(nameof(this.Capacity));
            this.buffer.Clear();
            this.buffer.Append(info.GetString(nameof(this.Content)));

            if (this.buffer.Length != info.GetInt32(nameof(this.Length)))
            {
                throw new SerializationException("Mismatch between processed content and expected length.");
            }
        }

        /// <summary>
        /// The cloneable constructor.
        /// </summary>
        /// <remarks>
        /// This constructor is used for any kind of class cloning.
        /// </remarks>
        /// <param name="order">
        /// The reference of <see cref="IByteOrder"/> to be used to determine 
        /// actual byte order.
        /// </param>
        /// <param name="buffer">
        /// The buffer to get all cloneable data from.
        /// </param>
        private BitCharger(IByteOrder order, StringBuilder buffer)
            : base()
        {
            this.helper = order;
            this.buffer = new StringBuilder(buffer.Capacity);
            this.buffer.Append(buffer); // Internally, ToString() is called.
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Capacity of the internal buffer.
        /// </summary>
        /// <remarks>
        /// This property is just for convenience. But be aware, setting 
        /// one bit beyond the capacity causes a memory re-allocation.
        /// </remarks>
        /// <value>
        /// The total capacity of the internal buffer.
        /// </value>
        public Int32 Capacity
        {
            get
            {
                return this.buffer.Capacity;
            }
        }

        /// <summary>
        /// Total length of managed bits.
        /// </summary>
        /// <remarks>
        /// The total length currently used by the internal buffer. 
        /// This amount is equivalent to the number of charged bits.
        /// </remarks>
        /// <value>
        /// The total number of bits currently managed.
        /// </value>
        /// <seealso cref="Bytes"/>
        public Int32 Length
        {
            get
            {
                return this.buffer.Length;
            }
        }

        /// <summary>
        /// Total length of managed bytes.
        /// </summary>
        /// <remarks>
        /// This amount is equivalent to the <see cref="Length"/> but 
        /// divided by <see cref="BitCharger.BitCount"/> (which is eight).
        /// </remarks>
        /// <value>
        /// The total number of bytes currently managed.
        /// </value>
        /// <seealso cref="Length"/>
        /// <seealso cref="BitCharger.BitCount"/>
        public Int32 Bytes
        {
            get
            {
                return this.buffer.Length / BitCharger.BitCount;
            }
        }

        /// <summary>
        /// Current plain content.
        /// </summary>
        /// <remarks>
        /// This is just the plain content of the internal buffer.
        /// </remarks>
        /// <value>
        /// The plain content of the internal buffer.
        /// </value>
        public String Content
        {
            get
            {
                return this.buffer.ToString();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the bit at <paramref name="index"/> according its 
        /// <paramref name="enabled"/> state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method allows to set a particular bit at <paramref name="index"/> 
        /// either to enabled or to disabled. 
        /// </para>
        /// <para>
        /// The <paramref name="index"/> is meant as direct offset of 
        /// the affected bit. For example, an index of zero means the 
        /// actual lowest bit no matter which byte order the underlying 
        /// system has. This in turn means, the index runs indeed from 
        /// the right to the left.
        /// </para>
        /// </remarks>
        /// <param name="index">
        /// The index of affected bit.
        /// </param>
        /// <param name="enabled">
        /// True to enable that bit and false to disable it.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if <paramref name="index"/> is less 
        /// than zero.
        /// </exception>
        /// <seealso cref="HasBitAt(Int32)"/>
        public void SetBitAt(Int32 index, Boolean enabled)
        {
            this.ValidateLowerBoundary(index, 0, nameof(index));

            index = this.InvertIndex(index);

            this.buffer[index] = enabled ? BitCharger.Enabled : BitCharger.Disabled;
        }

        /// <summary>
        /// Gets the state of a bit at given <paramref name="index"/>.
        /// </summary>
        /// <remarks>
        /// This method returns the state of a bit at provided 
        /// <paramref name="index"/>.
        /// </remarks>
        /// <param name="index">
        /// The position to get the bit state for.
        /// </param>
        /// <returns>
        /// True, the affected bit is set and false if not. False is also 
        /// returned if parameter index is out of managed range.
        /// </returns>
        public Boolean HasBitAt(Int32 index)
        {
            if (index < 0)
            {
                return false;
            }

            if (index >= this.buffer.Length)
            {
                return false;
            }

            index = this.InvertIndex(index);

            return this.buffer[index] == BitCharger.Enabled;
        }

        /// <summary>
        /// Sets all bits of the <paramref name="source"/> array starting 
        /// at offset zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of the <paramref name="source"/> 
        /// array starting at offset zero.
        /// </remarks>
        /// <param name="source">
        /// The array of bytes to take over all bits.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is throws in case of <paramref name="source"/> 
        /// is null.
        /// </exception>
        /// <seealso cref="SetBytes(Byte[], Int32)"/>
        public void SetBytes(Byte[] source)
        {
            this.SetBytes(source, 0);
        }

        /// <summary>
        /// Sets all bits of the <paramref name="source"/> array starting 
        /// at <paramref name="offset"/>.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of the <paramref name="source"/> 
        /// array starting at provided <paramref name="offset"/>.
        /// </remarks>
        /// <param name="source">
        /// The array of bytes to take over all bits.
        /// </param>
        /// <param name="offset">
        /// The index of the first byte to start from with taking over all 
        /// bits.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is throws in case of <paramref name="source"/> 
        /// is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if <paramref name="offset"/> is less 
        /// than zero.
        /// </exception>
        /// <seealso cref="SetBytes(Byte[])"/>
        public void SetBytes(Byte[] source, Int32 offset)
        {
            this.ValidateSourceBuffer(source, nameof(source));
            this.ValidateLowerBoundary(offset, 0, nameof(offset));

            Int32 length = source.Length;

            if (length == 0) { return; }

            // Skipped on most Windows platforms.
            if (!this.helper.IsLittleEndian)
            {
                source = this.InvertOrder(this.CloneSource(source));
            }

            this.EnsureLength((offset + length) * BitCharger.BitCount);

            // Copy source from back to front beginning 
            // at offset and running until length.

            Char[] value = new Char[BitCharger.BitCount];

            for (Int32 outer = length - 1; outer >= 0; outer--, offset++)
            {
                Int32 index = this.InvertIndex(offset * BitCharger.BitCount);

                this.ChargeBits(source[outer], ref value);

                for (Int32 inner = 0; inner < BitCharger.BitCount; inner++)
                {
                    this.buffer[index - inner] = value[BitCharger.BitCount - (inner + 1)];
                }
            }
        }

        /// <summary>
        /// Gets all currently available bytes.
        /// </summary>
        /// <remarks>
        /// This method returns all currently available bytes. The 
        /// returned array of bytes is ordered according to the byte 
        /// order of the underlying system.
        /// </remarks>
        /// <returns>
        /// All currently available bytes.
        /// </returns>
        /// <seealso cref="GetBytes(Int32, Int32)"/>
        public Byte[] GetBytes()
        {
            Int32 length = this.Bytes;

            return this.GetBytes(0, length);
        }

        /// <summary>
        /// Gets all bytes beginning at <paramref name="offset"/>.
        /// </summary>
        /// <remarks>
        /// This method returns all bytes beginning at 
        /// <paramref name="offset"/>. The returned array of bytes 
        /// is ordered according to the byte order of the underlying 
        /// system.
        /// </remarks>
        /// <param name="offset">
        /// The byte offset where to retrieve all bytes from.
        /// </param>
        /// <returns>
        /// The list of affected bytes.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if <paramref name="offset"/> is less 
        /// than zero.
        /// </exception>
        /// <seealso cref="GetBytes(Int32, Int32)"/>
        public Byte[] GetBytes(Int32 offset)
        {
            this.ValidateLowerBoundary(offset, 0, nameof(offset));

            Int32 length = this.Bytes;

            return this.GetBytes(offset, length - offset);
        }

        /// <summary>
        /// Gets all bytes beginning at <paramref name="offset"/> and for 
        /// the wanted <paramref name="length"/>.
        /// </summary>
        /// <remarks>
        /// This method returns all bytes beginning at <paramref name="offset"/> 
        /// and for the wanted <paramref name="length"/>. The returned array of 
        /// bytes is ordered according to the byte order of the underlying system.
        /// </remarks>
        /// <param name="offset">
        /// The byte offset where to retrieve all bytes from.
        /// </param>
        /// <param name="length">
        /// The number of bytes to get.
        /// </param>
        /// <returns>
        /// The list of affected bytes.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown either if <paramref name="offset"/> 
        /// or if <paramref name="length"/> is out of range or if the sum 
        /// of <paramref name="offset"/> and <paramref name="length"/> is 
        /// beyond upper bounds.
        /// </exception>
        /// <seealso cref="GetBytes()"/>
        /// <seealso cref="GetBytes(Int32)"/>
        public Byte[] GetBytes(Int32 offset, Int32 length)
        {
            Int32 lower = 0;
            Int32 upper = this.Bytes;

            this.ValidateOverallBounds(offset, lower, upper, nameof(offset));
            this.ValidateOverallBounds(length, lower, upper, nameof(length));
            this.ValidateUpperBoundary(offset + length, upper, $"{nameof(offset)}+{nameof(length)}");

            Byte[] result = new Byte[length];

            if (length == 0) { return result; }

            Char[] value = new Char[BitCharger.BitCount];

            for (Int32 outer = length - 1; outer >= 0; outer--, offset++)
            {
                Int32 index = this.InvertIndex(offset * BitCharger.BitCount);

                this.buffer.CopyTo((index + 1) - BitCharger.BitCount, value, 0, BitCharger.BitCount);

                this.ChargeBits(value, ref result[outer]);
            }

            // Skipped on most Windows platforms.
            if (!this.helper.IsLittleEndian)
            {
                result = this.InvertOrder(result);
            }

            return result;
        }

        /// <summary>
        /// Charges the bits of <paramref name="source"/> into internal 
        /// buffer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method charges the bits of source into internal buffer.
        /// </para>
        /// <para>
        /// The source buffer should only include zeros and ones. It is 
        /// allowed that the source buffer starts with "0b", the binary 
        /// literal marker. Furthermore, using underscores, dashes and 
        /// spaces is possible as well. But any other character is not 
        /// allowed and will be treated as binary zero.
        /// </para>
        /// </remarks>
        /// <param name="source">
        /// A string containing the bits to charge.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in all cases if provided source buffer 
        /// is considered as invalid.
        /// </exception>
        /// <seealso cref="Charge(String, Boolean)"/>
        public void Charge(String source)
        {
            this.Charge(source, false);
        }

        /// <summary>
        /// Charges the bits of source into internal buffer, but takes the 
        /// state of parameter <paramref name="reverse"/> into account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method charges the bits of source into internal buffer.
        /// </para>
        /// <para>
        /// The source buffer should only include zeros and ones. It is 
        /// allowed that the source buffer starts with "0b", the binary 
        /// literal marker. Furthermore, using underscores, dashes and 
        /// spaces is possible as well. But any other character is not 
        /// allowed and will be treated as binary zero.
        /// </para>
        /// <para>
        /// Be aware, with <paramref name="reverse"/> enabled zeros are prepended 
        /// at the last byte in case of the length of <paramref name="source"/> is 
        /// not a multiple of eight. with <paramref name="reverse"/> disabled this 
        /// padding is put at the front of the <paramref name="source"/> instead.
        /// </para>
        /// <list type="table">
        /// <listheader><term>Reverse</term><term>Source</term><term>Padding</term><term>Result</term></listheader>
        /// <item><term>true</term><term>1010 1100 <b>1111</b></term><term>1010 1100 <b>0000 1111</b></term><term><b>0000 1111</b> 1010 1100</term></item>
        /// <item><term>false</term><term><b>1010</b> 1100 1111</term><term><b>0000 1010</b> 1100 1111</term><term><b>0000 1010</b> 1100 1111</term></item>
        /// </list>
        /// </remarks>
        /// <param name="source">
        /// A string containing the bits to charge.
        /// </param>
        /// <param name="reverse">
        /// True to reverse the charged byte order and false to leave 
        /// byte order unchanged.
        /// </param>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in all cases if provided source buffer 
        /// is considered as invalid.
        /// </exception>
        /// <seealso cref="Charge(String)"/>
        /// <seealso cref="Purify(String)"/>
        /// <seealso cref="Adjust(String, Boolean)"/>
        public void Charge(String source, Boolean reverse)
        {
            if (String.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"Value of \"{nameof(source)}\" must not be null or empty or consist only of whitespaces.", nameof(source));
            }

            source = this.Purify(source);

            // May happen when source only consists of control characters 
            // such as "0b", "_" or "-" (with our without spaces in between).
            if (String.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"Value of \"{nameof(source)}\" must not only contain control characters.", nameof(source));
            }

            source = this.Adjust(source, reverse);

            // Should never happen but nobody knows...
            Debug.Assert((source.Length % BitCharger.BitCount) == 0);

            StringBuilder builder = new StringBuilder(source);

            Int32 length = builder.Length;
            Int32 offset = 0;

            Byte[] bytes = new Byte[length / BitCharger.BitCount];
            Char[] chunk = new Char[BitCharger.BitCount];

            for (Int32 index = 0; index < length; index += BitCharger.BitCount, offset++)
            {
                builder.CopyTo(index, chunk, 0, BitCharger.BitCount);

                this.ChargeBits(chunk, ref bytes[offset]);
            }

            if (reverse)
            {
                bytes = this.InvertOrder(bytes);
            }

            this.buffer.Clear();
            this.SetBytes(bytes, 0);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <remarks>
        /// This method creates a new object that is a copy of the current 
        /// instance.
        /// </remarks>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Object Clone()
        {
            return new BitCharger(this.helper, this.buffer);
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed 
        /// to serialize the target object.
        /// </summary>
        /// <remarks>
        /// This method populates a <see cref="SerializationInfo"/> with the 
        /// data needed to serialize the target object.
        /// </remarks>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> to populate with data.
        /// </param>
        /// <param name="context">
        /// The destination (see <see cref="StreamingContext"/>) for this 
        /// serialization.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of <paramref name="info"/> is 
        /// null.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// This exception is thrown in case of the caller does not have the 
        /// required permission.
        /// </exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(this.Capacity), this.Capacity);
            info.AddValue(nameof(this.Length), this.Length);
            info.AddValue(nameof(this.Content), this.Content);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <remarks>
        /// This method returns a string that represents the current 
        /// object.
        /// </remarks>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <seealso cref="ToString(Boolean)"/>
        public override String ToString()
        {
            return this.ToString(true);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <remarks>
        /// This method returns a string that represents the current object.
        /// A conversion into the byte order of underlying system is never 
        /// done.
        /// </remarks>
        /// <param name="grouped">
        /// True to separate each byte by a space and false to just return 
        /// current content.
        /// </param>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public String ToString(Boolean grouped)
        {
            if (!grouped)
            {
                return this.Content;
            }

            Int32 sourceIndex = 0;
            Int32 sourceLength = this.Length;

            Int32 targetIndex = 0;
            Int32 targetLength = sourceLength + ((this.Bytes - 1) * BitCharger.Delimiter.Length);

            StringBuilder result = new StringBuilder(targetLength) { Length = targetLength };

            for (; sourceIndex < sourceLength && targetIndex < targetLength; sourceIndex++, targetIndex++)
            {
                if (sourceIndex > 0 && (sourceIndex % BitCharger.BitCount) == 0)
                {
                    for (Int32 index = 0; index < BitCharger.Delimiter.Length; index++, targetIndex++)
                    {
                        result[targetIndex] = BitCharger.Delimiter[index];
                    }
                }

                result[targetIndex] = this.buffer[sourceIndex];
            }

            return result.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks that <paramref name="value"/> is not null.
        /// </summary>
        /// <remarks>
        /// This method checks that <paramref name="value"/> is not null 
        /// and throws an exception in case of <paramref name="value"/> is 
        /// actually null.
        /// </remarks>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <param name="label">
        /// The name of the parameter that is checked. This label is used 
        /// to generate the exception message.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown in case of <paramref name="value"/> 
        /// is null.
        /// </exception>
        private void ValidateSourceBuffer(Byte[] value, String label)
        {
            // Should never happen but nobody knows...
            Debug.Assert(!String.IsNullOrWhiteSpace(label));

            if (value is null)
            {
                throw new ArgumentNullException(label, $"Value of \"{label}\" must not be null.");
            }
        }

        /// <summary>
        /// Checks if <paramref name="value"/> is less than <paramref name="lower"/> 
        /// boundary.
        /// </summary>
        /// <remarks>
        /// This method checks if <paramref name="value"/> is less than 
        /// <paramref name="lower"/> boundary.
        /// </remarks>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <param name="lower">
        /// The lower boundary to check against.
        /// </param>
        /// <param name="label">
        /// The name of the parameter that is checked. This label is used 
        /// to generate the exception message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown in case of <paramref name="value"/> 
        /// is less than <paramref name="lower"/> boundary.
        /// </exception>
        private void ValidateLowerBoundary(Int32 value, Int32 lower, String label)
        {
            // Should never happen but nobody knows...
            Debug.Assert(!String.IsNullOrWhiteSpace(label));

            if (value < lower)
            {
                throw new ArgumentOutOfRangeException(label, $"Value {value} of argument \"{label}\" is less than lower limit of {lower}.");
            }
        }

        /// <summary>
        /// Checks if <paramref name="value"/> is greater than <paramref name="upper"/> 
        /// boundary.
        /// </summary>
        /// <remarks>
        /// This method checks if <paramref name="value"/> is greater than 
        /// <paramref name="upper"/> boundary.
        /// </remarks>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <param name="upper">
        /// The upper boundary to check against.
        /// </param>
        /// <param name="label">
        /// The name of the parameter that is checked. This label is used 
        /// to generate the exception message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown in case of <paramref name="value"/> 
        /// is greater than <paramref name="upper"/> boundary.
        /// </exception>
        private void ValidateUpperBoundary(Int32 value, Int32 upper, String label)
        {
            // Should never happen but nobody knows...
            Debug.Assert(!String.IsNullOrWhiteSpace(label));

            if (value > upper)
            {
                throw new ArgumentOutOfRangeException(label, $"Value {value} of argument \"{label}\" is greater than upper limit of {upper}.");
            }
        }

        /// <summary>
        /// Checks if <paramref name="value"/> is less than <paramref name="lower"/> 
        /// boundary or greater than <paramref name="upper"/> boundary.
        /// </summary>
        /// <remarks>
        /// This method checks if <paramref name="value"/> is less 
        /// than <paramref name="lower"/> boundary or greater than 
        /// <paramref name="upper"/> boundary.
        /// </remarks>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <param name="lower">
        /// The lower boundary to check against.
        /// </param>
        /// <param name="upper">
        /// The upper boundary to check against.
        /// </param>
        /// <param name="label">
        /// The name of the parameter that is checked. This label is used 
        /// to generate the exception message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown in case of <paramref name="value"/> 
        /// either less than <paramref name="lower"/> boundary or it is 
        /// greater than <paramref name="upper"/> boundary.
        /// </exception>
        private void ValidateOverallBounds(Int32 value, Int32 lower, Int32 upper, String label)
        {
            // Should never happen but nobody knows...
            Debug.Assert(!String.IsNullOrWhiteSpace(label));

            if (value < lower || value > upper)
            {
                throw new ArgumentOutOfRangeException(label, $"Value {value} of argument \"{label}\" is out of valid range of [{lower}..{upper}].");
            }
        }

        /// <summary>
        /// Converts parameter <paramref name="length"/> into a value that is 
        /// divisible by eight.
        /// </summary>
        /// <remarks>
        /// This method converts parameter <paramref name="length"/> into a 
        /// value that is divisible by eight.
        /// </remarks>
        /// <param name="length">
        /// The value (bit-count) to be adjusted.
        /// </param>
        /// <returns>
        /// The adjusted value (bit-count).
        /// </returns>
        /// <seealso cref="EnsureLength(Int32)"/>
        private Int32 AdjustLength(Int32 length)
        {
            Int32 padding = 0;

            if (length == 0 || (length % BitCharger.BitCount) != 0)
            {
                padding = BitCharger.BitCount - (length % BitCharger.BitCount);
            }

            return length + padding;
        }

        /// <summary>
        /// Ensures that the internal buffer has a valid length (bit-count).
        /// </summary>
        /// <remarks>
        /// This method checks if provided <paramref name="length"/> is beyond 
        /// upper bounds of internal buffer. If so than an additional buffer with 
        /// '0' is prepended at that buffer. The length of each prepended buffer 
        /// is divisible by eight.
        /// </remarks>
        /// <param name="length">
        /// The estimated number of bits.
        /// </param>
        /// <seealso cref="AdjustLength(Int32)"/>
        private void EnsureLength(Int32 length)
        {
            if (length > 0 && length > this.buffer.Length)
            {
                Int32 padding = this.AdjustLength(length - this.buffer.Length);

                this.buffer.Insert(0, String.Empty.PadLeft(padding, BitCharger.Disabled));
            }

            // Should never happen but nobody knows...
            Debug.Assert((this.buffer.Length % BitCharger.BitCount) == 0);
        }

        /// <summary>
        /// Turns the <paramref name="index"/> into its reverse representation.
        /// </summary>
        /// <remarks>
        /// The reverse index is counted from the internal buffer's end. 
        /// </remarks>
        /// <param name="index">
        /// The index to be inverted.
        /// </param>
        /// <returns>
        /// The inverted index.
        /// </returns>
        /// <seealso cref="EnsureLength(Int32)"/>
        private Int32 InvertIndex(Int32 index)
        {
            this.EnsureLength(index + 1);

            return this.buffer.Length - (index + 1);
        }

        /// <summary>
        /// Gets a clone of provided byte array.
        /// </summary>
        /// <remarks>
        /// Cloning the source byte array is actually required in case of the 
        /// underlying system uses big-endian byte order. Because otherwise 
        /// the caller's original byte array would be corrupted.
        /// </remarks>
        /// <param name="source"></param>
        /// <returns>
        /// A copy of the input byte array.
        /// </returns>
        /// SetBytes(Byte[] source, Int32 offset)
        private Byte[] CloneSource(Byte[] source)
        {
            Byte[] result = new Byte[source.Length];

            for (Int32 index = 0; index < source.Length; index++)
            {
                result[index] = source[index];
            }

            return result;
        }

        /// <summary>
        /// Inverts the order of given byte array.
        /// </summary>
        /// <remarks>
        /// This method just inverts the order of given byte array.
        /// </remarks>
        /// <param name="source">
        /// The byte array to be inverted.
        /// </param>
        /// <returns>
        /// The inverted byte array.
        /// </returns>
        private Byte[] InvertOrder(Byte[] source)
        {
            Int32 ltr = 0;                 // left-to-right index
            Int32 rtl = source.Length - 1;  // right-to-left index
            Int32 len = source.Length / 2;  // number of loops

            // Swap values from right to left.
            for (; ltr < len && rtl > 0; ltr++, rtl--)
            {
                Byte saved = source[ltr];
                source[ltr] = source[rtl];
                source[rtl] = saved;
            }

            return source;
        }

        /// <summary>
        /// Moves all bits of <paramref name="source"/> into the 
        /// <paramref name="result"/> character array.
        /// </summary>
        /// <remarks>
        /// This method just moves all bits of <paramref name="source"/> 
        /// into the <paramref name="result"/> character array. This movement 
        /// is done by a bit shift operation.
        /// </remarks>
        /// <param name="source">
        /// The source byte to take over all bits.
        /// </param>
        /// <param name="result">
        /// The character array consisting of '1' or '0' according to the 
        /// <paramref name="source"/> bits.
        /// </param>
        private void ChargeBits(Byte source, ref Char[] result)
        {
            // Should never happen but nobody knows...
            Debug.Assert(result.Length == BitCharger.BitCount);

            const Int32 mask = 0x00000001;

            Int32 shift = 0;
            Int32 index = BitCharger.BitCount - 1;

            for (; shift < result.Length && index >= 0; shift++, index--)
            {
                if (((source >> shift) & mask) == mask)
                {
                    result[index] = BitCharger.Enabled;
                }
                else
                {
                    result[index] = BitCharger.Disabled;
                }
            }
        }

        /// <summary>
        /// Moves all bits of <paramref name="source"/> into the 
        /// <paramref name="result"/> byte.
        /// </summary>
        /// <remarks>
        /// This method just moves all bits of <paramref name="source"/> 
        /// into the <paramref name="result"/> byte.
        /// </remarks>
        /// <param name="source">
        /// The source character array to take over all bits.
        /// </param>
        /// <param name="result">
        /// The byte representation of the bits of provided character 
        /// array.
        /// </param>
        private void ChargeBits(Char[] source, ref Byte result)
        {
            // Should never happen but nobody knows...
            Debug.Assert(source.Length == BitCharger.BitCount);

            result = 0; // Reset to default!

            Int32 shift = 0;
            Int32 index = BitCharger.BitCount - 1;

            for (; shift < BitCharger.BitCount; shift++, index--)
            {
                result |= (Byte)((source[index] == BitCharger.Enabled ? 1 : 0) << shift);
            }
        }

        /// <summary>
        /// Removes all control character from provided <paramref name="source"/>.
        /// </summary>
        /// <remarks>
        /// This method removes all control character from provided 
        /// <paramref name="source"/>. Control characters are underscores, 
        /// dashes and spaces (<see cref="BitCharger.Delimiter"/>) as well 
        /// as one leading binary literal "0b".
        /// </remarks>
        /// <param name="source">
        /// The source to be purified (cleaned).
        /// </param>
        /// <returns>
        /// The purified (cleaned) source.
        /// </returns>
        /// <seealso cref="Charge(String, Boolean)"/>
        private String Purify(String source)
        {
            // Binary strings can start with the binary literal (0b) and/or 
            // can contain underscores (_) as digit separator. Additionally, 
            // such strings may contain dashes (-) as digit separator as well. 
            // Therefore, those control character should be removed at all.

            source = source.Trim().Replace("_", String.Empty).Replace("-", String.Empty);

            if (source.StartsWith("0b", StringComparison.InvariantCultureIgnoreCase))
            {
                source = source.Substring(2);
            }

            return source.Replace(BitCharger.Delimiter, String.Empty);
        }

        /// <summary>
        /// Adjusts the content of <paramref name="source"/>.
        /// </summary>
        /// <remarks>
        /// The adjustments performed in this method is indeed a bit tricky 
        /// and are not really confirmed yet. In the hope of user feedback, 
        /// this method does it's job by best knowledge.
        /// </remarks>
        /// <param name="source">
        /// The source to be adjusted.
        /// </param>
        /// <param name="reverse">
        /// True to insert possibly missing zeros at the front of last byte 
        /// and false to prepent possibly missing zeros at the front of source.
        /// </param>
        /// <returns>
        /// The adjusted source.
        /// </returns>
        /// <seealso cref="Charge(String, Boolean)"/>
        private String Adjust(String source, Boolean reverse)
        {
            Int32 padding = BitCharger.BitCount - source.Length % BitCharger.BitCount;

            if (padding != BitCharger.BitCount)
            {
                if (reverse)
                {
                    Int32 offset = source.Length - BitCharger.BitCount + padding;

                    // Should never happen but nobody knows...
                    Debug.Assert(offset >= 0);

                    // input: 1010 1100 1111 => padding: 1010 1100 0000 1111 => finally: 0000 1111 1010 1100
                    source = source.Insert(offset, String.Empty.PadLeft(padding, BitCharger.Disabled));
                }
                else
                {
                    // input: 1010 1100 1111 => padding: 0000 1010 1100 1111 => finally: 0000 1010 1100 1111
                    source = source.Insert(0, String.Empty.PadLeft(padding, BitCharger.Disabled));
                }
            }

            return source;
        }

        #endregion
    }
}
