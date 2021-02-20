#pragma once

// base

#define GETTER_BASE(type, name) type Get##name()

#define SETTER_BASE(type, name) void Set##name(type InValue)

// ref

#define GETTER_REF(type, name) type& Get##name() { return name; }

#define SETTER_REF(type, name) void Set##name(type& InValue) { name = InValue; }

// ref const

#define GETTER_REF_CONST(type, name) const type& Get##name() const { return name; }

// default

#define GETTER_DEFAULT_IMPL(type, name) const { return name; }

#define SETTER_DEFAULT_IMPL(type, name) { name = InValue; }

// none

#define ACCESSOR_NONE(what, type, name)

// standard

#define ACCESSOR_STD(what, type, name) what##_BASE(type, name) what##_DEFAULT_IMPL(type, name)

// ref

#define ACCESSOR_STD_REF(what, type, name) what##_REF(type, name)

// ref const

#define ACCESSOR_STD_REF_CONST(what, type, name) what##_REF_CONST(type, name)

// virtual

#define ACCESSOR_STD_VIRTUAL(what, type, name) virtual ACCESSOR_STD(what, type, name)

// custom

#define ACCESSOR_CUSTOM(what, type, name) what##_BASE(type, name);

// custom const

#define ACCESSOR_CUSTOM_CONST(what, type, name) what##_BASE(type, name) const;

// custom virtual

#define ACCESSOR_CUSTOM_VIRTUAL(what, type, name) virtual ACCESSOR_CUSTOM(what, type, name)

// custom virtual const

#define ACCESSOR_CUSTOM_VIRTUAL_CONST(what, type, name) virtual ACCESSOR_CUSTOM_CONST(what, type, name)

// override

#define ACCESSOR_OVERRIDE(what, type, name) virtual what##_BASE(type, name) override;

// const override

#define ACCESSOR_CONST_OVERRIDE(what, type, name) virtual what##_BASE(type, name) const override;

// main

#define PROPERTY(type, name, get, set) \
    private: type name; \
    public: \
        ACCESSOR_##get(GETTER, type, name) \
        ACCESSOR_##set(SETTER, type, name)

#define DECLARE_SINGLETON( typeName) \
    private: \
        static typeName* Instance; \
    public: \
        typeName(const typeName&) = delete; \
        typeName& operator=(const typeName&) = delete; \
        static typeName& Get() { return *Instance; }