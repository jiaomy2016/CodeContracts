// CodeContracts
// 
// Copyright (c) Microsoft Corporation
// 
// All rights reserved. 
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Tests.Sources
{
    using System.Threading.Tasks;

    class Foo
    {
        private async Task<int> FooAsync(bool correct)
        {
            Contract.Ensures(Contract.Result<int>() == 42);
            Contract.EnsuresOnThrow<InvalidOperationException>(correct, "Ooops!");

            await Task.Delay(42);

            throw new InvalidOperationException();
        }

        public static async Task HandleFooAsync(bool shouldBeCorrect)
        {
            try
            {
                // If async postcondition are implemented properly
                // they should not affect exception handling when the method throws.
                await new Foo().FooAsync(shouldBeCorrect);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Exception should be handled!");
            }
        }
    }

    partial class TestMain
    {
        partial void Run()
        {
           Foo.HandleFooAsync(behave).Wait();
        }

        public ContractFailureKind NegativeExpectedKind = ContractFailureKind.PostconditionOnException;
        public string NegativeExpectedCondition = "correct";
    }
}
