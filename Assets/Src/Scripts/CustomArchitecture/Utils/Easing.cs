using UnityEngine;

namespace CustomArchitecture
{
    public static class Easing
    {
        public static float EaseLinear(float t)
        {
            return t;
        }

        //------------------------------------------------
        public static float EaseLinear4f(float t, float b, float c, float d)
        {
            return c*t/d + b;
        }
    
        public static float EaseInSine( float t )
        {
            return 1 + Mathf.Sin(Mathf.PI / 2f * (--t));
        }
        //------------------------------------------------
        public static float EaseOutSine( float t )
        {
            return Mathf.Sin(Mathf.PI / 2f * t);
        }

        //------------------------------------------------
        public static float EaseInOutSine( float t )
        {
            return 0.5f * (1 + Mathf.Sin( Mathf.PI * (t - 0.5f) ) );
        }
        //------------------------------------------------
        public static float EaseInSine4f(float t,float b , float c, float d)
        {
            return -c * Mathf.Cos(t/d * Mathf.PI / 2f) + c + b;
        }
        //------------------------------------------------
        public static float EaseOutSine4f(float t,float b , float c, float d)
        {
            return c * Mathf.Sin(t/d * Mathf.PI / 2f) + b;
        }
        //------------------------------------------------
        public static float EaseInOutSine4f(float t,float b , float c, float d)
        {
            return -c/2.0f * (Mathf.Cos(Mathf.PI*t/d) - 1) + b;
        }

        //------------------------------------------------
        // quad
        //------------------------------------------------
        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        //------------------------------------------------
        public static float EaseOutQuad(float t)
        {
            return t * (2 - t);
        }

        //------------------------------------------------
        public static float EaseInOutQuad(float t)
        {
            if (t < 0.5f)
                return 2 * t * t;
            return t * (4 - 2 * t) - 1;
        }

        //------------------------------------------------
        public static float EaseInQuad4f(float t, float b , float c, float d)
        {
            t = t / d;
            return c*t*t + b;
        }

        //------------------------------------------------
        public static float EaseOutQuad4f(float t,float b , float c, float d)
        {
            t = t / d;
            return -c *t*(t-2) + b;
        }

        //------------------------------------------------
        public static float EaseInOutQuad4f(float t,float b , float c, float d)
        {
            t = t * 2 / d;

            if (t < 1)
                return ((c/2)*(t*t)) + b;

            return -c/2 * (((t-1)*(t-3)) - 1) + b;
        }

        //------------------------------------------------
        // cubic
        //------------------------------------------------
        public static float EaseInCubic( float t )
        {
            return t * t * t;
        }
        
        //------------------------------------------------
        public static float EaseOutCubic( float t )
        {
            t = t - 1;
            return 1 + t * t * t;
        }
        
        //------------------------------------------------
        public static float EaseInOutCubic( float t )
        {
            t = t * 2;
            if (t < 1)
                return 0.5f * t * t * t;
        
            t = t - 2;
            return 0.5f * (t * t * t + 2);
        }

        //------------------------------------------------
        public static float EaseInCubic4f(float t,float b , float c, float d)
        {
            t = t / d;
            return c * t * t * t + b;
        }

        //------------------------------------------------
        public static float EaseOutCubic4f(float t,float b , float c, float d)
        {
            t=t/d-1;
            return c * (t*t*t + 1) + b;
        }

        //------------------------------------------------
        public static float EaseInOutCubic4f(float t,float b , float c, float d)
        {
            t = t / (d/2);
            if (t < 1)
                return c/2*t*t*t + b;
            t = t - 2;
            return c/2*(t*t*t + 2) + b;
        }

        //------------------------------------------------
        // quart
        //------------------------------------------------

        public static float EaseInQuart( float t )
        {
            t *= t;
            return t * t;
        }
        //------------------------------------------------
        public static float EaseOutQuart( float t )
        {
        //t = (--t) * t;
        t = t - 1;
        t = t * t;
        return 1 - t * t;
        }
        //------------------------------------------------
        public static float EaseInOutQuart( float t )
        {
        if (t < 0.5f)
        {
        t *= t;
        return 8 * t * t;
        }
        else
        {
        //t = (--t) * t;
        t = t - 1;
        t = t * t;
        return 1 - 8 * t * t;
        }
        }
        //------------------------------------------------
        public static float EaseInQuart4f(float t,float b , float c, float d)
        {
        t = t / d;
        return c* t*t*t*t + b;
        }
        //------------------------------------------------
        public static float EaseOutQuart4f(float t,float b , float c, float d)
        {
        t = t / d - 1;
        return -c * (t*t*t*t - 1) + b;
        }
        //------------------------------------------------
        public static float EaseInOutQuart4f(float t,float b , float c, float d)
        {
        t/=d/2;
        if (t < 1)
        return c/2 *t*t*t*t + b;
        t = t - 2;
        return -c/2 * (t*t*t*t - 2) + b;
        }
        //------------------------------------------------
        // quint
        //------------------------------------------------
        public static float EaseInQuint( float t )
        {
        float t2 = t * t;
        return t * t2 * t2;
        }
        //------------------------------------------------
        public static float EaseOutQuint( float t )
        {
        //float t2 = (--t) * t;
        //return 1 + t * t2 * t2;
        t = t - 1;
        float t2 = t * t;
        return 1 + t * t2 * t2;
        }
        //------------------------------------------------
        public static float EaseInOutQuint( float t )
        {
        float t2;
        if( t < 0.5 )
        {
        t2 = t * t;
        return 16 * t * t2 * t2;
        }
        else
        {
        //t2 = (--t) * t;
        //return 1 + 16 * t * t2 * t2;
        t = t - 1;
        t2 = t * t;
        return 1 + 16 * t * t2 * t2;
        }
        }
        //------------------------------------------------
        public static float EaseInQuint4f(float t,float b , float c, float d)
        {
        t = t / d;
        return c * t*t*t*t*t + b;
        }
        //------------------------------------------------
        public static float EaseOutQuint4f(float t,float b , float c, float d)
        {
        t = t / d - 1;
        return c* (t*t*t*t*t + 1) + b;
        }
        //------------------------------------------------
        public static float EaseInOutQuint4f(float t,float b , float c, float d)
        {
        t /= d / 2;

        if (t < 1)
        return c/2 * t*t*t*t*t + b;

        t = t - 2;
        return c/2 * (t*t*t*t*t + 2) + b;
        }
        //------------------------------------------------
        // expo
        //------------------------------------------------
        public static float EaseInExpo( float t )
        {
        return (Mathf.Pow(8 * t, 2) - 1) / 255;
        }
        //------------------------------------------------
        public static float EaseOutExpo( float t )
        {
        return 1 - Mathf.Pow(-8 * t, 2);
        }
        //------------------------------------------------
        public static float EaseInOutExpo( float t )
        {
        if( t < 0.5f )
        {
        return (Mathf.Pow(16 * t, 2) - 1) / 510;
        }
        else
        {
        return 1 - 0.5f * Mathf.Pow(-16 * (t - 0.5f), 2);
        }
        }
        //------------------------------------------------
        public static float EaseInExpo4f(float t,float b , float c, float d)
        {
        return (t==0) ? b : c * Mathf.Pow(10 * (t/d - 1), 2) + b;
        }
        //------------------------------------------------
        public static float EaseOutExpo4f(float t,float b , float c, float d)
        {
        return (t==d) ? b+c : c * (-Mathf.Pow(-10 * t/d, 2) + 1) + b;
        }
        //------------------------------------------------
        public static float EaseInOutExpo4f(float t,float b , float c, float d)
        {
        if (t==0)
        return b;
        if (t==d)
        return b+c;

        t /= d / 2;
        if (t < 1)
        return c/2 * Mathf.Pow(10 * (t - 1), 2) + b;

        return c/2 * (-Mathf.Pow(-10 * --t, 2) + 2) + b;
        }
        //------------------------------------------------
        // circ
        //------------------------------------------------
        public static float EaseInCirc( float t )
        {
        return 1 - Mathf.Sqrt( 1 - t );
        }
        //------------------------------------------------
        public static float EaseOutCirc( float t )
        {
        return Mathf.Sqrt( t );
        }
        //------------------------------------------------
        public static float EaseInOutCirc( float t )
        {
        if( t < 0.5f )
        {
        return (1 - Mathf.Sqrt( 1 - 2 * t )) * 0.5f;
        }
        else
        {
        return (1 + Mathf.Sqrt( 2 * t - 1 )) * 0.5f;
        }
        }
        //------------------------------------------------
        public static float EaseInCirc4f(float t,float b , float c, float d)
        {
        t = t / d;
        return -c * (Mathf.Sqrt(1 - t*t) - 1) + b;
        }
        //------------------------------------------------
        public static float EaseOutCirc4f(float t,float b , float c, float d)
        {
        t = t / d - 1;
        return c * Mathf.Sqrt(1 - t*t) + b;
        }
        //------------------------------------------------
        public static float EaseInOutCirc4f(float t,float b , float c, float d)
        {
        t /= d / 2;
        if (t < 1)
        return -c/2 * (Mathf.Sqrt(1 - t*t) - 1) + b;
        t = t - 2;
        return c/2 * (Mathf.Sqrt(1 - t*t) + 1) + b;
        }
        //------------------------------------------------
        // back
        //------------------------------------------------
        public static float EaseInBack( float t )
        {
        return t * t * (2.70158f * t - 1.70158f);
        }
        //------------------------------------------------
        public static float EaseOutBack( float t )
        {
        //return 1 + (--t) * t * (2.70158f * t + 1.70158f);
        t = t - 1;
        return 1 + t * t * (2.70158f * t + 1.70158f);
        }
        //------------------------------------------------
        public static float EaseInOutBack( float t )
        {
        if( t < 0.5f )
        {
        return t * t * (7 * t - 2.5f) * 2;
        }
        else
        {
        //return 1 + (--t) * t * 2 * (7 * t + 2.5f);
        t = t - 1;
        return 1 + t * t * 2 * (7 * t + 2.5f);
        }
        }
        //------------------------------------------------
        public static float EaseInBack4f(float t,float b , float c, float d)
        {
        float s = 1.70158f;
        float postFix = t/=d;
        return c*(postFix)*t*((s+1)*t - s) + b;
        }
        //------------------------------------------------
        public static float EaseOutBack4f(float t,float b , float c, float d)
        {
        float s = 1.70158f;
        t = t / d - 1;
        return c*(t*t*((s+1)*t + s) + 1) + b;
        }
        //------------------------------------------------
        public static float EaseInOutBack4f(float t,float b , float c, float d)
        {
        float s = 1.70158f;
        t /= d / 2;
        if (t < 1)
        {
        s*=1.525f;
        return c/2*(t*t*((s+1)*t - s)) + b;
        }

        t = t - 2;
        s*=1.525f;
        return c/2*(t*t*((s+1) * t + s) + 2) + b;
        }
        //------------------------------------------------
        // elastic
        //------------------------------------------------
        public static float EaseInElastic( float t )
        {
        float t2 = t * t;
        return t2 * t2 * Mathf.Sin( t * Mathf.PI * 4.5f );
        }
        //------------------------------------------------
        public static float EaseOutElastic(float t, float max_bounce)
        {
        float p = 0.3f;
        return Mathf.Pow(-10*t, 2) * Mathf.Sin((t-p/4)*(2*Mathf.PI)/p) * max_bounce + 1;
        }
        //------------------------------------------------
        public static float EaseOutElastic(float t)
        {
        return EaseOutElastic(t, 1.0f);
        }
        //------------------------------------------------
        public static float EaseInOutElastic( float t )
        {
        float t2;
        if( t < 0.45f )
        {
        t2 = t * t;
        return 8 * t2 * t2 * Mathf.Sin( t * Mathf.PI * 9 );
        }
        else if( t < 0.55f )
        {
        return 0.5f + 0.75f * Mathf.Sin( t * Mathf.PI * 4 );
        }
        else
        {
        t2 = (t - 1) * (t - 1);
        return 1 - 8 * t2 * t2 * Mathf.Sin( t * Mathf.PI * 9 );
        }
        }
        //------------------------------------------------
        public static float EaseInElastic4f(float t,float b , float c, float d)
        {
        if (t==0)
        return b;

        t = t / d;

        if (t==1)
        return b+c;

        float p=d*.3f;
        float a=c;
        float s=p/4;
        t = t - 1;
        return -(a*Mathf.Pow(10*t, 2) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )) + b;
        }
        //------------------------------------------------
        public static float EaseOutElastic4f(float t,float b , float c, float d)
        {
        if (t==0)
        {
        return b;
        }
        t = t / d;
        if (t==1)
        {
        return b+c;
        }
        float p=d*.3f;
        float a=c;
        float s=p/4;
        return (a*Mathf.Pow(-10*t, 2) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p ) + c + b);
        }
        //------------------------------------------------
        public static float EaseInOutElastic4f(float t,float b , float c, float d)
        {
        if (t==0)
        return b;

        t = t / (d / 2);
        if (t==2)
        return b+c;
        float p=d*(.3f*1.5f);
        float a=c;
        float s=p/4;


        if (t < 1)
        {
        t = t - 1;
        return -.5f*(a*Mathf.Pow(10*t, 2) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )) + b;
        }
        t = t - 1;
        return a*Mathf.Pow(-10*t, 2) * Mathf.Sin( (t*d-s)*(2*Mathf.PI)/p )*.5f + c + b;
        }
        //------------------------------------------------
        // bounce
        //------------------------------------------------
        public static float EaseInBounce( float t )
        {
        return Mathf.Pow(6 * (t - 1), 2) * Mathf.Abs(Mathf.Sin( t * Mathf.PI * 3.5f ) );
        }
        //------------------------------------------------
        public static float EaseOutBounce( float t )
        {
        return 1 - Mathf.Pow(-6 * t, 2) * Mathf.Abs( Mathf.Cos( t * Mathf.PI * 3.5f ) );
        }
        //------------------------------------------------
        public static float EaseInOutBounce( float t )
        {
        if( t < 0.5f )
        {
        return 8 * Mathf.Pow(8 * (t - 1), 2) * Mathf.Abs( Mathf.Sin( t * Mathf.PI * 7 ) );
        }
        else
        {
        return 1 - 8 * Mathf.Pow(-8 * t, 2) * Mathf.Abs( Mathf.Sin( t * Mathf.PI * 7 ) );
        }
        }//------------------------------------------------
        public static float EaseInBounce4f(float t,float b , float c, float d)
        {
        return c - EaseOutBounce4f (d-t, 0, c, d) + b;
        }
        //------------------------------------------------
        public static float EaseOutBounce4f(float t,float b , float c, float d)
        {
        t = t / d;
        if (t < (1 / 2.75f))
        {
        return c * (7.5625f*t*t) + b;
        }
        else if (t < (2/2.75f))
        {
        t = t - 1.5f/2.75f;
        return c*(7.5625f*t*t + .75f) + b;
        }
        else if (t < (2.5f/2.75f))
        {
        t = t - 2.25f/2.75f;
        return c*(7.5625f*t*t + .9375f) + b;
        }
        else
        {
        t =t - 2.625f/2.75f;
        return c*(7.5625f*t*t + .984375f) + b;
        }
        }

        //------------------------------------------------
        public static float EaseInOutBounce4f(float t,float b , float c, float d)
        {
        if (t < d/2)
        return EaseInBounce4f (t*2, 0, c, d) * .5f + b;
        else
        return EaseOutBounce4f (t*2-d, 0, c, d) * .5f + c*.5f + b;
        }
        //------------------------------------------------
        public static float EaseOutSmooth(float time)
        {
            float ts = time*time;
            float tc = ts*time;
            return (-4f*tc*ts + 13.5f*ts*ts + -17f*tc + 8.5f*ts);
        }
        //------------------------------------------------

    }
}
