// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;


public class GitHub_17435
{
    const int Pass = 100;
    const int Fail = 0;
    static int Main()
    {
        Ok(new[] { Vector128.Create(1.0f) });
        NotOk(new[] { Vector128.Create(1.0f) });
        return Pass;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector128<float> Add(Vector128<float> left, Vector128<float> right)
    {
        if (Sse.IsSupported)
        {
            return Sse.Add(left, right);
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Vector128<float> AddNoCheck(Vector128<float> left, Vector128<float> right)
    {
        return Sse.Add(left, right);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Ok(Vector128<float>[] src)
    {
        var vector = src[0];
        AddNoCheck(vector, vector);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void NotOk(Vector128<float>[] src)
    {
        // In this version, when the Add is inlined, the local copy propagation doesn't preserve the
        // assertion that both 'left' and 'right' are equal to 'vector', so we wind up with an extra
        // copy.
        var vector = src[0];
        Add(vector, vector);
    }
}
