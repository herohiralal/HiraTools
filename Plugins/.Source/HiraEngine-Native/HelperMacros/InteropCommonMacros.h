#pragma once

// CONCATENATE MACRO =================================================================
// 
// Concatenates two arguments wrapped up in 5 macros, to provide a buffer and
// give other macros a chance to be executed before concatenation.
// 
// ===================================================================================

#pragma region Concatenate_Wrapped

#define CONCATENATE_WRAPPED(arg1, arg2) CONCATENATE_WRAPPED_1(arg1, arg2)
#define CONCATENATE_WRAPPED_1(arg1, arg2) CONCATENATE_WRAPPED_2(arg1, arg2)
#define CONCATENATE_WRAPPED_2(arg1, arg2) CONCATENATE_WRAPPED_3(arg1, arg2)
#define CONCATENATE_WRAPPED_3(arg1, arg2) CONCATENATE_WRAPPED_4(arg1, arg2)
#define CONCATENATE_WRAPPED_4(arg1, arg2) arg1##arg2

#pragma endregion Concatenate_Wrapped

// MAKE_STRING MACRO =================================================================
// 
// Turns an argument into a string wrapped up in 5 macros, to provide a buffer and
// give other macros a chance to be executed before turning into string.
// 
// ===================================================================================

#pragma region Stringize_Wrapped

#define MAKE_STRING(arg) MAKE_STRING_1(arg)
#define MAKE_STRING_1(arg) MAKE_STRING_2(arg)
#define MAKE_STRING_2(arg) MAKE_STRING_3(arg)
#define MAKE_STRING_3(arg) MAKE_STRING_4(arg)
#define MAKE_STRING_4(arg) #arg

#pragma endregion Stringize_Wrapped

// COUNT_ARGUMENTS MACRO =============================================================
// 
// Counts the number of arguments in a variadic macro.
// 
// Currently supports upto 50 arguments.
// 
// ===================================================================================

#pragma region Counter

#define SEQUENCER(_1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15, _16, _17, _18, _19, _20, _21, _22, _23, _24, _25, _26, _27, _28, _29, _30, _31, _32, _33, _34, _35, _36, _37, _38, _39, _40, _41, _42, _43, _44, _45, _46, _47, _48, _49, _50, N, ...) N
#define COUNT_ARGUMENTS(...) SEQUENCER(__VA_ARGS__, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0)

#pragma endregion Counter

// COUNT_PAIRS MACRO =================================================================
// 
// Counts the number of pairs of arguments in a variadic macro.
// 
// Currently supports upto 50 pairs.
// 
// ===================================================================================

#pragma region Pair_Counter

#define PAIR_SEQUENCER(_1, _1x, _2, _2x, _3, _3x, _4, _4x, _5, _5x, _6, _6x, _7, _7x, _8, _8x, _9, _9x, _10, _10x, _11, _11x, _12, _12x, _13, _13x, _14, _14x, _15, _15x, _16, _16x, _17, _17x, _18, _18x, _19, _19x, _20, _20x, _21, _21x, _22, _22x, _23, _23x, _24, _24x, _25, _25x, _26, _26x, _27, _27x, _28, _28x, _29, _29x, _30, _30x, _31, _31x, _32, _32x, _33, _33x, _34, _34x, _35, _35x, _36, _36x, _37, _37x, _38, _38x, _39, _39x, _40, _40x, _41, _41x, _42, _42x, _43, _43x, _44, _44x, _45, _45x, _46, _46x, _47, _47x, _48, _48x, _49, _49x, _50, _50x, N, ...) N
#define COUNT_PAIRS(...) PAIR_SEQUENCER(__VA_ARGS__, 50, x, 49, x, 48, x, 47, x, 46, x, 45, x, 44, x, 43, x, 42, x, 41, x, 40, x, 39, x, 38, x, 37, x, 36, x, 35, x, 34, x, 33, x, 32, x, 31, x, 30, x, 29, x, 28, x, 27, x, 26, x, 25, x, 24, x, 23, x, 22, x, 21, x, 20, x, 19, x, 18, x, 17, x, 16, x, 15, x, 14, x,13, x, 12, x, 11, x, 10, x, 9, x, 8, x, 7, x, 6, x, 5, x, 4, x, 3, x, 2, x, 1, 0, 0, x)

#pragma endregion Pair_Counter

// COUNT_TRIPLETS MACRO ==============================================================
// 
// Counts the number of triplets of arguments in a variadic macro.
// 
// Currently supports upto 40 triplets. (cannot do more because of C++ macro argument limit)
// 
// ===================================================================================

#pragma region Triplet_Counter

#define TRIPLET_SEQUENCER(_1, _1x, _1y, _2, _2x, _2y, _3, _3x, _3y, _4, _4x, _4y, _5, _5x, _5y, _6, _6x, _6y, _7, _7x, _7y, _8, _8x, _8y, _9, _9x, _9y, _10, _10x, _10y, _11, _11x, _11y, _12, _12x, _12y, _13, _13x, _13y, _14, _14x, _14y, _15, _15x, _15y, _16, _16x, _16y, _17, _17x, _17y, _18, _18x, _18y, _19, _19x, _19y, _20, _20x, _20y, _21, _21x, _21y, _22, _22x, _22y, _23, _23x, _23y, _24, _24x, _24y, _25, _25x, _25y, _26, _26x, _26y, _27, _27x, _27y, _28, _28x, _28y, _29, _29x, _29y, _30, _30x, _30y, _31, _31x, _31y, _32, _32x, _32y, _33, _33x, _33y, _34, _34x, _34y, _35, _35x, _35y, _36, _36x, _36y, _37, _37x, _37y, _38, _38x, _38y, _39, _39x, _39y, _40, _40x, _40y, N, ...) N
#define COUNT_TRIPLETS(...) TRIPLET_SEQUENCER(__VA_ARGS__, 50, x, x, 49, x, x, 48, x, x, 47, x, x, 46, x, x, 45, x, x, 44, x, x, 43, x, x, 42, x, x, 41, x, x, 40, x, x, 39, x, x, 38, x, x, 37, x, x, 36, x, x, 35, x, x, 34, x, x, 33, x, x, 32, x, x, 31, x, x, 30, x, x, 29, x, x, 28, x, x, 27, x, x, 26, x, x, 25, x, x, 24, x, x, 23, x, x, 22, x, x, 21, x, x, 20, x, x, 19, x, x, 18, x, x, 17, x, x, 16, x, x, 15, x, x, 14, x, x, 13, x, x, 12, x, x, 11, x, x, 10, x, x, 9, x, x, 8, x, x, 7, x, x, 6, x, x, 5, x, x, 4, x, x, 3, x, x, 2, x, x, 1, x, x, 0, x, x)

#pragma endregion Triplet_Counter

// COUNT_QUADRUPLETS MACRO ===========================================================
// 
// Counts the number of quadruplets of arguments in a variadic macro.
// 
// Currently supports upto 30 quadruplets. (cannot do more because of C++ macro argument limit)
// 
// ===================================================================================

#pragma region Quadruplet_Counter

#define QUADRUPLET_SEQUENCER(_1, _1x, _1y, _1z, _2, _2x, _2y, _2z, _3, _3x, _3y, _3z, _4, _4x, _4y, _4z, _5, _5x, _5y, _5z, _6, _6x, _6y, _6z, _7, _7x, _7y, _7z, _8, _8x, _8y, _8z, _9, _9x, _9y, _9z, _10, _10x, _10y, _10z, _11, _11x, _11y, _11z, _12, _12x, _12y, _12z, _13, _13x, _13y, _13z, _14, _14x, _14y, _14z, _15, _15x, _15y, _15z, _16, _16x, _16y, _16z, _17, _17x, _17y, _17z, _18, _18x, _18y, _18z, _19, _19x, _19y, _19z, _20, _20x, _20y, _20z, _21, _21x, _21y, _21z, _22, _22x, _22y, _22z, _23, _23x, _23y, _23z, _24, _24x, _24y, _24z, _25, _25x, _25y, _25z, _26, _26x, _26y, _26z, _27, _27x, _27y, _27z, _28, _28x, _28y, _28z, _29, _29x, _29y, _29z, _30, _30x, _30y, _30z, N, ...) N
#define COUNT_QUADRUPLETS(...) QUADRUPLET_SEQUENCER(__VA_ARGS__, 50, x, x, x, 49, x, x, x, 48, x, x, x, 47, x, x, x, 46, x, x, x, 45, x, x, x, 44, x, x, x, 43, x, x, x, 42, x, x, x, 41, x, x, x, 40, x, x, x, 39, x, x, x, 38, x, x, x, 37, x, x, x, 36, x, x, x, 35, x, x, x, 34, x, x, x, 33, x, x, x, 32, x, x, x, 31, x, x, x, 30, x, x, x, 29, x, x, x, 28, x, x, x, 27, x, x, x, 26, x, x, x, 25, x, x, x, 24, x, x, x, 23, x, x, x, 22, x, x, x, 21, x, x, x, 20, x, x, x, 19, x, x, x, 18, x, x, x, 17, x, x, x, 16, x, x, x, 15, x, x, x, 14, x, x, x, 13, x, x, x, 12, x, x, x, 11, x, x, x,10, x, x, x, 9, x, x, x, 8, x, x, x, 7, x, x, x, 6, x, x, x, 5, x, x, x, 4, x, x, x, 3, x, x, x, 2, x, x, x, 1, x, x, x, 0, x, x, x)

#pragma endregion Quadruplet_Counter

// FOR_EACH MACRO ====================================================================
// 
// Use FOR_EACH(<target-macro>, (your arguments)) to iterate through (your arguments)
// and apply the <target-macro> macro.
// 
// Currently supports upto 50 arguments.
// 
// ===================================================================================

#pragma region For_Each_Iterator

#define FOR_EACH_0(...)

#define FOR_EACH_1(what, x, ...)\
	what(x)\
	FOR_EACH_0(what, __VA_ARGS__)

#define FOR_EACH_2(what, x, ...)\
	what(x)\
	FOR_EACH_1(what,  __VA_ARGS__)

#define FOR_EACH_3(what, x, ...)\
	what(x)\
	FOR_EACH_2(what, __VA_ARGS__)

#define FOR_EACH_4(what, x, ...)\
	what(x)\
	FOR_EACH_3(what,  __VA_ARGS__)

#define FOR_EACH_5(what, x, ...)\
	what(x)\
	FOR_EACH_4(what,  __VA_ARGS__)

#define FOR_EACH_6(what, x, ...)\
	what(x)\
	FOR_EACH_5(what,  __VA_ARGS__)

#define FOR_EACH_7(what, x, ...)\
	what(x)\
	FOR_EACH_6(what,  __VA_ARGS__)

#define FOR_EACH_8(what, x, ...)\
	what(x)\
	FOR_EACH_7(what,  __VA_ARGS__)

#define FOR_EACH_9(what, x, ...)\
	what(x)\
	FOR_EACH_8(what,  __VA_ARGS__)

#define FOR_EACH_10(what, x, ...)\
	what(x)\
	FOR_EACH_9(what,  __VA_ARGS__)

#define FOR_EACH_11(what, x, ...)\
    what(x)\
    FOR_EACH_10(what, __VA_ARGS__)

#define FOR_EACH_12(what, x, ...)\
    what(x)\
    FOR_EACH_11(what, __VA_ARGS__)

#define FOR_EACH_13(what, x, ...)\
    what(x)\
    FOR_EACH_12(what, __VA_ARGS__)

#define FOR_EACH_14(what, x, ...)\
    what(x)\
    FOR_EACH_13(what, __VA_ARGS__)

#define FOR_EACH_15(what, x, ...)\
    what(x)\
    FOR_EACH_14(what, __VA_ARGS__)

#define FOR_EACH_16(what, x, ...)\
    what(x)\
    FOR_EACH_15(what, __VA_ARGS__)

#define FOR_EACH_17(what, x, ...)\
    what(x)\
    FOR_EACH_16(what, __VA_ARGS__)

#define FOR_EACH_18(what, x, ...)\
    what(x)\
    FOR_EACH_17(what, __VA_ARGS__)

#define FOR_EACH_19(what, x, ...)\
    what(x)\
    FOR_EACH_18(what, __VA_ARGS__)

#define FOR_EACH_20(what, x, ...)\
    what(x)\
    FOR_EACH_19(what, __VA_ARGS__)

#define FOR_EACH_21(what, x, ...)\
    what(x)\
    FOR_EACH_20(what, __VA_ARGS__)

#define FOR_EACH_22(what, x, ...)\
    what(x)\
    FOR_EACH_21(what, __VA_ARGS__)

#define FOR_EACH_23(what, x, ...)\
    what(x)\
    FOR_EACH_22(what, __VA_ARGS__)

#define FOR_EACH_24(what, x, ...)\
    what(x)\
    FOR_EACH_23(what, __VA_ARGS__)

#define FOR_EACH_25(what, x, ...)\
    what(x)\
    FOR_EACH_24(what, __VA_ARGS__)

#define FOR_EACH_26(what, x, ...)\
    what(x)\
    FOR_EACH_25(what, __VA_ARGS__)

#define FOR_EACH_27(what, x, ...)\
    what(x)\
    FOR_EACH_26(what, __VA_ARGS__)

#define FOR_EACH_28(what, x, ...)\
    what(x)\
    FOR_EACH_27(what, __VA_ARGS__)

#define FOR_EACH_29(what, x, ...)\
    what(x)\
    FOR_EACH_28(what, __VA_ARGS__)

#define FOR_EACH_30(what, x, ...)\
    what(x)\
    FOR_EACH_29(what, __VA_ARGS__)

#define FOR_EACH_31(what, x, ...)\
    what(x)\
    FOR_EACH_30(what, __VA_ARGS__)

#define FOR_EACH_32(what, x, ...)\
    what(x)\
    FOR_EACH_31(what, __VA_ARGS__)

#define FOR_EACH_33(what, x, ...)\
    what(x)\
    FOR_EACH_32(what, __VA_ARGS__)

#define FOR_EACH_34(what, x, ...)\
    what(x)\
    FOR_EACH_33(what, __VA_ARGS__)

#define FOR_EACH_35(what, x, ...)\
    what(x)\
    FOR_EACH_34(what, __VA_ARGS__)

#define FOR_EACH_36(what, x, ...)\
    what(x)\
    FOR_EACH_35(what, __VA_ARGS__)

#define FOR_EACH_37(what, x, ...)\
    what(x)\
    FOR_EACH_36(what, __VA_ARGS__)

#define FOR_EACH_38(what, x, ...)\
    what(x)\
    FOR_EACH_37(what, __VA_ARGS__)

#define FOR_EACH_39(what, x, ...)\
    what(x)\
    FOR_EACH_38(what, __VA_ARGS__)

#define FOR_EACH_40(what, x, ...)\
    what(x)\
    FOR_EACH_39(what, __VA_ARGS__)

#define FOR_EACH_41(what, x, ...)\
    what(x)\
    FOR_EACH_40(what, __VA_ARGS__)

#define FOR_EACH_42(what, x, ...)\
    what(x)\
    FOR_EACH_41(what, __VA_ARGS__)

#define FOR_EACH_43(what, x, ...)\
    what(x)\
    FOR_EACH_42(what, __VA_ARGS__)

#define FOR_EACH_44(what, x, ...)\
    what(x)\
    FOR_EACH_43(what, __VA_ARGS__)

#define FOR_EACH_45(what, x, ...)\
    what(x)\
    FOR_EACH_44(what, __VA_ARGS__)

#define FOR_EACH_46(what, x, ...)\
    what(x)\
    FOR_EACH_45(what, __VA_ARGS__)

#define FOR_EACH_47(what, x, ...)\
    what(x)\
    FOR_EACH_46(what, __VA_ARGS__)

#define FOR_EACH_48(what, x, ...)\
    what(x)\
    FOR_EACH_47(what, __VA_ARGS__)

#define FOR_EACH_49(what, x, ...)\
    what(x)\
    FOR_EACH_48(what, __VA_ARGS__)

#define FOR_EACH_50(what, x, ...)\
    what(x)\
    FOR_EACH_49(what, __VA_ARGS__)

#pragma endregion For_Each_Iterator

#define FOR_EACH(what, ...) CONCATENATE_WRAPPED(FOR_EACH_, COUNT_ARGUMENTS(__VA_ARGS__))(what, __VA_ARGS__)

// FOR_EACH_2_ARGUMENTS MACRO ========================================================
// 
// Use FOR_EACH_2_ARGUMENTS(<target-macro>, (couples)) to iterate through (couples)
// and apply the <target-macro> macro.
// 
// Currently supports upto 50 couples.
// 
// ===================================================================================

#pragma region 2A_For_Each_Iterator

#define FOR_EACH_2_ARGUMENTS_0(...)

#define FOR_EACH_2_ARGUMENTS_1(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_0(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_2(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_1(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_3(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_2(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_4(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_3(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_5(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_4(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_6(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_5(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_7(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_6(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_8(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_7(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_9(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_8(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_10(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_9(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_11(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_10(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_12(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_11(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_13(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_12(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_14(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_13(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_15(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_14(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_16(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_15(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_17(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_16(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_18(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_17(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_19(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_18(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_20(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_19(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_21(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_20(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_22(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_21(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_23(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_22(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_24(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_23(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_25(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_24(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_26(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_25(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_27(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_26(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_28(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_27(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_29(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_28(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_30(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_29(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_31(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_30(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_32(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_31(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_33(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_32(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_34(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_33(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_35(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_34(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_36(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_35(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_37(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_36(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_38(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_37(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_39(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_38(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_40(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_39(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_41(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_40(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_42(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_41(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_43(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_42(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_44(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_43(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_45(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_44(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_46(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_45(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_47(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_46(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_48(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_47(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_49(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_48(what, __VA_ARGS__)

#define FOR_EACH_2_ARGUMENTS_50(what, x, y, ...)\
    what(x, y)\
    FOR_EACH_2_ARGUMENTS_49(what, __VA_ARGS__)

#pragma endregion 2A_For_Each_Iterator

#define FOR_EACH_2_ARGUMENTS(what, ...) CONCATENATE_WRAPPED(FOR_EACH_2_ARGUMENTS_, COUNT_PAIRS(__VA_ARGS__))(what, __VA_ARGS__)

// FOR_EACH_3_ARGUMENTS MACRO ========================================================
// 
// Use FOR_EACH_3_ARGUMENTS(<target-macro>, (tuples)) to iterate through (tuples)
// and apply the <target-macro> macro.
// 
// Currently supports upto 40 triplets. (bottle-necked by COUNT_TRIPLETS macro)
// 
// ===================================================================================

#pragma region 3A_For_Each_Iterator

#define FOR_EACH_3_ARGUMENTS_0(...)

#define FOR_EACH_3_ARGUMENTS_1(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_0(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_2(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_1(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_3(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_2(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_4(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_3(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_5(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_4(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_6(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_5(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_7(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_6(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_8(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_7(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_9(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_8(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_10(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_9(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_11(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_10(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_12(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_11(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_13(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_12(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_14(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_13(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_15(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_14(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_16(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_15(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_17(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_16(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_18(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_17(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_19(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_18(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_20(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_19(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_21(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_20(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_22(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_21(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_23(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_22(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_24(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_23(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_25(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_24(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_26(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_25(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_27(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_26(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_28(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_27(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_29(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_28(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_30(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_29(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_31(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_30(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_32(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_31(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_33(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_32(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_34(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_33(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_35(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_34(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_36(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_35(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_37(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_36(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_38(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_37(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_39(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_38(what, __VA_ARGS__)

#define FOR_EACH_3_ARGUMENTS_40(what, x, y, z, ...)\
    what(x, y, z)\
    FOR_EACH_3_ARGUMENTS_39(what, __VA_ARGS__)

#pragma endregion 3A_For_Each_Iterator

#define FOR_EACH_3_ARGUMENTS(what, ...) CONCATENATE_WRAPPED(FOR_EACH_3_ARGUMENTS_, COUNT_TRIPLETS(__VA_ARGS__))(what, __VA_ARGS__)

// FOR_EACH_4_ARGUMENTS MACRO ========================================================
// 
// Use FOR_EACH_4_ARGUMENTS(<target-macro>, (quadruplets)) to iterate through (quadruplets)
// and apply the <target-macro> macro.
// 
// Currently supports upto 30 triplets. (bottle-necked by COUNT_QUADRUPLETS macro)
// 
// ===================================================================================

#pragma region 4A_For_Each_Iterator

#define FOR_EACH_4_ARGUMENTS_0(...)

#define FOR_EACH_4_ARGUMENTS_1(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_0(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_2(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_1(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_3(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_2(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_4(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_3(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_5(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_4(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_6(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_5(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_7(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_6(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_8(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_7(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_9(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_8(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_10(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_9(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_11(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_10(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_12(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_11(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_13(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_12(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_14(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_13(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_15(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_14(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_16(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_15(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_17(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_16(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_18(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_17(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_19(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_18(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_20(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_19(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_21(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_20(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_22(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_21(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_23(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_22(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_24(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_23(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_25(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_24(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_26(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_25(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_27(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_26(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_28(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_27(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_29(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_28(what, __VA_ARGS__)

#define FOR_EACH_4_ARGUMENTS_30(what, x, y, z, w, ...)\
    what(x, y, z, w)\
    FOR_EACH_4_ARGUMENTS_29(what, __VA_ARGS__)

#pragma endregion 4A_For_Each_Iterator

#define FOR_EACH_4_ARGUMENTS(what, ...) CONCATENATE_WRAPPED(FOR_EACH_4_ARGUMENTS_, COUNT_QUADRUPLETS(__VA_ARGS__))(what, __VA_ARGS__)

// DECLARE_ARGUMENTS MACRO ===========================================================
// 
// Use DECLARE_ARGUMENTS(type1, name1, type2, name2, ...) to iterate through
// and convert them into arguments declaration for a function.
// 
// Currently supports upto 50 pairs of type and name.
// 
// ===================================================================================

#pragma region Declare_Arguments_Iterator

#define DECLARE_ARGUMENTS_0(...)

#define DECLARE_ARGUMENTS_1(typeName, varName, ...)\
    typeName varName

#define DECLARE_ARGUMENTS_2(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_1(__VA_ARGS__)

#define DECLARE_ARGUMENTS_3(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_2(__VA_ARGS__)

#define DECLARE_ARGUMENTS_4(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_3(__VA_ARGS__)

#define DECLARE_ARGUMENTS_5(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_4(__VA_ARGS__)

#define DECLARE_ARGUMENTS_6(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_5(__VA_ARGS__)

#define DECLARE_ARGUMENTS_7(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_6(__VA_ARGS__)

#define DECLARE_ARGUMENTS_8(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_7(__VA_ARGS__)

#define DECLARE_ARGUMENTS_9(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_8(__VA_ARGS__)

#define DECLARE_ARGUMENTS_10(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_9(__VA_ARGS__)

#define DECLARE_ARGUMENTS_11(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_10(__VA_ARGS__)

#define DECLARE_ARGUMENTS_12(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_11(__VA_ARGS__)

#define DECLARE_ARGUMENTS_13(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_12(__VA_ARGS__)

#define DECLARE_ARGUMENTS_14(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_13(__VA_ARGS__)

#define DECLARE_ARGUMENTS_15(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_14(__VA_ARGS__)

#define DECLARE_ARGUMENTS_16(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_15(__VA_ARGS__)

#define DECLARE_ARGUMENTS_17(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_16(__VA_ARGS__)

#define DECLARE_ARGUMENTS_18(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_17(__VA_ARGS__)

#define DECLARE_ARGUMENTS_19(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_18(__VA_ARGS__)

#define DECLARE_ARGUMENTS_20(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_19(__VA_ARGS__)

#define DECLARE_ARGUMENTS_21(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_20(__VA_ARGS__)

#define DECLARE_ARGUMENTS_22(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_21(__VA_ARGS__)

#define DECLARE_ARGUMENTS_23(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_22(__VA_ARGS__)

#define DECLARE_ARGUMENTS_24(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_23(__VA_ARGS__)

#define DECLARE_ARGUMENTS_25(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_24(__VA_ARGS__)

#define DECLARE_ARGUMENTS_26(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_25(__VA_ARGS__)

#define DECLARE_ARGUMENTS_27(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_26(__VA_ARGS__)

#define DECLARE_ARGUMENTS_28(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_27(__VA_ARGS__)

#define DECLARE_ARGUMENTS_29(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_28(__VA_ARGS__)

#define DECLARE_ARGUMENTS_30(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_29(__VA_ARGS__)

#define DECLARE_ARGUMENTS_31(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_30(__VA_ARGS__)

#define DECLARE_ARGUMENTS_32(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_31(__VA_ARGS__)

#define DECLARE_ARGUMENTS_33(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_32(__VA_ARGS__)

#define DECLARE_ARGUMENTS_34(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_33(__VA_ARGS__)

#define DECLARE_ARGUMENTS_35(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_34(__VA_ARGS__)

#define DECLARE_ARGUMENTS_36(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_35(__VA_ARGS__)

#define DECLARE_ARGUMENTS_37(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_36(__VA_ARGS__)

#define DECLARE_ARGUMENTS_38(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_37(__VA_ARGS__)

#define DECLARE_ARGUMENTS_39(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_38(__VA_ARGS__)

#define DECLARE_ARGUMENTS_40(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_39(__VA_ARGS__)

#define DECLARE_ARGUMENTS_41(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_40(__VA_ARGS__)

#define DECLARE_ARGUMENTS_42(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_41(__VA_ARGS__)

#define DECLARE_ARGUMENTS_43(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_42(__VA_ARGS__)

#define DECLARE_ARGUMENTS_44(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_43(__VA_ARGS__)

#define DECLARE_ARGUMENTS_45(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_44(__VA_ARGS__)

#define DECLARE_ARGUMENTS_46(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_45(__VA_ARGS__)

#define DECLARE_ARGUMENTS_47(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_46(__VA_ARGS__)

#define DECLARE_ARGUMENTS_48(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_47(__VA_ARGS__)

#define DECLARE_ARGUMENTS_49(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_48(__VA_ARGS__)

#define DECLARE_ARGUMENTS_50(typeName, varName, ...)\
    typeName varName,\
    DECLARE_ARGUMENTS_49(__VA_ARGS__)

#pragma endregion Declare_Arguments_Iterator

#define DECLARE_ARGUMENTS(...) ( CONCATENATE_WRAPPED(DECLARE_ARGUMENTS_, COUNT_PAIRS(__VA_ARGS__))(__VA_ARGS__) )

// PASS_ARGUMENTS MACRO ==============================================================
// 
// Use PASS_ARGUMENTS(type1, name1, type2, name2, ...) to iterate through
// and convert them into parameters passed to a function.
// 
// Currently supports upto 50 pairs of type and name.
// 
// ===================================================================================

#pragma region Pass_Arguments_Iterator

#define PASS_ARGUMENTS_0(...)

#define PASS_ARGUMENTS_1(typeName, varName, ...)\
    varName
    PASS_ARGUMENTS_0(__VA_ARGS__)

#define PASS_ARGUMENTS_2(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_1(__VA_ARGS__)

#define PASS_ARGUMENTS_3(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_2(__VA_ARGS__)

#define PASS_ARGUMENTS_4(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_3(__VA_ARGS__)

#define PASS_ARGUMENTS_5(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_4(__VA_ARGS__)

#define PASS_ARGUMENTS_6(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_5(__VA_ARGS__)

#define PASS_ARGUMENTS_7(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_6(__VA_ARGS__)

#define PASS_ARGUMENTS_8(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_7(__VA_ARGS__)

#define PASS_ARGUMENTS_9(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_8(__VA_ARGS__)

#define PASS_ARGUMENTS_10(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_9(__VA_ARGS__)

#define PASS_ARGUMENTS_11(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_10(__VA_ARGS__)

#define PASS_ARGUMENTS_12(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_11(__VA_ARGS__)

#define PASS_ARGUMENTS_13(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_12(__VA_ARGS__)

#define PASS_ARGUMENTS_14(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_13(__VA_ARGS__)

#define PASS_ARGUMENTS_15(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_14(__VA_ARGS__)

#define PASS_ARGUMENTS_16(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_15(__VA_ARGS__)

#define PASS_ARGUMENTS_17(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_16(__VA_ARGS__)

#define PASS_ARGUMENTS_18(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_17(__VA_ARGS__)

#define PASS_ARGUMENTS_19(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_18(__VA_ARGS__)

#define PASS_ARGUMENTS_20(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_19(__VA_ARGS__)

#define PASS_ARGUMENTS_21(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_20(__VA_ARGS__)

#define PASS_ARGUMENTS_22(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_21(__VA_ARGS__)

#define PASS_ARGUMENTS_23(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_22(__VA_ARGS__)

#define PASS_ARGUMENTS_24(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_23(__VA_ARGS__)

#define PASS_ARGUMENTS_25(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_24(__VA_ARGS__)

#define PASS_ARGUMENTS_26(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_25(__VA_ARGS__)

#define PASS_ARGUMENTS_27(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_26(__VA_ARGS__)

#define PASS_ARGUMENTS_28(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_27(__VA_ARGS__)

#define PASS_ARGUMENTS_29(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_28(__VA_ARGS__)

#define PASS_ARGUMENTS_30(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_29(__VA_ARGS__)

#define PASS_ARGUMENTS_31(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_30(__VA_ARGS__)

#define PASS_ARGUMENTS_32(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_31(__VA_ARGS__)

#define PASS_ARGUMENTS_33(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_32(__VA_ARGS__)

#define PASS_ARGUMENTS_34(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_33(__VA_ARGS__)

#define PASS_ARGUMENTS_35(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_34(__VA_ARGS__)

#define PASS_ARGUMENTS_36(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_35(__VA_ARGS__)

#define PASS_ARGUMENTS_37(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_36(__VA_ARGS__)

#define PASS_ARGUMENTS_38(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_37(__VA_ARGS__)

#define PASS_ARGUMENTS_39(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_38(__VA_ARGS__)

#define PASS_ARGUMENTS_40(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_39(__VA_ARGS__)

#define PASS_ARGUMENTS_41(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_40(__VA_ARGS__)

#define PASS_ARGUMENTS_42(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_41(__VA_ARGS__)

#define PASS_ARGUMENTS_43(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_42(__VA_ARGS__)

#define PASS_ARGUMENTS_44(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_43(__VA_ARGS__)

#define PASS_ARGUMENTS_45(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_44(__VA_ARGS__)

#define PASS_ARGUMENTS_46(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_45(__VA_ARGS__)

#define PASS_ARGUMENTS_47(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_46(__VA_ARGS__)

#define PASS_ARGUMENTS_48(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_47(__VA_ARGS__)

#define PASS_ARGUMENTS_49(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_48(__VA_ARGS__)

#define PASS_ARGUMENTS_50(typeName, varName, ...)\
    varName, \
    PASS_ARGUMENTS_49(__VA_ARGS__)

#pragma endregion Pass_Arguments_Iterator

#define PASS_ARGUMENTS(...) ( CONCATENATE_WRAPPED(PASS_ARGUMENTS_, COUNT_PAIRS(__VA_ARGS__))(__VA_ARGS__) )

// PASS_TYPES MACRO ==============================================================
// 
// Use PASS_TYPES(type1, name1, type2, name2, ...) to iterate through
// and convert them into types passed when declaring a delegate.
// 
// Currently supports upto 50 pairs of type and name.
// 
// ===================================================================================

#pragma region Pass_Types_Iterator

#define PASS_TYPES_0(...)

#define PASS_TYPES_1(typeName, varName, ...)\
    typeName
    PASS_TYPES_0(__VA_ARGS__)

#define PASS_TYPES_2(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_1(__VA_ARGS__)

#define PASS_TYPES_3(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_2(__VA_ARGS__)

#define PASS_TYPES_4(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_3(__VA_ARGS__)

#define PASS_TYPES_5(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_4(__VA_ARGS__)

#define PASS_TYPES_6(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_5(__VA_ARGS__)

#define PASS_TYPES_7(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_6(__VA_ARGS__)

#define PASS_TYPES_8(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_7(__VA_ARGS__)

#define PASS_TYPES_9(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_8(__VA_ARGS__)

#define PASS_TYPES_10(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_9(__VA_ARGS__)

#define PASS_TYPES_11(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_10(__VA_ARGS__)

#define PASS_TYPES_12(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_11(__VA_ARGS__)

#define PASS_TYPES_13(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_12(__VA_ARGS__)

#define PASS_TYPES_14(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_13(__VA_ARGS__)

#define PASS_TYPES_15(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_14(__VA_ARGS__)

#define PASS_TYPES_16(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_15(__VA_ARGS__)

#define PASS_TYPES_17(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_16(__VA_ARGS__)

#define PASS_TYPES_18(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_17(__VA_ARGS__)

#define PASS_TYPES_19(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_18(__VA_ARGS__)

#define PASS_TYPES_20(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_19(__VA_ARGS__)

#define PASS_TYPES_21(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_20(__VA_ARGS__)

#define PASS_TYPES_22(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_21(__VA_ARGS__)

#define PASS_TYPES_23(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_22(__VA_ARGS__)

#define PASS_TYPES_24(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_23(__VA_ARGS__)

#define PASS_TYPES_25(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_24(__VA_ARGS__)

#define PASS_TYPES_26(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_25(__VA_ARGS__)

#define PASS_TYPES_27(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_26(__VA_ARGS__)

#define PASS_TYPES_28(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_27(__VA_ARGS__)

#define PASS_TYPES_29(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_28(__VA_ARGS__)

#define PASS_TYPES_30(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_29(__VA_ARGS__)

#define PASS_TYPES_31(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_30(__VA_ARGS__)

#define PASS_TYPES_32(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_31(__VA_ARGS__)

#define PASS_TYPES_33(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_32(__VA_ARGS__)

#define PASS_TYPES_34(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_33(__VA_ARGS__)

#define PASS_TYPES_35(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_34(__VA_ARGS__)

#define PASS_TYPES_36(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_35(__VA_ARGS__)

#define PASS_TYPES_37(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_36(__VA_ARGS__)

#define PASS_TYPES_38(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_37(__VA_ARGS__)

#define PASS_TYPES_39(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_38(__VA_ARGS__)

#define PASS_TYPES_40(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_39(__VA_ARGS__)

#define PASS_TYPES_41(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_40(__VA_ARGS__)

#define PASS_TYPES_42(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_41(__VA_ARGS__)

#define PASS_TYPES_43(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_42(__VA_ARGS__)

#define PASS_TYPES_44(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_43(__VA_ARGS__)

#define PASS_TYPES_45(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_44(__VA_ARGS__)

#define PASS_TYPES_46(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_45(__VA_ARGS__)

#define PASS_TYPES_47(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_46(__VA_ARGS__)

#define PASS_TYPES_48(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_47(__VA_ARGS__)

#define PASS_TYPES_49(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_48(__VA_ARGS__)

#define PASS_TYPES_50(typeName, varName, ...)\
    typeName, \
    PASS_TYPES_49(__VA_ARGS__)

#pragma endregion Pass_Types_Iterator

#define PASS_TYPES(...) ( CONCATENATE_WRAPPED(PASS_TYPES_, COUNT_PAIRS(__VA_ARGS__))(__VA_ARGS__) )

#define CALLING_CONVENTION __cdecl

#if _WIN32
#define DLLEXPORT(type) extern "C" __declspec(dllexport) type CALLING_CONVENTION
#else
#define DLLEXPORT(type) extern "C" type CALLING_CONVENTION
#endif