#pragma once
#include <unordered_map>
#include <functional>
#include <vector>
#include <typeindex>
#include <memory>
#include "Event.h"

class EventBus {
public:
	EventBus() = default;
	template <typename EventType>
	void subscribe(std::function<void(const EventType&)> callback) {
		auto& listeners = subscribers[typeid(EventType)];
		listeners.push_back(
			[func = std::move(callback)](const Event& event) {
				func(static_cast<const EventType&>(event));
			}
		);
	}
	template <typename EventType>
	void publish(const EventType& event) const {
		auto it = subscribers.find(typeid(EventType));
		if (it != subscribers.end()) {
			for (const auto& listener : it->second) {
				listener(event);
			}
		}
	}
private:
	std::unordered_map<std::type_index, std::vector<std::function<void(const Event&)>>> subscribers;
};