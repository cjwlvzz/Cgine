#pragma once
#include "ComponentsCommon.h"
namespace Cgine
{
	namespace game_entity {
		struct entity_info
		{

		};
		entity_id create_game_entity(const entity_info& info);
		void remove_game_entity(entity_id id);
	}
}
