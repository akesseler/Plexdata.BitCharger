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

namespace Plexdata.Converters.Factories
{
    /// <summary>
    /// The factory to create instances of class <see cref="IBitCharger"/>.
    /// </summary>
    /// <remarks>
    /// This factory serves as alternative in case of dependency injection 
    /// is not possible.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static class BitChargerFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IBitCharger"/>.
        /// </summary>
        /// <remarks>
        /// This method creates an instance of <see cref="IBitCharger"/>.
        /// </remarks>
        /// <returns>
        /// An instance of <see cref="IBitCharger"/>.
        /// </returns>
        public static IBitCharger Create()
        {
            return new BitCharger();
        }

        /// <summary>
        /// Creates an instance of <see cref="IBitCharger"/>.
        /// </summary>
        /// <remarks>
        /// This method creates an instance of <see cref="IBitCharger"/> 
        /// using an initial <paramref name="length"/>.
        /// </remarks>
        /// <param name="length">
        /// The value to be used as initial length.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IBitCharger"/>.
        /// </returns>
        public static IBitCharger Create(Int32 length)
        {
            return new BitCharger(length);
        }
    }
}
