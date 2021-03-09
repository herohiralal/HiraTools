#pragma once

#include "HelperMacros.h"

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
    typeName varName\
    DECLARE_ARGUMENTS_0(__VA_ARGS__)

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

#pragma endregion

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
    varName \
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

#pragma endregion

#define PASS_ARGUMENTS(...) ( CONCATENATE_WRAPPED(PASS_ARGUMENTS_, COUNT_PAIRS(__VA_ARGS__))(__VA_ARGS__) )

// PASS_TYPES MACRO ==================================================================
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
    typeName \
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

#pragma endregion

#define PASS_TYPES(...) ( CONCATENATE_WRAPPED(PASS_TYPES_, COUNT_PAIRS(__VA_ARGS__))(__VA_ARGS__) )