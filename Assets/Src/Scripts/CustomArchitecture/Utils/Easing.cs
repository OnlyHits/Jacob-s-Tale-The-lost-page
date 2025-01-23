// namespace CustomArchitecture

// //------------------------------------------------
// //------------------------------------------------
// TEasingFunctionAssoc CEasing::M_EasingFunctionsArray[] =
// {
// {EASING_FUNCTION_LINEAR, &EaseLinear, &EaseLinear4f, "Linear"},

// {EASING_FUNCTION_IN_SINE, &EaseInSine, &EaseInSine4f, "Sine In"},
// {EASING_FUNCTION_OUT_SINE, &EaseOutSine, &EaseOutSine4f, "Sine Out"},
// {EASING_FUNCTION_IN_OUT_SINE, &EaseInOutSine, &EaseInOutSine4f, "Sine In/Out"},

// {EASING_FUNCTION_IN_QUAD, &EaseInQuad, &EaseInQuad4f, "Quad In"},
// {EASING_FUNCTION_OUT_QUAD, &EaseOutQuad, &EaseOutQuad4f, "Quad Out"},
// {EASING_FUNCTION_IN_OUT_QUAD, &EaseInOutQuad, &EaseInOutQuad4f, "Quad In/Out"},

// {EASING_FUNCTION_IN_CUBIC, &EaseInCubic, &EaseInCubic4f, "Cubic In"},
// {EASING_FUNCTION_OUT_CUBIC, &EaseOutCubic, &EaseOutCubic4f, "Cubic Out"},
// {EASING_FUNCTION_IN_OUT_CUBIC, &EaseInOutCubic, &EaseInOutCubic4f, "Cubic In/Out"},

// {EASING_FUNCTION_IN_QUART, &EaseInQuart, &EaseInQuart4f, "Quart In"},
// {EASING_FUNCTION_OUT_QUART, &EaseOutQuart, &EaseOutQuart4f, "Quart Out"},
// {EASING_FUNCTION_IN_OUT_QUART, &EaseInOutQuart, &EaseInOutQuart4f, "Quart In/Out"},

// {EASING_FUNCTION_IN_QUINT, &EaseInQuint, &EaseInQuint4f, "Quint In"},
// {EASING_FUNCTION_OUT_QUINT, &EaseOutQuint, &EaseOutQuint4f, "Quint Out"},
// {EASING_FUNCTION_IN_OUT_QUINT, &EaseInOutQuint, &EaseInOutQuint4f, "Quint In/out"},

// {EASING_FUNCTION_IN_EXPO, &EaseInExpo, &EaseInExpo4f, "Expo In"},
// {EASING_FUNCTION_OUT_EXPO, &EaseOutExpo, &EaseOutExpo4f, "Expo Out"},
// {EASING_FUNCTION_IN_OUT_EXPO, &EaseInOutExpo, &EaseInOutExpo4f, "Expo In/Out"},

// {EASING_FUNCTION_IN_CIRC, &EaseInCirc, &EaseInCirc4f, "Circ In"},
// {EASING_FUNCTION_OUT_CIRC, &EaseOutCirc, &EaseOutCirc4f, "Circ Out"},
// {EASING_FUNCTION_IN_OUT_CIRC, &EaseInOutCirc, &EaseInOutCirc4f, "Circ In/Out"},

// {EASING_FUNCTION_IN_BACK, &EaseInBack, &EaseInBack4f, "Back In"},
// {EASING_FUNCTION_OUT_BACK, &EaseOutBack, &EaseOutBack4f, "Back Out"},
// {EASING_FUNCTION_IN_OUT_BACK, &EaseInOutBack, &EaseOutBack4f, "Back In/Out"},

// {EASING_FUNCTION_IN_ELASTIC, &EaseInElastic, &EaseInElastic4f, "Elastic In"},
// {EASING_FUNCTION_OUT_ELASTIC, &EaseOutElastic, &EaseOutElastic4f, "Elastic Out"},
// {EASING_FUNCTION_IN_OUT_ELASTIC, &EaseInOutElastic, &EaseInOutElastic4f, "Elastic In/Out"},

// {EASING_FUNCTION_IN_BOUNCE, &EaseInBounce, &EaseInBounce4f, "Bounce In"},
// {EASING_FUNCTION_OUT_BOUNCE, &EaseOutBounce, &EaseOutBounce4f, "Bounce Out"},
// {EASING_FUNCTION_IN_OUT_BOUNCE, &EaseInOutBounce, &EaseInOutBounce4f, "Bounce In/Out"},
// };
// //------------------------------------------------
// // todo: use better index to avoid search
// //------------------------------------------------
// std::string CEasing::GetEasingFunctionName(EEasingFunction func)
// {
// for (int i = 0; i < EASING_FUNCTION_COUNT_; ++i)
// {
// if (M_EasingFunctionsArray[i].func_id == func)
// {
// return M_EasingFunctionsArray[i].name;
// }
// }

// MOBI_ASSERT_PRINT(false, "CEasing::GetEasingFunction: unknown function\n");

// return "unknown";
// }
// //------------------------------------------------
// TEasingFunction CEasing::GetEasingFunction(EEasingFunction func)
// {
// for (int i = 0; i < EASING_FUNCTION_COUNT_; ++i)
// {
// if (M_EasingFunctionsArray[i].func_id == func)
// {
// return M_EasingFunctionsArray[i].func;
// }
// }

// MOBI_ASSERT_PRINT(false, "CEasing::GetEasingFunction: unknown function\n");

// return NULL;
// }
// //------------------------------------------------
// TEasingFunction4f CEasing::GetEasingFunction4f(EEasingFunction func)
// {
// for (int i = 0; i < EASING_FUNCTION_COUNT_; ++i)
// {
// if (M_EasingFunctionsArray[i].func_id == func)
// {
// return M_EasingFunctionsArray[i].func_4f;
// }
// }

// MOBI_ASSERT_PRINT(false, "CEasing::GetEasingFunction4f: unknown function\n");

// return NULL;
// }

// //------------------------------------------------
// // linear
// //------------------------------------------------
// float CEasing::EaseLinear(float t)
// {
// return t;
// };
// //------------------------------------------------
// float CEasing::EaseLinear4f(float t, float b, float c, float d)
// {
// return c*t/d + b;
// };
// //------------------------------------------------
// // sine
// //------------------------------------------------
// float CEasing::EaseInSine( float t )
// {
// return 1 + sinf(PIOVERTWOf * (--t));
// }
// //------------------------------------------------
// float CEasing::EaseOutSine( float t )
// {
// return sinf(PIOVERTWOf * t);
// }
// //------------------------------------------------
// float CEasing::EaseInOutSine( float t )
// {
// return 0.5f * (1 + sinf( PIf * (t - 0.5f) ) );
// }
// //------------------------------------------------
// float CEasing::EaseInSine4f(float t,float b , float c, float d)
// {
// return -c * cosf(t/d * PIOVERTWOf) + c + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutSine4f(float t,float b , float c, float d)
// {
// return c * sinf(t/d * PIOVERTWOf) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutSine4f(float t,float b , float c, float d)
// {
// return -c/2.0f * (cosf(PIf*t/d) - 1) + b;
// }
// //------------------------------------------------
// // quad
// //------------------------------------------------
// float CEasing::EaseInQuad( float t )
// {
// return t * t;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuad( float t )
// {
// return t * (2 - t);
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuad( float t )
// {
// if (t < 0.5f)
// return 2 * t * t;
// return t * (4 - 2 * t) - 1;
// }
// //------------------------------------------------
// float CEasing::EaseInQuad4f(float t,float b , float c, float d)
// {
// t = t / d;
// return c*t*t + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuad4f(float t,float b , float c, float d)
// {
// t = t / d;
// return -c *t*(t-2) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuad4f(float t,float b , float c, float d)
// {
// t = t * 2 / d;

// if (t < 1)
// return ((c/2)*(t*t)) + b;

// //return -c/2 * (((--t) * (t-2)) - 1) + b;
// // original line is not compliant with c++
// // this version should do the same... not sure
// return -c/2 * (((t-1)*(t-3)) - 1) + b;
// }
// //------------------------------------------------
// // cubic
// //------------------------------------------------
// float CEasing::EaseInCubic( float t )
// {
// return t * t * t;
// }
// //------------------------------------------------
// float CEasing::EaseOutCubic( float t )
// {
// t = t - 1;
// return 1 + t * t * t;
// }
// //------------------------------------------------
// float CEasing::EaseInOutCubic( float t )
// {
// t = t * 2;
// if (t < 1)
// return 0.5f * t * t * t;
// t = t - 2;
// return 0.5f * (t * t * t + 2);

// /*
// if (t < 0.5f)
// return 4 * t * t * t;
// else
// {
// // 1 + (--t) * (2 * (--t)) * (2 * t);
// // original line is not compliant with c++
// return 1 + (t-1) * (2 * t - 2) * (2 * t - 2);
// }
// */
// }
// //------------------------------------------------
// float CEasing::EaseInCubic4f(float t,float b , float c, float d)
// {
// t = t / d;
// return c * t * t * t + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutCubic4f(float t,float b , float c, float d)
// {
// t=t/d-1;
// return c * (t*t*t + 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutCubic4f(float t,float b , float c, float d)
// {
// t = t / (d/2);
// if (t < 1)
// return c/2*t*t*t + b;
// t = t - 2;
// return c/2*(t*t*t + 2) + b;
// }
// //------------------------------------------------
// // quart
// //------------------------------------------------
// float CEasing::EaseInQuart( float t )
// {
// t *= t;
// return t * t;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuart( float t )
// {
// //t = (--t) * t;
// t = t - 1;
// t = t * t;
// return 1 - t * t;
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuart( float t )
// {
// if (t < 0.5f)
// {
// t *= t;
// return 8 * t * t;
// }
// else
// {
// //t = (--t) * t;
// t = t - 1;
// t = t * t;
// return 1 - 8 * t * t;
// }
// }
// //------------------------------------------------
// float CEasing::EaseInQuart4f(float t,float b , float c, float d)
// {
// t = t / d;
// return c* t*t*t*t + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuart4f(float t,float b , float c, float d)
// {
// t = t / d - 1;
// return -c * (t*t*t*t - 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuart4f(float t,float b , float c, float d)
// {
// t/=d/2;
// if (t < 1)
// return c/2 *t*t*t*t + b;
// t = t - 2;
// return -c/2 * (t*t*t*t - 2) + b;
// }
// //------------------------------------------------
// // quint
// //------------------------------------------------
// float CEasing::EaseInQuint( float t )
// {
// float t2 = t * t;
// return t * t2 * t2;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuint( float t )
// {
// //float t2 = (--t) * t;
// //return 1 + t * t2 * t2;
// t = t - 1;
// float t2 = t * t;
// return 1 + t * t2 * t2;
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuint( float t )
// {
// float t2;
// if( t < 0.5 )
// {
// t2 = t * t;
// return 16 * t * t2 * t2;
// }
// else
// {
// //t2 = (--t) * t;
// //return 1 + 16 * t * t2 * t2;
// t = t - 1;
// t2 = t * t;
// return 1 + 16 * t * t2 * t2;
// }
// }
// //------------------------------------------------
// float CEasing::EaseInQuint4f(float t,float b , float c, float d)
// {
// t = t / d;
// return c * t*t*t*t*t + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutQuint4f(float t,float b , float c, float d)
// {
// t = t / d - 1;
// return c* (t*t*t*t*t + 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutQuint4f(float t,float b , float c, float d)
// {
// t /= d / 2;

// if (t < 1)
// return c/2 * t*t*t*t*t + b;

// t = t - 2;
// return c/2 * (t*t*t*t*t + 2) + b;
// }
// //------------------------------------------------
// // expo
// //------------------------------------------------
// float CEasing::EaseInExpo( float t )
// {
// return (powf( 2, 8 * t ) - 1) / 255;
// }
// //------------------------------------------------
// float CEasing::EaseOutExpo( float t )
// {
// return 1 - powf( 2, -8 * t );
// }
// //------------------------------------------------
// float CEasing::EaseInOutExpo( float t )
// {
// if( t < 0.5f )
// {
// return (powf( 2, 16 * t ) - 1) / 510;
// }
// else
// {
// return 1 - 0.5f * powf( 2, -16 * (t - 0.5f) );
// }
// }
// //------------------------------------------------
// float CEasing::EaseInExpo4f(float t,float b , float c, float d)
// {
// return (t==0) ? b : c * powf(2, 10 * (t/d - 1)) + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutExpo4f(float t,float b , float c, float d)
// {
// return (t==d) ? b+c : c * (-powf(2, -10 * t/d) + 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutExpo4f(float t,float b , float c, float d)
// {
// if (t==0)
// return b;
// if (t==d)
// return b+c;

// t /= d / 2;
// if (t < 1)
// return c/2 * powf(2, 10 * (t - 1)) + b;

// return c/2 * (-powf(2, -10 * --t) + 2) + b;
// }
// //------------------------------------------------
// // circ
// //------------------------------------------------
// float CEasing::EaseInCirc( float t )
// {
// return 1 - sqrtf( 1 - t );
// }
// //------------------------------------------------
// float CEasing::EaseOutCirc( float t )
// {
// return sqrtf( t );
// }
// //------------------------------------------------
// float CEasing::EaseInOutCirc( float t )
// {
// if( t < 0.5f )
// {
// return (1 - sqrtf( 1 - 2 * t )) * 0.5f;
// }
// else
// {
// return (1 + sqrtf( 2 * t - 1 )) * 0.5f;
// }
// }
// //------------------------------------------------
// float CEasing::EaseInCirc4f(float t,float b , float c, float d)
// {
// t = t / d;
// return -c * (sqrtf(1 - t*t) - 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutCirc4f(float t,float b , float c, float d)
// {
// t = t / d - 1;
// return c * sqrtf(1 - t*t) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutCirc4f(float t,float b , float c, float d)
// {
// t /= d / 2;
// if (t < 1)
// return -c/2 * (sqrtf(1 - t*t) - 1) + b;
// t = t - 2;
// return c/2 * (sqrtf(1 - t*t) + 1) + b;
// }
// //------------------------------------------------
// // back
// //------------------------------------------------
// float CEasing::EaseInBack( float t )
// {
// return t * t * (2.70158f * t - 1.70158f);
// }
// //------------------------------------------------
// float CEasing::EaseOutBack( float t )
// {
// //return 1 + (--t) * t * (2.70158f * t + 1.70158f);
// t = t - 1;
// return 1 + t * t * (2.70158f * t + 1.70158f);
// }
// //------------------------------------------------
// float CEasing::EaseInOutBack( float t )
// {
// if( t < 0.5f )
// {
// return t * t * (7 * t - 2.5f) * 2;
// }
// else
// {
// //return 1 + (--t) * t * 2 * (7 * t + 2.5f);
// t = t - 1;
// return 1 + t * t * 2 * (7 * t + 2.5f);
// }
// }
// //------------------------------------------------
// float CEasing::EaseInBack4f(float t,float b , float c, float d)
// {
// float s = 1.70158f;
// float postFix = t/=d;
// return c*(postFix)*t*((s+1)*t - s) + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutBack4f(float t,float b , float c, float d)
// {
// float s = 1.70158f;
// t = t / d - 1;
// return c*(t*t*((s+1)*t + s) + 1) + b;
// }
// //------------------------------------------------
// float CEasing::EaseInOutBack4f(float t,float b , float c, float d)
// {
// float s = 1.70158f;
// t /= d / 2;
// if (t < 1)
// {
// s*=1.525f;
// return c/2*(t*t*((s+1)*t - s)) + b;
// }

// t = t - 2;
// s*=1.525f;
// return c/2*(t*t*((s+1) * t + s) + 2) + b;
// }
// //------------------------------------------------
// // elastic
// //------------------------------------------------
// float CEasing::EaseInElastic( float t )
// {
// float t2 = t * t;
// return t2 * t2 * sinf( t * PI * 4.5f );
// }
// //------------------------------------------------
// float CEasing::EaseOutElastic(float t, float max_bounce)
// {
// float p = 0.3f;
// return powf(2,-10*t) * sinf((t-p/4)*(TWOPIf)/p) * max_bounce + 1;
// }
// //------------------------------------------------
// float CEasing::EaseOutElastic(float t)
// {
// return EaseOutElastic(t, 1.0f);
// }
// //------------------------------------------------
// float CEasing::EaseInOutElastic( float t )
// {
// float t2;
// if( t < 0.45f )
// {
// t2 = t * t;
// return 8 * t2 * t2 * sinf( t * PIf * 9 );
// }
// else if( t < 0.55f )
// {
// return 0.5f + 0.75f * sinf( t * PIf * 4 );
// }
// else
// {
// t2 = (t - 1) * (t - 1);
// return 1 - 8 * t2 * t2 * sinf( t * PIf * 9 );
// }
// }
// //------------------------------------------------
// float CEasing::EaseInElastic4f(float t,float b , float c, float d)
// {
// if (t==0)
// return b;

// t = t / d;

// if (t==1)
// return b+c;

// float p=d*.3f;
// float a=c;
// float s=p/4;
// t = t - 1;
// return -(a*powf(2,10*t) * sinf( (t*d-s)*(TWOPIf)/p )) + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutElastic4f(float t,float b , float c, float d)
// {
// if (t==0)
// {
// return b;
// }
// t = t / d;
// if (t==1)
// {
// return b+c;
// }
// float p=d*.3f;
// float a=c;
// float s=p/4;
// return (a*powf(2,-10*t) * sinf( (t*d-s)*(TWOPIf)/p ) + c + b);
// }
// //------------------------------------------------
// float CEasing::EaseInOutElastic4f(float t,float b , float c, float d)
// {
// if (t==0)
// return b;

// t = t / (d / 2);
// if (t==2)
// return b+c;
// float p=d*(.3f*1.5f);
// float a=c;
// float s=p/4;


// if (t < 1)
// {
// t = t - 1;
// return -.5f*(a*powf(2,10*t) * sinf( (t*d-s)*(TWOPIf)/p )) + b;
// }
// t = t - 1;
// return a*powf(2,-10*t) * sinf( (t*d-s)*(TWOPIf)/p )*.5f + c + b;
// }
// //------------------------------------------------
// // bounce
// //------------------------------------------------
// float CEasing::EaseInBounce( float t )
// {
// return powf( 2, 6 * (t - 1) ) * fabsf( sinf( t * PIf * 3.5f ) );
// }
// //------------------------------------------------
// float CEasing::EaseOutBounce( float t )
// {
// return 1 - powf( 2, -6 * t ) * fabsf( cosf( t * PIf * 3.5f ) );
// }
// //------------------------------------------------
// float CEasing::EaseInOutBounce( float t )
// {
// if( t < 0.5f )
// {
// return 8 * powf( 2, 8 * (t - 1) ) * fabsf( sinf( t * PIf * 7 ) );
// }
// else
// {
// return 1 - 8 * powf( 2, -8 * t ) * fabsf( sinf( t * PIf * 7 ) );
// }
// }//------------------------------------------------
// float CEasing::EaseInBounce4f(float t,float b , float c, float d)
// {
// return c - EaseOutBounce4f (d-t, 0, c, d) + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutBounce4f(float t,float b , float c, float d)
// {
// t = t / d;
// if (t < (1 / 2.75f))
// {
// return c * (7.5625f*t*t) + b;
// }
// else if (t < (2/2.75f))
// {
// t = t - 1.5f/2.75f;
// return c*(7.5625f*t*t + .75f) + b;
// }
// else if (t < (2.5f/2.75f))
// {
// t = t - 2.25f/2.75f;
// return c*(7.5625f*t*t + .9375f) + b;
// }
// else
// {
// t =t - 2.625f/2.75f;
// return c*(7.5625f*t*t + .984375f) + b;
// }
// }
// //------------------------------------------------
// float CEasing::EaseInOutBounce4f(float t,float b , float c, float d)
// {
// if (t < d/2)
// return EaseInBounce4f (t*2, 0, c, d) * .5f + b;
// else
// return EaseOutBounce4f (t*2-d, 0, c, d) * .5f + c*.5f + b;
// }
// //------------------------------------------------
// float CEasing::EaseOutSmooth(float time)
// {
//     float ts = time*time;
//     float tc = ts*time;
//     return (-4.f*tc*ts + 13.5f*ts*ts + -17.f*tc + 8.5f*ts);
// }
// //------------------------------------------------

