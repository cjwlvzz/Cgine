#pragma once
#include <stdint.h>
//unsigned intergers,unreal like
using uint8 = uint8_t;
using uint16 = uint16_t;
using uint32 = uint32_t;
using uint64 = uint64_t;
//signed intergers,unreal like
using int8 = int8_t;
using int16 = int16_t;
using int32 = int32_t;
using int64 = int64_t;

using f32 = float;

//Invalid identifier
constexpr uint64 uint64_invalid_id{ 0xffff'ffff'ffff'ffffui64 };
constexpr uint32 uint32_invalid_id{ 0xffff'ffffui32 };
constexpr uint16 uint16_invalid_id{ 0xffffui16 };
constexpr uint8 uint8_invalid_id{ 0xffui8 };