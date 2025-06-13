#pragma once
#include <bitset>
#include <cstdint>

using ComponentType = std::uint8_t;
constexpr std::size_t MAX_COMPONENTS = 32;
using Signature = std::bitset<MAX_COMPONENTS>;