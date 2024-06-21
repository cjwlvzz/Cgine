#pragma once
#include "ComponentsCommon.h"
namespace Cgine
{

#define INIT_INFO(component) namespace component {struct init_info;} 

	INIT_INFO(transform)

		namespace game_entity {
		struct entity_info
		{
			transform::init_info* transform{ nullptr };
		};
		entity_id create_game_entity(const entity_info& info);
		void remove_game_entity(entity_id id);
		bool is_alive(entity_id id);
	}
}
