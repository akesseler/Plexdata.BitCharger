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

namespace Plexdata.Converters.Abstraction
{
    /// <summary>
    /// Determines the system's byte order.
    /// </summary>
    /// <remarks>
    /// In contrast to property <see cref="BitConverter.IsLittleEndian"/>, which 
    /// determines a system's byte order (aka "endianness") at compile time, this 
    /// interface is designed to determine a system's byte order at runtime.
    /// </remarks>
    public interface IByteOrder
    {
        /// <summary>
        /// Indicates the byte order (aka "endianness") in which data is stored in 
        /// current computer architecture at runtime.
        /// </summary>
        /// <remarks>
        /// This property allows to determine the byte order (aka "endianness") in 
        /// which data is stored in current computer architecture at runtime.
        /// </remarks>
        /// <value>
        /// If <c>true</c> than the underlying system uses little-endian byte order. 
        /// Otherwise the return value is <c>false</c>.
        /// </value>
        Boolean IsLittleEndian { get; }
    }
}
