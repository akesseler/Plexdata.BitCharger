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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Plexdata.Converters.Internals
{
    /// <summary>
    /// Determines the system's byte order.
    /// </summary>
    /// <remarks>
    /// In contrast to property <see cref="BitConverter.IsLittleEndian"/>, which 
    /// determines a system's byte order (aka "endianness") at compile time, this 
    /// implementation is designed to determine a system's byte order at runtime.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class ByteOrder : IByteOrder
    {
        #region Private fields

        /// <summary>
        /// An instance of the system endianness helper.
        /// </summary>
        /// <remarks>
        /// This field represents an instance of the system endianness helper.
        /// </remarks>
        private static readonly ByteOrderHelper helper = new ByteOrderHelper();

        #endregion

        #region Construction

        /// <summary>
        /// Static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all static class fields.
        /// </remarks>
        static ByteOrder()
        {
            ByteOrder.helper = new ByteOrderHelper() { Value = 1 };
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// This constructor actually does nothing.
        /// </remarks>
        public ByteOrder()
            : base()
        {
        }

        #endregion

        #region Public properties

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
        public Boolean IsLittleEndian
        {
            get
            {
                return ByteOrder.helper.Byte0 == 1;
            }
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Helper structure to determine a system's endianness.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This helper structure behaves like its union equivalent in C.
        /// </para>
        /// <code>
        /// union { int i; char c[sizeof(int)]; } x;
        /// </code>
        /// <para>
        /// To be able to determine the current system's endianness just set 
        /// <see cref="Value"/> to one and then check <see cref="Byte0"/> if 
        /// it's one. If so, than the system is a little-endian system.
        /// </para>
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        private struct ByteOrderHelper
        {
            /// <summary>
            /// Byte 0 of integer <see cref="Value"/>.
            /// </summary>
            /// <remarks>
            /// The endianness dependent byte 0 of integer <see cref="Value"/>.
            /// </remarks>
            [FieldOffset(0)]
            public Byte Byte0;

            /// <summary>
            /// Byte 1 of integer <see cref="Value"/>.
            /// </summary>
            /// <remarks>
            /// The endianness dependent byte 1 of integer <see cref="Value"/>.
            /// </remarks>
            [FieldOffset(1)]
            public Byte Byte1;

            /// <summary>
            /// Byte 2 of integer <see cref="Value"/>.
            /// </summary>
            /// <remarks>
            /// The endianness dependent byte 2 of integer <see cref="Value"/>.
            /// </remarks>
            [FieldOffset(2)]
            public Byte Byte2;

            /// <summary>
            /// Byte 3 of integer <see cref="Value"/>.
            /// </summary>
            /// <remarks>
            /// The endianness dependent byte 3 of integer <see cref="Value"/>.
            /// </remarks>
            [FieldOffset(3)]
            public Byte Byte3;

            /// <summary>
            /// Integer representation of byte 0 .. 3.
            /// </summary>
            /// <remarks>
            /// The integer representation of byte 0 .. 3.
            /// </remarks>
            [FieldOffset(0)]
            public Int32 Value;
        }

        #endregion
    }
}
