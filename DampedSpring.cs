/******************************************************************************
  Copyright (c) 2008-2012 Ryan Juckett
  http://www.ryanjuckett.com/
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.
 
  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:
 
  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
 
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
 
  3. This notice may not be removed or altered from any source
     distribution.
******************************************************************************/

using System.Numerics;

class DampedSpring
{
    public static void CalculateDampedString
    (
        ref float position,
        ref float velocity,
        float restPosition,
        float deltaTime,
        float frequency,
        float damping
    )
    {
        const float epsilon = 0.0001f;

        if (damping < 0)
            damping = 0;
        if (frequency < 0)
            frequency = 0;

        if (frequency < epsilon)
            return;

        float oldPosition = position - restPosition;
        float oldVelocity = velocity;
        
        // over-damped
        if (damping > 1.0f + epsilon)
        {
            float za = -frequency * damping;
            float zb = frequency * MathF.Sqrt(MathF.Pow(damping, 2) - 1.0f);
            float z1 = za - zb;
            float z2 = za + zb;

            float e1 = MathF.Exp(z1 * deltaTime);
            float e2 = MathF.Exp(z2 * deltaTime);

            float invTwoZb = 1.0f / (2.0f*zb); // = 1 / (z2 - z1)
                
            float e1oTwoZb = e1*invTwoZb;
            float e2oTwoZb = e2*invTwoZb;

            float z1e1oTwoZb = z1*e1oTwoZb;
            float z2e2oTwoZb = z2*e2oTwoZb;
            
            position = oldPosition * (e1oTwoZb * z2 - z2e2oTwoZb + e2) + oldVelocity * (-e1oTwoZb + e2oTwoZb) + restPosition;
            velocity = oldPosition * ((z1e1oTwoZb - z2e2oTwoZb + e2) * z2) + oldVelocity * (-z1e1oTwoZb + z2e2oTwoZb);
        }
        // under-damped
        else if (damping < 1.0f - epsilon)
        {
            float omegaZeta = frequency * damping;
            float alpha     = frequency * MathF.Sqrt(1.0f - MathF.Pow(damping, 2));

            float expTerm = MathF.Exp(-omegaZeta * deltaTime);
            float cosTerm = MathF.Cos(alpha * deltaTime);
            float sinTerm = MathF.Sin(alpha * deltaTime);
                
            float invAlpha = 1.0f / alpha;

            float expSin = expTerm * sinTerm;
            float expCos = expTerm * cosTerm;
            float expOmegaZetaSinoAlpha = expTerm * omegaZeta * sinTerm * invAlpha;

            position = oldPosition * (expCos + expOmegaZetaSinoAlpha) + oldVelocity * (expSin * invAlpha) + restPosition;
            velocity = oldPosition * (-expSin * alpha - omegaZeta * expOmegaZetaSinoAlpha) + oldVelocity * (expCos - expOmegaZetaSinoAlpha);
        }
        // critically damped
        else
        {
            float expTerm     = MathF.Exp(-frequency * deltaTime);
            float timeExp     = deltaTime * expTerm;
            float timeExpFreq = timeExp * frequency;

            position = oldPosition * (timeExpFreq + expTerm) + oldVelocity * (timeExp) + restPosition;
            velocity = oldPosition * (-frequency * timeExpFreq) + oldVelocity * (-timeExpFreq + expTerm);
        }
    }

    public static void CalculateDampedString
    (
        ref Vector2 position,
        ref Vector2 velocity,
        Vector2 restPosition,
        float deltaTime,
        float frequency,
        float damping
    )
    {
        CalculateDampedString
        (
            ref position.X,
            ref velocity.X,
            restPosition.X,
            deltaTime,
            frequency,
            damping
        );

        CalculateDampedString
        (
            ref position.Y,
            ref velocity.Y,
            restPosition.Y,
            deltaTime,
            frequency,
            damping
        );
    }
}