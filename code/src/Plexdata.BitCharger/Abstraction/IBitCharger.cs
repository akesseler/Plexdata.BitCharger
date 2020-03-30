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

using System;
using System.Runtime.Serialization;

namespace Plexdata.Converters.Abstraction
{
    /// <summary>
    /// The bit-charger interface.
    /// </summary>
    /// <remarks>
    /// This interface represents the whole functionality of the 
    /// bit-charger. Furthermore, this interface supports cloning 
    /// as well as serialization.
    /// </remarks>
    /// <seealso cref="ICloneable"/>
    /// <seealso cref="ISerializable"/>
    public interface IBitCharger : ICloneable, ISerializable
    {
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
        Int32 Capacity { get; }

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
        Int32 Length { get; }

        /// <summary>
        /// Total length of managed bytes.
        /// </summary>
        /// <remarks>
        /// This amount is equivalent to the <see cref="Length"/> but 
        /// divided by eight.
        /// </remarks>
        /// <value>
        /// The total number of bytes currently managed.
        /// </value>
        /// <seealso cref="Length"/>
        Int32 Bytes { get; }

        /// <summary>
        /// Current plain content.
        /// </summary>
        /// <remarks>
        /// This is just the plain content of the internal buffer.
        /// </remarks>
        /// <value>
        /// The plain content of the internal buffer.
        /// </value>
        String Content { get; }

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
        void SetBitAt(Int32 index, Boolean enabled);

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
        Boolean HasBitAt(Int32 index);

        /// <summary>
        /// Sets all bits of the <paramref name="source"/> array starting 
        /// at index zero.
        /// </summary>
        /// <remarks>
        /// This method sets all bits of the <paramref name="source"/> 
        /// array starting at index zero.
        /// </remarks>
        /// <param name="source">
        /// The array of bytes to take over all bits.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is throws in case of <paramref name="source"/> 
        /// is null.
        /// </exception>
        /// <seealso cref="SetBytes(Byte[], Int32)"/>
        void SetBytes(Byte[] source);

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
        void SetBytes(Byte[] source, Int32 offset);

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
        Byte[] GetBytes();

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
        Byte[] GetBytes(Int32 offset);

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
        Byte[] GetBytes(Int32 offset, Int32 length);

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
        void Charge(String source);

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
        void Charge(String source, Boolean reverse);

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
        String ToString(Boolean grouped);
    }
}
